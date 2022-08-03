using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace azureapp.mymapapp
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
            var appConfig = new ConnectorConfig(config);
            var azureMapHelper = new AzureMapHelper(appConfig);

            //CUSTOMERS

            var apiCustomer = new BusinessCentralHelper(appConfig, "ApiCustomersCoords", log);
            var customers = apiCustomer.GetCustomers();

            var counter = 0;
            if (customers != null && customers.Value != null && customers.Value.Count > 0)
            {
                customers.Value = customers.Value.Where(a => a.NblLatitude == 0).ToList();
                foreach (var customer in customers.Value)
                {
                    var azureResult = azureMapHelper.Get_Bc_CustomerCoordinates(customer);
                    var filter = string.Format("(no='{0}')", customer.No);
                    var updateResult = await apiCustomer.UpdateCustomer(customer, azureResult, filter);
                    if (updateResult != null)
                        log.LogInformation(string.Format("Customer : {0} - Completed {1} of {2}", customer.No, (counter++).ToString(), customers.Value.Count.ToString()));
                    else
                        log.LogError(string.Format("Customer : {0} {1} - Error {2} of {3}", customer.No, customer.FullAddress, (counter++).ToString(), customers.Value.Count.ToString()));
                }
            }
            else
                log.LogInformation(string.Format("All customers already processed"));

            // SHIP TO ADDRESS
            var apiShipToAddress = new BusinessCentralHelper(appConfig, "ApiShipToAddressCoords", log);
            var shipToAddresses = apiShipToAddress.GetShipToAddress();

            counter = 0;
            if (shipToAddresses != null && shipToAddresses.Value != null && shipToAddresses.Value.Count > 0)
            {
                shipToAddresses.Value = shipToAddresses.Value.Where(a => a.Latitude == 0).ToList();
                foreach (var shipToAddress in shipToAddresses.Value)
                {
                    var azureResult = azureMapHelper.Get_Bc_ShipToAddressCoordinates(shipToAddress);
                    var filter = string.Format("(code='{0}',customerNo='{1}')", shipToAddress.Code, shipToAddress.customerNo);
                    var updateResult = await apiShipToAddress.UpdateShipToAddress(shipToAddress, azureResult, filter);
                    if (updateResult != null)
                        log.LogInformation(string.Format("Ship To Address : {0} - Completed {1} of {2}", shipToAddress.customerNo, (counter++).ToString(), shipToAddresses.Value.Count.ToString()));
                    else
                        log.LogError(string.Format("Ship To Address : {0} {1} - Error {2} of {3}", shipToAddress.customerNo, shipToAddress.FullAddress, (counter++).ToString(), shipToAddresses.Value.Count.ToString()));
                }

            }
            else
                log.LogInformation(string.Format("All ship to address already processed"));




        }

    }
}
