using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Services;

namespace CapitalGains.Application.Factories
{
    /// <summary>
    /// Fábrica para criar processadores de operações de ações
    /// </summary>
    public class StockOperationProcessorFactory : IOperationProcessorFactory
    {
        public IOperationProcessor CreateProcessor()
        {
            var portfolioState = new PortfolioState();
            var taxCalculator = new StockTaxCalculator(portfolioState);
            return new StockOperationProcessor(portfolioState, taxCalculator);
        }
    }
}