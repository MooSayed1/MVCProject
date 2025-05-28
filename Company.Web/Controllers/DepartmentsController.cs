using Company.Service.Interfaces;
using Company.Service.Interfaces.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    // GET: /Departments
    public IActionResult Index()
    {
        var departments = _departmentService.GetAll();
        return View(departments);
    }

    // GET: /Departments/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Departments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DepartmentDto department)
    {
        if (!ModelState.IsValid)
            return View(department);

        _departmentService.Add(department);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Departments/Details/5
    public IActionResult Details(int? id)
    {
        if (id is null)
            return RedirectToAction(nameof(Error404));

        var department = _departmentService.GetById(id);
        if (department is null)
            return RedirectToAction(nameof(Error404));

        return View(department);
    }

    // GET: /Departments/Update/5
    [HttpGet]
    public IActionResult Update(int? id)
    {
        if (id is null)
            return RedirectToAction(nameof(Error404));

        var department = _departmentService.GetById(id);
        if (department is null)
            return RedirectToAction(nameof(Error404));

        return View(department);
    }

    // POST: /Departments/Update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(DepartmentDto departmentDto)
    {
        if (!ModelState.IsValid)
            return View(departmentDto);

        if (departmentDto.Code is null)
        {
            ModelState.AddModelError(nameof(departmentDto.Code), "Code is required.");
            return View(departmentDto);
        }

        _departmentService.Update(departmentDto);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Departments/Delete/5
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id is null)
            return RedirectToAction(nameof(Error404));

        var department = _departmentService.GetById(id);
        if (department is null)
            return RedirectToAction(nameof(Error404));

        return View(department);
    }

    // POST: /Departments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmDelete(int id)
    {
        var department = _departmentService.GetById(id);
        if (department is null)
            return RedirectToAction(nameof(Error404));

        _departmentService.Delete(department);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Departments/Error404
    public IActionResult Error404()
    {
        return View();
    }
}
