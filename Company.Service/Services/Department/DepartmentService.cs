using AutoMapper;
using Company.Repository.Interfaces;
using Company.Service.Interfaces;
using Company.Service.Interfaces.Dto;

namespace Company.Service.Services.Department;

public class DepartmentService : IDepartmentService
{
    // private readonly IDepartmentRepo _departmentRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;    
    public DepartmentService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public DepartmentDto? GetById(int? id)
    {
        if (id is null)
            return null;
        
        var department = _unitOfWork.DepartmentRepo.GetById(id.Value);
        
        DepartmentDto departmentDto = _mapper.Map<DepartmentDto>(department); // DepartmentDto is the destination and department is the source
        
        return departmentDto;
    }

    public IEnumerable<DepartmentDto> GetAll()
    {
        var departments = _unitOfWork.DepartmentRepo.GetAll();
        var mappedDepartments = departments.Select(department => new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            CreateAt = department.CreateAt,
        }).ToList();
        
        
        return mappedDepartments;
    }

    public void Add(DepartmentDto departmentDto)
    {
        var mappedDepartment = new Data.Models.Department
        {
            Name = departmentDto.Name,
            Description = departmentDto.Description ?? string.Empty,
            CreateAt = DateTime.UtcNow,
        };
        _unitOfWork.DepartmentRepo.Add(mappedDepartment);
        _unitOfWork.Complete();
    }

    public void Update(DepartmentDto departmentDto)
    {
        // var mappedDepartment = new Data.Models.Department
        // {
        //     Id = department.Id,
        //     Name = department.Name,
        //     Description = department.Description ?? string.Empty,
        //     CreateAt = DateTime.UtcNow,
        //     Code = department.Code,
        // };
        
         Data.Models.Department mappedDepartment = _mapper.Map<Data.Models.Department>(departmentDto);
        
        _unitOfWork.DepartmentRepo.Update(mappedDepartment);
        
        _unitOfWork.Complete();
    }


    public void Delete(DepartmentDto? departmentDto)
    {
        if (departmentDto != null)
        {
            Data.Models.Department? department = _unitOfWork.DepartmentRepo.GetById(departmentDto.Id);
        
            if (department != null)
            {
                // _unitOfWork.DepartmentRepo.Delete(department);
            
                department.IsDeleted = true;
            
                _unitOfWork.Complete();
            }
        }
    }
}