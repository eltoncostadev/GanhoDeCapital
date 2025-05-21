using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Services.Abstract;

namespace CapitalGains.Domain.Services
{
    /// <summary>
    /// Processador de operações específico para ações
    /// </summary>
    public class StockOperationProcessor : AbstractOperationProcessor
    {
        private readonly IPortfolioState _portfolio;
        
        public StockOperationProcessor(IPortfolioState portfolio, ITaxCalculator taxCalculator) 
            : base(taxCalculator)
        {
            _portfolio = portfolio;
        }
        
        public override void Reset()
        {
            _portfolio.UpdateShareCount(0);
            _portfolio.UpdateWeightedAveragePrice(0);
            _portfolio.UpdateAccumulatedLoss(0);
        }
        
        protected override void PreProcessOperation(IOperation operation)
        {
            if (operation.OperationType.ToLower() == "buy")
            {
                // Atualiza o preço médio ponderado
                if (_portfolio.SharesOwned == 0)
                {
                    _portfolio.UpdateWeightedAveragePrice(operation.UnitCost);
                }
                else
                {
                    decimal totalValue = (_portfolio.SharesOwned * _portfolio.WeightedAveragePrice) + 
                                         (operation.Quantity * operation.UnitCost);
                    decimal newAverage = Math.Round(totalValue / (_portfolio.SharesOwned + operation.Quantity), 2);
                    _portfolio.UpdateWeightedAveragePrice(newAverage);
                }

                // Atualiza a quantidade de ações
                _portfolio.UpdateShareCount(_portfolio.SharesOwned + operation.Quantity);
            }
        }
        
        protected override decimal CalculateTax(IOperation operation)
        {
            // Delega o cálculo do imposto para o calculador de impostos
            return _taxCalculator.CalculateTax(operation);
        }
        
        protected override void PostProcessOperation(IOperation operation, decimal tax)
        {
            if (operation.OperationType.ToLower() == "sell")
            {
                // Verifica se houve prejuízo
                decimal profitOrLoss = (operation.UnitCost - _portfolio.WeightedAveragePrice) * operation.Quantity;
                
                if (profitOrLoss < 0)
                {
                    // Acumula prejuízo
                    _portfolio.UpdateAccumulatedLoss(_portfolio.AccumulatedLoss + Math.Abs(profitOrLoss));
                }
                else if (profitOrLoss > 0)
                {
                    // Se houver lucro e prejuízo acumulado, deduz do prejuízo
                    // Esta lógica já está implementada no StockTaxCalculator.ApplyTaxRate
                }
                
                // Atualiza a quantidade de ações
                _portfolio.UpdateShareCount(_portfolio.SharesOwned - operation.Quantity);
            }
        }
    }
}