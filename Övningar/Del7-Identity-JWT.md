# Del 7 - Identity och Autentisering med JWT

## 🎯 Mål
- Förstå vad ASP.NET Identity är och varför det används
- Kunna registrera och logga in användare
- Förstå hur JWT fungerar och hur man skyddar API:er med det
- Implementera roller och auktorisering

## 📚 Teori

### Vad är Identity i ASP.NET?
ASP.NET Core Identity är ett medlemssystem som lägger till inloggningsfunktionalitet till ASP.NET Core-appar. Det hanterar:
- Användarregistrering och inloggning
- Lösenordshantering och hashning
- Rollhantering och claims
- Tvåfaktorsautentisering
- Säkerhetstoken (JWT)

### Autentisering vs Auktorisering
- **Autentisering (Authentication)**: *Vem är du?* - Verifierar användarens identitet
- **Auktorisering (Authorization)**: *Vad får du göra?* - Kontrollerar behörigheter

### JWT (JSON Web Tokens)
JWT är en säker metod för att överföra information mellan parter. Består av tre delar:
- **Header**: Typ av token och signeringsalgoritm
- **Payload**: Claims (användarinfo, roller, etc.)
- **Signature**: Verifierar att token inte har ändrats

## ✅ Övning 1 – Introduktion till Identity

### Installera NuGet-paket
```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

### Uppdatera ApplicationDbContext
Ändra så att den ärver från `IdentityDbContext<IdentityUser>` istället för bara `DbContext`:

```csharp
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseInstance> CourseInstances { get; set; }
    public DbSet<Grade> Grades { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    // Existing OnModelCreating method...
}
```

### Registrera Identity i Program.cs
```csharp
// Lägg till efter AddDbContext
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    // Lösenordsregler
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    
    // Användarregler
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Lägg till före var app = builder.Build();
```

### Aktivera Identity endpoints
```csharp
// Lägg till efter app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();
```

## ✅ Övning 2 – Registrera användare

### Automatiska endpoints som skapas:
- `POST /register` - Registrera ny användare
- `POST /login` - Logga in användare
- `POST /refresh` - Förnya access token
- `GET /confirmEmail` - Bekräfta e-post
- `POST /resendConfirmationEmail` - Skicka bekräftelse igen
- `POST /forgotPassword` - Glömt lösenord
- `POST /resetPassword` - Återställ lösenord

### Test i HTTP-fil:
```http
### Register new user
POST {{SimpleApi_HostAddress}}/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}

### Register with weak password (should fail)
POST {{SimpleApi_HostAddress}}/register
Content-Type: application/json

{
  "email": "user2@example.com",
  "password": "123"
}

### Try to register same email twice (should fail)
POST {{SimpleApi_HostAddress}}/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "AnotherPassword123!"
}
```

## ✅ Övning 3 – Logga in användare och skapa JWT

### Login endpoint:
```http
### Login user
POST {{SimpleApi_HostAddress}}/login?useCookies=false
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}

### Login with wrong password (should fail)
POST {{SimpleApi_HostAddress}}/login?useCookies=false
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "WrongPassword"
}
```

### Förväntat svar vid lyckad inloggning:
```json
{
  "tokenType": "Bearer",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "refreshToken": "..."
}
```

## ✅ Övning 4 – Skydda endpoints med [Authorize]

### Aktivera JWT-autentisering i Program.cs
```csharp
// Lägg till efter AddIdentityApiEndpoints
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder();
```

### Skydda controller med [Authorize]
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Kräver autentisering för alla endpoints
public class StudentsController : ControllerBase
{
    // Alla metoder kräver nu inloggning
}
```

### Test med och utan token:
```http
### Try to access protected endpoint without token (should return 401)
GET {{SimpleApi_HostAddress}}/api/Students
Accept: application/json

### Access protected endpoint with valid token (should work)
GET {{SimpleApi_HostAddress}}/api/Students
Accept: application/json
Authorization: Bearer {{accessToken}}
```

### Tillåt anonyma anrop med [AllowAnonymous]
```csharp
[HttpGet]
[AllowAnonymous] // Tillåter anrop utan inloggning
public ActionResult<List<Student>> GetAllStudents()
{
    return Ok(_studentService.GetAll());
}
```

## ✅ Övning 5 – Förstå claims

### Hämta användarinfo från JWT i controller:
```csharp
[HttpGet("profile")]
[Authorize]
public ActionResult GetUserProfile()
{
    var userId = User.FindFirst("sub")?.Value; // User ID
    var email = User.FindFirst("email")?.Value; // Email
    var userName = User.Identity?.Name; // Username
    
    return Ok(new {
        UserId = userId,
        Email = email,
        UserName = userName,
        IsAuthenticated = User.Identity?.IsAuthenticated ?? false
    });
}
```

### Test användarinfo:
```http
### Get current user profile
GET {{SimpleApi_HostAddress}}/api/Students/profile
Accept: application/json
Authorization: Bearer {{accessToken}}
```

## ✅ Övning 6 – Bonus: Roller och auktorisering

