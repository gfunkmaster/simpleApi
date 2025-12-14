# Del 8 – Middleware och anropskontroll

## Syfte
Lära sig vad middleware är, hur det används i ASP.NET Core, och hur man kan skapa och använda egen middleware för att kontrollera anrop och användarstatus.

---

## Övning 1 – Förstå middlewareflödet
1. Öppna `Program.cs` och identifiera befintliga middleware (t.ex. `UseAuthentication`, `UseAuthorization`).
2. Lägg till en enkel loggande middleware:
   ```csharp
   app.Use(async (context, next) =>
   {
       Console.WriteLine($"Request path: {context.Request.Path}");
       await next.Invoke();
   });
   ```
3. Starta appen och gör anrop via Postman. Observera loggarna.
4. Testa att flytta logg-middleware före/efter t.ex. `UseAuthorization` och diskutera ordningen.

**Diskussionsfråga:**
- Hur vet man i vilken ordning middleware körs, och varför är det viktigt?

---

## Övning 2 – Skapa en egen middlewareklass
1. Skapa mappen `Middleware` och filen `ActiveUserMiddleware.cs`.
2. Implementera grundstrukturen för middleware.
3. Lägg till en extension-metod i `ActiveUserMiddlewareExtensions.cs`.
4. Registrera med `app.UseActiveUserCheck();` i `Program.cs`.

---

## Övning 3 – Kontrollera aktiva användare
1. Lägg till egenskapen `IsActive` i `ApplicationUser`.
2. I `ActiveUserMiddleware`, kontrollera:
   - Om användaren är inloggad
   - Hämta UserId från claims
   - Hämta användaren via `UserManager`
   - Om `IsActive == false`, returnera 403 och JSON-felmeddelande
3. Testa med både aktiv och inaktiv användare.

---

## Övning 4 – Begränsa middleware till specifika endpoints
1. Låt kontrollen bara gälla för endpoints som börjar med `/api/courses`.
2. Diskutera: Vilka fördelar har detta jämfört med `[Authorize]` på varje controller?

---

## Övning 5 – Hantera svar och felmeddelanden
1. Returnera JSON med felmeddelande och statuskod 403 om användaren är inaktiv.
2. Testa i Postman.

---

## Reflektionsuppgift
1. Förklara skillnaden mellan:
   - Middleware
   - Filters
   - Controller-logik
2. Ge exempel på andra scenarion där middleware kan användas:
   - Loggning
   - Mätning av svarstider
   - API-key-verifiering
   - Rate limiting

---

## Svar på diskussionsfrågor

**Ordning på middleware:**
Middleware körs i den ordning de registreras i `Program.cs`. Det är viktigt eftersom vissa middleware (t.ex. autentisering) måste köras före andra (t.ex. auktorisering eller custom checks). Om ordningen är fel kan t.ex. användarinformation saknas eller felaktiga kontroller göras.

**Varför använda middleware istället för [Authorize]?**
Middleware kan hantera logik som gäller för flera endpoints eller hela applikationen, och kan vara mer flexibel än attribut på controllers. Det kan också användas för loggning, mätning, eller andra tvärgående funktioner.

**Skillnad mellan middleware, filters och controller-logik:**
- Middleware: Hanterar request/response i hela pipelinen, kan stoppa eller modifiera anrop innan de når controllers.
- Filters: Körs nära controller/action, används för t.ex. validering, logging, eller policies på actions.
- Controller-logik: Själva affärslogiken för en specifik endpoint.

**Exempel på middleware-scenarion:**
- Loggning av requests
- Mätning av svarstider
- API-nyckelkontroll
- Rate limiting
- Global felhantering
