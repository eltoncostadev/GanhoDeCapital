using CapitalGains.Application;
using CapitalGains.Application.Factories;
using CapitalGains.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CapitalGains.Infrastructure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configura��o da inje��o de depend�ncias
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            
            // Obt�m o servi�o de c�lculo de impostos
            var taxService = serviceProvider.GetRequiredService<TaxCalculationService>();
            
            string? line;
            while ((line = Console.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
            {
                var result = taxService.ProcessLine(line);
                if (result != null)
                {
                    Console.WriteLine(result);
                }
            }
        }
        
        private static void ConfigureServices(IServiceCollection services)
        {
            // Registra a f�brica para cria��o de processadores
            services.AddSingleton<IOperationProcessorFactory, StockOperationProcessorFactory>();
            
            // Registra o servi�o principal
            services.AddSingleton<TaxCalculationService>();
        }
    }
}