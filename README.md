# Calculadora de Ganho de Capital

Uma aplica��o de linha de comando (CLI) que inicialmente calcula o imposto a ser pago sobre lucros ou preju�zos de opera��es no mercado financeiro de a��es, implementada em C# com .NET 8.

## Decis�es T�cnicas e Arquiteturais

### Vis�o Geral da Arquitetura

A solu��o foi estruturada seguindo princ�pios de Clean Architecture e design orientado a dom�nio (DDD), com uma clara separa��o de responsabilidades atrav�s de camadas bem definidas:

1. **Domain Layer**: Cont�m as entidades de dom�nio, interfaces e regras de neg�cio
2. **Application Layer**: Orquestra o fluxo da aplica��o e a execu��o das regras de neg�cio
3. **Infrastructure Layer**: Implementa a entrada/sa�da e a intera��o com o usu�rio

### Principais Componentes

#### Domain Layer
- **Entities**: `Operation`, `TaxResult`
- **Interfaces**: `IOperation`, `IOperationProcessor`, `IOperationProcessorFactory`, `IPortfolioState`, `ITaxCalculator`, `ITaxResult`
- **Abstract Services**: `AbstractOperationProcessor`, `AbstractTaxCalculator`
- **Concrete Services**: `PortfolioState`, `StockTaxCalculator`, `StockOperationProcessor`

#### Application Layer
- **TaxCalculationService**: Servi�o principal que processa opera��es de entrada e produz resultados
- **Factories**: `StockOperationProcessorFactory`, implementando o Factory Method Pattern

#### Infrastructure Layer
- **Program**: Ponto de entrada da aplica��o, gerencia I/O e inicializa os servi�os

### Padr�es de Design Utilizados

- **Strategy Pattern**: Diferentes implementa��es de `ITaxCalculator` para c�lculos espec�ficos de impostos
- **Template Method**: Classes abstratas como `AbstractOperationProcessor` com m�todos que podem ser customizados
- **Factory Method**: `IOperationProcessorFactory` para criar inst�ncias apropriadas
- **Dependency Injection**: Para flexibilidade, testabilidade e invers�o de controle

### Extensibilidade

A arquitetura foi projetada para ser facilmente extens�vel:

1. **Novos Tipos de Ativos**: Novas implementa��es de `ITaxCalculator` podem ser criadas para diferentes regras fiscais (ex: criptomoedas, fundos imobili�rios)
2. **Novas Regras de Neg�cio**: O processamento de opera��es pode ser estendido atrav�s de novas implementa��es de `IOperationProcessor`
3. **Mudan�as de Requisitos**: Altera��es em regras fiscais podem ser isoladas nas classes implementadoras espec�ficas

### Abordagem de Testes

A solu��o inclui testes unit�rios e de integra��o abrangentes:

- **Testes Unit�rios**: Testam componentes isolados com mock de depend�ncias
- **Testes de Integra��o**: Testam o fluxo completo da aplica��o, validando todos os casos da especifica��o

## Compila��o e Execu��o

### Pr�-requisitos

- .NET 8 SDK instalado
- Sistema operacional: Windows, Linux ou macOS

### Compila��o

# Compilar o projeto
```bash
dotnet build
```

### Execu��o | Exemplos de Uso

```bash
dotnet run --project GanhoCapital
```

### E inserir manualmente as opera��es
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

### Formato de Entrada/Sa�da

- **Entrada**: Listas de opera��es em formato JSON, uma lista por linha
- **Sa�da**: Listas de resultados de impostos em formato JSON, uma lista por linha

### Precis�o Decimal

A solu��o utiliza o tipo `decimal` para c�lculos financeiros, garantindo precis�o em opera��es monet�rias, e arredonda valores para duas casas decimais conforme a especifica��o.

### Tratamento de Erros

Embora a especifica��o assuma entradas sem erros, a aplica��o implementa valida��es b�sicas para opera��es inconsistentes, como tentar vender mais a��es do que se possui.