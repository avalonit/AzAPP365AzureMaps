using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace azureapp.app365
{
    public static class ProcessCustomers
    {
        [FunctionName("ProcessCustomers")]
        public static async void Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var bcConfig = new ConnectorConfig(config);
            var azureMapHelper = new AzureMapHelper(bcConfig);
            var bcHelper = new BusinessCentralHelper(bcConfig);
            var customers=await bcHelper.GetCustomers();

            if (customers != null && customers.Value != null && customers.Value.Count > 0)
            {
                foreach (var customer in customers.Value)
                {
                    var azureResult = (await azureMapHelper.GetCoordinates(customer));
                    await bcHelper.UpdateCustomer(customer,azureResult);
                }

            }

        }

    }
}
