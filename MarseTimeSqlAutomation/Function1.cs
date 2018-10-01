using System;
using System.IO;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MarseTimeSqlAutomation
{
    public static class Function1
    {
        [FunctionName("calcprofit-func")]
        public static void Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            // Download Data
            FileHandler fh = new FileHandler();
            string result = fh.ReadData(fh.UnzipData(fh.DownloadData()));
            // Read Data
            DataParsing dp = new DataParsing();
            dp.ParseData(result);
            String url = Environment.GetEnvironmentVariable("DataSourceLocation");

            log.LogInformation($"Data Source Location: {result}");
        }
    }
}
