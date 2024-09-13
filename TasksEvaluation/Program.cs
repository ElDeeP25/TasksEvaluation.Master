using System.Net;
using System.Net.Mail;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Infrastructure.Helpers;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Infrastructure.Services;
using TasksEvaluation.Infrastructure.Repository;
using TasksEvaluation.Core.Mapper;
using TasksEvaluation.Core.IRepositories;
using TasksEvaluation.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConfig"));

});


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IAssignmentService, AssignmentService>();
builder.Services.AddTransient<ISolutionService, SolutionService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IGradeService, GradeService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IEvaluatedAssignmentService, EvaluatedAssignmentService>();
builder.Services.AddTransient<IGroupService, GroupService>();
builder.Services.AddTransient<IEmailBodyBuilder, EmailBodyBuilder>();



// Register mappers
builder.Services.AddScoped<IBaseMapper<Student, StudentDTO>, BaseMapper<Student, StudentDTO>>();
builder.Services.AddScoped<IBaseMapper<StudentDTO, Student>, BaseMapper<StudentDTO, Student>>();
builder.Services.AddScoped<IBaseMapper<Assignment, AssignmentDTO>, BaseMapper<Assignment, AssignmentDTO>>();
builder.Services.AddScoped<IBaseMapper<AssignmentDTO, Assignment>, BaseMapper<AssignmentDTO, Assignment>>();
builder.Services.AddScoped<IBaseMapper<Solution, SolutionDTO>, BaseMapper<Solution, SolutionDTO>>();
builder.Services.AddScoped<IBaseMapper<SolutionDTO, Solution>, BaseMapper<SolutionDTO, Solution>>();
builder.Services.AddScoped<IBaseMapper<UploadSolutionDTO, SolutionDTO>, BaseMapper<UploadSolutionDTO, SolutionDTO>>();
builder.Services.AddScoped<IBaseMapper<SolutionDTO, UploadSolutionDTO>, BaseMapper<SolutionDTO, UploadSolutionDTO>>();
builder.Services.AddScoped<IBaseMapper<Course, CourseDTO>, BaseMapper<Course, CourseDTO>>();
builder.Services.AddScoped<IBaseMapper<CourseDTO, Course>, BaseMapper<CourseDTO, Course>>();
builder.Services.AddScoped<IBaseMapper<Group, GroupDTO>, BaseMapper<Group, GroupDTO>>();
builder.Services.AddScoped<IBaseMapper<GroupDTO, Group>, BaseMapper<GroupDTO, Group>>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Add Fluent Validation
builder.Services.AddValidatorsFromAssembly(typeof(CourseDTO).Assembly);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using var scope = scopeFactory.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//Seed Data In DataBase 
//Seed Data In DataBase 
await DbInitializer.Seed(app, roleManager, userManager);

app.Run();
