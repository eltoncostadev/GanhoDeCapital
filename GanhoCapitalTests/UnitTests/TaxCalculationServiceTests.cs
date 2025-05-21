using CapitalGains.Application;
using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CapitalGains.Tests.UnitTests
{
    public class TaxCalculationServiceTests
    {
        private readonly Mock<IOperationProcessorFactory> _factoryMock;
        private readonly Mock<IOperationProcessor> _processorMock;
        private readonly TaxCalculationService _service;
        
        public TaxCalculationServiceTests()
        {
            _processorMock = new Mock<IOperationProcessor>();
            _factoryMock = new Mock<IOperationProcessorFactory>();
            _factoryMock.Setup(f => f.CreateProcessor()).Returns(_processorMock.Object);
            
            _service = new TaxCalculationService(_factoryMock.Object);
        }
        
        [Fact]
        public void ProcessOperations_ShouldReturnCorrectTaxes()
        {
            // Arrange
            var operations = new List<Operation>
            {
                new Operation { OperationName = "buy", UnitCost = 10.00m, Quantity = 100 },
                new Operation { OperationName = "sell", UnitCost = 15.00m, Quantity = 50 },
                new Operation { OperationName = "sell", UnitCost = 15.00m, Quantity = 50 }
            };
            
            _processorMock.SetupSequence(p => p.ProcessOperation(It.IsAny<IOperation>()))
                .Returns(0m)
                .Returns(0m)
                .Returns(0m);
            
            // Act
            var results = _service.ProcessOperations(operations);
            
            // Assert
            var resultList = new List<ITaxResult>(results);
            Assert.Equal(3, resultList.Count);
            Assert.Equal(0m, resultList[0].Tax);
            Assert.Equal(0m, resultList[1].Tax);
            Assert.Equal(0m, resultList[2].Tax);
            
            _processorMock.Verify(p => p.ProcessOperation(It.IsAny<IOperation>()), Times.Exactly(3));
        }
        
        [Fact]
        public void ProcessLine_WithValidJson_ShouldReturnCorrectTaxes()
        {
            // Arrange
            var json = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}, " +
                       "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]";
            
            _processorMock.SetupSequence(p => p.ProcessOperation(It.IsAny<IOperation>()))
                .Returns(0m)
                .Returns(0m);
            
            // Act
            var result = _service.ProcessLine(json);
            
            // Assert
            Assert.Equal("[{\"tax\":0},{\"tax\":0}]", result);
        }
        
        [Fact]
        public void ProcessLine_WithEmptyInput_ShouldReturnNull()
        {
            // Act
            var result = _service.ProcessLine("");
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public void ProcessLine_WithInvalidJson_ShouldReturnEmptyArray()
        {
            // Arrange
            var json = "invalid json";
            
            // Act
            var result = _service.ProcessLine(json);
            
            // Assert
            Assert.Equal("[]", result);
        }
    }
}