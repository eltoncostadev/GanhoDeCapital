namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para processadores de operações financeiras
    /// </summary>
    public interface IOperationProcessor
    {
        /// <summary>
        /// Processa uma operação e retorna o imposto devido
        /// </summary>
        decimal ProcessOperation(IOperation operation);
        
        /// <summary>
        /// Reseta o estado do processador para uma nova simulação
        /// </summary>
        void Reset();
    }
}