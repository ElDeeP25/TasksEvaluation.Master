using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Assignment, AssignmentDTO>().ReverseMap();
            CreateMap<RegisterDTO, ApplicationUser>().ReverseMap();
            CreateMap<SolutionDTO, Solution>().ReverseMap();
            CreateMap<SolutionDTO, UploadSolutionDTO>().ReverseMap();
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Group, GroupDTO>().ReverseMap();


            // Add other mappings here
        }
    }
}
