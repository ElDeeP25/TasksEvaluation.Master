using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Infrastructure.Data;
using Group = TasksEvaluation.Core.Entities.Business.Group;

namespace TasksEvaluation.Infrastructure.Helpers
{
    public class DbInitializer
    {
        public static async Task Seed(IApplicationBuilder app, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Seed Roles
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Receptionist"));
            }

            // Seed Users
            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    FirstName = "admin",
                    LastName = "user",
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                var user = await userManager.FindByEmailAsync(admin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(admin, "123123123@Zftt");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Seed Courses
            if (!context.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new Course { Id = 1, Title = "Front End", Type = "FrontEnd", ImageUrl = "https://example.com/front-end.jpg" },
                    new Course { Id = 2, Title = "Back End", Type = "BackEnd", ImageUrl = "https://example.com/back-end.jpg" },
                };
                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }

            // Seed Groups
            if (!context.Groups.Any())
            {
                var groups = new List<Group>
                {
                    new Group { Id = 1, Title = "A" },
                    new Group { Id = 2, Title = "B" },
                };
                context.Groups.AddRange(groups);
                await context.SaveChangesAsync();
            }

            // Seed EvaluationGrades
            if (!context.EvaluationGrades.Any())
            {
                var evaluationGrades = new List<EvaluationGrade>
                {
                    new EvaluationGrade { Id = 1, Grade = "Good" },
                    new EvaluationGrade { Id = 2, Grade = "Very Good" },
                    new EvaluationGrade { Id = 3, Grade = "Excellent" }
                };
                context.EvaluationGrades.AddRange(evaluationGrades);
                await context.SaveChangesAsync();
            }

            // Seed Students
            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student { Id = 1, FullName = "Ahmed Mangawy", Email = "Mangawy010@gmail.com", MobileNumber = "01011100222" },
                    new Student { Id = 2, FullName = "Mostafa Metwaly", Email = "MetoElgamed12@gmail.com", MobileNumber = "01224678900" },
                };
                context.Students.AddRange(students);
                await context.SaveChangesAsync();
            }
        }
    }
}
