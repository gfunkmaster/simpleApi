# Del 5 – Validering och Felhantering

## Övning 1 – Grundläggande validering för Student
• Skapa en POST-endpoint för att lägga till en student.
• Lägg till logik som kontrollerar:
  - Namn får inte vara tomt.
  - Email måste vara i rätt format (t.ex. innehålla @).
• Om valideringen misslyckas ska controllern returnera 400 Bad Request och ett felmeddelande.
• Testa med Postman:
  - Skicka in giltiga värden → ska fungera.
  - Skicka in ogiltiga värden → ska returnera fel.

## Övning 2 – Validering för Course
• Skapa en POST-endpoint för att lägga till en kurs.
• Lägg till regler:
  - Kursnamn måste vara minst 3 tecken långt.
  - Kurskod får inte vara tom.
• Testa med Postman:
  - Giltig kurs → 201 Created.
  - Ogiltig kurs → 400 Bad Request + felmeddelande.

## Övning 3 – Validering för CourseInstance
• Skapa en POST-endpoint för att lägga till ett kurstillfälle.
• Lägg till regler:
  - Startdatum får inte ligga i det förflutna.
  - Slutdatum måste vara senare än startdatum.
  - Kursen som kurstillfället tillhör måste finnas (annars 404 Not Found).
• Testa olika kombinationer i Postman.

## Övning 4 – Validering för Grade
• Skapa en POST-endpoint för att lägga till ett betyg för en student i ett kurstillfälle.
• Lägg till regler:
  - Studenten måste finnas.
  - Kurstillfället måste finnas.
  - Betyg måste vara ett av de giltiga värdena (A, B, C, D, E, F).
• Vid fel ska ni returnera 400 Bad Request (felaktigt värde) eller 404 Not Found (om student eller kurs saknas).

## Övning 5 – Samla felmeddelanden
• Ändra er valideringslogik så att ni kan returnera flera fel i samma svar.
• Exempel: Om både namn och email är fel, ska svaret innehålla båda felen i en lista.
• Fundera: varför är detta bättre än att bara visa första felet?

## Övning 6 – Bonus: Data Annotation Validation
• Utforska attribut som [Required], [EmailAddress], [Range] och [StringLength].
• Lägg till dessa i dina modeller (Student, Course, etc.).
• Låt .NET automatiskt validera inkommande data och returnera 400 Bad Request.
• Jämför med din egen manuella validering – vad är fördelar/nackdelar med respektive metod?

## Poängen med övningarna:
• Ni tränar på både manuell och automatisk validering.
• De får fundera på affärsregler (ex. datumlogik).
• Ni lär sig att använda rätt statuskoder och felmeddelanden.

## Reflektionsuppgift
**Vad händer om jag skapar en kurs med en StartDate som infaller efter EndDate? Vad borde hända? Hur skulle du implementera detta?**

### 💡 **Svar:**

#### **Vad händer nu:**
Med vår `EndDateAfterStartDateAttribute` validering:
- Begäran returnerar `400 Bad Request`
- ModelState innehåller valideringsfel: "End date must be after start date"
- Objektet skapas INTE i systemet
- Användaren får tydlig felmeddelande

#### **Vad som BORDE hända:**
✅ **Exakt detta!** Systemet beter sig korrekt:

1. **Validering på modellnivå** - Fel upptäcks innan data når business logic
2. **Tydliga felmeddelanden** - Användaren förstår vad som är fel
3. **HTTP-status 400** - Korrekt statuskod för valideringsfel
4. **Ingen förorenad data** - Inget ogiltigt objekt skapas

#### **Implementering (redan gjord):**

```csharp
[EndDateAfterStartDate(ErrorMessage = "End date must be after start date")]
public class CreateCourseInstanceRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    // ...
}

// Custom validation attribute
public class EndDateAfterStartDateAttribute : ValidationAttribute
{
    public override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is CreateCourseInstanceRequest request)
        {
            if (request.EndDate <= request.StartDate)
            {
                return new ValidationResult(ErrorMessage ?? "End date must be after start date");
            }
        }
        return ValidationResult.Success;
    }
}

// I controller
if (!ModelState.IsValid)
{
    return ValidationProblem(ModelState); // Returnerar 400 med detaljer
}
```

#### **Alternativa implementeringar:**

**1. Fluent Validation (externt bibliotek):**
```csharp
RuleFor(x => x.EndDate)
    .GreaterThan(x => x.StartDate)
    .WithMessage("End date must be after start date");
```

**2. Business Logic Validation:**
```csharp
// I service-lagret
if (request.EndDate <= request.StartDate)
{
    throw new BusinessValidationException("Invalid date range");
}
```

**3. Database Constraints:**
```sql
ALTER TABLE CourseInstances 
ADD CONSTRAINT CK_DateRange 
CHECK (EndDate > StartDate);
```

#### **Bästa praxis:**
✅ **Kombinera flera lager:**
1. **Model Validation** (som vi har) - Tidigt fångar fel
2. **Business Logic** - Komplex validering
3. **Database Constraints** - Sista försvar

**Vår nuvarande implementation är mycket bra och följer .NET Core-konventioner perfekt!**

---
**Status: ✅ ALLA KLARA**