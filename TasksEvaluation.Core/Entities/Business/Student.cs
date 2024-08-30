using System.ComponentModel.DataAnnotations;
using TasksEvaluation.Consts;
using TasksEvaluation.Core.Entities.Common;

namespace TasksEvaluation.Core.Entities.Business
{
    public class Student : Base
    {
        [Required(ErrorMessage = Errors.RequiredField)]
        [MaxLength(100, ErrorMessage = Errors.MaxLength100char)]
        public string FullName { get; set; }

        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Mobile number must be 11 digits")]
        public string MobileNumber { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(100, ErrorMessage = Errors.MaxLength100char)]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePic { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<Solution> Solutions { get; set; }
    }
}
