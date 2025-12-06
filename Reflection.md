# Del 4 – Reflektion

## Övning 10 – Diskussionsfrågor

### 1. Varför är det bättre att låta controllern vara tunn?

En tunn controller har flera fördelar:

- **Separation of Concerns (Ansvarsfördelning)**: Controllern fokuserar endast på HTTP-hantering (ta emot requests, validera input, returnera responses) medan affärslogik ligger i services
- **Testbarhet**: Enklare att testa affärslogik isolerat utan att behöva mocka HTTP-kontext
- **Återanvändbarhet**: Samma affärslogik kan användas från olika controllers eller andra delar av systemet
- **Underhållbarhet**: Förändringar i affärslogik påverkar inte HTTP-lagret och vice versa
- **Single Responsibility Principle**: Varje klass har ett tydligt och avgränsat ansvar

**Exempel från projektet**: Våra controllers delegerar direkt till services utan att innehålla komplex logik:
```csharp
[HttpPost]
public ActionResult<Student> CreateStudent(CreateStudentRequest request)
{
    var student = _studentService.CreateStudent(request);
    return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
}
```

### 2. Vad vinner man på att använda services jämfört med att lägga all logik i controllern?

Services-lagret ger flera viktiga fördelar:

- **Affärslogik centralisering**: All verksamhetslogik samlas på ett ställe istället för att spridas över flera controllers
- **Kodåteranvändning**: Samma service kan användas av flera controllers eller andra komponenter
- **Enklare testing**: Services kan testas utan HTTP-beroenden - bara ren C#-kod
- **Bättre struktur**: Tydlig separation mellan HTTP-hantering (controllers) och affärslogik (services)
- **Skalbarhet**: När applikationen växer blir det enklare att hantera komplex affärslogik

**Exempel från projektet**: 
- `StudentService` hanterar all logik kring studenthantering
- `CourseInstanceService` hanterar komplex logik för kurser med studenter och validering
- Controllers fokuserar bara på HTTP-aspekter som status codes och routing

### 3. Vad vinner man på att använda repositories med interface jämfört med att bara skriva allting i service-klasserna?

Repository-pattern med interfaces ger betydande arkitekturella fördelar:

**Abstraction av datakälla**:
- Services behöver inte veta om data kommer från minnet, databas eller API
- Enkelt att byta datakälla utan att påverka affärslogik

**Testbarhet**:
- Enkelt att mocka repositories i enhetstester
- Services kan testas isolerat från datalagret

**Flexibility**:
- Olika implementationer för olika miljöer (in-memory för test, databas för produktion)
- Möjlighet att implementera caching eller andra optimeringar transparent

**Separation of Concerns**:
- Services fokuserar på affärslogik
- Repositories fokuserar på dataåtkomst
- Tydlig gräns mellan lager

**Exempel från projektet**:
```csharp
// Service fokuserar på affärslogik
public class StudentService
{
    private readonly IStudentRepository _repository;
    
    public Student CreateStudent(CreateStudentRequest request)
    {
        // Affärslogik här
        return _repository.CreateStudent(request);
    }
}

// Repository fokuserar på dataåtkomst
public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();
    
    public Student CreateStudent(CreateStudentRequest request)
    {
        // Endast datahantering här
    }
}
```

### 4. Hur skulle du byta ut in-memory-lagring mot en databas (Entity Framework)?

Övergången från in-memory till Entity Framework skulle göras enligt följande steg:

**1. Installera Entity Framework NuGet-paket**:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**2. Skapa DbContext**:
```csharp
public class SimpleApiDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseInstance> CourseInstances { get; set; }
    
    public SimpleApiDbContext(DbContextOptions<SimpleApiDbContext> options) 
        : base(options) { }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurera relationer och constraints
    }
}
```

**3. Skapa nya repository-implementationer**:
```csharp
public class EfStudentRepository : IStudentRepository
{
    private readonly SimpleApiDbContext _context;
    
    public EfStudentRepository(SimpleApiDbContext context)
    {
        _context = context;
    }
    
    public List<Student> GetAllStudents()
    {
        return _context.Students.ToList();
    }
    
    public Student CreateStudent(CreateStudentRequest request)
    {
        var student = new Student(/*...*/);
        _context.Students.Add(student);
        _context.SaveChanges();
        return student;
    }
    // ... andra metoder
}
```

**4. Uppdatera Program.cs**:
```csharp
// Ersätt in-memory registreringar med EF
builder.Services.AddDbContext<SimpleApiDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IStudentRepository, EfStudentRepository>();
builder.Services.AddScoped<ICourseRepository, EfCourseRepository>();
builder.Services.AddScoped<ICourseInstanceRepository, EfCourseInstanceRepository>();
```

**5. Skapa migrations**:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Fördelar med denna approach**:
- **Ingen kod ändras i controllers eller services** - de använder samma interfaces
- **Gradvis migrering möjlig** - kan byta en repository i taget
- **Testning bibehålls** - kan fortsätta använda in-memory repositories i tester
- **Rollback möjlig** - enkelt att gå tillbaka till in-memory om problem uppstår

Repository-pattern gör denna typ av infrastrukturell förändring mycket enkel och säker!

---

## Reflektionsfråga - Uppgift 5 (Tidigare diskussion)

### Fråga
Varför är det en bättre modellering att koppla Grade till Student + CourseInstance än att lägga en lista med betyg direkt på studenten?

### Svar

Det är bättre att koppla Grade till både Student och CourseInstance av flera viktiga anledningar:

**1. Specifikt kurstillfälle**
- En kurs (t.ex. "Programming") kan köras flera gånger per år
- Samma student kan ta samma kurs flera gånger
- Betyget gäller för ett specifikt kurstillfälle, inte bara kursen generellt
- Exempel: Alice kan få "C" på Programming våren 2024 och "A" på Programming hösten 2024

**2. Tidsaspekt och kontext**
- CourseInstance innehåller StartDate och EndDate
- Viktigt att veta NÄR betyget gavs
- Olika lärare kan ha olika kurser vid olika tillfällen
- Curriculum kan ändras mellan kurstillfällen

**3. Datamodellering och normalisering**
- Undviker duplicering av data
- Om betyget låg direkt på Student skulle vi behöva upprepa kursinformation
- Grade skapar en tydlig relation mellan Student och CourseInstance
- Följer relationsdatabas-principer

**4. Flexibilitet och skalbarhet**
- Lätt att hitta alla betyg för ett specifikt kurstillfälle
- Lätt att hitta alla betyg för en student
- Kan enkelt lägga till fler attribut på Grade (datum, kommentarer, etc.)
- Stödjer komplexa queries som "alla studenter som fick A i Programming under 2024"

**5. Verkliga scenarion**
I verkligheten:
- Studenter tar om kurser
- Kurser ges flera gånger
- Betyg gäller för specifika terminer/år
- Detta är samma modell som universitets betygssystem använder

**Slutsats:** Grade som en separat entitet som kopplar Student till CourseInstance ger en mer korrekt, flexibel och skalbar datamodell som bättre representerar verkligheten.