using System;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace azureapp.mymapapp
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

        public AzureMapResults Get_Bc_CustomerCoordinates(BC_Customer customer)
        {
            try
            {
                var address = string.Format("{0}, {1}, {2}, {3}, ({4})"
                    , customer.Address
                    , customer.PostCode
                    , customer.City
                    , customer.County
                    , customer.Country);
                var url = string.Format(AzureMapEndpoint, AzureMapKey, address);

                using (var client = new WebClient())
                {
                    var uri = new Uri(url);

                    var response = client.DownloadString(uri).ToString();
                    var result = JsonConvert.DeserializeObject<AzureMapResults>(response.ToString());
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public AzureMapResults Get_Nav_CustomerCoordinates(Nav_Customer customer)
        {
            try
            {
                var address = string.Format("{0}, {1}, {2}, {3}, ({4})"
                    , customer.Address
                    , customer.PostCode
                    , customer.City
                    , customer.County
                    , customer.CountryRegionCode);
                var url = string.Format(AzureMapEndpoint, AzureMapKey, address);

                using (var client = new WebClient())
                {
                    var uri = new Uri(url);

                    var response = client.DownloadString(uri).ToString();
                    var result = JsonConvert.DeserializeObject<AzureMapResults>(response.ToString());
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public AzureMapResults Get_Bc_ShipToAddressCoordinates(BC_ShipToAddress customer)
        {
            try
            {
                var address = string.Format("{0}, {1}, {2}, {3}, ({4})"
                    , customer.Address
                    , customer.PostCode
                    , customer.City
                    , customer.County
                    , customer.Country);
                var url = string.Format(AzureMapEndpoint, AzureMapKey, address);

                using (var client = new WebClient())
                {
                    var uri = new Uri(url);

                    var response = client.DownloadString(uri).ToString();
                    var result = JsonConvert.DeserializeObject<AzureMapResults>(response.ToString());
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
