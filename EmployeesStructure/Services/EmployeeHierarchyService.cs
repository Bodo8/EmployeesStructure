using EmployeesStructure.Data;
using EmployeesStructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesStructure.Services
{
    public class EmployeeHierarchyService : IEmployeeHierarchyService
    {
        private readonly EmployeeContext _context;
        private EmployeeStructures _employeeStructures;

        public EmployeeHierarchyService(EmployeeContext context)
        {
            _context = context;
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            if (_employeeStructures == null)
            {
                BuildHierarchyFromDatabase();
            }

            return _employeeStructures.GetSuperiorRowOfEmployee(employeeId, superiorId);
        }

        public EmployeeStructures BuildHierarchyFromDatabase()
        {
            var employees = _context.Employees
                .ToList();
            _employeeStructures = FillEmployeesStructure(employees);

            return _employeeStructures;
        }

        public EmployeeStructures FillEmployeesStructure(List<Employee> employees)
        {
            var structure = new EmployeeStructures();
            var employeeDict = employees.ToDictionary(e => e.Id, e => e);

            foreach (var employee in employees)
            {
                TraverseHierarchy(employee, employeeDict, structure);
            }

            return structure;
        }

        private void TraverseHierarchy(
            Employee employee,
            Dictionary<int, Employee> employeeDict,
            EmployeeStructures structure)
        {
            int currentRow = 1;
            int? currentSuperiorId = employee.SuperiorId;

            while (currentSuperiorId.HasValue &&
                   employeeDict.ContainsKey(currentSuperiorId.Value))
            {
                structure.AddRelation(employee.Id, currentSuperiorId.Value, currentRow);

                var superior = employeeDict[currentSuperiorId.Value];
                currentSuperiorId = superior.SuperiorId;
                currentRow++;
            }
        }

    }
}