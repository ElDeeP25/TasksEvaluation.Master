using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.IRepositories
{
    public interface IUnitOfWork
    {
        public IBaseRepositories<ApplicationUser> ApplicationUsers { get; }
        public IBaseRepositories<Student> Students { get; }
        public IBaseRepositories<Assignment> Assignments { get; }
        public IBaseRepositories<Solution> Solutions { get; }

        int Complete();
    }
}
