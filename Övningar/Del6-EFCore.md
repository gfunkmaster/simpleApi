# Del 6 - Entity Framework Core

## Mål
Migrera från in-memory listor till Entity Framework Core och implementera databasrelationer.

## ✅ Genomförda Uppgifter

### Del 1 – Introduktion till EF Core och In-Memory-databas

**Installera och konfigurera EF Core:**
- ✅ Installerat `Microsoft.EntityFrameworkCore.InMemory` (version 8.0.11)
- ✅ Skapat `ApplicationDbContext` som ärver från `DbContext`
- ✅ Registrerat DbContext i `Program.cs` med `.UseInMemoryDatabase("SimpleApiDb")`
- ✅ Bytt ut hårdkodade listor mot EF Core `DbSet<T>`

**Implementering:**
```csharp
// ApplicationDbContext.cs
public class ApplicationDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseInstance> CourseInstances { get; set; }
    public DbSet<Grade> Grades { get; set; }
}

// Program.cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SimpleApiDb"));
```

### Del 2 – En-till-många-relationer

**Implementerade relationer:**
- ✅ **Course → CourseInstance** (en kurs kan ha många kurstillfällen)
- ✅ **Student → Grade** (en student kan ha många betyg)
- ✅ **CourseInstance → Grade** (ett kurstillfälle kan ha många betyg)

**Navigation Properties:**
```csharp
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<CourseInstance> CourseInstances { get; set; } = new();
}

public class CourseInstance
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public List<Student> EnrolledStudents { get; set; } = new();
    public List<Grade> Grades { get; set; } = new();
}
```

### Del 3 – Många-till-många-relationer

**Student ↔ CourseInstance relation:**
- ✅ Implementerat många-till-många mellan Student och CourseInstance
- ✅ EF Core skapar automatisk join-tabell (CourseInstanceStudent)
- ✅ Navigation properties på båda sidor

**Konfiguration:**
```csharp
// I ApplicationDbContext.cs OnModelCreating
modelBuilder.Entity<CourseInstance>()
    .HasMany(ci => ci.EnrolledStudents)
    .WithMany();
```

**Enrollment Endpoints:**
- ✅ `POST /api/CourseInstances/{courseInstanceId}/enroll/{studentId}`
- ✅ `DELETE /api/CourseInstances/{courseInstanceId}/unenroll/{studentId}`

### Del 4 – Validering + EF Core

**Implementerad validering:**
- ✅ Förhindrar dubbel enrollment av samma student
- ✅ Validerar att CourseInstance och Student finns innan enrollment
- ✅ Validerar att Course finns innan CourseInstance skapas
- ✅ Returnerar korrekt HTTP-statuskoder (200, 400, 404)

**Exempel på validering:**
```csharp
public bool EnrollStudent(int courseInstanceId, int studentId)
{
    var courseInstance = _context.CourseInstances
        .Include(ci => ci.EnrolledStudents)
        .FirstOrDefault(ci => ci.Id == courseInstanceId);
        
    var student = _context.Students.Find(studentId);
    
    if (courseInstance == null || student == null) return false;
    
    // Förhindra dubbel enrollment
    if (courseInstance.EnrolledStudents.Any(s => s.Id == studentId)) return false;
    
    courseInstance.EnrolledStudents.Add(student);
    _context.SaveChanges();
    return true;
}
```

## 🧪 Testning

**HTTP Requests för testning:**
```http
# Hämta alla course instances
GET {{baseUrl}}/api/CourseInstances

# Enrolla studenter
POST {{baseUrl}}/api/CourseInstances/1/enroll/1
POST {{baseUrl}}/api/CourseInstances/1/enroll/2

# Verifiera enrollment
GET {{baseUrl}}/api/CourseInstances/1

# Unenrolla student
DELETE {{baseUrl}}/api/CourseInstances/1/unenroll/1
```

## 📊 Databasstruktur

**Seed Data:**
- 3 Students (John Doe, Jane Smith, Bob Johnson)
- 2 Courses (Programming, Math)
- 2 CourseInstances (kopplad till respektive kurs)
- 3 Grades (kopplade till studenter och kurstillfällen)

