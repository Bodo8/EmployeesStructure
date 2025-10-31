using System.Collections.Generic;

namespace EmployeesStructure.Models
{
    public class EmployeeStructures
    {
        private Dictionary<int, Dictionary<int, int>> _hierarchyMap;

        public EmployeeStructures()
        {
            _hierarchyMap = new Dictionary<int, Dictionary<int, int>>();
        }

        public void AddRelation(int employeeId, int superiorId, int row)
        {
            if (!_hierarchyMap.ContainsKey(employeeId))
            {
                _hierarchyMap[employeeId] = new Dictionary<int, int>();
            }
            _hierarchyMap[employeeId][superiorId] = row;
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            if (_hierarchyMap.ContainsKey(employeeId) &&
                _hierarchyMap[employeeId].ContainsKey(superiorId))
            {
                return _hierarchyMap[employeeId][superiorId];
            }
            return null;
        }
    }
}