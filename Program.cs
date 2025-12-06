

using SimpleApi.src.Models;
using Microsoft.AspNetCore.Identity;
using Services;
using Repositories.Interfaces;
using Repositories;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Data;
using SimpleApi.Repositories; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext> (options => 
    options.UseInMemoryDatabase("SimpleApiDb"));
builder.Services.AddDbContext<IdentityAppDbContext>(options =>
    options.UseInMemoryDatabase("SimpleApiDb"));
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;

    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<IdentityAppDbContext>();

builder.Services.AddAuthorization();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();

    var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityAppDbContext>();
    identityDbContext.Database.EnsureCreated();    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapIdentityApi<IdentityUser>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();



