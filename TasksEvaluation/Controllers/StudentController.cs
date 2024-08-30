using Microsoft.AspNetCore.Mvc;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TasksEvaluation.Infrastructure.Services;

namespace TasksEvaluation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public StudentController(IStudentService studentService, IGroupService groupService, IMapper mapper)
        {
            _studentService = studentService;
            _groupService = groupService;
            _mapper = mapper;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetStudents();
            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return View(studentDTOs);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return View(studentDTO);
        }

        // GET: Student/Create
        public async Task<IActionResult> Create()
        {
            var groups = await _groupService.GetGroups();
            ViewBag.Groups = new SelectList(groups, "Id", "Title");
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,MobileNumber,Email,ProfilePic,GroupId")] StudentDTO studentDTO)
        {
            if (ModelState.IsValid)
            {
                await _studentService.Create(studentDTO);
                return RedirectToAction(nameof(Index));
            }

            var groups = await _groupService.GetGroups();
            ViewBag.Groups = new SelectList(groups, "Id", "Title");
            return View(studentDTO);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            var groups = await _groupService.GetGroups();
            ViewBag.Groups = new SelectList(groups, "Id", "Title");
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return View(studentDTO);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,MobileNumber,Email,GroupId,ProfilePic")] StudentDTO studentDTO)
        {
            if (id != studentDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.Update(studentDTO);
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            var groups = await _groupService.GetGroups();
            ViewBag.Groups = new SelectList(groups, "Id", "Title");
            return View(studentDTO);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);
            return View(studentDTO);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
