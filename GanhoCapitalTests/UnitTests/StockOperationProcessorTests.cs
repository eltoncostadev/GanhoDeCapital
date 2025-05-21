using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Services;
using Moq;
using Xunit;

namespace CapitalGains.Tests.UnitTests
{
    public class StockOperationProcessorTests
    {
        private readonly Mock<IPortfolioState> _portfolioStateMock;
        private readonly Mock<ITaxCalculator> _taxCalculatorMock;
        private readonly StockOperationProcessor _processor;
        
        public StockOperationProcessorTests()
        {
            _portfolioStateMock = new Mock<IPortfolioState>();
            _taxCalculatorMock = new Mock<ITaxCalculator>();
            _processor = new StockOperationProcessor(_portfolioStateMock.Object, _taxCalculatorMock.Object);
        }
        
        [Fact]
        public void ProcessOperation_BuyOperation_ShouldUpdatePortfolio()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "buy",
                UnitCost = 10.00m,
                Quantity = 100
            };
            
            _portfolioStateMock.Setup(p => p.SharesOwned).Returns(0);
            
            // Act
            var tax = _processor.ProcessOperation(operation);
            
            // Assert
            _portfolioStateMock.Verify(p => p.UpdateWeightedAveragePrice(10.00m), Times.Once);
            _portfolioStateMock.Verify(p => p.UpdateShareCount(100), Times.Once);
        }
        
        [Fact]
        public void ProcessOperation_BuyOperationWithExistingShares_ShouldUpdateAveragePrice()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "buy",
                UnitCost = 20.00m,
                Quantity = 50
            };
            
            _portfolioStateMock.Setup(p => p.SharesOwned).Returns(100);
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            
            // Act
            var tax = _processor.ProcessOperation(operation);
            
            // Assert
            // Novo preço médio: ((100 * 10) + (50 * 20)) / 150 = 13.33
            _portfolioStateMock.Verify(p => p.UpdateWeightedAveragePrice(13.33m), Times.Once);
            _portfolioStateMock.Verify(p => p.UpdateShareCount(150), Times.Once);
        }
        
        [Fact]
        public void ProcessOperation_SellOperationWithProfit_ShouldCalculateTaxAndUpdateShares()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "sell",
                UnitCost = 15.00m,
                Quantity = 50
            };
            
            _portfolioStateMock.Setup(p => p.SharesOwned).Returns(100);
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            _taxCalculatorMock.Setup(c => c.CalculateTax(It.IsAny<IOperation>())).Returns(500m);
            
            // Act
            var tax = _processor.ProcessOperation(operation);
            
            // Assert
            Assert.Equal(500m, tax);
            _portfolioStateMock.Verify(p => p.UpdateShareCount(50), Times.Once);
        }
        
        [Fact]
        public void ProcessOperation_SellOperationWithLoss_ShouldUpdateAccumulatedLoss()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "sell",
                UnitCost = 5.00m,
                Quantity = 50
            };
            
            _portfolioStateMock.Setup(p => p.SharesOwned).Returns(100);
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            _portfolioStateMock.Setup(p => p.AccumulatedLoss).Returns(0m);
            _taxCalculatorMock.Setup(c => c.CalculateTax(It.IsAny<IOperation>())).Returns(0m);
            
            // Act
            var tax = _processor.ProcessOperation(operation);
            
            // Assert
            // Prejuízo: (5 - 10) * 50 = -250
            _portfolioStateMock.Verify(p => p.UpdateAccumulatedLoss(250m), Times.Once);
            _portfolioStateMock.Verify(p => p.UpdateShareCount(50), Times.Once);
        }
        
        [Fact]
        public void Reset_ShouldResetPortfolioState()
        {
            // Act
            _processor.Reset();
            
            // Assert
            _portfolioStateMock.Verify(p => p.UpdateShareCount(0), Times.Once);
            _portfolioStateMock.Verify(p => p.UpdateWeightedAveragePrice(0), Times.Once);
            _portfolioStateMock.Verify(p => p.UpdateAccumulatedLoss(0), Times.Once);
        }
    }
}