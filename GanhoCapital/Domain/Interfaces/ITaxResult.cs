namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Representa o resultado do c�lculo de imposto
    /// </summary>
    public interface ITaxResult
    {
        decimal Tax { get; }
    }
}