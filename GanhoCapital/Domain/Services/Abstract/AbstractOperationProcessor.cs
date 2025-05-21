using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Domain.Services.Abstract
{
    /// <summary>
    /// Classe base abstrata para processadores de operações
    /// Implementa o template method pattern
    /// </summary>
    public abstract class AbstractOperationProcessor : IOperationProcessor
    {
        protected readonly ITaxCalculator _taxCalculator;
        
        protected AbstractOperationProcessor(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }
        
        public decimal ProcessOperation(IOperation operation)
        {
            // Template method
            PreProcessOperation(operation);
            var tax = CalculateTax(operation);
            PostProcessOperation(operation, tax);
            
            return tax;
        }
        
        public abstract void Reset();
        
        // Métodos abstratos que devem ser implementados por classes concretas
        protected abstract void PreProcessOperation(IOperation operation);
        protected abstract decimal CalculateTax(IOperation operation);
        protected abstract void PostProcessOperation(IOperation operation, decimal tax);
    }
}