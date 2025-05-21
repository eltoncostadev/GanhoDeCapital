namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Representa uma operação financeira
    /// </summary>
    public interface IOperation
    {
        string OperationType { get; }
        decimal UnitCost { get; }
        int Quantity { get; }
        decimal TotalValue { get; }
    }
}