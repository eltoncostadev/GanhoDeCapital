using CapitalGains.Application;
using CapitalGains.Application.Factories;
using CapitalGains.Domain.Entities;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace CapitalGains.Tests.IntegrationTests
{
    public class ApplicationIntegrationTests
    {
        private readonly TaxCalculationService _service;

        public ApplicationIntegrationTests()
        {
            var factory = new StockOperationProcessorFactory();
            _service = new TaxCalculationService(factory);
        }

        [Fact]
        public void Case1_ShouldReturnCorrectTaxes()
        {
            // Arrange - Caso #1 da especificação
            var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}," +
                        "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}," +
                        "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]";

            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 0.00m };

            // Act
            var result = _service.ProcessLine(input);

            // Assert
            var taxResults = JsonSerializer.Deserialize<List<TaxResult>>(result);
            Assert.Equal(3, taxResults.Count);

            for (int i = 0; i < taxResults.Count; i++)
            {
                Assert.Equal(
                    Math.Round(expectedTaxes[i], 2),
                    Math.Round(taxResults[i].Tax, 2)
                );
            }
        }

        [Fact]
        public void Case2_ShouldReturnCorrectTaxes()
        {
            // Arrange - Caso #2 da especificação
            var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                        "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}," +
                        "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]";

            var expectedTaxes = new List<decimal> { 0.00m, 10000.00m, 0.00m };

            // Act
            var result = _service.ProcessLine(input);

            // Assert
            var taxResults = JsonSerializer.Deserialize<List<TaxResult>>(result);
            Assert.Equal(3, taxResults.Count);

            for (int i = 0; i < taxResults.Count; i++)
            {
                Assert.Equal(
                    Math.Round(expectedTaxes[i], 2),
                    Math.Round(taxResults[i].Tax, 2)
                );
            }
        }

        [Fact]
        public void Case3_ShouldReturnCorrectTaxes()
        {
            // Arrange - Caso #3 da especificação
            var input = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                        "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}," +
                        "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 3000}]";

            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 1000.00m };

            // Act
            var result = _service.ProcessLine(input);

            // Assert
            var taxResults = JsonSerializer.Deserialize<List<TaxResult>>(result);
            Assert.Equal(3, taxResults.Count);

            for (int i = 0; i < taxResults.Count; i++)
            {
                Assert.Equal(
                    Math.Round(expectedTaxes[i], 2),
                    Math.Round(taxResults[i].Tax, 2)
                );
            }
        }

        [Fact]
        public void MultipleSimulations_ShouldBeProcessedIndependently()
        {
            // Arrange
            var input1 = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}," +
                        "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}," +
                        "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]";

            var input2 = "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                        "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}," +
                        "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]";

            var expectedTaxes1 = new List<decimal> { 0.00m, 0.00m, 0.00m };
            var expectedTaxes2 = new List<decimal> { 0.00m, 10000.00m, 0.00m };

            // Act
            var result1 = _service.ProcessLine(input1);
            var result2 = _service.ProcessLine(input2);

            // Assert
            var taxResults1 = JsonSerializer.Deserialize<List<TaxResult>>(result1);
            var taxResults2 = JsonSerializer.Deserialize<List<TaxResult>>(result2);

            Assert.Equal(3, taxResults1.Count);
            Assert.Equal(3, taxResults2.Count);

            for (int i = 0; i < taxResults1.Count; i++)
            {
                Assert.Equal(
                    Math.Round(expectedTaxes1[i], 2),
                    Math.Round(taxResults1[i].Tax, 2)
                );
            }

            for (int i = 0; i < taxResults2.Count; i++)
            {
                Assert.Equal(
                    Math.Round(expectedTaxes2[i], 2),
                    Math.Round(taxResults2[i].Tax, 2)
                );
            }
        }
    }
}