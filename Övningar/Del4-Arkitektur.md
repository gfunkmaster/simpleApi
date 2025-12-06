# Del 4 – Arkitektur: Controllers → Services → Repositories

## Del 1 – Grundläggande controllers

I dessa övningar vill jag att ni använder er av en hårdkodad lista med studenter och tillhörande kurser och betyg. Detta kommer att bli en utmaning för oss som vi sedan skall lösa på bättre sätt. Men ett steg i taget.

### Övning 1 – Skapa en egen controller
1. Skapa en ny controller `StudentController` (om du inte redan har en)
2. Lägg till en endpoint `GET /students` som returnerar en lista av hårdkodade studenter
3. Lägg till en endpoint `GET /students/{id}` som returnerar en student baserat på Id
   - Om ingen student finns → returnera 404 Not Found

**Status: ✅ KLART** - `StudentController` skapad med GET-endpoints

### Övning 2 – CRUD i controllern
1. Lägg till följande endpoints i `StudentController`:
   - `POST /students` → Skapa en ny student
   - `PUT /students/{id}` → Uppdatera en student
   - `DELETE /students/{id}` → Ta bort en student
2. Testa varje endpoint i Postman

**Status: ✅ KLART** - Full CRUD implementerad i controller

### Övning 3 – Bygg fler controllers
1. Skapa `CourseController` och `CourseInstanceController`
2. Implementera minst en GET-metod i vardera controller för att returnera hårdkodad data
3. Testa alla tre controllers i Postman

💡 **Poängen här är att se att controllers fungerar som bryggor mellan HTTP och kod.**

**Status: ✅ KLART** - Alla tre controllers implementerade

---

## Del 2 – Refaktorering till Services

### Övning 4 – Flytta logik till en service
1. Skapa en mapp `Services`
2. Skapa `StudentService` med metoderna:
   - `GetAll()`
   - `GetById(int id)`
   - `Add(Student student)`
   - `Update(int id, Student updated)`
   - `Delete(int id)`
3. Flytta all logik från `StudentController` till `StudentService`
4. Ändra `StudentController` så att den bara anropar service-metoderna

💡 **Studenterna ser nu hur controllern blir tunnare.**

**Status: ✅ KLART** - `StudentService` skapad, controller refaktorerad

### Övning 5 – Dependency Injection
1. Registrera `StudentService` i `Program.cs`:
   ```csharp
   builder.Services.AddSingleton<StudentService>();
   ```
2. Ändra `StudentController` så att den tar emot `StudentService` via konstruktorn (injektion)
3. Testa alla endpoints igen i Postman

💡 **Här ser studenterna hur DI fungerar i praktiken.**

**Status: ✅ KLART** - DI konfigurerad och fungerande

### Övning 6 – Applicera samma sak på Course och CourseInstance
1. Skapa `CourseService` och `CourseInstanceService`
2. Flytta logik från respektive controller till sina services
3. Registrera services i `Program.cs`

**Status: ✅ KLART** - Alla services implementerade och registrerade

---

## Del 3 – Refaktorering till Repositories

### Övning 7 – Skapa repository-interface
1. Skapa en mapp `Repositories`
2. Skapa ett interface `IStudentRepository` med metoder för CRUD
3. Skapa en implementation `InMemoryStudentRepository` som använder en lista i minnet

**Status: ✅ KLART** - Repository pattern implementerat för Student

### Övning 8 – Använd repository i service
1. Ändra `StudentService` så att den tar emot ett `IStudentRepository` i konstruktorn
2. Ändra logiken i service-metoderna så att de anropar repository i stället för att hantera listan själva
3. Registrera repository i `Program.cs`:
   ```csharp
   builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
   ```

💡 **Nu ser ni poängen med att kunna byta ut datakällan senare.**

**Status: ✅ KLART** - Repository injicerat i service

### Övning 9 – Applicera på Course och CourseInstance
1. Skapa `ICourseRepository` och `ICourseInstanceRepository` med in-memory-implementationer
2. Koppla ihop `CourseService` och `CourseInstanceService` med respektive repository
3. Testa endpoints i Postman igen

