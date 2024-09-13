using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.IRepositories;

namespace TasksEvaluation.Controllers
{
    public class StudentSolutionController : Controller
    {
        private readonly ISolutionService _solutionService;
        private readonly IMapper _mapper;
        private readonly IAssignmentService _assignmentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Student> _studentBaseRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentSolutionController(IMapper mapper, IBaseRepository<Student> studentBaseRepository, IUnitOfWork unitOfWork, IAssignmentService assignmentService, ISolutionService solutionService, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _studentBaseRepository = studentBaseRepository;
            _unitOfWork = unitOfWork;
            _assignmentService = assignmentService;
            _solutionService = solutionService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.SetString("Student", JsonConvert.SerializeObject(null));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(StudentLogInDTO model)
        {
            if (ModelState.IsValid)
            {
                var students = await _unitOfWork.Students.GetAll();

                if (students.Any(s => s.Email == model.Email))
                {
                    var student = await _studentBaseRepository.GetByEmail(model.Email);
                    HttpContext.Session.SetString("Student", JsonConvert.SerializeObject(student));
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var assignments = await _assignmentService.GetAssignmentsWhere(assignment => assignment.GroupId == student.GroupId.Value);

                return View(assignments);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult UploadSolution(int assignmentId)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var model = new SolutionDTO
                {
                    AssignmentId = assignmentId,
                    StudentId = student.Id
                };
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> UploadSolution(UploadSolutionDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _solutionService.UploadSolution(model);

                if (!string.IsNullOrEmpty(result.Notes))
                {
                    ModelState.AddModelError(string.Empty, result.Notes);
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> EditSolution(int id)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var solution = await _solutionService.GetSolution(id);
                if (solution != null)
                {
                    return View(solution);
                }
                return NotFound();
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> EditSolution(UploadSolutionDTO model)
        {
            if (ModelState.IsValid)
            {
                await _solutionService.Update(model);



                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSolution(int id)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var solution = await _solutionService.GetSolution(id);
                if (solution != null)
                {
                    return View(solution);
                }
                return NotFound();
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpPost, ActionName("DeleteSolution")]
        public async Task<IActionResult> DeleteSolutionConfirmed(int id)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var solution = await _solutionService.GetSolution(id);
                if (solution != null)
                {
                    var filePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/pdfs", solution.SolutionFile);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    await _solutionService.DeleteSolution(id);
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Login));
        }



        public async Task<IActionResult> Details(int assignmentId)
        {
            var studentJson = HttpContext.Session.GetString("Student");
            var student = studentJson == null ? null : JsonConvert.DeserializeObject<Student>(studentJson);
            ViewBag.Student = student;

            if (student != null)
            {
                var assignment = await _assignmentService.GetAssignment(assignmentId);
                if (assignment == null)
                {
                    return NotFound();
                }

                var solution = await _solutionService.GetSolution(assignmentId, student.Id);
                ViewBag.Solution = solution;

                var model = new AssignmentDTO
                {
                    Id = assignment.Id,
                    Title = assignment.Title,
                    Description = assignment.Description,
                    DeadLine = assignment.DeadLine,
                };

                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }


    }
}
