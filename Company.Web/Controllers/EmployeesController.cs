using Company.Service.Interfaces;
using Company.Service.Interfaces.Dto;
using Company.Service.Interfaces.Employee;
using Company.Service.Interfaces.Employee.Dto;
using Microsoft.AspNetCore.Mvc;
using Company.Service.Services.Employee;

namespace Company.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeesController(
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            ILogger<EmployeeService> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index(string searchInp = null)
        {
            try
            {
                ViewBag.SearchInput = searchInp;
                
                var employees = string.IsNullOrEmpty(searchInp) 
                    ? _employeeService.GetAll() 
                    : _employeeService.GetEmployeeByName(searchInp);
                
                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees list");
                TempData["Error"] = "An error occurred while loading employees. Please try again.";
                return View(Array.Empty<EmployeeDto>());
            }
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Employee details requested with null ID");
                return BadRequest("Employee ID is required");
            }

            try
            {
                var employee = _employeeService.GetById(id.Value);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found", id);
                    return NotFound($"Employee with ID {id} not found");
                }

                // Populate ViewBag.Departments for department name lookup
                ViewBag.Departments = _departmentService.GetAll() ?? new List<DepartmentDto>();
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee details for ID {EmployeeId}", id);
                TempData["Error"] = "An error occurred while retrieving employee details.";
                return RedirectToAction(nameof(Index));
            }
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                ViewBag.Departments = _departmentService.GetAll();
                return View(new EmployeeDto { HiringDate = DateTime.Now });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing employee creation form");
                TempData["Error"] = "An error occurred while preparing the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeDto employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest("No employee data provided");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Departments = _departmentService.GetAll(); // important so dropdown list doesn't break
                    return View(employee);
                }
        
                // Image upload is handled in your service, no need to manually handle it here
                _employeeService.Add(employee);
                TempData["Success"] = "Employee created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                ModelState.AddModelError("", "An error occurred while creating the employee.");
                ViewBag.Departments = _departmentService.GetAll();
                return View(employee);
            }
        }
        
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return BadRequest("Employee ID is required");
            }

            try
            {
                var employee = _employeeService.GetById(id);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found");
                }

                ViewBag.Departments = _departmentService.GetAll();
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing employee update form for ID {EmployeeId}", id);
                TempData["Error"] = "An error occurred while preparing the update form.";
                return RedirectToAction(nameof(Index));
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(EmployeeDto employee)
        {
            var shit = employee;
            
            if (employee == null)
            {
                return BadRequest("No employee data provided");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Departments = _departmentService.GetAll();
                    return View(employee);
                }
        
                // No need to handle image upload here if your service handles it
                
                _employeeService.Update(employee);
                TempData["Success"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID {EmployeeId}", employee.Id);
                ModelState.AddModelError("", "An error occurred while updating the employee.");
                ViewBag.Departments = _departmentService.GetAll();
                return View(employee);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Employee ID is required");
            }

            try
            {
                var employee = _employeeService.GetById(id);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found");
                }
                
                _employeeService.Delete(employee);
                TempData["Success"] = "Employee deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee with ID {EmployeeId}", id);
                TempData["Error"] = "An error occurred while deleting the employee.";
                return RedirectToAction(nameof(Index));
            }
        }
        
        [HttpGet]
        public IActionResult SearchEmployees(string searchTerm)
        {
            try
            {
                var employees = string.IsNullOrEmpty(searchTerm)
                    ? _employeeService.GetAll()
                    : _employeeService.GetEmployeeByName(searchTerm);
            
                return PartialView("_EmployeeTable", employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching employees with term: {SearchTerm}", searchTerm);
                return PartialView("_EmployeeTable", Array.Empty<EmployeeDto>());
            }
        }
        
        // Confirm deletion view to prevent accidental deletions
        [HttpGet]
        public IActionResult ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Employee ID is required");
            }

            try
            {
                var employee = _employeeService.GetById(id);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found");
                }
                
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error preparing deletion confirmation for employee ID {EmployeeId}", id);
                TempData["Error"] = "An error occurred while preparing the deletion confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }
        
    }
}
