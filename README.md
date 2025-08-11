# Dotnet Tutorial Guide 

This tutorial provides a conceptual roadmap for building a Web API project like TutoProject, focusing on architecture and best practices rather than code implementation 

## Project Architecture Overview

### 1. Layered Structure
- **Presentation Layer**: Controllers handle HTTP requests/responses
- **Business Logic**: Repository pattern encapsulates data access
- **Data Access**: Entity Framework Core with SQLite
- **Models**: Entity classes and DTOs

### 2. Key Components
- **Controllers**: Minimal endpoints that delegate to services
- **Repositories**: Abstract data access details
- **DbContext**: Bridge between code and database
- **Models**: Define application entities and data structures

## Development Workflow

### 1. Initial Setup
- Create WebAPI project template
- Configure base infrastructure (logging, dependency injection)
- Set up development environment (launch settings, app configurations)

### 2. Database Integration
- Entity Framework Core configuration
- SQLite database setup
- Migration system for schema changes
- Connection string management

### 3. API Development
- RESTful endpoint design
- Proper HTTP status codes
- Request/response payload design
- Error handling strategy

### 4. Documentation
- Swagger/OpenAPI integration
- Endpoint descriptions
- Example requests/responses
- API versioning considerations

## Best Practices Highlight

1. **Separation of Concerns**:
   - Keep controllers thin
   - Business logic in services
   - Data access in repositories

2. **Dependency Injection**:
   - Proper service registration
   - Interface-based dependencies
   - Scoped lifetime management

3. **Testing Approach**:
   - Unit testable components
   - Mocking dependencies
   - Integration test scenarios

4. **Performance Considerations**:
   - Async/await pattern
   - Efficient query design
   - Proper disposal of resources

## Teaching Recommendations

1. **Conceptual Roadmap**:
   - Start with project structure explanation
   - Explain dependency relationships
   - Highlight configuration flow

2. **Development Process**:
   - Demonstrate iterative building
   - Show debugging techniques
   - Explain migration workflow

3. **Common Pitfalls**:
   - Connection management
   - Async method chaining
   - Disposal patterns
   - Migration conflicts

4. **Extension Points**:
   - Where to add authentication
   - How to extend data model
   - Adding new API features
   - Performance optimization areas

### cmd useful instructions
Here's the complete tutorial with all command-line instructions clearly organized:



#### creating the Project
```bash
dotnet new webapi -n TutoProject
cd TutoProject
```

#### Adding Required NuGet Packages
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore  # For enhanced Swagger UI
```

#### Setting Up Database Context
```bash
# Create necessary folders (Windows)
mkdir Models Data Migrations

# Or on Linux/Mac
mkdir -p Models Data Migrations
```

#### Creating and Applying Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Running the Application
```bash
dotnet run
```

#### Testing API Endpoints
After running the application, open in your browser:
```
https://localhost:<port>/swagger
```

### Additional Useful Commands

### Check installed packages
```bash
dotnet list package
```

### Remove a package
```bash
dotnet remove package <PACKAGE_NAME>
```

### Create new migration after model changes
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Run in watch mode (auto-reload on changes)
```bash
dotnet watch run
```

### Restore packages
```bash
dotnet restore
```
