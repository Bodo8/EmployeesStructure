using EmployeesStructure.Data.Repositories;
using EmployeesStructure.Models;
using EmployeesStructure.Services;
using System;
using System.Linq;
using System.Web.Http;

namespace EmployeesStructure.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeHierarchyService _hierarchyService;
        private readonly IVacationRepository _vacationRepository;
        private readonly IEmployeeVacationService _employeeVacationService;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IEmployeeHierarchyService hierarchyService,
            IVacationRepository vacationRepository,
            IEmployeeVacationService employeeVacationService
            )
        {
            _employeeRepository = employeeRepository;
            _hierarchyService = hierarchyService;
            _vacationRepository = vacationRepository;
            _employeeVacationService = employeeVacationService;
        }

        [HttpGet]
        [Route("api/GetSuperiorRowOfEmployee")]
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
        [Route("api/CreateEmployee")]
        public IHttpActionResult CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var superior = _employeeRepository.GetSuperiorById(employee.SuperiorId ?? 0)
                .Count();

            if (superior != 1)
            {
                return BadRequest($"The specified SuperiorId: {employee.SuperiorId} does not exist.");
            }

            _employeeRepository.Add(employee);
            _employeeRepository.Save();

            _hierarchyService.BuildHierarchyFromDatabase();

            return Ok(employee.Id);
        }

        [HttpGet]
        [Route("api/CountFreeDaysForEmployee")]
        public IHttpActionResult CountFreeDaysForEmployee(int employeeId)
        {
            var employee = _employeeRepository.GetByIdEager(employeeId);

            if (employee == null)
            {
                return BadRequest($"Employee with Id: {employeeId} does not exist.");
            }

            var today = DateTime.Today;
            var vacations = _vacationRepository.GetEmployeeVacationStats(employee.Id, today);
            var freeDays = _employeeVacationService.CountFreeDaysForEmployee(employee, vacations, employee.VacationPackage);

            return Ok(new
            {
                employeeId,
                freeDays
            });
        }

        [HttpGet]
        [Route("api/IfEmployeeCanRequestVacation")]
        public IHttpActionResult IfEmployeeCanRequestVacation(int employeeId)
        {
            var employee = _employeeRepository.GetByIdEager(employeeId);

            if (employee == null)
            {
                return BadRequest($"Employee with Id: {employeeId} does not exist.");
            }

            var today = DateTime.Today;
            var vacations = _vacationRepository.GetEmployeeVacationStats(employee.Id, today);
            bool canRequestVacation = _employeeVacationService.IfEmployeeCanRequestVacation(employee, vacations, employee.VacationPackage);

            return Ok(new
            {
                canRequestVacation
            });
        }
    }
}