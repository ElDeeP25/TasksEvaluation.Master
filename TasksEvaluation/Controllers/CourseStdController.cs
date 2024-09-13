using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    public class CourseStdController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseStdController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            var courseDTOs = await _courseService.GetCourses();
            return View(courseDTOs);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var courseDTO = await _courseService.GetCourse(id);
            if (courseDTO == null)
            {
                return NotFound();
            }
            return View(courseDTO);
        }
    }
}
