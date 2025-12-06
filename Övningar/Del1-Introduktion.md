# Del 1 - Introduktion till Web API

## Övning 1 – Skapa ett nytt projekt
• Skapa ett nytt Web API-projekt i .NET 9.
• Kör projektet och verifiera att det fungerar genom att besöka https://localhost:xxxx/weatherforecast.
• Skriv kort i en README-fil vilken port och URL som används.

## Övning 2 – Utforska projektets struktur
• Identifiera var Program.cs finns och förklara (i en kommentar i koden) vilken roll den filen har.

## Övning 3 – Ändra standardkoden
• Byt namn på WeatherForecastController till något eget (exempelvis HelloController).
• Ändra så att endpointen returnerar ett enkelt "Hello World!".

## Reflektionsfrågor
**Vad är fördelen med att använda Web API istället för en vanlig konsolapplikation? Vilka andra typer av klienter kan konsumera ditt API?**

### 💡 **Svar:**

#### **Fördelar med Web API:**

**1. Plattformsoberoende kommunikation:**
- HTTP är universellt protokoll som alla enheter förstår
- JSON är lätt att läsa och skriva för alla programmeringsspråk
- Konsol-app är begränsad till samma maskin/nätverk

**2. Flera klienter samtidigt:**
- Web API kan hantera tusentals förfrågningar parallellt
- Konsolapp kan bara hantera en användare åt gången
- Skalbarhet och prestanda

**3. Separation of Concerns:**
- API fokuserar på business logic och data
- Klienten fokuserar på presentation och user experience
- Löst kopplad arkitektur

#### **Klienttyper som kan konsumera vårt API:**

**Frontend-applikationer:**
- **React/Angular/Vue.js** - Webbappar
- **Mobile apps** - iOS (Swift), Android (Kotlin/Java)
- **Desktop apps** - WPF, WinUI, Electron

**Andra system:**
- **Andra APIs** - Mikroservices som kommunicerar
- **Batch-jobb** - Automatiserade processer
- **IoT-enheter** - Sensorer som skickar data

**Verktyg och tester:**
- **Postman** - API-testning (som vi använder)
- **curl** - Kommandoradsverktyg
- **Automated tests** - Integration tests

**Business Intelligence:**
- **Power BI** - Rapporter och dashboards
- **Excel** - Dataanalys via Power Query
- **Tableau** - Datavisualisering

#### **Verkliga exempel:**
Om vårt Student API var produktions-redo kunde:
- **Skolans webbsida** visa studentinfo
- **Mobil-app** låta studenter se sina kurser
- **Administrativt system** hantera inskrivningar
- **Rapportsystem** skapa statistik

**Slutsats:** Web API ger flexibilitet, skalbarhet och möjlighet att bygga hela ekosystem av applikationer som alla delar samma data och logik!

---
**Status: ✅ KLART** (Vi byggde vidare på detta med Student/Course/CourseInstance API)