**Status: ✅ KLART** - Alla repositories implementerade och integrerade

---

## Del 4 – Reflektion

### Övning 10 – Diskussionsfrågor

#### 1. **Varför är det bättre att låta controllern vara tunn?**

### 💡 **Svar:**

**Single Responsibility Principle:**
- Controller ska bara hantera HTTP-kommunikation (request/response)
- Business logic hör hemma i service-lagret
- Data access hör hemma i repository-lagret

**Fördelar med tunna controllers:**

**Testbarhet:**
```csharp
// THICK Controller (svår att testa)
[HttpGet("{id}")]
public ActionResult<Student> GetById(int id)
{
    // 50 rader business logic här...
    // Svårt att unit-testa utan HTTP-context
}

// THIN Controller (lätt att testa)  
[HttpGet("{id}")]
public ActionResult<Student> GetById(int id)
{
    var student = _studentService.GetById(id);
    return student == null ? NotFound() : Ok(student);
}
```

**Återanvändbarhet:**
- Service-logik kan användas av andra controllers
- Samma logik kan anropas från Web API, Console App, Background Service
- Business rules är inte bundna till HTTP

**Underhållbarhet:**
- Enklare att hitta och ändra business logic
- Färre anledningar att ändra controllern
- Tydlig separation av concerns

#### 2. **Vad vinner man på att använda services jämfört med att lägga all logik i controllern?**

### 💡 **Svar:**

**Separation of Concerns:**
```csharp
// Controller ansvar: HTTP-hantering
public class StudentController : ControllerBase
{
    public ActionResult<Student> Create(CreateStudentRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        
        var student = _studentService.Create(request); // Business logic i service
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
    }
}

// Service ansvar: Business logic  
public class StudentService
{
    public Student Create(CreateStudentRequest request)
    {
        // Validera business rules
        // Transformera data
        // Koordinera med repository
        // Returnera resultat
    }
}
```

**Testbarhet:**
```csharp
// Unit test för service (inget HTTP behövs)
[Test]
public void Create_ValidRequest_ReturnsStudent()
{
    var service = new StudentService(mockRepository);
    var request = new CreateStudentRequest { Name = "Alice", Email = "alice@test.com" };
    
    var result = service.Create(request);
    
    Assert.That(result.Name, Is.EqualTo("Alice"));
}
```

**Återanvändning:**
- Service kan användas av Web API, Console App, Background Jobs
- Business logic är inte bunden till HTTP-protokollet
- Samma service kan användas av flera controllers

**Skalbarhet:**
- Services kan injicera andra services
- Komplex business logic får egen plats
- Lättare att lägga till caching, logging, etc.

#### 3. **Vad vinner man på att använda repositories med interface jämfört med att bara skriva allting i service-klasserna?**

### 💡 **Svar:**

**Abstraction av data access:**
```csharp
// Service behöver inte veta VAR data kommer ifrån
public class StudentService
{
    private readonly IStudentRepository _repository; // Interface!
    
    public Student GetById(int id)
    {
        return _repository.GetById(id); // Kan vara minne, databas, API, fil...
    }
}
```

**Testbarhet med mocking:**
```csharp
[Test]  
public void GetById_ExistingStudent_ReturnsStudent()
{
    // Arrange
    var mockRepo = new Mock<IStudentRepository>();
    mockRepo.Setup(r => r.GetById(1)).Returns(new Student { Id = 1, Name = "Alice" });
    var service = new StudentService(mockRepo.Object);
    
    // Act
    var result = service.GetById(1);
    
    // Assert - Inget databas-beroende!
    Assert.That(result.Name, Is.EqualTo("Alice"));
}
```

**Flexibilitet att byta implementering:**
```csharp
// Utveckling: In-memory
builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();

// Produktion: Databas
builder.Services.AddScoped<IStudentRepository, EntityFrameworkStudentRepository>();

// Test: Mock eller test-databas
builder.Services.AddSingleton<IStudentRepository, TestStudentRepository>();
```

