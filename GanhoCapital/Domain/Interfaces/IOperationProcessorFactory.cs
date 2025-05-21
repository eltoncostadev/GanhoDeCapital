namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para fábricas de processadores de operações
    /// </summary>
    public interface IOperationProcessorFactory
    {
        IOperationProcessor CreateProcessor();
    }
}