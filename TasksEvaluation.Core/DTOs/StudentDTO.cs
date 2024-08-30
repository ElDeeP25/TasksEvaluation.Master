using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.DTOs
{
    public class StudentDTO : BaseDTO
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int? GroupId { get; set; }
       
        public string ProfilePic { get; set; }
    }
}
