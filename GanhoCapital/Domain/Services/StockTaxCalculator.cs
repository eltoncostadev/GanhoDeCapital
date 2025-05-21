using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Services.Abstract;

namespace CapitalGains.Domain.Services
{
    /// <summary>
    /// Implementação específica para cálculo de impostos sobre operações de ações
    /// </summary>
    public class StockTaxCalculator : AbstractTaxCalculator
    {
        private readonly IPortfolioState _portfolioState;
        private const decimal TaxRate = 0.2m; // 20%
        private const decimal ExemptionThreshold = 20000m;
        
        public StockTaxCalculator(IPortfolioState portfolioState)
        {
            _portfolioState = portfolioState;
        }
        
        protected override bool IsExempt(IOperation operation)
        {
            // Operações de compra são isentas
            if (operation.OperationType.ToLower() == "buy")
                return true;
                
            // Operações com valor total <= 20000 são isentas de impostos
            return operation.TotalValue <= ExemptionThreshold;
        }
        
        protected override decimal CalculateProfitOrLoss(IOperation operation)
        {
            if (operation.OperationType.ToLower() != "sell")
                return 0;
                
            var averageCost = _portfolioState.WeightedAveragePrice;
            return (operation.UnitCost - averageCost) * operation.Quantity;
        }
        
        protected override decimal ApplyTaxRate(decimal profit)
        {
            // Deduz o prejuízo acumulado do lucro
            var accumulatedLoss = _portfolioState.AccumulatedLoss;
            if (accumulatedLoss > 0)
            {
                if (accumulatedLoss >= profit)
                {
                    _portfolioState.UpdateAccumulatedLoss(accumulatedLoss - profit);
                    return 0;
                }
                else
                {
                    var taxableProfit = profit - accumulatedLoss;
                    _portfolioState.UpdateAccumulatedLoss(0);
                    return Math.Round(taxableProfit * TaxRate, 2); // Arredonda para 2 casas decimais
                }
            }
            
            return Math.Round(profit * TaxRate, 2); // Arredonda para 2 casas decimais
        }
    }
}