### Skapa roller i Program.cs (efter app.Build())
```csharp
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
    // Skapa roller
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    
    if (!await roleManager.RoleExistsAsync("Student"))
        await roleManager.CreateAsync(new IdentityRole("Student"));
    
    // Skapa admin-användare
    var adminEmail = "admin@example.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser 
        { 
            UserName = adminEmail, 
            Email = adminEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "AdminPassword123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
```

### Skapa endpoint som kräver Admin-roll:
```csharp
[HttpGet("admin-only")]
[Authorize(Roles = "Admin")]
public ActionResult GetAdminData()
{
    return Ok(new { 
        Message = "This is admin-only data!",
        AdminUser = User.Identity?.Name
    });
}

[HttpPost("assign-role")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult> AssignRole([FromBody] AssignRoleRequest request)
{
    var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
    var user = await userManager.FindByEmailAsync(request.Email);
    
    if (user == null)
        return NotFound("User not found");
        
    await userManager.AddToRoleAsync(user, request.Role);
    return Ok($"Role {request.Role} assigned to {request.Email}");
}

public class AssignRoleRequest
{
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}
```

### Test rollbaserad auktorisering:
```http
### Try admin endpoint as regular user (should return 403)
GET {{SimpleApi_HostAddress}}/api/Students/admin-only
Accept: application/json
Authorization: Bearer {{studentToken}}

### Login as admin
POST {{SimpleApi_HostAddress}}/login?useCookies=false
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "AdminPassword123!"
}

### Access admin endpoint as admin (should work)
GET {{SimpleApi_HostAddress}}/api/Students/admin-only
Accept: application/json
Authorization: Bearer {{adminToken}}

### Assign Student role to user
POST {{SimpleApi_HostAddress}}/api/Students/assign-role
Content-Type: application/json
Authorization: Bearer {{adminToken}}

{
  "email": "user@example.com",
  "role": "Student"
}
```

## 🧪 Komplett testfil

```http
@SimpleApi_HostAddress = http://localhost:5077

### === AUTHENTICATION & AUTHORIZATION TESTS ===

### Register new user
POST {{SimpleApi_HostAddress}}/register
Content-Type: application/json

{
  "email": "student@example.com",
  "password": "StudentPass123!"
}

###

### Login user
POST {{SimpleApi_HostAddress}}/login?useCookies=false
Content-Type: application/json

{
  "email": "student@example.com",
  "password": "StudentPass123!"
}

# Save the accessToken from response for next requests

###

### Test protected endpoint without token (401 Unauthorized)
GET {{SimpleApi_HostAddress}}/api/Students
Accept: application/json

###

### Test protected endpoint with token (should work)
GET {{SimpleApi_HostAddress}}/api/Students
Accept: application/json
Authorization: Bearer YOUR_ACCESS_TOKEN_HERE

###

### Get user profile
GET {{SimpleApi_HostAddress}}/api/Students/profile
Accept: application/json
Authorization: Bearer YOUR_ACCESS_TOKEN_HERE

###

### Login as admin
POST {{SimpleApi_HostAddress}}/login?useCookies=false
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "AdminPassword123!"
}

###

### Test admin-only endpoint
GET {{SimpleApi_HostAddress}}/api/Students/admin-only
Accept: application/json
Authorization: Bearer YOUR_ADMIN_TOKEN_HERE

###
```

## 🤔 Diskussionsfrågor

### 1. Skillnaden mellan autentisering och auktorisering?
- **Autentisering**: Verifierar identitet ("Är du verkligen John?")
- **Auktorisering**: Kontrollerar behörigheter ("Får John läsa denna fil?")

### 2. Vad händer om en JWT blir stulen?
- Tokens kan användas av angripare tills de går ut
- Därför är kort livslängd viktigt (1-24 timmar)
- Refresh tokens kan återkallas
- HTTPS är kritiskt för att förhindra stöld

### 3. Hur länge bör en JWT vara giltig?
- **Access tokens**: Kort (15min - 24h)
- **Refresh tokens**: Längre (dagar/månader)
- Balance mellan säkerhet och användarupplevelse
- Kritiska system: kortare livslängd

### 4. Varför använda IdentityUser istället för egen modell?
- ✅ Färdiga säkerhetsfunktioner (hashning, validation)
- ✅ Standardiserade claims och roller
- ✅ Kompatibilitet med ASP.NET Core middleware
- ✅ Vältestad och säker implementation
- ✅ Automatisk hantering av lösenordsregler

## 🎯 Sammanfattning

Efter Del 7 förstår du:
- ✅ Hur man integrerar Identity i sitt projekt
- ✅ Användarregistrering och inloggning med JWT
- ✅ Skydda endpoints med [Authorize]
- ✅ Claims och rollbaserad auktorisering
- ✅ Säkerhetsaspekter med tokens
- ✅ Skillnaden mellan autentisering och auktorisering

**Nästa steg**: Migrera till SQL Server databas med riktiga användarkonton och avancerad rollhantering.