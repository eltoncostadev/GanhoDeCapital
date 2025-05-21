using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Interfaces;
using CapitalGains.Domain.Services;
using Moq;
using Xunit;

namespace CapitalGains.Tests.UnitTests
{
    public class StockTaxCalculatorTests
    {
        private readonly Mock<IPortfolioState> _portfolioStateMock;
        private readonly StockTaxCalculator _calculator;
        
        public StockTaxCalculatorTests()
        {
            _portfolioStateMock = new Mock<IPortfolioState>();
            _calculator = new StockTaxCalculator(_portfolioStateMock.Object);
        }
        
        [Fact]
        public void CalculateTax_BuyOperation_ShouldReturnZero()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "buy",
                UnitCost = 10.00m,
                Quantity = 100
            };
            
            // Act
            var tax = _calculator.CalculateTax(operation);
            
            // Assert
            Assert.Equal(0, tax);
        }
        
        [Fact]
        public void CalculateTax_SellOperationBelowThreshold_ShouldReturnZero()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "sell",
                UnitCost = 15.00m,
                Quantity = 10 // 15 * 10 = 150 < 20000
            };
            
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            
            // Act
            var tax = _calculator.CalculateTax(operation);
            
            // Assert
            Assert.Equal(0, tax);
        }
        
        [Fact]
        public void CalculateTax_SellOperationWithProfit_ShouldCalculateTax()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "sell",
                UnitCost = 30.00m,
                Quantity = 1000 // 30 * 1000 = 30000 > 20000
            };
            
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            _portfolioStateMock.Setup(p => p.AccumulatedLoss).Returns(0m);
            
            // Act
            var tax = _calculator.CalculateTax(operation);
            
            // Assert
            // Lucro: (30 - 10) * 1000 = 20000, Imposto: 20000 * 0.2 = 4000
            Assert.Equal(4000m, tax);
        }
        
        [Fact]
        public void CalculateTax_SellOperationWithProfitAndAccumulatedLoss_ShouldOffsetLoss()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "sell",
                UnitCost = 30.00m,
                Quantity = 1000 // 30 * 1000 = 30000 > 20000
            };
            
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            _portfolioStateMock.Setup(p => p.AccumulatedLoss).Returns(15000m);
            
            // Act
            var tax = _calculator.CalculateTax(operation);
            
            // Assert
            // Lucro: (30 - 10) * 1000 = 20000
            // Prejuízo acumulado: 15000
            // Lucro tributável: 20000 - 15000 = 5000
            // Imposto: 5000 * 0.2 = 1000
            Assert.Equal(1000m, tax);
            
            // Verifica se o prejuízo acumulado foi zerado
            _portfolioStateMock.Verify(p => p.UpdateAccumulatedLoss(0m), Times.Once);
        }
        
        [Fact]
        public void CalculateTax_SellOperationWithLoss_ShouldReturnZero()
        {
            // Arrange
            var operation = new Operation
            {
                OperationName = "sell",
                UnitCost = 5.00m,
                Quantity = 1000 // 5 * 1000 = 5000 < 20000
            };
            
            _portfolioStateMock.Setup(p => p.WeightedAveragePrice).Returns(10.00m);
            
            // Act
            var tax = _calculator.CalculateTax(operation);
            
            // Assert
            Assert.Equal(0m, tax);
        }
    }
}