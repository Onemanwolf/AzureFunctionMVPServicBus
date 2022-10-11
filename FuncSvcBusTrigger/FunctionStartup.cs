using FuncSvcBusTrigger;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Azure.AppConfiguration.Functions.Worker;
using System.Linq;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

[assembly: FunctionsStartup(typeof(FuncSvcBusTrigger.Startup))]

namespace FuncSvcBusTrigger
{
    public class Startup : FunctionsStartup
    {



        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            var config = builder.ConfigurationBuilder
                 .AddEnvironmentVariables()
                 .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            IConfigurationRoot root = config.Build();
            _serviceConfiguration = new();
            root.Bind(_serviceConfiguration);
        }
        private ServiceConfiguration _serviceConfiguration;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IntergrationService>();
            builder.Services.AddSingleton<ServiceConfiguration>();
            builder.Services.AddOptions<ServiceConfiguration>()
            .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("ServiceConfiguration").Bind(settings);
                });
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddFilter(level => level >= LogLevel.Information);
            });





        }


    }
}