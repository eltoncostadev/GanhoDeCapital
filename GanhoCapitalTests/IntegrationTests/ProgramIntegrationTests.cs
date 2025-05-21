using CapitalGains.Domain.Entities;
using CapitalGains.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

namespace CapitalGains.Tests.IntegrationTests
{
    public class ProgramIntegrationTests
    {
        [Fact]
        public void Program_WithMultipleInputs_ShouldProduceCorrectOutput()
        {
            // Arrange
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]" + Environment.NewLine +
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]" + Environment.NewLine;
            
            // Valores esperados com possibilidade de duas casas decimais
            var expectedTaxes = new List<List<decimal>>
            {
                new List<decimal> { 0.00m, 0.00m, 0.00m },
                new List<decimal> { 0.00m, 10000.00m, 0.00m }
            };
            
            using var inputStream = new StringReader(input);
            using var outputStream = new StringWriter();
            
            var originalIn = Console.In;
            var originalOut = Console.Out;
            
            Console.SetIn(inputStream);
            Console.SetOut(outputStream);
            
            try
            {
                // Act
                Program.Main(Array.Empty<string>());
                
                // Assert - Compara os valores numéricos com precisão de duas casas decimais
                var output = outputStream.ToString();
                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                
                Assert.Equal(2, lines.Length);
                
                for (int i = 0; i < lines.Length; i++)
                {
                    var taxResults = JsonSerializer.Deserialize<List<TaxResult>>(lines[i]);
                    Assert.NotNull(taxResults);
                    Assert.Equal(expectedTaxes[i].Count, taxResults.Count);
                    
                    for (int j = 0; j < taxResults.Count; j++)
                    {
                        // Compara os valores arredondados para duas casas decimais
                        Assert.Equal(
                            Math.Round(expectedTaxes[i][j], 2), 
                            Math.Round(taxResults[j].Tax, 2)
                        );
                    }
                }
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }
        
        [Fact]
        public void Program_WithAllTestCases_ShouldProduceCorrectOutput()
        {
            // Arrange
            var input = 
                // Caso #1
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]" + Environment.NewLine +
                // Caso #2
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]" + Environment.NewLine +
                // Caso #3
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 3000}]" + Environment.NewLine;
            
            // Valores esperados com possibilidade de duas casas decimais
            var expectedTaxes = new List<List<decimal>>
            {
                new List<decimal> { 0.00m, 0.00m, 0.00m },
                new List<decimal> { 0.00m, 10000.00m, 0.00m },
                new List<decimal> { 0.00m, 0.00m, 1000.00m }
            };
            
            using var inputStream = new StringReader(input);
            using var outputStream = new StringWriter();
            
            var originalIn = Console.In;
            var originalOut = Console.Out;
            
            Console.SetIn(inputStream);
            Console.SetOut(outputStream);
            
            try
            {
                // Act
                Program.Main(Array.Empty<string>());
                
                // Assert - Compara os valores numéricos com precisão de duas casas decimais
                var output = outputStream.ToString();
                var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                
                Assert.Equal(3, lines.Length);
                
                for (int i = 0; i < lines.Length; i++)
                {
                    var taxResults = JsonSerializer.Deserialize<List<TaxResult>>(lines[i]);
                    Assert.NotNull(taxResults);
                    Assert.Equal(expectedTaxes[i].Count, taxResults.Count);
                    
                    for (int j = 0; j < taxResults.Count; j++)
                    {
                        // Compara os valores arredondados para duas casas decimais
                        Assert.Equal(
                            Math.Round(expectedTaxes[i][j], 2),
                            Math.Round(taxResults[j].Tax, 2)
                        );
                    }
                }
            }
            finally
            {
                Console.SetIn(originalIn);
                Console.SetOut(originalOut);
            }
        }
    }
}