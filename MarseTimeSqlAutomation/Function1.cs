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
        [FunctionName("marseTimeAutomation")]
        public static void Run([TimerTrigger("0 0 */1 * * *")]TimerInfo myTimer, ILogger log)
        {
            // Execute every 1 Hour to Update Database
            // Download Data
            log.LogInformation($"Downloading data...");
            FileHandler fh = new FileHandler();
            string location = fh.DownloadData();
            if (location == "")
            {
                // Add Log - Download Failed
                SqlHandler sqlh = new SqlHandler();
                log.LogInformation($"Download Failed");
                sqlh.LogData("Download Failed, No Changes Made");
            }
            else
            {
                string result = fh.ReadData(fh.UnzipData(location));
                log.LogInformation($"Reading data...");
                // Read Data
                DataParsing dp = new DataParsing();
                dp.ParseData(result);

                log.LogInformation($"Storing data...");
                // Store Data
                SqlHandler sqlh = new SqlHandler();
                bool diffrence = sqlh.CheckDifference();
                if (diffrence)
                {
                    // Update Database
                    log.LogInformation($"Clear SQL...");
                    sqlh.ClearData();
                    log.LogInformation($"Insert SQL...");
                    sqlh.ImportData();
                    sqlh.LogData("New Data - New Week");
                    // Add Log - Changes Made
                }
                else
                {
                    // Add Log - No Changes
                    log.LogInformation($"No Changes Made");
                    sqlh.LogData("No Changes Made");
                }
                log.LogInformation($"Data updated successfully");
            }
        }
    }
}
