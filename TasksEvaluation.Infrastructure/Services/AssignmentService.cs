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
    public class AssignmentService : IAssignmentService
    {
        private readonly IBaseMapper<Assignment, AssignmentDTO> _assignmentDTOMapper;
        private readonly IBaseMapper<AssignmentDTO, Assignment> _assignmentMapper;
        private readonly IBaseRepository<Assignment> _assignmentRepository;

        public AssignmentService(
            IBaseMapper<Assignment, AssignmentDTO> assignmentDTOMapper,
            IBaseMapper<AssignmentDTO, Assignment> assignmentMapper,
            IBaseRepository<Assignment> assignmentRepository)
        {
            _assignmentDTOMapper = assignmentDTOMapper;
            _assignmentMapper = assignmentMapper;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<AssignmentDTO> Create(AssignmentDTO model)
        {
            var entity = _assignmentMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _assignmentDTOMapper.MapModel(await _assignmentRepository.Create(entity));
        }

        public async Task Delete(int id)
        {
            var entity = await _assignmentRepository.GetById(id);
            await _assignmentRepository.Delete(entity);
        }

        public async Task<AssignmentDTO> GetAssignment(int id) => _assignmentDTOMapper.MapModel(await _assignmentRepository.GetById(id));

        public async Task<IEnumerable<AssignmentDTO>> GetAssignments() => _assignmentDTOMapper.MapList(await _assignmentRepository.GetAll());
        public async Task<IEnumerable<AssignmentDTO>> GetAssignmentsWhere(Expression<Func<Assignment, bool>> match) => _assignmentDTOMapper.MapList(await _assignmentRepository.GetAllWhere(match));

        public async Task Update(AssignmentDTO model)
        {
            var existingData = _assignmentMapper.MapModel(model);
            existingData.UpdateDate = DateTime.Now;
            await _assignmentRepository.Update(existingData);
        }
    }
}
