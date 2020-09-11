using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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


            var apiShipToAddress = new BusinessCentralHelper(bcConfig, "ApiShipToAddressCoords");
            var shipToAddresses = await apiShipToAddress.GetShipToAddress();

            int counter = 0;
            if (shipToAddresses != null && shipToAddresses.Value != null && shipToAddresses.Value.Count > 0)
            {
                shipToAddresses.Value = shipToAddresses.Value.Where(a => a.NblLatitude == 0).ToList();
                foreach (var shipToAddress in shipToAddresses.Value)
                {
                    var azureResult = (await azureMapHelper.GetShipToAddressCoordinates(shipToAddress));
                    var filter = String.Format("(code='{0}',customerNo='{1}')", shipToAddress.Code, shipToAddress.customerNo);
                    await apiShipToAddress.UpdateShipToAddress(shipToAddress, azureResult, filter);
                    log.LogInformation(String.Format("Ship To Address : {0} - Completed {1} of {2}", shipToAddress.Code, (counter++).ToString(), shipToAddresses.Value.Count.ToString()));
                }

            }


            var apiCustomer = new BusinessCentralHelper(bcConfig, "ApiCustomersCoords");
            var customers = await apiCustomer.GetCustomers();

            counter = 0;
            if (customers != null && customers.Value != null && customers.Value.Count > 0)
            {
                customers.Value = customers.Value.Where(a => a.NblLatitude == 0).ToList();
                foreach (var customer in customers.Value)
                {
                    var azureResult = (await azureMapHelper.GetCustomerCoordinates(customer));
                    var filter = String.Format("(no='{0}')", customer.No);
                    await apiCustomer.UpdateCustomer(customer, azureResult, filter);
                    log.LogInformation(String.Format("Customer : {0} - Completed {1} of {2}", customer.No, (counter++).ToString(), customers.Value.Count.ToString()));
                }

            }


        }

    }
}
