using CapitalGains.Domain.Services;
using Xunit;

namespace CapitalGains.Tests.UnitTests
{
    public class PortfolioStateTests
    {
        [Fact]
        public void InitialState_ShouldBeZero()
        {
            // Arrange & Act
            var state = new PortfolioState();
            
            // Assert
            Assert.Equal(0, state.SharesOwned);
            Assert.Equal(0, state.WeightedAveragePrice);
            Assert.Equal(0, state.AccumulatedLoss);
        }
        
        [Fact]
        public void UpdateMethods_ShouldChangeProperties()
        {
            // Arrange
            var state = new PortfolioState();
            
            // Act
            state.UpdateShareCount(100);
            state.UpdateWeightedAveragePrice(10.5m);
            state.UpdateAccumulatedLoss(25.75m);
            
            // Assert
            Assert.Equal(100, state.SharesOwned);
            Assert.Equal(10.5m, state.WeightedAveragePrice);
            Assert.Equal(25.75m, state.AccumulatedLoss);
        }
        
        [Fact]
        public void Reset_ShouldReturnToInitialState()
        {
            // Arrange
            var state = new PortfolioState();
            state.UpdateShareCount(100);
            state.UpdateWeightedAveragePrice(10.5m);
            state.UpdateAccumulatedLoss(25.75m);
            
            // Act
            state.Reset();
            
            // Assert
            Assert.Equal(0, state.SharesOwned);
            Assert.Equal(0, state.WeightedAveragePrice);
            Assert.Equal(0, state.AccumulatedLoss);
        }
    }
}