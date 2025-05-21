namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para f�bricas de processadores de opera��es
    /// </summary>
    public interface IOperationProcessorFactory
    {
        IOperationProcessor CreateProcessor();
    }
}