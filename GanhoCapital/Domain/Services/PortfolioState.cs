// Domain/Services/PortfolioState.cs
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Domain.Services
{
    /// <summary>
    /// Implementação concreta do estado de um portfólio
    /// </summary>
    public class PortfolioState : IPortfolioState
    {
        public int SharesOwned { get; private set; }
        public decimal WeightedAveragePrice { get; private set; }
        public decimal AccumulatedLoss { get; private set; }
        
        public PortfolioState()
        {
            Reset();
        }
        
        public void Reset()
        {
            SharesOwned = 0;
            WeightedAveragePrice = 0;
            AccumulatedLoss = 0;
        }
        
        public void UpdateShareCount(int newShareCount)
        {
            SharesOwned = newShareCount;
        }
        
        public void UpdateWeightedAveragePrice(decimal newAverage)
        {
            WeightedAveragePrice = newAverage;
        }
        
        public void UpdateAccumulatedLoss(decimal newLoss)
        {
            AccumulatedLoss = newLoss;
        }
    }
}