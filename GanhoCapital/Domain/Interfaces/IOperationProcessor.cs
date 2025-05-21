namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para processadores de opera��es financeiras
    /// </summary>
    public interface IOperationProcessor
    {
        /// <summary>
        /// Processa uma opera��o e retorna o imposto devido
        /// </summary>
        decimal ProcessOperation(IOperation operation);
        
        /// <summary>
        /// Reseta o estado do processador para uma nova simula��o
        /// </summary>
        void Reset();
    }
}