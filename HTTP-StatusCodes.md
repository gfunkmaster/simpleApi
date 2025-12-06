# Övning 1 - HTTP-svarskoder

## Förklaring av HTTP-statuskoder

### 200 OK
- **Vad det betyder:** Begäran lyckades perfekt
- **När används:** 
  - GET-requests som returnerar data
  - PUT-requests som uppdaterar befintlig data
  - Alla framgångsrika operationer där data returneras
- **Exempel:** GET /students returnerar lista med studenter

### 201 Created
- **Vad det betyder:** En ny resurs har skapats framgångsrikt
- **När används:**
  - POST-requests som skapar nya objekt
  - När något nytt läggs till i databasen/listan
- **Exempel:** POST /students skapar en ny student

### 204 No Content
- **Vad det betyder:** Operationen lyckades men ingen data returneras
- **När används:**
  - DELETE-requests som tar bort något
  - PUT/PATCH när uppdatering lyckas men inget behöver returneras
- **Exempel:** DELETE /students/1 tar bort student med ID 1

### 400 Bad Request
- **Vad det betyder:** Klienten skickade felaktig eller ogiltig data
- **När används:**
  - Saknade obligatoriska fält
  - Felaktigt format på data
  - Ogiltiga värden (t.ex. negativt ID)
  - Valideringsfel
- **Exempel:** POST /students utan namn eller email

### 404 Not Found
- **Vad det betyder:** Den begärda resursen kunde inte hittas
- **När används:**
  - GET /students/999 där student 999 inte existerar
  - PUT/DELETE på icke-existerande objekt
  - Felaktiga URL:er
- **Exempel:** GET /students/123 när student 123 inte finns

### 500 Internal Server Error
- **Vad det betyder:** Ett oväntat fel uppstod på servern
- **När används:**
  - Programkrascher/exceptions
  - Databasfel
  - Oväntade fel i koden
  - Problem som klienten inte kan lösa
- **Exempel:** Databas är nere eller kod har en bugg

## Sammanfattning
- **2xx** = Framgång (200, 201, 204)
- **4xx** = Klientfel (400, 404) 
- **5xx** = Serverfel (500)