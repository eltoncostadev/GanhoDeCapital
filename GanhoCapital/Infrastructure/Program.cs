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
            // Configuração da injeção de dependências
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            
            // Obtém o serviço de cálculo de impostos
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
            // Registra a fábrica para criação de processadores
            services.AddSingleton<IOperationProcessorFactory, StockOperationProcessorFactory>();
            
            // Registra o serviço principal
            services.AddSingleton<TaxCalculationService>();
        }
    }
}