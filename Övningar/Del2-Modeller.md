# Del 2 - Modeller och Endpoints

## Övning 1 – Skapa en modell för Student
Skapa en klass Student med egenskaper:
- Id (int)
- Name (string)
- Email (string)

Det spelar ingen roll vilken typ av klass ni väljer här. Det kan vara traditionella klasser eller primary constructor. Välj det som passar er bäst.

**a.** Använd nu modellen genom att skapa ett studentobjekt i Program.cs.
**b.** Skapa nu en ny endpoint som skall returnera ditt studentobjekt.
**c.** Fortsätt att skapa en lista med studentobjekt, fortfarande i Program.cs
**d.** Skapa en ny endpoint som returnerar listan med studentobjekten.

## Övning 2 – Skapa en modell för Course
• Skapa en klass Course med egenskaper:
  - Id (int)
  - Title (string)
  - Description (string)

Poängen med denna klass är att vi strax skall koppla ihop den med studenter för att ta reda på vilka studenter som går på vilka kurser. Men vi behöver också ha en endpoint som visar oss vilka kurser som finns och kanske även en endpoint som innehåller information för en given kurs.

**a.** Skapa en lista med kursobjekt i Program.cs
**b.** Skapa en ny endpoint som returnerar listan med kurser.
**c.** Extrauppgift: Skapa en endpoint som returnerar en specifik kurs baserat på kursens id.

## Övning 3 – Skapa en modell för CourseInstance (kurstillfälle)
• Skapa en klass CourseInstance med egenskaper:
  - Id (int)
  - StartDate (DateTime)
  - EndDate (DateTime)
  - En egenskap Course som visar vilken kurs det gäller.
  - En lista Students som visar vilka studenter som deltar.

Poängen med denna klass är att innehålla information om vilka studenter som går på vilka kurser. Vi jobbar vidare med lite fler datatyper för att träna på dem.

**a.** Skapa en lista med kurstillfällen i Program.cs (ni börjar se att det blir mycket data i Program.cs nu? Detta är ett problem som vi skall lösa senare).
**b.** Skapa en ny endpoint som returnerar listan med kurstillfällen.
**c.** Extrauppgift: Skapa en ny endpoint som returnerar alla kurser som en given student går på.
**d.** Extrauppgift: Skapa en ny endpoint som returnerar alla kurser mellan två givna datum.

## Övning 4 – Skapa en modell för Grade (betyg)
• Skapa en klass Grade med egenskaper:
  - Id (int)
  - Value (string, ex. "A", "B", "C", …)
  - CourseInstance – Beskriver vilket kurstillfälle som skall användas
  - Student – Beskriver vilken student som skall få betyget för kurstillfället.

**❗ Viktigt:** Grade tillhör både en student och ett kurstillfälle, inte studenten direkt.

Poängen med denna klass är att innehålla en beskrivning av våra betyg. Ni kan välja vilka värden på betyg som ni själva vill.

**a.** Skapa en lista med grade-objekt i Program.cs.
**b.** Skapa en endpoint som returnerar betygobjekten.
**c.** Du kan nu skapa egna endpoints som t.ex. visar alla betyg för en student samt vilka kurser som betyget gäller.

## Övning 5 – Reflektionsfråga
• **Varför är det en bättre modellering att koppla Grade till Student + CourseInstance än att lägga en lista med betyg direkt på studenten?**

### 💡 **Svar:**

Det är bättre att koppla Grade till både Student och CourseInstance av flera viktiga anledningar:

#### **1. Specifikt kurstillfälle**
- En kurs (t.ex. "Programming") kan köras flera gånger per år
- Samma student kan ta samma kurs flera gånger  
- Betyget gäller för ett specifikt kurstillfälle, inte bara kursen generellt
- **Exempel:** Alice kan få "C" på Programming våren 2024 och "A" på Programming hösten 2024

#### **2. Tidsaspekt och kontext**
- CourseInstance innehåller StartDate och EndDate
- Viktigt att veta NÄR betyget gavs
- Olika lärare kan ha olika kurser vid olika tillfällen
- Curriculum kan ändras mellan kurstillfällen

#### **3. Datamodellering och normalisering**
- Undviker duplicering av data
- Om betyget låg direkt på Student skulle vi behöva upprepa kursinformation
- Grade skapar en tydlig relation mellan Student och CourseInstance
- Följer relationsdatabas-principer

#### **4. Flexibilitet och skalbarhet**
- Lätt att hitta alla betyg för ett specifikt kurstillfälle
- Lätt att hitta alla betyg för en student
- Kan enkelt lägga till fler attribut på Grade (datum, kommentarer, etc.)
- Stödjer komplexa queries som "alla studenter som fick A i Programming under 2024"

#### **5. Verkliga scenarion**
I verkligheten:
- Studenter tar om kurser
- Kurser ges flera gånger
- Betyg gäller för specifika terminer/år
- Detta är samma modell som universitets betygssystem använder

**Slutsats:** Grade som en separat entitet som kopplar Student till CourseInstance ger en mer korrekt, flexibel och skalbar datamodell som bättre representerar verkligheten.

---
**Status: ✅ KLART** (Alla modeller skapade med controllers och full CRUD-funktionalitet)