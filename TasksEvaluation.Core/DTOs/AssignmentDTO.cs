using System;
using System.ComponentModel.DataAnnotations;
using TasksEvaluation.Core.Filters;

namespace TasksEvaluation.Core.DTOs
{
    public class AssignmentDTO : BaseDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        
        public string Description { get; set; }

        [DateLaterThanNow]
        public DateTime DeadLine { get; set; }
        public int? GroupId { get; set; }
    }
}
