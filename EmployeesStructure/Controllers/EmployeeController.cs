using EmployeesStructure.Data;
using EmployeesStructure.Models;
using EmployeesStructure.Services;
using System.Linq;
using System.Web.Http;

namespace EmployeesStructure.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly EmployeeContext _context;
        private readonly IEmployeeHierarchyService _hierarchyService;

        public EmployeeController()
        {
            _context = new EmployeeContext();
            _hierarchyService = new EmployeeHierarchyService(_context);
        }

        [HttpGet]
        [Route("hierarchy/GetSuperiorRowOfEmployee")]
        public IHttpActionResult GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            if (employeeId == 0 || superiorId == 0 || employeeId == superiorId)
            {
                return BadRequest("Zero or same employeeId and superiorId");
            }

            var row = _hierarchyService.GetSuperiorRowOfEmployee(employeeId, superiorId);

            return Ok(new
            {
                employeeId,
                superiorId,
                row,
                isSuperior = row.HasValue
            });
        }

        [HttpPost]
        [Route("CreateEmployee")]
        public IHttpActionResult CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var superior = _context.Employees.FirstOrDefault(e => e.Id == employee.SuperiorId);

            if (superior == null)
            {
                return BadRequest($"The specified SuperiorId: {employee.SuperiorId} does not exist.");
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();

            _hierarchyService.BuildHierarchyFromDatabase();

            return Ok(employee.Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}