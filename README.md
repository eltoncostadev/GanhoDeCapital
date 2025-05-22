# Calculadora de Ganho de Capital

Uma aplicação de linha de comando (CLI) que inicialmente calcula o imposto a ser pago sobre lucros ou prejuízos de operações no mercado financeiro de ações, implementada em C# com .NET 8.

## Decisões Técnicas e Arquiteturais

### Visão Geral da Arquitetura

A solução foi estruturada seguindo princípios de Clean Architecture e design orientado a domínio (DDD), com uma clara separação de responsabilidades através de camadas bem definidas:

1. **Domain Layer**: Contém as entidades de domínio, interfaces e regras de negócio
2. **Application Layer**: Orquestra o fluxo da aplicação e a execução das regras de negócio
3. **Infrastructure Layer**: Implementa a entrada/saída e a interação com o usuário

### Principais Componentes

#### Domain Layer
- **Entities**: `Operation`, `TaxResult`
- **Interfaces**: `IOperation`, `IOperationProcessor`, `IOperationProcessorFactory`, `IPortfolioState`, `ITaxCalculator`, `ITaxResult`
- **Abstract Services**: `AbstractOperationProcessor`, `AbstractTaxCalculator`
- **Concrete Services**: `PortfolioState`, `StockTaxCalculator`, `StockOperationProcessor`

#### Application Layer
- **TaxCalculationService**: Serviço principal que processa operações de entrada e produz resultados
- **Factories**: `StockOperationProcessorFactory`, implementando o Factory Method Pattern

#### Infrastructure Layer
- **Program**: Ponto de entrada da aplicação, gerencia I/O e inicializa os serviços

### Padrões de Design Utilizados

- **Strategy Pattern**: Diferentes implementações de `ITaxCalculator` para cálculos específicos de impostos
- **Template Method**: Classes abstratas como `AbstractOperationProcessor` com métodos que podem ser customizados
- **Factory Method**: `IOperationProcessorFactory` para criar instâncias apropriadas
- **Dependency Injection**: Para flexibilidade, testabilidade e inversão de controle

### Extensibilidade

A arquitetura foi projetada para ser facilmente extensível:

1. **Novos Tipos de Ativos**: Novas implementações de `ITaxCalculator` podem ser criadas para diferentes regras fiscais (ex: criptomoedas, fundos imobiliários)
2. **Novas Regras de Negócio**: O processamento de operações pode ser estendido através de novas implementações de `IOperationProcessor`
3. **Mudanças de Requisitos**: Alterações em regras fiscais podem ser isoladas nas classes implementadoras específicas

### Abordagem de Testes

A solução inclui testes unitários e de integração abrangentes:

- **Testes Unitários**: Testam componentes isolados com mock de dependências
- **Testes de Integração**: Testam o fluxo completo da aplicação, validando todos os casos da especificação

## Compilação e Execução

### Pré-requisitos

- .NET 8 SDK instalado
- Sistema operacional: Windows, Linux ou macOS

### Compilação

# Compilar o projeto
```bash
dotnet build
```

### Execução | Exemplos de Uso

```bash
dotnet run --project GanhoCapital
```

### E inserir manualmente as operações
```bash
[{"operation":"buy", "unit-cost":10.00, "quantity": 100},{"operation":"sell", "unit-cost":15.00, "quantity": 50},{"operation":"sell", "unit-cost":15.00, "quantity": 50}]
[{"operation":"buy", "unit-cost":10.00, "quantity": 10000},{"operation":"sell", "unit-cost":20.00, "quantity": 5000},{"operation":"sell", "unit-cost":5.00, "quantity": 5000}]
```


## Executando os Testes

### Executar Todos os Testes

```bash
dotnet test .\GanhoCapitalTests\GanhoCapitalTests.csproj
```

## Notas Adicionais

### Formato de Entrada/Saída

- **Entrada**: Listas de operações em formato JSON, uma lista por linha
- **Saída**: Listas de resultados de impostos em formato JSON, uma lista por linha

### Precisão Decimal

A solução utiliza o tipo `decimal` para cálculos financeiros, garantindo precisão em operações monetárias, e arredonda valores para duas casas decimais conforme a especificação.

### Tratamento de Erros

Embora a especificação assuma entradas sem erros, a aplicação implementa validações básicas para operações inconsistentes, como tentar vender mais ações do que se possui.