using EmployeesStructure.Models;
using System.Collections.Generic;

namespace EmployeesStructure.Services
{
    public interface IEmployeeVacationService
    {
        int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
        bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
    }
}
