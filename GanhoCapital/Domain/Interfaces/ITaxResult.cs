namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Representa o resultado do cálculo de imposto
    /// </summary>
    public interface ITaxResult
    {
        decimal Tax { get; }
    }
}