using CapitalGains.Domain.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CapitalGains.Application
{
    /// <summary>
    /// Serviço principal de cálculo de impostos que gerencia o processamento de operações
    /// </summary>
    public class TaxCalculationService
    {
        private readonly IOperationProcessorFactory _processorFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        
        public TaxCalculationService(IOperationProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }
        
        public IEnumerable<ITaxResult> ProcessOperations(IEnumerable<IOperation> operations)
        {
            var processor = _processorFactory.CreateProcessor();
            var results = new List<ITaxResult>();
            
            foreach (var operation in operations)
            {
                var tax = processor.ProcessOperation(operation);
                results.Add(new Domain.Entities.TaxResult { Tax = tax });
            }
            
            return results;
        }
        
        public string? ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;
                
            try
            {
                var operations = JsonSerializer.Deserialize<List<Domain.Entities.Operation>>(line, _jsonOptions);
                if (operations == null)
                    return "[]";
                
                var results = ProcessOperations(operations);
                return JsonSerializer.Serialize(results, _jsonOptions);
            }
            catch (JsonException ex)
            {
                Console.Error.WriteLine($"Erro ao processar JSON: {ex.Message}");
                return "[]";
            }
        }
    }
}