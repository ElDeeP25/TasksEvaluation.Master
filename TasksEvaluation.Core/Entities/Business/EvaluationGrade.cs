using System.ComponentModel.DataAnnotations;
using TasksEvaluation.Consts;
using TasksEvaluation.Core.Entities.Common;

namespace TasksEvaluation.Core.Entities.Business
{
    public class EvaluationGrade : Base
    {

        [Required(ErrorMessage = Errors.RequiredField)]
        public string Grade { get; set; }
        public ICollection<Solution> Solutions { get; set; }
    }
}
