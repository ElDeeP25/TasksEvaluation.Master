using Microsoft.AspNetCore.Hosting;
using TasksEvaluation.Core.Interfaces.IServices;

namespace TasksEvaluation.Infrastructure.Services
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailBodyBuilder(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetEmailBody(string templateName, Dictionary<string, string> placeholders)
        {
            var templatePath = Path.Combine(_webHostEnvironment.WebRootPath, "EmailTemplates", $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template {templateName} not found.");
            }

            var template = File.ReadAllText(templatePath);

            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return template;
        }

        // إذا كان لديك طريقة BuildEmailBody، تأكد من تنفيذها هنا
        public string BuildEmailBody(string templateName, object model)
        {
            throw new NotImplementedException();
        }
    }
}
