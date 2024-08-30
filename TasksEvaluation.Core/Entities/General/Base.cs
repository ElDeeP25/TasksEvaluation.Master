using System.ComponentModel.DataAnnotations;

namespace TasksEvaluation.Core.Entities.Common
{
    public class Base
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Entry Date")]
        public DateTime? EntryDate { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
