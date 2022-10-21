using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace LoggersComparison
{
    public class GreetingService: IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private readonly IConfiguration _config;
        private readonly IFileLogger _fileLogger;

        public GreetingService(ILogger<GreetingService> log, IConfiguration config, IFileLogger fileLogger)
        {
            _log = log;
            _config = config;
            _fileLogger = fileLogger;
        }
        public void RunSeriFileLogger()
        {
            _log.LogInformation("Seri Log - Logging Started");


            var stopWatch = Stopwatch.StartNew();
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                _log.LogInformation("Run number {runNumber}", i);
            }
            
            stopWatch.Stop();

            Console.WriteLine($"SeriLog Logger {_config.GetValue<int>("LoopTimes")} in {stopWatch.Elapsed}");

        }


        public async Task RunCustomFileLogger()
        {
            var stopWatch = Stopwatch.StartNew();
            var awitableTasks =new List<Task>();
            awitableTasks.Add(_fileLogger.LogAsync("0000-0000-0000-0000", "Customer Logger - Logging Started", LoggingLevel.INF));

            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                awitableTasks.Add(_fileLogger.LogAsync($"{i}", $"Run number {i}", LoggingLevel.INF));
            }
            await Task.WhenAll(awitableTasks);
            await _fileLogger.Flush();
            stopWatch.Stop();
            Console.WriteLine($"Custom Logger {_config.GetValue<int>("LoopTimes")} in {stopWatch.Elapsed}");

        }

    }
}
