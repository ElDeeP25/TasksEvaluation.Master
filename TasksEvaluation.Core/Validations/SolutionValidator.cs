using FluentValidation;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Core.Entities.Business;

namespace TaskEvaluation.Web.EntityConfigs
{
    public class SolutionValidator : AbstractValidator<SolutionDTO>
    {
        public SolutionValidator()
        {
            //RuleFor(s => Path.GetExtension(s.SolutionFile))
            //    .Matches(@"^(.png|.jpg|.jpeg|.zip|.pdf)")
            //    .WithMessage("Solution file must end with .png|.jpg|.jpeg|.zip|.pdf"); ;
            
        }
    }
}
