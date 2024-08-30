using FluentValidation;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TaskEvaluation.Web.EntityConfigs
{
    public class StudentValidator : AbstractValidator<StudentDTO>
    {
        public StudentValidator()
        {
            RuleFor(student => student.FullName).NotEmpty().WithMessage("Full name is required");
            RuleFor(student => student.MobileNumber).NotEmpty().Matches("^(00201|\\+201|01)[0-2,5]{1}[0-9]{8}$").WithMessage("Mobile number is required"); //
            RuleFor(student => student.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format");
        }
    }
}
