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
    public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentDTO>> GetAssignments();
        Task<IEnumerable<AssignmentDTO>> GetAssignmentsWhere(Expression<Func<Assignment, bool>> match);
        Task<AssignmentDTO> GetAssignment(int id);
        Task<AssignmentDTO> Create(AssignmentDTO model);
        Task Update(AssignmentDTO model);
        Task Delete(int id);
    }
}
