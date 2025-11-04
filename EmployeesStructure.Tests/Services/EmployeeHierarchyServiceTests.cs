using EmployeesStructure.Data.Repositories;
using EmployeesStructure.Models;
using EmployeesStructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesStructure.Tests.Services
{
    [TestClass]
    public class EmployeeHierarchyServiceTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private IEmployeeHierarchyService _service;
        private List<Employee> _testEmployees;

        [TestInitialize]
        public void Setup()
        {
            _testEmployees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Jan Kowalski", SuperiorId = null },
                new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
                new Employee { Id = 3, Name = "Anna Lewandowska", SuperiorId = 1 },
                new Employee { Id = 4, Name = "Andrzej Abacki", SuperiorId = 2 },
                new Employee { Id = 5, Name = "Piotr Wiśniewski", SuperiorId = 2 },
                new Employee { Id = 6, Name = "Maria Dąbrowska", SuperiorId = 3 }
            };

            _employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _service = new EmployeeHierarchyService(_employeeRepositoryMock.Object);
        }

        [TestMethod]
        public void fill_employeesstructure_with_valid_data_should_build_correct_hierarchy()
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
        public void fill_employeesstructure_with_empty_list_should_return_empty_structure()
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
        public void get_superior_row_of_employee_direct_superior_should_return1()
        {
            // Arrange
            _employeeRepositoryMock.Setup(repo => repo.GetAll())
                .Returns(_testEmployees.AsQueryable());
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(2, 1);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void get_superior_row_of_employee_second_level_superior_should_return2()
        {
            // Arrange
            _employeeRepositoryMock.Setup(repo => repo.GetAll())
                .Returns(_testEmployees.AsQueryable());
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(4, 1);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void get_superior_row_of_employee_not_a_superior_should_return_null()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(4, 3);

            // Assert
            Assert.IsNull(result, "Anna Lewandowska nie jest przełożoną Andrzeja Abackiego");
        }

        [TestMethod]
        public void get_superior_row_of_employee_same_employee_should_return_null()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(2, 2);

            // Assert
            Assert.IsNull(result, "Pracownik nie może być swoim własnym przełożonym");
        }

        [TestMethod]
        public void get_superior_row_of_employee_non_existent_employee_should_return_null()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(999, 1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void get_superior_row_of_employee_non_existent_superior_should_return_null()
        {
            // Arrange
            _service.BuildHierarchyFromDatabase();

            // Act
            var result = _service.GetSuperiorRowOfEmployee(2, 999);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void fill_employeesstructure_with_deep_hierarchy_should_calculate_all_levels()
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
        public void fill_employeesstructure_with_multiple_branches_should_handle_correctly()
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
    }
}
