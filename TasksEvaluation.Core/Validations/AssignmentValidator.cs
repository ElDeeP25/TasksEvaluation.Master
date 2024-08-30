using FluentValidation;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Validations
{
    public class AssignmentValidator : AbstractValidator<AssignmentDTO>
    {
        public AssignmentValidator()
        {
            RuleFor(task => task.Title).NotNull().NotEmpty().WithMessage("Title is required");
            RuleFor(task => task.Description).NotNull().NotEmpty().WithMessage("Description is required");
            RuleFor(task => task.DeadLine).NotNull().NotEmpty().GreaterThanOrEqualTo(DateTime.Now).WithMessage("Deadline is required"); //
        }
    }
}