**Relationer i praktiken:**
- Course "Programming" har CourseInstance med id 1
- Course "Math" har CourseInstance med id 2
- Students kan enrollas i CourseInstances via many-to-many
- Grades kopplar Students till CourseInstances med betyg

## 💭 Del 5 – Diskussionsfrågor (Besvarade)

### 1. Vad är skillnaden mellan att arbeta med listor i minnet och EF Core?

**In-Memory Listor (före):**
- ✅ Manuell hantering av `List<Student> students = new()`
- ❌ Förlorar data vid omstart av appen
- ❌ Manuell relationhantering (hitta relaterade objekt själv)
- ❌ Ingen automatisk validering av relationer
- ❌ Svårt att skala och underhålla

**EF Core (nu):**
- ✅ `DbSet<Student>` med automatisk persistering
- ✅ Data finns kvar mellan anrop (inom samma app-session)
- ✅ Automatisk relationhantering via navigation properties
- ✅ `Include()` för att ladda relaterade data automatiskt
- ✅ Enkel att migrera till riktig databas (SQL Server, etc.)

### 2. Varför är navigation properties viktiga?

**Navigation Properties gör att:**
- ✅ `courseInstance.Course` - direkt åtkomst till relaterad kurs
- ✅ `courseInstance.EnrolledStudents` - alla studenter i kurstillfället
- ✅ `student.Grades` - alla betyg för en student
- ✅ EF Core laddar automatiskt relaterade objekt med `Include()`
- ✅ JSON-serialisering inkluderar nested objekt automatiskt

**Exempel från vårt API:**
```json
{
  "id": 1,
  "course": {
    "title": "Programming",
    "description": "Learn programming"
  },
  "enrolledStudents": [
    {
      "name": "John Doe",
      "email": "john@example.com"
    }
  ]
}
```

### 3. Vad vinner vi på att EF Core automatiskt skapar join-tabeller?

**Automatisk Join-Tabell (CourseInstanceStudent):**
- ✅ Ingen manuell kod för att hantera many-to-many
- ✅ EF Core skapar `CourseInstanceStudent` tabellen automatiskt
- ✅ Enkel enrollment: `courseInstance.EnrolledStudents.Add(student)`
- ✅ Automatisk validering (förhindrar dubletter)
- ✅ Optimerade SQL-queries bakom kulisserna

**Jämför med manuell hantering:**
```csharp
// Manuellt (komplicerat)
var enrollment = new CourseInstanceStudent { 
    CourseInstanceId = 1, 
    StudentId = 2 
};

// EF Core (enkelt)
courseInstance.EnrolledStudents.Add(student);
```

### 4. När räcker in-memory-databasen inte längre till?

**In-Memory räcker INTE när:**
- ❌ Data måste sparas permanent mellan app-omstarter
- ❌ Flera användare behöver dela samma data samtidigt
- ❌ Performance blir viktigt (stora datamängder)
- ❌ Backup och recovery behövs
- ❌ Rapporter och analytics krävs
- ❌ Production-miljö (riktig applikation)

**In-Memory är BRA för:**
- ✅ Utveckling och testning
- ✅ Lära sig EF Core fundamentals
- ✅ Unit testing (snabb och isolerad)
- ✅ Prototyper och demos
- ✅ Förberedelse innan SQL Server migration

## ✨ Fördelar med EF Core

**Jämfört med in-memory listor:**
- ✅ Automatisk relationhantering
- ✅ LINQ-frågor mot DbSet
- ✅ Automatisk join-tabell för many-to-many
- ✅ Navigation properties laddas automatiskt med Include()
- ✅ Enkel att migrera till riktig databas senare

## 🎯 Resultat

Del 6 är **100% komplett** med:
- Fungerande EF Core In-Memory databas
- Alla relationer (1:many och many:many) implementerade
- Enrollment/Unenrollment funktionalitet
- Validering och felhantering
- Verifierad funktionalitet via API-testning

**Nästa steg:** Migrera till SQL Server databas med EF Core Migrations.