using FluentValidation;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Common;

namespace TaskEvaluation.Web.Validator
{
    public class BaseValidator<T> : AbstractValidator<BaseDTO> 
    {
        public BaseValidator()
        {
            RuleFor(entity => entity.EntryDate).NotNull().WithMessage("Entry date is required");
            RuleFor(entity => entity.UpdateDate).NotNull().WithMessage("Update time is required");
        }
    }
}
