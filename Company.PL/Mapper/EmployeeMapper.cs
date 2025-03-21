using AutoMapper;
using Company.DAL.Models;
using Company.PL.DTOs;

namespace Company.PL.Mapper
{
    public class EmployeeMapper : Profile
    {

        public EmployeeMapper()
        {
            // source -> target
            // هحول من CreatEmployeeDTO الي Employee
            CreateMap< CreatEmployeeDTO, Employee>().ReverseMap();
        }
    }
}
