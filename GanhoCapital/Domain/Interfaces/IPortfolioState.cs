namespace CapitalGains.Domain.Interfaces
{
    /// <summary>
    /// Interface para manter o estado de um portfólio
    /// </summary>
    public interface IPortfolioState
    {
        int SharesOwned { get; }
        decimal WeightedAveragePrice { get; }
        decimal AccumulatedLoss { get; }
        
        void UpdateShareCount(int newShareCount);
        void UpdateWeightedAveragePrice(decimal newAverage);
        void UpdateAccumulatedLoss(decimal newLoss);
    }
}