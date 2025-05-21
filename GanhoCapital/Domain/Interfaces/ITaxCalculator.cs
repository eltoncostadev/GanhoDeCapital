namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para implementa��o de calculadoras de impostos
    /// </summary>
    public interface ITaxCalculator
    {
        /// <summary>
        /// Calcula o imposto para uma opera��o
        /// </summary>
        decimal CalculateTax(IOperation operation);
    }
}