using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                ApplicationDbContext context = app.ApplicationServices
                    .CreateScope()
                    .ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("User"));
                    await roleManager.CreateAsync(new IdentityRole("Reciptionist"));
                }
                if (!userManager.Users.Any())
                {
                    ApplicationUser admin = new()
                    {
                        FirstName = "admin",
                        LastName = "user",
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                      
                        PasswordHash = "123123123@Zftt"
                    };

                    var user = await userManager.FindByEmailAsync(admin.Email);
                    if (user is null)
                    {
                        await userManager.CreateAsync(admin, "123123123@Zftt");
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            if (!context.Courses.Any())
            {
                var courses = new List<Course>()
                {
                    new Course{Id=1, Title="Front End",},
                    new Course{Id=2, Title="Back End",},

                };
                foreach (var course in courses)
                {
                    context.Courses.Add(course);
                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Courses ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Courses OFF;");
                    context.Database.CloseConnection();
                }
            }
            if (!context.Groups.Any())
            {
                var groups = new List<Group>()
                {
                    new Group{Id=1, Title="A",},
                    new Group{Id=2, Title="B",},

                };
                foreach (var group in groups)
                {
                    context.Groups.Add(group);
                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Groups ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Groups OFF;");
                    context.Database.CloseConnection();
                }
            }
            if (!context.EvaluationGrades.Any())
            {
                var evaluationGrades = new List<EvaluationGrade>()
                {
                    new EvaluationGrade{Id=1, Grade="Good",},
                    new EvaluationGrade{Id=2, Grade="Very Good",},
                    new EvaluationGrade{Id=3, Grade="Excellent",}


                };
                foreach (var evaluationGrade in evaluationGrades)
                {
                    context.EvaluationGrades.Add(evaluationGrade);
                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.EvaluationGrades ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.EvaluationGrades OFF;");
                    context.Database.CloseConnection();
                }
            }
            if (!context.Students.Any())
            {
                var students = new List<Student>()
                {
                    new Student{Id=1, FullName="Ahmed Mangawy", Email="Mangawy010@gmail.com" , MobileNumber="01011100222"},
                    new Student{Id=2, FullName="Mostafa Metwaly", Email="MetoElgamed12@gmail.com" , MobileNumber="01224678900"},

                };

                foreach (var student in students)
                {
                    context.Students.Add(student);
                    context.Database.OpenConnection();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Students ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Students OFF;");
                    context.Database.CloseConnection();
                }
            }
        }

        }
    }

