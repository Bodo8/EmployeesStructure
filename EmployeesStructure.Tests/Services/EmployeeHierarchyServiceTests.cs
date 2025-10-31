using EmployeesStructure.Data;
using EmployeesStructure.Models;
using EmployeesStructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EmployeesStructure.Tests.Services
{
    [TestClass]
    public class EmployeeHierarchyServiceTests
    {
        private Mock<EmployeeContext> _mockContext;
        private Mock<DbSet<Employee>> _mockEmployeeSet;
        private EmployeeHierarchyService _service;
        private List<Employee> _testEmployees;

        [TestInitialize]
        public void Setup()
        {
            // Przygotowanie testowych danych
            _testEmployees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Jan Kowalski", SuperiorId = null },
                new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
                new Employee { Id = 3, Name = "Anna Lewandowska", SuperiorId = 1 },
                new Employee { Id = 4, Name = "Andrzej Abacki", SuperiorId = 2 },
                new Employee { Id = 5, Name = "Piotr Wiśniewski", SuperiorId = 2 },
                new Employee { Id = 6, Name = "Maria Dąbrowska", SuperiorId = 3 }
            };

            // Mock DbSet
            _mockEmployeeSet = CreateMockDbSet(_testEmployees);

            // Mock Context
            _mockContext = new Mock<EmployeeContext>();
            _mockContext.Setup(e => e.Set<Employee>()).Returns(_mockEmployeeSet.Object);

            // Inicjalizacja serwisu
            _service = new EmployeeHierarchyService(_mockContext.Object);
        }

        [TestMethod]
        public void FillEmployeesStructure_WithValidData_ShouldBuildCorrectHierarchy()
        {
            // Act
            var structure = _service.FillEmployeesStructure(_testEmployees);

            // Assert
            Assert.IsNotNull(structure);
            var row1 = structure.GetSuperiorRowOfEmployee(2, 1);
            var row2 = structure.GetSuperiorRowOfEmployee(4, 2);
            var row3 = structure.GetSuperiorRowOfEmployee(4, 1);

            Assert.AreEqual(1, row1, "Kamil Nowak powinien mieć Jana Kowalskiego jako przełożonego 1. rzędu");
            Assert.AreEqual(1, row2, "Andrzej Abacki powinien mieć Kamila Nowaka jako przełożonego 1. rzędu");
            Assert.AreEqual(2, row3, "Andrzej Abacki powinien mieć Jana Kowalskiego jako przełożonego 2. rzędu");
        }

        [TestMethod]
        public void FillEmployeesStructure_WithEmptyList_ShouldReturnEmptyStructure()
        {
            // Arrange
            var emptyList = new List<Employee>();

            // Act
            var structure = _service.FillEmployeesStructure(emptyList);

            // Assert
            Assert.IsNotNull(structure);
            var result = structure.GetSuperiorRowOfEmployee(1, 2);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSuperiorRowOfEmployee_DirectSuperior_ShouldReturn1()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(2, 1);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetSuperiorRowOfEmployee_SecondLevelSuperior_ShouldReturn2()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(4, 1);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetSuperiorRowOfEmployee_NotASuperior_ShouldReturnNull()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(4, 3);

            // Assert
            Assert.IsNull(result, "Anna Lewandowska nie jest przełożoną Andrzeja Abackiego");
        }

        [TestMethod]
        public void GetSuperiorRowOfEmployee_SameEmployee_ShouldReturnNull()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(2, 2);

            // Assert
            Assert.IsNull(result, "Pracownik nie może być swoim własnym przełożonym");
        }

        [TestMethod]
        public void GetSuperiorRowOfEmployee_NonExistentEmployee_ShouldReturnNull()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(999, 1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSuperiorRowOfEmployee_NonExistentSuperior_ShouldReturnNull()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(2, 999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSuperiorRowOfEmployee_WithoutInitialization_ShouldThrowException()
        {
            // Arrange
            var context = new Mock<EmployeeContext>();
            var newService = new EmployeeHierarchyService(context.Object);

            // Act - powinno rzucić wyjątek
            newService.GetSuperiorRowOfEmployee(2, 1);
        }

        [TestMethod]
        public void FillEmployeesStructure_WithDeepHierarchy_ShouldCalculateAllLevels()
        {
            // Arrange - tworzymy głęboką hierarchię 5 poziomów
            var deepHierarchy = new List<Employee>
            {
                new Employee { Id = 1, SuperiorId = null },
                new Employee { Id = 2, SuperiorId = 1 },
                new Employee { Id = 3, SuperiorId = 2 },
                new Employee { Id = 4, SuperiorId = 3 },
                new Employee { Id = 5, SuperiorId = 4 }
            };

            // Act
            var structure = _service.FillEmployeesStructure(deepHierarchy);

            // Assert
            Assert.AreEqual(1, structure.GetSuperiorRowOfEmployee(5, 4));
            Assert.AreEqual(2, structure.GetSuperiorRowOfEmployee(5, 3));
            Assert.AreEqual(3, structure.GetSuperiorRowOfEmployee(5, 2));
            Assert.AreEqual(4, structure.GetSuperiorRowOfEmployee(5, 1));
        }

        [TestMethod]
        public void FillEmployeesStructure_WithMultipleBranches_ShouldHandleCorrectly()
        {
            // Arrange - struktura z wieloma gałęziami
            var multiBranch = new List<Employee>
            {
                new Employee { Id = 1, SuperiorId = null },      // CEO
                new Employee { Id = 2, SuperiorId = 1 },         // Manager 1
                new Employee { Id = 3, SuperiorId = 1 },         // Manager 2
                new Employee { Id = 4, SuperiorId = 2 },         // Employee pod Manager 1
                new Employee { Id = 5, SuperiorId = 3 }          // Employee pod Manager 2
            };

            // Act
            var structure = _service.FillEmployeesStructure(multiBranch);

            // Assert
            Assert.AreEqual(1, structure.GetSuperiorRowOfEmployee(4, 2));
            Assert.AreEqual(2, structure.GetSuperiorRowOfEmployee(4, 1));
            Assert.IsNull(structure.GetSuperiorRowOfEmployee(4, 3), "Manager 3 nie jest przełożonym Employee 4");

            Assert.AreEqual(1, structure.GetSuperiorRowOfEmployee(5, 3));
            Assert.AreEqual(2, structure.GetSuperiorRowOfEmployee(5, 1));
            Assert.IsNull(structure.GetSuperiorRowOfEmployee(5, 2), "Manager 2 nie jest przełożonym Employee 5");
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockSet;
        }

    }
}