**Single Responsibility:**
- Repository: Bara data access
- Service: Bara business logic  
- Controller: Bara HTTP-hantering

**Performance optimering:**
- Repository kan optimera queries
- Caching på repository-nivå
- Batch operations

#### 4. **Hur skulle du byta ut in-memory-lagring mot en databas (Entity Framework)?**

### 💡 **Svar:**

**Steg 1: Installera Entity Framework**
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Steg 2: Skapa DbContext**
```csharp
public class SchoolDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseInstance> CourseInstances { get; set; }
    public DbSet<Grade> Grades { get; set; }
    
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurera relationer, constraints, etc.
    }
}
```

**Steg 3: Skapa ny repository-implementering**
```csharp
public class EntityFrameworkStudentRepository : IStudentRepository
{
    private readonly SchoolDbContext _context;
    
    public EntityFrameworkStudentRepository(SchoolDbContext context)
    {
        _context = context;
    }
    
    public Student? GetById(int id)
    {
        return _context.Students.FirstOrDefault(s => s.Id == id);
    }
    
    public IEnumerable<Student> GetAll()
    {
        return _context.Students.ToList();
    }
    
    public Student Create(Student student)
    {
        _context.Students.Add(student);
        _context.SaveChanges();
        return student;
    }
    
    public Student? Update(int id, Student updated)
    {
        var existing = _context.Students.FirstOrDefault(s => s.Id == id);
        if (existing == null) return null;
        
        existing.Name = updated.Name;
        existing.Email = updated.Email;
        // ... uppdatera andra properties
        
        _context.SaveChanges();
        return existing;
    }
    
    public bool Delete(int id)
    {
        var student = _context.Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return false;
        
        _context.Students.Remove(student);
        _context.SaveChanges();
        return true;
    }
}
```

**Steg 4: Registrera i Program.cs**
```csharp
// Registrera DbContext
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Byt ut repository-registrering
// INNAN:
// builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();

// EFTER:
builder.Services.AddScoped<IStudentRepository, EntityFrameworkStudentRepository>();
```

**Steg 5: Lägg till connection string**
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=true;"
  }
}
```

**Steg 6: Skapa och köra migrations**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**VIKTIGT: Ingen annan kod behöver ändras!**
- Controllers fortsätter använda Services
- Services fortsätter använda IStudentRepository  
- Bara implementeringen av repository har bytts ut
- Detta är kraften i Dependency Injection och Interface Segregation!

**Resultat:**
- Samma API-endpoints fungerar exakt likadant
- Data sparas nu i databas istället för minne
- Tester kan fortfarande använda mock-repositories
- Perfekt exempel på Open/Closed Principle

---

## Sammanfattning: Arkitektonisk utveckling

### **Evolution av vår kodbas:**

**Stadium 1: Fat Controllers**
```
HTTP Request → Controller (All logic) → HTTP Response
```

**Stadium 2: Controller + Service**  
```
HTTP Request → Controller → Service (Business Logic) → Controller → HTTP Response
```

**Stadium 3: Layered Architecture**
```
HTTP Request → Controller → Service → Repository → Database
                    ↓         ↓          ↓
               HTTP Logic  Business   Data Access
```

### **Fördelar med slutgiltig arkitektur:**

1. **Testbarhet** - Varje lager kan testas isolerat
2. **Maintainability** - Tydlig ansvarsfördelning
3. **Flexibility** - Lätt att byta implementation
4. **Scalability** - Kan lägga till komplexitet utan att förstöra struktur
5. **Team Development** - Olika utvecklare kan jobba på olika lager

### **SOLID-principer som följs:**

- **S**ingle Responsibility: Varje klass har ett ansvar
- **O**pen/Closed: Kan utöka utan att ändra befintlig kod  
- **L**iskov Substitution: Implementations kan bytas ut
- **I**nterface Segregation: Små, fokuserade interfaces
- **D**ependency Inversion: Beroende på abstractions, inte konkreta klasser

**Detta är professionell, enterprise-ready arkitektur! 🏗️**

---
**Status: ✅ ALLA ÖVNINGAR OCH REFLEKTIONER KLARA**