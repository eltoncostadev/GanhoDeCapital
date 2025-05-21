using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Services;

namespace CapitalGains.Application.Factories
{
    /// <summary>
    /// F�brica para criar processadores de opera��es de a��es
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