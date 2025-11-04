using EmployeesStructure.Data.Repositories;
using EmployeesStructure.Models;
using EmployeesStructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesStructure.Tests.Services
{
    [TestClass]
    public class EmployeeVacationServiceTests
    {
        private Mock<ICalendarRepository> _calendarRepositoryMock;
        private EmployeeVacationService _service;

        [TestInitialize]
        public void Setup()
        {
            _calendarRepositoryMock = new Mock<ICalendarRepository>();
            _service = new EmployeeVacationService(_calendarRepositoryMock.Object);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _calendarRepositoryMock = null;
            _service = null;
        }

        [TestMethod]
        public void employee_can_request_vacation()
        {
            // Arrange
            var employeeId = 1;
            var employee = CreateEmployee(employeeId, "Jan Kowalski");
            var vacationPackage = CreateVacationPackage(20);
            var year = DateTime.Now.Year;
            var startDate = new DateTime(year, 6, 3);
            var vacations = new List<Vacation>
            {
                CreateVacation(startDate, new DateTime(year, 6, 7), employeeId) // 5 dni
            };

            SetupCalendarMock(5, startDate); // 5 dni roboczych wykorzystanych

            // Act
            var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            // Assert
            Assert.IsTrue(result, "Pracownik z 15 wolnymi dniami (20-5) powinien móc wnioskować o urlop");
        }

        [TestMethod]
        public void employee_cant_request_vacation()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, Name = "Maria Lewandowska" };
            var vacationPackage = CreateVacationPackage(20);
            var year = DateTime.Now.Year;
            var startDate = new DateTime(year, 1, 1);
            SetupCalendarMock(20, startDate);

            // Dokładnie 20 dni wykorzystanych
            var vacations = new List<Vacation>
            {
                CreateVacation (startDate, new DateTime(year, 1, 20), employeeId), // 20 dni
            };

            // Act
            var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            // Assert
            Assert.IsFalse(result, "Pracownik z 0 wolnych dni nie powinien móc wnioskować");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void vacationPackage_with_null_vacationPackage_throws_Exception()
        {
            // Arrange
            Employee employee = null;
            var vacations = new List<Vacation>();
            VacationPackage vacationPackage = null;

            // Act
            _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            // Assert - oczekujemy wyjątku
        }

        [TestMethod]
        public void employee_with_multiple_vacations_can_request_vacation()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, Name = "Maria Lewandowska" };
            var vacationPackage = new VacationPackage { GrantedDays = 26 };
            var year = DateTime.Now.Year;
            var startDate = new DateTime(year, 1, 1);
            var startDate2 = new DateTime(year, 4, 1);

            var vacations = new List<Vacation>
            {
                CreateVacation (startDate, new DateTime(year, 1, 6), employeeId), // 6 dni
                CreateVacation (startDate2, new DateTime(year, 4, 7), employeeId), // 7 dni
            };

            var calendarDays = GenerateCalendarDays(6, startDate);
            calendarDays.AddRange(GenerateCalendarDays(7, startDate2));

            _calendarRepositoryMock.Setup(x => x.GetAllForYear(year)).Returns(calendarDays.AsQueryable());

            // Act
            var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            // Assert
            Assert.IsTrue(result, "Pracownik powinien mieć jeszcze wolne dni");
        }

        [TestMethod]
        public void employee_with_multiple_vacations_should_return_17()
        {
            // Arrange
            var employeeId = 1;
            var employee = new Employee { Id = employeeId, Name = "Maria Lewandowska" };
            var vacationPackage = new VacationPackage { GrantedDays = 26 };
            var year = DateTime.Now.Year;
            var startDate = new DateTime(year - 1, 12, 24);
            var startDate2 = new DateTime(year, 12, 26);

            var vacations = new List<Vacation>
            {
                CreateVacation (startDate, new DateTime(year, 1, 3), employeeId), // 11 dni tylko 3 w roku 2025
                CreateVacation (startDate2, new DateTime(year + 1, 1, 8), employeeId), // 12 dni tylko 6 w roku 2025
            };
            var calendarDays = GenerateCalendarDays(11, startDate);
            calendarDays.AddRange(GenerateCalendarDays(12, startDate2));
            calendarDays = calendarDays
                .Where(d => d.Date.Year == year)
                .ToList();

            _calendarRepositoryMock.Setup(x => x.GetAllForYear(year)).Returns(calendarDays.AsQueryable());

            // Act
            var result = _service.CountFreeDaysForEmployee(employee, vacations, vacationPackage);

            // Assert
            Assert.AreEqual(17, result, "Pracownik powinien mieć jeszcze 13 wolnych dni");
        }


        private Employee CreateEmployee(int id, string name)
        {
            return new Employee
            {
                Id = id,
                Name = name
            };
        }

        private VacationPackage CreateVacationPackage(int grantedDays)
        {
            return new VacationPackage
            {
                GrantedDays = grantedDays
            };
        }

        private Vacation CreateVacation(DateTime dateFrom, DateTime dateTo, int employeeId)
        {
            return new Vacation
            {
                DateSince = dateFrom,
                DateUntil = dateTo,
                EmployeeId = employeeId
            };
        }

        private void SetupCalendarMock(int workingDaysCount, DateTime startDate)
        {
            var calendarDays = GenerateCalendarDays(workingDaysCount, startDate);

            _calendarRepositoryMock
                .Setup(x => x.GetAllForYear(startDate.Year))
                .Returns(calendarDays.AsQueryable());
        }

        private List<Calendar> GenerateCalendarDays(int workingDaysCount, DateTime startDate)
        {
            var calendarDays = new List<Calendar>();

            for (int i = 0; i < workingDaysCount; i++)
            {
                calendarDays.Add(new Calendar
                {
                    Date = startDate.AddDays(i),
                    IsWeekend = false,
                    IsHoliday = false
                });
            }

            return calendarDays;
        }
    }
}
