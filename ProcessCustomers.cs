using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

            var baseUrl = bcConfig.WebServiceMainUrl;
            var url = String.Format(baseUrl, bcConfig.WebServiceCompany);

            var authData = string.Format("{0}:{1}", bcConfig.WebServiceUser, bcConfig.WebServicePassword);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            log.LogInformation(bcConfig.WebServiceUser);
            log.LogInformation(bcConfig.WebServicePassword);
            log.LogInformation(url);

            using (WebClient client = new WebClient())
            {
                var uri = new Uri(url);

                client.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeaderValue;

                log.LogInformation(url);

                var response = client.DownloadString(uri).ToString();
                var customers = JsonConvert.DeserializeObject<Customers>(response.ToString());

                if (customers != null && customers.Value != null && customers.Value.Count > 0)
                {
                    foreach (var customer in customers.Value)
                    {
                        var azureResult=(await azureMapHelper.GetCoordinates(customer));
                    }

                }

            }
        }

    }
}
