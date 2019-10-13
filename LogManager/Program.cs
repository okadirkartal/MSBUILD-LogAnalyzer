using LogManager.Core.BLL;
using LogManager.Core.BLL;
using LogManager.Core.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LogManager
{
    class Program
    {

        private static IServiceProvider _serviceProvider;

        private static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            SetBasePath();

            RegisterServices();

            var logger = _serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogDebug("Starting Application");

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Configuration[Common.Constants.LogFile]);

            Utility.CheckDatabaseDirectory();

            var logParseService = _serviceProvider.GetService<ILogParser>();

            var jsonData = logParseService.GetJsonDataFromFile(filePath);

            logger.LogDebug("Json data received from logs.txt");
            logger.LogInformation($"json data is : {jsonData}");

            var saveableData = logParseService.GetSaveableData(jsonData);

            logger.LogDebug("Json data received from logs.txt");
            logger.LogInformation($"Saveable data is : {saveableData}");

            var result = _serviceProvider.GetService<IDocumentPersistenceService>().SaveDocument(saveableData);

            logger.LogDebug("Json data saved to liteDB");
            logger.LogInformation($"saved result is : {result}");

            DisposeServices();

            logger.LogDebug("All done.");


            Console.ReadKey();
        }

        #region Private Methods
        private static void SetBasePath()
        {
            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

        }

        private static void RegisterServices()
        {
            _serviceProvider = new ServiceCollection()
            .AddOptions()
            .AddLogging()
            .AddSingleton<IConfiguration>(Configuration)
            .AddScoped<IDocumentPersistenceService, DocumentPersistenceService> ()
            .AddScoped<ILogParser,LogParser>()
            .BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
        #endregion
    }
}
