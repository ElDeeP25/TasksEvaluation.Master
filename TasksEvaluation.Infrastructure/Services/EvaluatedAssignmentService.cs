using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.Mapper;

namespace TasksEvaluation.Infrastructure.Services
{

    public class EvaluatedAssignmentService : IEvaluatedAssignmentService
    {
        private readonly IBaseMapper<Solution, SolutionDTO> _solutionDTOMapper;
        private readonly IBaseMapper<SolutionDTO, Solution> _solutionMapper;
        private readonly IBaseRepository<Solution> _solutionRepository;
        private readonly IBaseMapper<Assignment, AssignmentDTO> _assignmentDTOMapper;
        private readonly IBaseMapper<AssignmentDTO, Assignment> _assignmentMapper;
        private readonly IBaseRepository<Assignment> _assignmentRepository;

        public EvaluatedAssignmentService(
            IBaseMapper<Solution, SolutionDTO> solutionDTOMapper,
            IBaseMapper<SolutionDTO, Solution> solutionMapper,
        IBaseMapper<Assignment, AssignmentDTO> assignmentDTOMapper,
        IBaseRepository<Solution> solutionRepository,
        IBaseMapper<AssignmentDTO, Assignment> assignmentMapper,
            IBaseRepository<Assignment> assignmentRepository)
        {
            _assignmentDTOMapper = assignmentDTOMapper;
            _assignmentMapper = assignmentMapper;
            _assignmentRepository = assignmentRepository;
            _solutionDTOMapper = solutionDTOMapper;
            _solutionMapper = solutionMapper;
            _solutionRepository = solutionRepository;
        }

        public async Task<AssignmentDTO> GetAssignment(int id) => _assignmentDTOMapper.MapModel(await _assignmentRepository.GetById(id));
        public async Task<IEnumerable<AssignmentDTO>> GetAssignments() => _assignmentDTOMapper.MapList(await _assignmentRepository.GetAll());
        public async Task<IEnumerable<AssignmentDTO>> GetAssignmentsWhere(Expression<Func<Assignment, bool>> match) => _assignmentDTOMapper.MapList(await _assignmentRepository.GetAllWhere(match));
        public async Task<IEnumerable<AssignmentDTO>> GetAssignmentsWithsolutions(int studentId)
        {
            var assignments = _assignmentDTOMapper.MapList(await _assignmentRepository.GetAll());
            var solutions = await GetEvaluatedSolutions(studentId);
            var assignmentList = new List<AssignmentDTO>();
            foreach(var sol in solutions)
            {
                if (sol.AssignmentId > 0 && sol.GradeId > 0)
                {
                    assignmentList.Add(await GetAssignment(sol.AssignmentId.Value));
                }
            }
            return assignmentList;
        }

        public async Task<IEnumerable<SolutionStudentDTO>> GetEvaluatedSolutions(int studentId)
        {
            var solutions = await _solutionRepository.FindAll(s => s.GradeId > 0, include: source => source.Include(s => s.Student).Include(s => s.Grade)); // Ensure this fetches all solutions or use a more optimized query
            var sol = solutions.Where(s =>  s.StudentId == studentId).ToList();
            return sol.Select(solution => new SolutionStudentDTO
            {
                Id = solution.Id,
                SolutionFile = solution.SolutionFile,
                Notes = solution.Notes,
                StudentId = solution.StudentId,
                AssignmentId = solution.AssignmentId,
                GradeId = solution.GradeId,
                StudentName = solution.Student?.FullName, // Mapping Student's name
                AssignmentTitle = solution.Assignment?.Title, // Mapping Assignment's title
                GardeName = solution.Grade.Grade,
            }).ToList();
        }

        public async Task<SolutionStudentDTO> GetSolutionWithGrade(int assignmentId, int studentId)
        {
            var solutions = await _solutionRepository.GetAll(); // Ensure this fetches all solutions or use a more optimized query
            var sol = solutions.FirstOrDefault(s => s.AssignmentId == assignmentId && s.StudentId == studentId);

            if (sol == null)
            {
                return null;
            }

            var solution = await _solutionRepository.Find(s => s.Id == sol.Id, include: source => source.Include(s => s.Student).Include(s => s.Assignment).Include(s => s.Grade));
            if (solution == null)
            {
                return null;
            }

            return new SolutionStudentDTO
            {
                Id = solution.Id,
                SolutionFile = solution.SolutionFile,
                Notes = solution.Notes,
                StudentId = solution.StudentId,
                AssignmentId = solution.AssignmentId,
                GradeId = solution.GradeId,
                StudentName = solution.Student?.FullName ?? "Unknown", // Mapping Student's name with null check
                AssignmentTitle = solution.Assignment?.Title ?? "Untitled", // Mapping Assignment's title with null check
                GardeName = solution.Grade?.Grade ?? "Not Graded", // Mapping Grade's name with null check
            };
        }

    }
}
