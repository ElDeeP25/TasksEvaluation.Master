using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Interfaces.IServices
{
    public interface IEvaluatedAssignmentService
    {
        Task<IEnumerable<AssignmentDTO>> GetAssignments();
        Task<IEnumerable<AssignmentDTO>> GetAssignmentsWhere(Expression<Func<Assignment, bool>> match);
        Task<AssignmentDTO> GetAssignment(int id);
        Task<IEnumerable<AssignmentDTO>> GetAssignmentsWithsolutions(int studentId);
        Task<IEnumerable<SolutionStudentDTO>> GetEvaluatedSolutions(int studentId);
        Task<SolutionStudentDTO> GetSolutionWithGrade(int assignmentId, int studentId);
    }
}
