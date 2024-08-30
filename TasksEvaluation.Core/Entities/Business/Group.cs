using System.ComponentModel.DataAnnotations;
using TasksEvaluation.Consts;
using TasksEvaluation.Core.Entities.Common;

namespace TasksEvaluation.Core.Entities.Business
{
    public class Group : Base
    {
        [Required(ErrorMessage = Errors.RequiredField)]
        public string Title { get; set; }
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
