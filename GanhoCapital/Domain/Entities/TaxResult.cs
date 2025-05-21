using System.Text.Json.Serialization;
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Domain.Entities
{
    /// <summary>
    /// Implementa��o concreta do resultado de imposto
    /// </summary>
    public class TaxResult : ITaxResult
    {
        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }
    }
}