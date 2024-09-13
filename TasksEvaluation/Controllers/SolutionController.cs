using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SolutionController : Controller
    {
        private readonly ISolutionService _solutionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SolutionController(ISolutionService solutionService, IWebHostEnvironment webHostEnvironment)
        {
            _solutionService = solutionService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Solution
        public async Task<IActionResult> Index()
        {
            var solutions = await _solutionService.GetSolutions(); // Get solutions
            return View(solutions);
        }

        // GET: Solution/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var solution = await _solutionService.GetSolution(id);
            if (solution == null)
            {
                return NotFound();
            }
            return View(solution);
        }

        // GET: Solution/Create
        public async Task<IActionResult> Create()
        {
            // Optionally load additional data here if needed for dropdowns
            ViewBag.Grades = new SelectList(await _solutionService.GetEvaluationGrades(), "Id", "Grade");
            return View();
        }

        // POST: Solution/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SolutionFile, Notes, GradeId, StudentId, AssignmentId")] SolutionDTO solutionDTO)
        {
            if (ModelState.IsValid)
            {
                // Consider using a service method to handle file uploads if needed
                await _solutionService.Create(solutionDTO);
                TempData["SuccessMessage"] = "Solution created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Optionally reload additional data if needed for dropdowns
            ViewBag.Grades = new SelectList(await _solutionService.GetEvaluationGrades(), "Id", "Grade");
            return View(solutionDTO);
        }

        // GET: Solution/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var solution = await _solutionService.GetSolution(id);
            if (solution == null)
            {
                return NotFound();
            }

            // Set grades for dropdown
            var grades = await _solutionService.GetEvaluationGrades();
            ViewBag.Grades = new SelectList(grades, "Id", "Grade", solution.GradeId);

            return View(solution);
        }

        // POST: Solution/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SolutionFile,Id, Notes, GradeId, StudentId, AssignmentId")] SolutionDTO solutionDTO)
        {
            if (id != solutionDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSolution = await _solutionService.GetSolution(id);
                    solutionDTO.StudentId = existingSolution.StudentId;
                    solutionDTO.AssignmentId = existingSolution.AssignmentId;

                    // Handle file upload logic if a new file is provided
                    if (solutionDTO.SolutionFile != null)
                    {
                        // Handle file upload logic here
                    }
                    else
                    {
                        // Maintain the existing file if no new file is provided
                        var existingSolutionFile = existingSolution.SolutionFile;
                        solutionDTO.SolutionFile = existingSolutionFile;
                    }

                    await _solutionService.Update(solutionDTO);
                    TempData["SuccessMessage"] = "Solution updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the solution.";
                    return View(solutionDTO);
                }
            }

            ViewBag.Grades = new SelectList(await _solutionService.GetEvaluationGrades(), "Id", "Grade");
            return View(solutionDTO);
        }

        // GET: Solution/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var solution = await _solutionService.GetSolution(id);
            if (solution == null)
            {
                return NotFound();
            }
            return View(solution);
        }

        // POST: Solution/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _solutionService.DeleteSolution(id);
            TempData["SuccessMessage"] = "Solution deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
