using FluentValidation;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TaskEvaluation.Web.EntityConfigs
{
    public class EvaluationGradeValidator : AbstractValidator<EvaluationGradeDTO>
    {
        public EvaluationGradeValidator()
        {
            RuleFor(grade => grade.Grade).NotEmpty().WithMessage("Grade is required");

        }
    }
}
