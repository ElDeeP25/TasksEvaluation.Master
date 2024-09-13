using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.DTOs
{
    public class EvaluationGradeDTO : BaseDTO
    {
        public int Id { get; set; }
        public string Grade { get; set; } // تأكد من أن هذه الخاصية متوافقة مع البيانات    }
    }
}
