using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksEvaluation.Core.Interfaces.IServices
{
    public interface IEmailBodyBuilder
    {
        string GetEmailBody(string templateName, Dictionary<string, string> placeholders);
        // يمكنك إضافة الطريقة BuildEmailBody هنا إذا كانت مطلوبة
    }
}




