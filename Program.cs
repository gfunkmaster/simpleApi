

using SimpleApi.src.Models;
using Microsoft.AspNetCore.Identity;
using Services;
using Repositories.Interfaces;
using Repositories;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Data;
using SimpleApi.Repositories; 
using SimpleApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext> (options => 
    options.UseInMemoryDatabase("SimpleApiDb"));
builder.Services.AddDbContext<IdentityAppDbContext>(options =>
    options.UseInMemoryDatabase("SimpleApiDb"));
builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SimpleApi - Student Management System",
        Version = "v1",
        Description = "API för att hantera studenter, kurser, kursinstanser och betyg med JWT autentisering",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "SimpleApi Team",
            Email = "support@simpleapi.com"
        }
    });

    // JWT Authentication för Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Inkludera XML-kommentarer
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});
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


// Logging middleware for Övning 1
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}");
    await next.Invoke();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});

// Skip HTTPS redirect in development to avoid warnings
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseActiveUserCheck();
app.UseAuthorization();
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();




app.Run();



