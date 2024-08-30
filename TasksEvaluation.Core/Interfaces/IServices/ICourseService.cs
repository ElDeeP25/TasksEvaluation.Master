using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;

namespace TasksEvaluation.Core.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetCourses();
        Task<CourseDTO> GetCourse(int id);
        Task<CourseDTO> Create(CourseDTO model);
        Task Update(CourseDTO model);
        Task Delete(int id);
    }
}
