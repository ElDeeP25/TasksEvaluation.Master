using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Infrastructure.Services
{
    public class GradeService :IGradeService
    {
        private readonly IBaseRepository<EvaluationGrade> _evaluationGradeRepository;

        public GradeService(IBaseRepository<EvaluationGrade> evaluationGradeRepository)
        {
            _evaluationGradeRepository = evaluationGradeRepository;
        }

        public async Task<IEnumerable<EvaluationGrade>> GetGrades()
        {
            return await _evaluationGradeRepository.GetAll();
        }
    }
}
