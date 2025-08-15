# PDV System Optimization Guidelines

## Code Quality Standards

### Clean Code Principles
- Use meaningful names for variables, methods, and classes
- Keep methods small and focused on single responsibility
- Avoid deep nesting - prefer early returns
- Use consistent naming conventions (PascalCase for public members, camelCase for private)
- Remove dead code and unused using statements

### SOLID Principles
- Single Responsibility: Each class should have one reason to change
- Open/Closed: Open for extension, closed for modification
- Liskov Substitution: Derived classes must be substitutable for base classes
- Interface Segregation: Many client-specific interfaces are better than one general-purpose interface
- Dependency Inversion: Depend on abstractions, not concretions

## Architecture Guidelines

### Clean Architecture Layers
- **Domain**: Contains business entities, value objects, and domain services
- **Application**: Contains use cases, commands, queries, and handlers
- **Infrastructure**: Contains data access, external services, and technical concerns
- **Presentation**: Contains controllers, UI, and API endpoints

### Dependency Rules
- Dependencies should point inward toward the domain
- Domain layer should not depend on any other layer
- Application layer can depend only on domain
- Infrastructure and Presentation can depend on Application and Domain

## Implementation Standards

### Error Handling
- Use custom exceptions for business logic errors
- Implement global exception handling middleware
- Log exceptions with appropriate context
- Return meaningful error messages to clients
- Use proper HTTP status codes

### Validation
- Use FluentValidation for input validation
- Validate at the application boundary (controllers, handlers)
- Separate validation logic from business logic
- Provide clear validation error messages

### Async/Await
- Use async/await for all I/O operations
- Pass CancellationToken to async methods
- Avoid async void except for event handlers
- Use ConfigureAwait(false) in library code

### Testing
- Write unit tests for business logic
- Write integration tests for data access
- Use meaningful test names that describe the scenario
- Follow AAA pattern (Arrange, Act, Assert)
- Mock external dependencies

## Database Guidelines

### Entity Framework Best Practices
- Use async methods for database operations
- Implement proper entity configurations
- Use migrations for schema changes
- Avoid N+1 queries with proper includes
- Use projection for read-only scenarios

### Performance
- Add appropriate indexes for frequently queried columns
- Use pagination for large result sets
- Implement caching for frequently accessed data
- Monitor and optimize slow queries

## Security Guidelines

### Authentication & Authorization
- Use JWT tokens for API authentication
- Implement role-based access control
- Validate permissions at the action level
- Use HTTPS for all communications
- Hash passwords with strong algorithms (bcrypt, Argon2)

### Input Validation
- Validate all input at the API boundary
- Sanitize user input to prevent injection attacks
- Use parameterized queries to prevent SQL injection
- Implement rate limiting for API endpoints

## Build and Deployment

### Build Process
- Run tests on every build
- Use static code analysis tools
- Check for security vulnerabilities
- Ensure all warnings are addressed

### Configuration
- Use appsettings.json for configuration
- Override with environment variables for deployment
- Keep secrets out of source code
- Use different configurations for different environments

## Specific to PDV System

### Business Rules
- Always validate stock availability before sales
- Update inventory in real-time during transactions
- Maintain audit trail for all financial operations
- Ensure data consistency across related entities

### Performance Considerations
- Cache frequently accessed product information
- Use background services for heavy operations
- Implement proper connection pooling
- Monitor database performance and optimize queries

### Integration Points
- RabbitMQ for asynchronous processing
- External payment gateways
- Fiscal receipt services (SEFAZ integration)
- Backup and monitoring systems