using EmployeesStructure.Data.Repositories;
using EmployeesStructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesStructure.Services
{
    public class EmployeeVacationService : IEmployeeVacationService
    {
        private readonly IRepository<Calendar> _calendarRepository;

        public EmployeeVacationService(IRepository<Calendar> calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            if (vacationPackage == null)
            {
                throw new ArgumentNullException(nameof(vacationPackage));
            }

            int grantedDays = vacationPackage.GrantedDays;
            
            if (vacations.Count == 0)
            {
                return grantedDays;
            }

            DateTime today = DateTime.Today;
            var yearStart = new DateTime(today.Year, 1, 1);
            int usedDays = 0;

            foreach (var vacation in vacations)
            {
                usedDays += _calendarRepository.GetAll()
                    .Count(c => c.Date >= vacation.DateSience
                             && c.Date <= vacation.DateUntil
                             && c.Date >= yearStart
                             && !c.IsWeekend
                             && !c.IsHoliday);
            }

            int freeDays = grantedDays - usedDays;

            return freeDays >= 0 ? freeDays : 0;
        }

        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            return CountFreeDaysForEmployee(employee, vacations, vacationPackage) > 0;
        }
    }
}