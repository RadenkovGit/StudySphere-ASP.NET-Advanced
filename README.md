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

The web project follows a layered structure:

- **Data**
  - `ApplicationDbContext`
  - entity models
  - EF Core persistence
- **Services**
  - business logic behind controllers
  - contracts separated in `Services/Contracts`
- **Controllers**
  - user-facing MVC controllers
  - admin controllers under `Areas/Admin`
- **Models**
  - input models and view models used by the UI
- **Infrastructure / Extensions**
  - seeding
  - helper extensions for authenticated user access

This structure keeps controllers lightweight and moves business rules into services, which also makes testing easier.

## Service layer responsibilities

The main services are responsible for:

- retrieving and filtering course data
- loading lesson and resource details
- managing user reviews
- managing personal study tasks
- aggregating admin dashboard information

This keeps the application closer to a clean separation between presentation logic and business logic.

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
5. Build the solution.
6. Run the web project.
7. On first startup, the database is created and seeded automatically.
8. Log in with the seeded administrator account if you want to test the admin area.
9. Optionally run the automated tests with `dotnet test` or Test Explorer.
10. If the seeded data does not appear, restart the application and verify the database connection again.
11. For manual testing, use both a normal user flow and the seeded administrator account.

## Deployment notes

The project can be deployed to a standard .NET hosting environment such as Azure App Service or another ASP.NET Core compatible host.

Before deployment, make sure that:

- the production connection string is configured correctly
- the target database server is reachable from the hosting environment
- the required environment variables and secrets are configured
- static files and Razor views are published correctly
- the application starts successfully with the production configuration

## Default administrator access

After the initial database seeding, the application provides a default administrator account:

- email: `admin@studysphere.local`
- password: `Admin123!`

This account can be used to open the admin dashboard and manage categories and courses.

## Environment configuration notes

Before running the application in a different environment, verify the following:

- the connection string points to the correct SQL Server / LocalDB instance
- the database user has sufficient permissions
- the seeded administrator account is created successfully
- the application can access static files and Razor views correctly
- the database is initialized before testing the admin area

## Testing

Run the test project with:

```bash
dotnet test

