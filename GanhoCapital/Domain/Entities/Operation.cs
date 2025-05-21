using System.Text.Json.Serialization;
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Domain.Entities
{
    /// <summary>
    /// Implementação concreta de uma operação financeira
    /// </summary>
    public class Operation : IOperation
    {
        [JsonPropertyName("operation")]
        public string OperationName { get; set; } = string.Empty;

        [JsonPropertyName("unit-cost")]
        public decimal UnitCost { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        
        public string OperationType => OperationName;
        public decimal TotalValue => UnitCost * Quantity;
    }
}