using System.Data.Common;
using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace azureapp.app365
{
    public static class ProcessNavCustomers
    {
        [FunctionName("ProcessNavCustomers")]
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
            var appConfig = new ConnectorConfig(config);
            var azureMapHelper = new AzureMapHelper(appConfig);

            var apiCustomer = new NavHelper(appConfig, log);
            var odataNextLink = string.Empty;
            do
            {

                var customers = await apiCustomer.Get<Nav_Customers>("Prime365Customers", odataNextLink);
                odataNextLink = customers.OdataNextLink.ToString();

                var counter = 0;
                if (customers != null && customers.Value != null && customers.Value.Count > 0)
                {
                    customers.Value = customers.Value.Where(a => a.Prime365Lat == 0).ToList();
                    foreach (var customer in customers.Value)
                    {
                        var azureResult = azureMapHelper.Get_Nav_CustomerCoordinates(customer);
                        var filter = string.Format("(No='{0}')", customer.No);
                        await apiCustomer.UpdateCustomer("Prime365Customers", customer, azureResult, filter);
                        log.LogInformation(string.Format("Customer : {0} - Completed {1} of {2}", customer.No, (counter++).ToString(), customers.Value.Count.ToString()));
                    }

                }
            } while (!string.IsNullOrEmpty(odataNextLink));



        }

    }
}
