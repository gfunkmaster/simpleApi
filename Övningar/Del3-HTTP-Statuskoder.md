# Del 3 – HTTP-statuskoder och CRUD-operationer

## Övning 1 – Utforska HTTP-svarskoder
• Skriv ner en kort förklaring av följande koder och när de används i API:et:
  - 200 OK
  - 201 Created
  - 204 No Content
  - 400 Bad Request
  - 404 Not Found
  - 500 Internal Server Error

Här sätter vi ramarna för vad som är "korrekt" svar i olika situationer.
För mer information angående svarskoder kan ni kika på följande länk: [HTTP Status Codes explained](https://http.cat/)

### 💡 **Svar - HTTP-statuskoder förklaring:**

#### **2xx Success (Framgångsrika förfrågningar)**

**200 OK**
- **Betydelse:** Förfrågan lyckades och servern returnerar data
- **Användning i vårt API:** 
  - `GET /students` - Returnerar lista med studenter
  - `GET /students/{id}` - Returnerar en specifik student
  - `PUT /students/{id}` - Student uppdaterades framgångsrikt
- **Exempel:** Hämta alla studenter returnerar 200 + JSON-array

**201 Created**
- **Betydelse:** Resursen skapades framgångsrikt
- **Användning i vårt API:**
  - `POST /students` - Ny student skapad
  - `POST /courses` - Ny kurs skapad
  - `POST /courseinstances` - Nytt kurstillfälle skapat
  - `POST /grades` - Nytt betyg skapat
- **Best practice:** Inkludera Location-header med URL till ny resurs

**204 No Content**
- **Betydelse:** Operationen lyckades men inget innehåll returneras
- **Användning i vårt API:**
  - `DELETE /students/{id}` - Student borttagen
  - `DELETE /courses/{id}` - Kurs borttagen
- **Anledning:** Efter DELETE finns inget objekt kvar att returnera

#### **4xx Client Error (Klientfel)**

**400 Bad Request**
- **Betydelse:** Förfrågan är felaktig eller innehåller ogiltig data
- **Användning i vårt API:**
  - Validation misslyckas (tomt namn, ogiltigt email)
  - Ogiltiga datum (EndDate före StartDate)
  - Ogiltigt betyg (inte A-F)
  - Felaktigt JSON-format
- **Exempel:** Skapa student utan email → 400 + valideringsfel

**404 Not Found**
- **Betydelse:** Resursen kunde inte hittas
- **Användning i vårt API:**
  - `GET /students/999` - Student med ID 999 finns inte
  - `PUT /students/999` - Försöker uppdatera obefintlig student
  - `DELETE /courses/999` - Försöker ta bort obefintlig kurs
- **Viktigt:** Returnera inte 404 för tomma listor, använd 200 + tom array

#### **5xx Server Error (Serverfel)**

**500 Internal Server Error**
- **Betydelse:** Något gick fel på servern
- **Användning i vårt API:**
  - Ohanterade exceptions
  - Databaskoppling misslyckas
  - Kod-buggar som inte fångats
- **Best practice:** Logga detaljer men returnera generiskt meddelande till klient

---

## Övning 2 – CRUD för Student med rätt statuskoder
Skapa följande endpoint:
1. **GET /students**
   - Returnera alla studenter → 200 OK
   - Om inga studenter finns → returnera en tom lista men fortfarande 200 OK
2. **GET /students/{id}**
   - Om studenten finns → returnera 200 OK + student
   - Om studenten inte finns → returnera 404 Not Found
3. **POST /students**
   - Om all data är giltig → skapa studenten och returnera 201 Created
   - Om något fält saknas eller är ogiltigt → returnera 400 Bad Request
4. **PUT /students/{id}**
   - Om studenten finns och uppdateringen lyckas → 200 OK
   - Om studenten inte finns → 404 Not Found
5. **DELETE /students/{id}**
   - Om studenten finns och tas bort → 204 No Content
   - Om studenten inte finns → 404 Not Found

**Status: ✅ KLART** - Alla endpoints implementerade i `StudentController`

---

## Övning 3 – CRUD för Course  
*(Övning 3 verkar saknas i ursprungstexten, men vi har implementerat den)*
1. Skapa endpoints för Course:
   - GET /courses
   - GET /courses/{id}
   - POST /courses
   - PUT /courses/{id}
   - DELETE /courses/{id}
2. Använd samma regler för statuskoder som med Student

**Status: ✅ KLART** - Alla endpoints implementerade i `CourseController`

---

## Övning 4 – CRUD för Course
*(Denna verkar vara duplicerad från Övning 3)*
1. Skapa endpoints för Course:
   - GET /courses
   - GET /courses/{id}
   - POST /courses
   - PUT /courses/{id}
   - DELETE /courses/{id}
2. Använd samma regler för statuskoder som med Student

**Status: ✅ KLART** - Implementerad som del av Övning 3

---

## Övning 5 – CRUD för CourseInstance
1. Skapa endpoints för CourseInstance:
   - GET /courseinstances
   - GET /courseinstances/{id}
   - POST /courseinstances
   - PUT /courseinstances/{id}
   - DELETE /courseinstances/{id}
2. Statuskoder:
   - 201 Created när nytt kurstillfälle skapas
   - 400 Bad Request om datan är ogiltig
   - 404 Not Found om Id inte finns
3. Valideringsexempel:
   - StartDate och EndDate är obligatoriska
   - EndDate måste vara efter StartDate → annars 400 Bad Request

**Status: ✅ KLART** - Alla endpoints implementerade i `CourseInstanceController`

---

## Övning 6 – (Frivillig utmaning) Hantera Grade
• Skapa en endpoint POST /grades som låter läraren sätta ett betyg för en student i ett kurstillfälle
• Regler:
  - Om både student och kurstillfälle existerar → 201 Created
  - Om student eller kurstillfälle saknas → 404 Not Found
  - Om betyget är ogiltigt (t.ex. inte A–F) → 400 Bad Request

**Status: ✅ KLART** - Komplett Grade-system implementerat med full CRUD i `GradeController`

---

## Implementationsdetaljer

### **Korrekt användning av statuskoder i våra controllers:**

```csharp
// GET - Returnerar 200 OK med data eller tom lista
[HttpGet]
public ActionResult<IEnumerable<Student>> GetAll()
{
    return Ok(_studentService.GetAll()); // 200 även om tom lista
}

// GET by ID - 200 OK eller 404 Not Found
[HttpGet("{id}")]
public ActionResult<Student> GetById(int id)
{
    var student = _studentService.GetById(id);
    return student == null ? NotFound() : Ok(student);
}

// POST - 201 Created eller 400 Bad Request
[HttpPost]
public ActionResult<Student> Create(CreateStudentRequest request)
{
    if (!ModelState.IsValid)
        return ValidationProblem(ModelState); // 400 Bad Request
        
    var student = _studentService.Create(request);
    return CreatedAtAction(nameof(GetById), new { id = student.Id }, student); // 201 Created
}

// PUT - 200 OK eller 404 Not Found
[HttpPut("{id}")]
public IActionResult Update(int id, CreateStudentRequest request)
{
    if (!ModelState.IsValid)
        return ValidationProblem(ModelState); // 400 Bad Request
        
    var updated = _studentService.Update(id, request);
    return updated == null ? NotFound() : Ok(updated); // 404 eller 200
}

// DELETE - 204 No Content eller 404 Not Found
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    var deleted = _studentService.Delete(id);
    return deleted ? NoContent() : NotFound(); // 204 eller 404
}
```

### **Viktiga principer:**
1. **Konsistent användning** - Samma logik för alla controllers
2. **Informativa svar** - Rätt statuskod för rätt situation  
3. **Validation first** - Kontrollera ModelState innan business logic
4. **RESTful design** - Följa HTTP-standarder och konventioner

---

## Reflektionsfråga
**Varför är det viktigt att använda rätt HTTP-statuskoder? Vad händer om vi alltid returnerar 200 OK?**

### 💡 **Svar:**

#### **Varför rätt statuskoder är kritiskt:**

**1. Semantisk tydlighet**
- Klienten förstår omedelbart resultatet utan att läsa response body
- 404 betyder "hittades inte" - klienten behöver inte gissa
- 201 betyder "skapades" - klienten vet att operationen lyckades

**2. Automatiserad felhantering**
- HTTP-klienter kan hantera olika statuskoder automatiskt
- Retry-logik: 500 → försök igen, 404 → ge upp
- Caching: 200 → cache, 404 → cache inte

**3. Monitoring och logging**
- Infrastruktur kan filtrera och analysera baserat på statuskoder
- "Alla 5xx-fel senaste timmen" → enkelt att övervaka
- Load balancers kan omdirigera trafik vid 5xx-fel

**4. Developer Experience**
- API-dokumentation blir tydligare
- Postman och andra verktyg visar rätt färgkoder
- Debugging blir enklare

#### **Problem med alltid 200 OK:**

**Dold felhantering:**
```json
// BAD: Alltid 200 OK
{
  "success": false,
  "error": "Student not found",
  "data": null
}

// GOOD: 404 Not Found + tom body
HTTP/1.1 404 Not Found
```

**Klientproblem:**
- Måste alltid parsa response body för att förstå resultatet
- Kan inte använda HTTP-standarder för felhantering
- Svårare att implementera generisk error-handling

**Infrastrukturproblem:**
- Monitoring-verktyg ser allt som "framgångsrikt"
- Load balancers kan inte identifiera fel-responses
- Caching fungerar felaktigt

#### **Verkligt exempel:**
```csharp
// BAD - Alltid 200
return Ok(new { success = false, message = "Student not found" });

// GOOD - Semantiskt korrekt  
return NotFound(); // HTTP 404
```

**Slutsats:** Rätt statuskoder är inte bara "nice to have" - de är fundamental del av HTTP-protokollet och gör API:et mer robust, användarvänligt och standardkompatibelt! 🚀

---
**Status: ✅ ALLA ÖVNINGAR KLARA**