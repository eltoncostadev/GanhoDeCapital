using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Domain.Services.Abstract
{
    /// <summary>
    /// Classe base abstrata para calculadoras de impostos
    /// </summary>
    public abstract class AbstractTaxCalculator : ITaxCalculator
    {
        /// <summary>
        /// Método template para cálculo de impostos
        /// </summary>
        public decimal CalculateTax(IOperation operation)
        {
            // Calcula o lucro ou prejuízo
            var profitOrLoss = CalculateProfitOrLoss(operation);
            
            // Se for prejuízo, não tem imposto
            if (profitOrLoss <= 0)
                return 0;
                
            // Se for isento, não tem imposto, mas o lucro ainda é usado para deduzir prejuízos
            if (IsExempt(operation))
                return 0;
                
            // Aplica a alíquota de imposto
            return ApplyTaxRate(profitOrLoss);
        }
        
        // Métodos abstratos que devem ser implementados por classes concretas
        protected abstract bool IsExempt(IOperation operation);
        protected abstract decimal CalculateProfitOrLoss(IOperation operation);
        protected abstract decimal ApplyTaxRate(decimal profit);
    }
}