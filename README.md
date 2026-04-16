# StudySphere

StudySphere is an ASP.NET Core MVC web application for organizing learning content, study tasks and course feedback.  
The project is designed to satisfy the typical requirements of an **ASP.NET Advanced** individual final project: layered architecture, services, Identity, validation, seeding, admin area, custom error pages, pagination, filtering and unit tests.

## Project idea

StudySphere is a lightweight learning planner focused on programming education.
It combines a course catalog, curated learning resources, personal study task tracking and course feedback in one web application.

The main goal of the platform is to help users organize self-paced learning in a structured way:
browse courses, inspect lessons and resources, enroll, track personal study work and evaluate course quality through reviews.
Administrators can maintain the catalog through a dedicated administration area.

## Main features

- ASP.NET Core MVC application with Razor views
- SQL Server database with Entity Framework Core
- ASP.NET Identity with **User** and **Administrator** roles
- Separate admin area for course and category management
- Course catalog with filtering by category and keyword search
- Course details page with lessons, linked resources and reviews
- Enrollment workflow for authenticated users
- Personal study task manager with create, edit, delete and completion toggle
- Resource catalog with search and pagination
- Review management for authenticated users
- Custom 404 and 500 error pages
- Anti-forgery protection, server-side validation and encoded Razor output
- Dependency Injection with service contracts and implementations
- Seeded initial data and default administrator account
- Unit tests for core service logic

## Architecture

The solution contains two projects:

- `src/StudySphere.Web` – main MVC application
- `tests/StudySphere.Tests` – xUnit unit tests

The web project is organized in these layers:

- **Data**
  - `ApplicationDbContext`
  - entity models
- **Services**
  - business logic behind controllers
  - contracts separated in `Services/Contracts`
- **Controllers**
  - user-facing MVC controllers
  - admin controllers under `Areas/Admin`
- **Models**
  - input models and view models
- **Infrastructure / Extensions**
  - seeding
  - claims extensions

## Entity models

The solution includes at least 6 business entities:

- `Category`
- `Course`
- `Lesson`
- `StudyTask`
- `ResourceItem`
- `Enrollment`
- `Review`

It also extends `ApplicationUser` from ASP.NET Identity.

## Pages / views coverage

The project contains more than 15 views, including:

- Home
- Privacy
- Courses index / details
- Lessons details
- Tasks index / create / edit / delete
- Resources index / details
- Reviews mine / edit / delete
- Admin dashboard
- Admin courses index / create / edit / delete
- Admin categories index / create / edit / delete
- Custom 404 / 500 pages
- partial views for forms, pagination, cards and status messages

## Validation and security

- Data annotations for input validation
- Auto anti-forgery validation on all unsafe HTTP verbs
- Identity-based authentication and role-based authorization
- Razor output encoding protects against XSS in rendered content
- EF Core LINQ queries prevent SQL injection by parameterization
- Restricted admin endpoints through role checks
- User-owned data checks in services for tasks and reviews

## Seed data

On application startup, the seeder ensures:

- Roles: `Administrator`, `User`
- Default administrator:
  - email: `admin@studysphere.local`
  - password: `Admin123!`
- Initial categories
- Initial courses
- Initial lessons
- Initial resources

## Technologies

- C#
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server / LocalDB
- ASP.NET Identity
- Bootstrap 5
- xUnit

## How to run locally

1. Open the solution in **Visual Studio 2022** or **JetBrains Rider**.
2. Restore NuGet packages.
3. Make sure LocalDB / SQL Server is available.
4. Verify the connection string in `src/StudySphere.Web/appsettings.json`.
5. Run the web project.
6. On first startup, the database is created and seeded automatically.

## Testing

Run the test project with:

```bash
dotnet test
```
