using EmployeesStructure.Models;
using System.Collections.Generic;

namespace EmployeesStructure.Services
{
    public interface IEmployeeHierarchyService
    {
        EmployeeStructures FillEmployeesStructure(List<Employee> employees);
        int? GetSuperiorRowOfEmployee(int employeeId, int superiorId);
        EmployeeStructures BuildHierarchyFromDatabase();
    }
}