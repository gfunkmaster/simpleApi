# SimpleApi - .NET Web API with Entity Framework Core

A comprehensive RESTful API built with .NET 8, Entity Framework Core, and In-Memory database for learning purposes.

## 🚀 Features

- **Full CRUD Operations** for Students, Courses, CourseInstances, and Grades
- **Entity Framework Core** with In-Memory database
- **One-to-Many Relationships** (Course → CourseInstance, Student → Grade, etc.)
- **Many-to-Many Relationships** (Student ↔ CourseInstance enrollment)
- **Data Validation** with custom attributes and model validation
- **Repository Pattern** with dependency injection
- **Comprehensive HTTP Status Codes** (200, 201, 400, 404, etc.)
- **Enrollment Management** system for students in course instances

## 🏗️ Architecture

```
SimpleApi/
├── Controllers/           # API Controllers (Students, Courses, etc.)
├── Services/             # Business logic layer
├── Repositories/         # Data access layer with EF Core
│   └── Interfaces/       # Repository interfaces
├── Data/                 # EF Core DbContext and configuration
├── src/Models/           # Entity models (Student, Course, etc.)
├── Validations/          # Custom validation attributes
└── Övningar/            # Exercise documentation (Swedish)
```

## 📊 Data Model

### Entities
- **Student** - Name, Email, Address
- **Course** - Title, Description  
- **CourseInstance** - StartDate, EndDate, Course reference
- **Grade** - Value, Student reference, CourseInstance reference

### Relationships
- `Course` → `CourseInstance` (One-to-Many)
- `Student` → `Grade` (One-to-Many) 
- `CourseInstance` → `Grade` (One-to-Many)
- `Student` ↔ `CourseInstance` (Many-to-Many via EnrolledStudents)

## 🛠️ Setup & Installation

### Prerequisites
- .NET 8 SDK
- Visual Studio Code or Visual Studio 2022

### Installation
1. Clone the repository:
```bash
git clone https://github.com/gfunkmaster/simpleApi.git
cd simpleApi
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

The API will be available at `http://localhost:5077`

## 📡 API Endpoints

### Students API
- `GET /api/Students` - Get all students
- `GET /api/Students/{id}` - Get student by ID
- `POST /api/Students` - Create new student
- `PUT /api/Students/{id}` - Update student
- `DELETE /api/Students/{id}` - Delete student
- `GET /api/Students/search/{name}` - Search students by name

### Courses API
- `GET /api/Courses` - Get all courses
- `GET /api/Courses/{id}` - Get course by ID
- `POST /api/Courses` - Create new course
- `PUT /api/Courses/{id}` - Update course
- `DELETE /api/Courses/{id}` - Delete course

### Course Instances API
- `GET /api/CourseInstances` - Get all course instances
- `GET /api/CourseInstances/{id}` - Get course instance by ID
- `POST /api/CourseInstances` - Create new course instance
- `PUT /api/CourseInstances/{id}` - Update course instance
- `DELETE /api/CourseInstances/{id}` - Delete course instance
- `GET /api/CourseInstances/date-range/{from}/{to}` - Filter by date range
- `GET /api/CourseInstances/student/{studentId}` - Get instances for student

### Enrollment API (Many-to-Many)
- `POST /api/CourseInstances/{courseInstanceId}/enroll/{studentId}` - Enroll student
- `DELETE /api/CourseInstances/{courseInstanceId}/unenroll/{studentId}` - Unenroll student

### Grades API
- `GET /api/Grades` - Get all grades (with nested relationships)
- `GET /api/Grades/{id}` - Get grade by ID
- `POST /api/Grades` - Create new grade
- `PUT /api/Grades/{id}` - Update grade
- `DELETE /api/Grades/{id}` - Delete grade
- `GET /api/Grades/student/{studentId}` - Get grades for student
- `GET /api/Grades/courseinstance/{courseInstanceId}` - Get grades for course instance

## 🧪 Testing

### Using the HTTP file
The project includes `SimpleApi.http` with all API endpoints ready to test:

1. Open `SimpleApi.http` in VS Code
2. Make sure the API is running (`dotnet run`)
3. Click "Send Request" on any endpoint to test

### Example API Calls

**Create a Student:**
```http
POST http://localhost:5077/api/Students
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "address": {
    "street": "123 Main St",
    "city": "Stockholm",
    "zipCode": "12345"
  }
}
```

**Enroll Student in Course Instance:**
```http
POST http://localhost:5077/api/CourseInstances/1/enroll/1
```

## 🔧 Technologies Used

- **.NET 8** - Web API framework
- **Entity Framework Core 8.0.11** - ORM with In-Memory provider
- **ASP.NET Core** - Web framework
- **System.ComponentModel.DataAnnotations** - Model validation
- **Dependency Injection** - Built-in DI container

## 📝 Custom Validations

The API includes several custom validation attributes:
- **CustomEmailAttribute** - Enhanced email validation
- **EndDateAfterStartDateAttribute** - Ensures end date is after start date
- **FutureDateAttribute** - Validates future dates
- **ValidGradeAttribute** - Validates grade values (A, B, C, D, F)

## 📚 Learning Exercises

This project was built following structured exercises in Swedish:
- **Del 1** - Introduction to basic Web API
- **Del 2** - Models and data structures
- **Del 3** - HTTP Status Codes implementation
- **Del 4** - Repository architecture pattern
- **Del 5** - Data validation and custom attributes
- **Del 6** - Entity Framework Core with relationships
- **Del 7** – Identity and JWT Authentication
  - ASP.NET Identity integration
  - User registration and login
  - JWT token generation and validation
  - Protecting endpoints with [Authorize]
  - Role-based authorization (Student/Admin)
  - Claims and custom claims in JWT

See the `Övningar/` folder for detailed exercise documentation.

## 🗃️ Database

Currently uses **EF Core In-Memory database** for:
- ✅ Quick setup and testing
- ✅ No external dependencies
- ✅ Perfect for learning and development

**Note:** Data is reset on each application restart. For production use, migrate to SQL Server or PostgreSQL.

## 🎯 Sample Data

The application includes seed data:
- **3 Students** (John Doe, Jane Smith, Bob Johnson)
- **2 Courses** (Programming, Math)
- **2 Course Instances** (linked to courses)
- **3 Grades** (linking students to course instances)

## 🚀 Next Steps

To extend this project:
1. **Add Authentication** (JWT tokens, Identity)
2. **Migrate to SQL Server** with EF Core Migrations
3. **Add Unit Tests** (xUnit, Moq)
4. **Implement Caching** (Redis, Memory Cache)
5. **Add API Documentation** (Swagger/OpenAPI)
6. **Deploy to Cloud** (Azure, AWS)

## 🤝 Contributing

This is a learning project, but feel free to:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📄 License

This project is for educational purposes. Feel free to use and modify as needed for learning.

---

**Built with ❤️ for learning .NET Web API development**