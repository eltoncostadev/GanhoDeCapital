# Capital Gains Calculator

A command-line interface (CLI) application that initially calculates taxes to be paid on profits or losses from stock market trading operations, implemented in C# with .NET 8.

## Technical and Architectural Decisions

### Architecture Overview

The solution was structured following Clean Architecture principles and domain-driven design (DDD), with a clear separation of responsibilities through well-defined layers:

1. **Domain Layer**: Contains domain entities, interfaces, and business rules
2. **Application Layer**: Orchestrates application flow and business rule execution
3. **Infrastructure Layer**: Implements input/output and user interaction

### Main Components

#### Domain Layer
- **Entities**: `Operation`, `TaxResult`
- **Interfaces**: `IOperation`, `IOperationProcessor`, `IOperationProcessorFactory`, `IPortfolioState`, `ITaxCalculator`, `ITaxResult`
- **Abstract Services**: `AbstractOperationProcessor`, `AbstractTaxCalculator`
- **Concrete Services**: `PortfolioState`, `StockTaxCalculator`, `StockOperationProcessor`

#### Application Layer
- **TaxCalculationService**: Main service that processes input operations and produces results
- **Factories**: `StockOperationProcessorFactory`, implementing the Factory Method Pattern

#### Infrastructure Layer
- **Program**: Application entry point, manages I/O and initializes services

### Design Patterns Used

- **Strategy Pattern**: Different implementations of `ITaxCalculator` for specific tax calculations
- **Template Method**: Abstract classes like `AbstractOperationProcessor` with methods that can be customized
- **Factory Method**: `IOperationProcessorFactory` to create appropriate instances
- **Dependency Injection**: For flexibility, testability, and inversion of control

### Extensibility

The architecture was designed to be easily extensible:

1. **New Asset Types**: New implementations of `ITaxCalculator` can be created for different tax rules (e.g., cryptocurrencies, real estate funds)
2. **New Business Rules**: Operation processing can be extended through new implementations of `IOperationProcessor`
3. **Requirement Changes**: Changes in tax rules can be isolated in specific implementing classes

### Testing Approach

The solution includes comprehensive unit and integration tests:

- **Unit Tests**: Test isolated components with mocked dependencies
- **Integration Tests**: Test the complete application flow, validating all specification cases

## Compilation and Execution

### Prerequisites

- .NET 8 SDK installed
- Operating system: Windows, Linux, or macOS

### Compilation

# Compile the project
```bash
dotnet build
```

### Execution | Usage Examples

```bash
dotnet run --project GanhoCapital
```

### And manually input the operations
```bash
[{"operation":"buy", "unit-cost":10.00, "quantity": 100},{"operation":"sell", "unit-cost":15.00, "quantity": 50},{"operation":"sell", "unit-cost":15.00, "quantity": 50}]
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000},{"operation":"sell", "unit-cost":20.00, "quantity": 5000},{"operation":"sell", "unit-cost":5.00, "quantity": 5000}]
```

## Running Tests

### Execute All Tests

```bash
dotnet test .\GanhoCapitalTests\GanhoCapitalTests.csproj
```

## Additional Notes

### Input/Output Format

- **Input**: Lists of operations in JSON format, one list per line
- **Output**: Lists of tax results in JSON format, one list per line

### Decimal Precision

The solution uses the `decimal` type for financial calculations, ensuring precision in monetary operations, and rounds values to two decimal places according to the specification.

### Error Handling

Although the specification assumes error-free inputs, the application implements basic validations for inconsistent operations, such as trying to sell more shares than owned.
