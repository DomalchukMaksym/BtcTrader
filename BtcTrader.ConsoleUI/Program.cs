using BtcTrader.ConsoleUI;
using BtcTrader.ExchangeServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration((context, builder) =>
			{
				builder.SetBasePath(Directory.GetCurrentDirectory());
			})
			.ConfigureServices((context, services) =>
			{
				services.AddTransient<IInputDataService, InputDataService>();
				services.AddTransient<OrderCalculationService>();
				services.AddTransient<Service>();
			}).Build();

host.Services.GetService<Service>().Start(args);