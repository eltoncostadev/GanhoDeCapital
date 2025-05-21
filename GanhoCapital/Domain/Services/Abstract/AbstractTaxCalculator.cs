using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Domain.Services.Abstract
{
    /// <summary>
    /// Classe base abstrata para calculadoras de impostos
    /// </summary>
    public abstract class AbstractTaxCalculator : ITaxCalculator
    {
        /// <summary>
        /// M�todo template para c�lculo de impostos
        /// </summary>
        public decimal CalculateTax(IOperation operation)
        {
            // Calcula o lucro ou preju�zo
            var profitOrLoss = CalculateProfitOrLoss(operation);
            
            // Se for preju�zo, n�o tem imposto
            if (profitOrLoss <= 0)
                return 0;
                
            // Se for isento, n�o tem imposto, mas o lucro ainda � usado para deduzir preju�zos
            if (IsExempt(operation))
                return 0;
                
            // Aplica a al�quota de imposto
            return ApplyTaxRate(profitOrLoss);
        }
        
        // M�todos abstratos que devem ser implementados por classes concretas
        protected abstract bool IsExempt(IOperation operation);
        protected abstract decimal CalculateProfitOrLoss(IOperation operation);
        protected abstract decimal ApplyTaxRate(decimal profit);
    }
}