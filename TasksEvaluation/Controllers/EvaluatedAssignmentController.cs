using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Controllers
{
    public class EvaluatedAssignmentController : Controller
    {
        private readonly IEvaluatedAssignmentService _evaluatedAssignmentService;

        public EvaluatedAssignmentController(IEvaluatedAssignmentService evaluatedAssignmentService)
        {
            _evaluatedAssignmentService = evaluatedAssignmentService;
        }

        public async Task<IActionResult> Index()
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var assignments = await _evaluatedAssignmentService.GetAssignmentsWithsolutions(student.Id);
                return View(assignments);
            }

            return RedirectToAction("Login", "StudentSolution");

        }
        public async Task<IActionResult> Details(int assignmentId)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            if (student == null)
            {
                return RedirectToAction("Login", "StudentSolution");
            }

            var solution = await _evaluatedAssignmentService.GetSolutionWithGrade(assignmentId, student.Id);
            if (solution == null)
            {
                return NotFound(); // Or you could return a custom error view/message
            }

            return View(solution);
        }

    }
}
