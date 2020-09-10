using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace azureapp.app365
{
    public partial class AzureMapHelper
    {

        string AzureMapEndpoint;
        string AzureMapKey;
        ConnectorConfig config;

        public AzureMapHelper(ConnectorConfig config)
        {
            this.config = config;
            this.AzureMapEndpoint = config.AzureMapEndpoint;
            this.AzureMapKey = config.AzureMapKey;
        }

        public async Task<AzureMapResults> GetCustomerCoordinates(Customer customer)
        {
            try
            {
                var address = String.Format("{0}, {1}, {2}, {3}, ({4})"
                    , customer.Address
                    , customer.PostCode
                    , customer.City
                    , customer.County
                    , customer.Country);
                var url = String.Format(AzureMapEndpoint, AzureMapKey, address);

                using (var client = new WebClient())
                {
                    var uri = new Uri(url);

                    var response = client.DownloadString(uri).ToString();
                    var result = JsonConvert.DeserializeObject<AzureMapResults>(response.ToString());
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<AzureMapResults> GetShipToAddressCoordinates(ShipToAddress customer)
        {
            try
            {
                var address = String.Format("{0}, {1}, {2}, {3}, ({4})"
                    , customer.Address
                    , customer.PostCode
                    , customer.City
                    , customer.County
                    , customer.Country);
                var url = String.Format(AzureMapEndpoint, AzureMapKey, address);

                using (var client = new WebClient())
                {
                    var uri = new Uri(url);

                    var response = client.DownloadString(uri).ToString();
                    var result = JsonConvert.DeserializeObject<AzureMapResults>(response.ToString());
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
