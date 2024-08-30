using System.ComponentModel.DataAnnotations;
using TasksEvaluation.Consts;
using TasksEvaluation.Core.Entities.Common;
using TasksEvaluation.Core.Filters;

namespace TasksEvaluation.Core.Entities.Business
{
    public class Assignment : Base
    {
        [Required(ErrorMessage = Errors.RequiredField)]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [DateLaterThanNow]
        public DateTime DeadLine { get; set; }
        
        public int? GroupId { get; set; }
        public Group Group { get; }
        
        public ICollection<Solution> Solutions { get; set; }

    }
}
