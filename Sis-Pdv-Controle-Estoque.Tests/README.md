# PDV System Test Suite

This comprehensive test suite covers all aspects of the PDV (Point of Sale) system, following best practices for testing in .NET applications.

## Test Structure

The test suite is organized into the following categories:

### 📁 Test Organization

```
Sis-Pdv-Controle-Estoque.Tests/
├── Infrastructure/           # Test infrastructure and base classes
│   ├── TestFixture.cs       # Service provider setup for tests
│   ├── IntegrationTestBase.cs # Base class for integration tests
│   └── WebApplicationTestFixture.cs # Web API test setup
├── UnitTests/               # Unit tests with mocking
│   ├── Handlers/           # Command/Query handler tests
│   ├── Services/           # Service layer tests
│   └── Validators/         # Validation logic tests
├── IntegrationTests/       # Integration tests with real dependencies
│   ├── Controllers/        # API endpoint tests
│   └── Repositories/       # Data access tests
└── EndToEndTests/          # Complete workflow tests
    ├── SalesWorkflowTests.cs
    ├── InventoryWorkflowTests.cs
    └── UserManagementWorkflowTests.cs
```

## Test Categories

### 🔬 Unit Tests
- **Purpose**: Test individual components in isolation
- **Scope**: Handlers, Services, Validators
- **Dependencies**: Mocked using Moq
- **Database**: Not used (mocked repositories)

### 🔗 Integration Tests
- **Purpose**: Test component interactions
- **Scope**: Controllers, Repositories, Database operations
- **Dependencies**: Real services with in-memory database
- **Database**: In-memory Entity Framework

### 🌐 End-to-End Tests
- **Purpose**: Test complete business workflows
- **Scope**: Full user scenarios across multiple components
- **Dependencies**: Full application stack
- **Database**: In-memory database with test data

## Running Tests

### Prerequisites
- .NET 8 SDK
- All project dependencies restored

### Quick Start
```bash
# Run all tests
dotnet test

# Or use the PowerShell script
.\run-tests.ps1
```

### Test Categories
```bash
# Unit tests only
.\run-tests.ps1 -TestCategory unit

# Integration tests only
.\run-tests.ps1 -TestCategory integration

# End-to-end tests only
.\run-tests.ps1 -TestCategory e2e

# Specific component tests
.\run-tests.ps1 -TestCategory handlers
.\run-tests.ps1 -TestCategory repositories
.\run-tests.ps1 -TestCategory controllers
```

### Coverage Reports
```bash
# Run tests with code coverage
.\run-tests.ps1 -Coverage

# Verbose output
.\run-tests.ps1 -Verbose
```

## Test Patterns

### AAA Pattern
All tests follow the Arrange-Act-Assert pattern:

```csharp
[Fact]
public async Task Handle_ValidRequest_ShouldCreateCliente()
{
    // Arrange
    var request = new AdicionarClienteRequest { /* test data */ };
    var mockRepository = new Mock<IRepositoryCliente>();
    
    // Act
    var result = await handler.Handle(request, CancellationToken.None);
    
    // Assert
    result.Should().NotBeNull();
    result.Success.Should().BeTrue();
}
```

### Test Data Generation
- **AutoFixture**: Generates test data automatically
- **Custom builders**: For specific test scenarios
- **Realistic data**: Uses valid business data formats

### Assertions
- **FluentAssertions**: Provides readable assertions
- **Specific validations**: Tests exact business rules
- **Error scenarios**: Validates error handling

## Key Test Scenarios

### 🛒 Sales Workflow
- Complete sale process from product creation to order completion
- Stock validation and updates
- Customer validation
- Payment processing simulation

### 📦 Inventory Management
- Stock adjustments and tracking
- Low stock alerts
- Movement history
- Bulk operations

### 👥 User Management
- User registration and authentication
- Role and permission management
- Session tracking
- Password security

### 🔒 Security Testing
- Authentication flows
- Authorization checks
- Input validation
- SQL injection prevention

### 📊 Data Validation
- Business rule enforcement
- Input sanitization
- Format validation (CPF/CNPJ, email, etc.)
- Duplicate prevention

## Test Configuration

### In-Memory Database
Tests use Entity Framework In-Memory provider for isolation:

```csharp
services.AddDbContext<PdvContext>(options =>
    options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
```

### Mock Services
External dependencies are mocked:

```csharp
var mockRepository = new Mock<IRepositoryCliente>();
mockRepository.Setup(x => x.Existe(It.IsAny<Func<Cliente, bool>>()))
              .Returns(false);
```

### Test Authentication
API tests can simulate authentication:

```csharp
// Tests can include JWT tokens for authenticated endpoints
client.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", testToken);
```

## Continuous Integration

### Build Pipeline
Tests are designed to run in CI/CD pipelines:

```yaml
- name: Run Tests
  run: dotnet test --configuration Release --logger trx --collect:"XPlat Code Coverage"
```

### Test Reports
- **TRX format**: For Azure DevOps integration
- **Coverage reports**: XML format for SonarQube
- **JUnit format**: For Jenkins integration

## Best Practices

### ✅ Do's
- Write tests for all business logic
- Use meaningful test names that describe the scenario
- Test both success and failure paths
- Keep tests independent and isolated
- Use realistic test data
- Mock external dependencies
- Test edge cases and boundary conditions

### ❌ Don'ts
- Don't test framework code (Entity Framework, ASP.NET Core)
- Don't make tests dependent on each other
- Don't use production data in tests
- Don't skip error scenario testing
- Don't write overly complex test setups

## Troubleshooting

### Common Issues

**Tests fail with database errors:**
- Ensure in-memory database is properly configured
- Check that test data doesn't conflict
- Verify entity configurations are correct

**Mock setup issues:**
- Verify mock setups match actual method signatures
- Check that async methods are properly mocked
- Ensure return types match expectations

**Integration test failures:**
- Check that all required services are registered
- Verify configuration values are set correctly
- Ensure test database is properly seeded

### Debug Tips
- Use `--verbosity normal` for detailed test output
- Add logging to test methods for debugging
- Use debugger breakpoints in test methods
- Check test output for specific error messages

## Contributing

When adding new tests:

1. Follow the existing folder structure
2. Use appropriate test category (Unit/Integration/E2E)
3. Include both positive and negative test cases
4. Add meaningful assertions with FluentAssertions
5. Update this README if adding new test patterns

## Coverage Goals

- **Unit Tests**: 80%+ code coverage
- **Integration Tests**: All API endpoints covered
- **E2E Tests**: All critical business workflows covered
- **Edge Cases**: All validation rules tested