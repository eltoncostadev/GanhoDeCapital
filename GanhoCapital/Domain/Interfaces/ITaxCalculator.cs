namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para implementação de calculadoras de impostos
    /// </summary>
    public interface ITaxCalculator
    {
        /// <summary>
        /// Calcula o imposto para uma operação
        /// </summary>
        decimal CalculateTax(IOperation operation);
    }
}