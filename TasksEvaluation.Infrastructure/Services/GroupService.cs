using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Core.Mapper;

namespace TasksEvaluation.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IBaseMapper<Group, GroupDTO> _groupDTOMapper;
        private readonly IBaseMapper<GroupDTO, Group> _groupMapper;
        private readonly IBaseRepository<Group> _groupRepository;

        public GroupService(
            IBaseMapper<Group, GroupDTO> groupDTOMapper,
            IBaseMapper<GroupDTO, Group> groupMapper,
            IBaseRepository<Group> groupRepository)
        {
            _groupDTOMapper = groupDTOMapper;
            _groupMapper = groupMapper;
            _groupRepository = groupRepository;
        }

        public async Task<GroupDTO> Create(GroupDTO model)
        {
            var entity = _groupMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;
            return _groupDTOMapper.MapModel(await _groupRepository.Create(entity));
        }


        public async Task Delete(int id)
        {
            var entity = await _groupRepository.GetById(id);
            await _groupRepository.Delete(entity);
        }

        public async Task<GroupDTO> GetGroup(int id) => _groupDTOMapper.MapModel(await _groupRepository.GetById(id));

        public async Task<IEnumerable<GroupDTO>> GetGroups() => _groupDTOMapper.MapList(await _groupRepository.GetAll());

        public async Task Update(GroupDTO model)
        {
            var existingData = _groupMapper.MapModel(model);
            existingData.UpdateDate = DateTime.Now;
            await _groupRepository.Update(existingData);
        }
    }
}
