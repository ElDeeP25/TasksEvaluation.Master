using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.DTOs
{
    public class CourseDTO : BaseDTO
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
