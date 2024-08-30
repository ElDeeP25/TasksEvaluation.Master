using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.DTOs;

namespace TasksEvaluation.Core.Interfaces.IServices
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDTO>> GetGroups();
        Task<GroupDTO> GetGroup(int id);
        Task<GroupDTO> Create(GroupDTO model);
        Task Update(GroupDTO model);
        Task Delete(int id);
    }
}
