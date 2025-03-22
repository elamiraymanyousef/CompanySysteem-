using System.Data;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.DTOs;
using Company.PL.HelperImage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Infrastructure;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        // ================= oldway=================
        //private readonly IEmployeeRepository _employeeRepository;

        // ================== new way =================
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IMapper mapper ,IUnitOfWork unitOfWork    /*,IEmployeeRepository employeeRepository*/  /*IDepartmentRepository departmentRepository*/)
        {
            //_employeeRepository = employeeRepository;

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Index(string? SearchName)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchName))
            {
                 employees =await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
                // for search by name
                employees =await _unitOfWork.employeeRepository.GetByNameAsync(SearchName);
            }
            

            return View(employees);
        }



        [HttpGet]
        public IActionResult Create(/*[FromServices] IDepartmentRepository _departmentRepositor*/)
        {

            //ViewData["Department"] = await_departmentRepository.GetAllAsync();
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreatEmployeeDTO creatEmployeeDTO)
        {
            if (ModelState.IsValid)
            {
                #region Manual Maping 
                //var employee = new Employee()
                //{
                //    Name = creatEmployeeDTO.Name,
                //    Email = creatEmployeeDTO.Email,
                //    Address = creatEmployeeDTO.Address,
                //    Salary = creatEmployeeDTO.Salary,
                //    HiringDate = creatEmployeeDTO.HiringDate,
                //    IsACtive = creatEmployeeDTO.IsACtive,
                //    IsDeleted = creatEmployeeDTO.IsDeleted,
                //    Phone = creatEmployeeDTO.Phone,
                //    Age = creatEmployeeDTO.Age,
                //    DepartmentId = creatEmployeeDTO.DepartmentId
                //}; 
                #endregion

                if(creatEmployeeDTO.Image is not null)
                {
                    // save image
                    creatEmployeeDTO.ImageName= DecumentSettings.UploadImage(creatEmployeeDTO.Image, "Images");
                }
                // =================== Using AutoMapper ===================

                var employee = _mapper.Map<Employee>(creatEmployeeDTO);
               
                await _unitOfWork.employeeRepository.AddAsync(employee);
                int count =await _unitOfWork.completeAsync();
                
                // ========================================================

                if (count > 0)
                {
                    TempData["Message"] = "Employee Added Successfully";
                    return RedirectToAction("Index");
                }
            }
            return View(creatEmployeeDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewStat = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee =await _unitOfWork.employeeRepository.GetAsync(id.Value);
            if (employee is null)
                return NotFound(new { StatusCode = 400, Message = $"Employee with id : {id} not found" });
            return View(viewStat, employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int? id ,[FromServices] IDepartmentRepository _departmentRepository)
        {
            
            ViewData["Department"] =await _departmentRepository.GetAllAsync();
            //return await Details(id, "Edit");
            if (id is null)
                return BadRequest();
            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);
            if (employee is null)
                return NotFound(new { StatusCode = 400, Message = $"Employee with id : {id} not found" });
            var employeeDTO = _mapper.Map<CreatEmployeeDTO>(employee);

            return View(employeeDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, CreatEmployeeDTO employeeDTO)
        {
            if (ModelState.IsValid)
            {
                #region Manual Mping
                //var employee = new Employee()
                //{
                //    Name = employeeDTO.Name,
                //    Email = employeeDTO.Email,
                //    Address = employeeDTO.Address,
                //    Salary = employeeDTO.Salary,
                //    HiringDate = employeeDTO.HiringDate,
                //    IsACtive = employeeDTO.IsACtive,
                //    IsDeleted = employeeDTO.IsDeleted,
                //    Phone = employeeDTO.Phone,
                //    Age = employeeDTO.Age,
                //    DepartmentId = employeeDTO.DepartmentId
                //}; 
                #endregion

                // if image is not null and image name is not null
                if (employeeDTO.ImageName  is not null && employeeDTO.Image is not null)     
                {
                    // delet image
                     DecumentSettings.DeleteImage("Images", employeeDTO.ImageName);
                }

                if (employeeDTO.Image is not null)
                {
                    // save image
                    employeeDTO.ImageName = DecumentSettings.UploadImage(employeeDTO.Image, "Images");
                }
                //=================== Using AutoMapper ===================
                var employee = _mapper.Map<Employee>(employeeDTO);
                employee.Id = id.Value;

                 _unitOfWork.employeeRepository.Update(employee);

                int count =await _unitOfWork.completeAsync();

                if (count > 0)
                {
                    TempData["Message"] = "Employee Updated Successfully";
                    return RedirectToAction("Index");
                }
                return View(employee);
            }
            return View(employeeDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }








        #region Old Delete
        //[HttpPost]
        //public async Task<IActionResult> Delete([FromRoute] int? id, CreatEmployeeDTO employeeDTO)
        //{
        //    if (!ModelState.IsValid) return BadRequest();

        //    #region Manual Maping 
        //    //var employee = new Employee()
        //    //{
        //    //    Id = id.Value,
        //    //    Name = employeeDTO.Name,
        //    //    Email = employeeDTO.Email,
        //    //    Address = employeeDTO.Address,
        //    //    Salary = employeeDTO.Salary,
        //    //    HiringDate = employeeDTO.HiringDate,
        //    //    IsACtive = employeeDTO.IsACtive,
        //    //    IsDeleted = employeeDTO.IsDeleted,
        //    //    Phone = employeeDTO.Phone,
        //    //    Age = employeeDTO.Age
        //    //};

        //    #endregion
        //    //======================== outoMapper ========================
        //    var employee = _mapper.Map<Employee>(employeeDTO);
        //    employee.Id = id.Value;
        //    _unitOfWork.employeeRepository.Delete(employee);
        //    int count =await _unitOfWork.completeAsync();
        //    if (count > 0)
        //    {
        //        DecumentSettings.DeleteImage("Images", employeeDTO.ImageName);
        //        TempData["Message"] = "Employee Deleted Successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(employeeDTO);
        //} 
        #endregion


        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, CreatEmployeeDTO employeeDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            

            //======================== outoMapper ========================
            var employee = _mapper.Map<Employee>(employeeDTO);
            employee.Id = id.Value;
            _unitOfWork.employeeRepository.Delete(employee);
            int count = await _unitOfWork.completeAsync();
            if (count > 0)
            {
                DecumentSettings.DeleteImage("Images", employeeDTO.ImageName);
                TempData["Message"] = "Employee Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeDTO);
        }
    }
}
