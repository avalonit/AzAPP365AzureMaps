using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace azureapp.mymapapp
{
    public static class ProcessNavTimeTrigger
    {
        [FunctionName("ProcessNavTimeTrigger")]
        public static async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation(string.Format("{0} {1}", context.FunctionName, "Started"));


            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var appConfig = new ConnectorConfig(config);
            await ProcessNavCustomers.Process(log,appConfig);

            log.LogInformation(string.Format("{0} {1}", context.FunctionName, "Completed"));
        }

       
    }
}



