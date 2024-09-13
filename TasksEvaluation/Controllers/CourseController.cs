using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
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

        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.Create(courseDTO);
                    TempData["SuccessMessage"] = "Course created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception
                    ModelState.AddModelError("", $"An error occurred while creating the course: {ex.Message}");
                }
            }
            return View(courseDTO);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var courseDTO = await _courseService.GetCourse(id);
            if (courseDTO == null)
            {
                return NotFound();
            }
            return View(courseDTO);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsCompleted")] CourseDTO courseDTO)
        {
            if (id != courseDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.Update(courseDTO);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseDTO);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var courseDTO = await _courseService.GetCourse(id);
            if (courseDTO == null)
            {
                return NotFound();
            }
            return View(courseDTO);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
