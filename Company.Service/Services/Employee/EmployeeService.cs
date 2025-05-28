using AutoMapper;
using Company.Repository.Interfaces;
using Company.Service.Helper;
using Company.Service.Interfaces.Employee;
using Company.Service.Interfaces.Employee.Dto;
using Microsoft.Extensions.Logging;

namespace Company.Service.Services.Employee;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IUnitOfWork unitOfWork,IMapper mapper,ILogger<EmployeeService> logger)
    {
        
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public EmployeeDto? GetById(int? id)
    {
        if (id != null)
        {
            var employee =  _unitOfWork.EmployeeRepo.GetById(id.Value);
            if (employee is null)
                return null;
            var mappedEmployee = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                Age = employee.Age,
                Email = employee.Email,
                Address = employee.Address,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                HiringDate = employee.HiringDate,
                ImageUrl = employee.ImageUrl,
            };
        
            return mappedEmployee;
        }

        return null;
    }

    public IEnumerable<EmployeeDto> GetAll()
    {
        var employees = _unitOfWork.EmployeeRepo.GetAll();
        var mappedEmployees = employees.Select(employee => new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            PhoneNumber = employee.PhoneNumber,
            Age = employee.Age,
            Email = employee.Email,
            Address = employee.Address,
            DepartmentId = employee.DepartmentId,
            Salary = employee.Salary,
            HiringDate = employee.HiringDate,
            ImageUrl = employee.ImageUrl,
        }).ToList();
        
        return mappedEmployees;
    }

    public void Add(EmployeeDto employeeDto)
    {
        if (employeeDto == null)
        {
            throw new ArgumentNullException(nameof(employeeDto));
        }

        // Handle image upload
        employeeDto.ImageUrl = employeeDto.Image != null
            ? DocumentSettings.UploudFile(employeeDto.Image, "Images")
            : "/Files/Images/default.jpg";

        var mappedEmployee = _mapper.Map<Data.Models.Employee>(employeeDto);
        _unitOfWork.EmployeeRepo.Add(mappedEmployee);
        _unitOfWork.Complete();
    }

    public void Update(EmployeeDto employeeDto)
    {
        if (employeeDto == null)
        {
            throw new ArgumentNullException(nameof(employeeDto));
        }

        try
        {
            var existingEmployee = _unitOfWork.EmployeeRepo.GetById(employeeDto.Id);
            if (existingEmployee == null)
            {
                throw new InvalidOperationException($"Employee with ID {employeeDto.Id} not found.");
            }

            // Validate DepartmentId
            if (employeeDto.DepartmentId.HasValue)
            {
                var departmentExists = _unitOfWork.DepartmentRepo.GetAll()
                    .Any(d => d.Id == employeeDto.DepartmentId.Value);

                if (!departmentExists)
                {
                    throw new InvalidOperationException($"Invalid DepartmentId: {employeeDto.DepartmentId}");
                }
            }

            // Handle image upload
            if (employeeDto.Image != null)
            {
                try
                {
                    employeeDto.ImageUrl = DocumentSettings.UploudFile(employeeDto.Image, "Images");

                    // Delete old image
                    if (!string.IsNullOrEmpty(existingEmployee.ImageUrl) && existingEmployee.ImageUrl != "/Files/Images/default.jpg")
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingEmployee.ImageUrl.TrimStart('/'));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to upload image: {ex.Message}", ex);
                }
            }
            else
            {
                employeeDto.ImageUrl = existingEmployee.ImageUrl; // Keep existing image
            }

            // 🛠️ Update existing employee manually
            existingEmployee.Name = employeeDto.Name;
            existingEmployee.Email = employeeDto.Email;
            existingEmployee.Age = employeeDto.Age;
            existingEmployee.Salary = employeeDto.Salary;
            existingEmployee.Address = employeeDto.Address;
            existingEmployee.PhoneNumber = employeeDto.PhoneNumber;
            existingEmployee.HiringDate = employeeDto.HiringDate;
            existingEmployee.DepartmentId = employeeDto.DepartmentId;
            existingEmployee.ImageUrl = employeeDto.ImageUrl;

            _unitOfWork.EmployeeRepo.Update(existingEmployee);
            _unitOfWork.Complete();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID {EmployeeId}. Inner Exception: {InnerException}", employeeDto.Id, ex.InnerException?.Message);
            throw;
        }
    }

    public void Delete(EmployeeDto? employeeDto)
    {
        if (employeeDto != null)
        {
            var employee = _unitOfWork.EmployeeRepo.GetById(employeeDto.Id);
        
            if (employee != null)
            {
                employee.IsDeleted = true;
                _unitOfWork.Complete();
            }
        }
    }

    public IEnumerable<EmployeeDto>? GetEmployeeByName(string name)
    {
        // return _unitOfWork.EmployeeRepo.GetEmployeeByName(name);
        
        var employees = _unitOfWork.EmployeeRepo.GetEmployeeByName(name);
        var mappedEmployees = employees.Select(employee => new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            PhoneNumber = employee.PhoneNumber,
            Age = employee.Age,
            Email = employee.Email,
            Address = employee.Address,
            DepartmentId = employee.DepartmentId,
            Salary = employee.Salary,
            HiringDate = employee.HiringDate,
            ImageUrl = employee.ImageUrl,
        }).ToList();

        return mappedEmployees;
    }
}