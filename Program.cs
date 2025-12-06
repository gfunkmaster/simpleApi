

using SimpleApi.src.Models;
using Services;
using Repositories.Interfaces;
using Repositories;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Data;
using SimpleApi.Repositories; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext> (options => 
    options.UseInMemoryDatabase("SimpleApiDb"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// Alla services ska vara Scoped när de använder EF Core
builder.Services.AddScoped<StudentServices>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseInstanceService>();
builder.Services.AddScoped<GradeService>();

// Alla repositories använder nu EF Core
builder.Services.AddScoped<IStudentRepository, EFStudentRepository>();
builder.Services.AddScoped<ICourseRepository, EFCourseRepository>();
builder.Services.AddScoped<ICourseInstanceRepository, EFCourseInstanceRepository>();
builder.Services.AddScoped<IGradeRepository, EFGradeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();



