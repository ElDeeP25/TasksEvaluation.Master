using FluentValidation;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TaskEvaluation.Core.Validation
{
    public class GroupValidator : AbstractValidator<GroupDTO>
    {
        public GroupValidator()
        {
            RuleFor(group => group.Title).NotEmpty().WithMessage("Title is required");


        }
    }
}
