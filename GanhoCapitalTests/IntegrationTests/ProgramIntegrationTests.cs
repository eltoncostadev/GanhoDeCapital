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
        public void Program_WithCase1_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #1
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 100}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 50}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 0.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase2_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #2
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 10000.00m, 0.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase3_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #3
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":5.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 3000}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 1000.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase4_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #4
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"buy\", \"unit-cost\":25.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 10000}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 0.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase5_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #5
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"buy\", \"unit-cost\":25.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 5000}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 0.00m, 10000.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase6_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #6
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":2.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}," +
                "{\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 1000}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 0.00m, 0.00m, 0.00m, 3000.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase7_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #7
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":2.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}," +
                "{\"operation\":\"sell\", \"unit-cost\":20.00, \"quantity\": 2000}," +
                "{\"operation\":\"sell\", \"unit-cost\":25.00, \"quantity\": 1000}," +
                "{\"operation\":\"buy\", \"unit-cost\":20.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":15.00, \"quantity\": 5000}," +
                "{\"operation\":\"sell\", \"unit-cost\":30.00, \"quantity\": 4350}," +
                "{\"operation\":\"sell\", \"unit-cost\":30.00, \"quantity\": 650}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 
                0.00m, 0.00m, 0.00m, 0.00m, 3000.00m, 
                0.00m, 0.00m, 3700.00m, 0.00m 
            };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase8_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #8
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":10.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":50.00, \"quantity\": 10000}," +
                "{\"operation\":\"buy\", \"unit-cost\":20.00, \"quantity\": 10000}," +
                "{\"operation\":\"sell\", \"unit-cost\":50.00, \"quantity\": 10000}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 0.00m, 80000.00m, 0.00m, 60000.00m };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
        [Fact]
        public void Program_WithCase9_ShouldProduceCorrectOutput()
        {
            // Arrange - Caso #9
            var input = 
                "[{\"operation\":\"buy\", \"unit-cost\":5000.00, \"quantity\": 10}," +
                "{\"operation\":\"sell\", \"unit-cost\":4000.00, \"quantity\": 5}," +
                "{\"operation\":\"buy\", \"unit-cost\":15000.00, \"quantity\": 5}," +
                "{\"operation\":\"buy\", \"unit-cost\":4000.00, \"quantity\": 2}," +
                "{\"operation\":\"buy\", \"unit-cost\":23000.00, \"quantity\": 2}," +
                "{\"operation\":\"sell\", \"unit-cost\":20000.00, \"quantity\": 1}," +
                "{\"operation\":\"sell\", \"unit-cost\":12000.00, \"quantity\": 10}," +
                "{\"operation\":\"sell\", \"unit-cost\":15000.00, \"quantity\": 3}]" + Environment.NewLine;
            
            var expectedTaxes = new List<decimal> { 
                0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 
                0.00m, 1000.00m, 2400.00m 
            };
            
            // Act & Assert
            RunProgramTest(input, new List<List<decimal>> { expectedTaxes });
        }
        
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
            
            var expectedTaxes = new List<List<decimal>>
            {
                new List<decimal> { 0.00m, 0.00m, 0.00m },
                new List<decimal> { 0.00m, 10000.00m, 0.00m }
            };
            
            // Act & Assert
            RunProgramTest(input, expectedTaxes);
        }
        
        /// <summary>
        /// Executa o programa com a entrada fornecida e verifica se a saída corresponde às taxas esperadas
        /// </summary>
        private void RunProgramTest(string input, List<List<decimal>> expectedTaxes)
        {
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
                
                Assert.Equal(expectedTaxes.Count, lines.Length);
                
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