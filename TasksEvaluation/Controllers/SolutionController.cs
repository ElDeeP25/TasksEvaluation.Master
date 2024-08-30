using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SolutionController : Controller
    {
        private readonly ISolutionService _solutionService;

        public SolutionController(ISolutionService solutionService)
        {
            _solutionService = solutionService;
        }

        // GET: Solution
        public async Task<IActionResult> Index()
        {
            var solutions = await _solutionService.GetSolutions();
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

            // Optionally load additional data here if needed for dropdowns
            return View(solution);
        }

        // POST: Solution/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, SolutionFile, Notes, GradeId, StudentId, AssignmentId")] SolutionDTO solutionDTO)
        {
            if (id != solutionDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _solutionService.Update(solutionDTO);
                TempData["SuccessMessage"] = "Solution updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Optionally reload additional data if needed for dropdowns
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
