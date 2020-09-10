using System.Linq;
using System;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;

namespace azureapp.app365
{
    public partial class BusinessCentralHelper
    {

        string apiEndpoint;
        string authHeaderValue;
        ConnectorConfig config;

        public BusinessCentralHelper(ConnectorConfig config, string entity)
        {
            this.config = config;
            this.apiEndpoint = String.Format(config.WebServiceMainUrl, config.WebServiceCompany, entity);

            var authData = string.Format("{0}:{1}", config.WebServiceUser, config.WebServicePassword);
            this.authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
        }

        public async Task<Customers> GetCustomers()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var uri = new Uri(this.apiEndpoint);

                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeaderValue;

                    var response = client.DownloadString(uri).ToString();
                    var entities = JsonConvert.DeserializeObject<Customers>(response.ToString());
                    return entities;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<ShipToAddresses> GetShipToAddress()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var uri = new Uri(this.apiEndpoint);

                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeaderValue;

                    var response = client.DownloadString(uri).ToString();
                    var entities = JsonConvert.DeserializeObject<ShipToAddresses>(response.ToString());
                    return entities;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public async Task<CustomerCoordinates> UpdateCustomer(Customer customer, AzureMapResults azureMapResults, string filter)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var coords = new CustomerCoordinates();
                    coords.NblLatitude = azureMapResults.Results.ElementAt(0).Position.Lat;
                    coords.NblLogitude = azureMapResults.Results.ElementAt(0).Position.Lon;
                    coords.No = customer.No;

                    apiEndpoint += filter;
                    var request = new HttpRequestMessage(HttpMethod.Patch, new Uri(apiEndpoint));
                    var json = JsonConvert.SerializeObject(coords);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.TryAddWithoutValidation("If-Match", customer.OdataEtag);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", this.authHeaderValue);

                    var response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var customerCoord = JsonConvert.DeserializeObject<CustomerCoordinates>(responseJson);
                        return customerCoord;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public async Task<ShipToAddresses> UpdateShipToAddress(ShipToAddress customer, AzureMapResults azureMapResults, string filter)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var coords = new ShipToAddressCoordinates();
                    coords.NblLatitude = azureMapResults.Results.ElementAt(0).Position.Lat;
                    coords.NblLogitude = azureMapResults.Results.ElementAt(0).Position.Lon;
                    coords.Code = customer.Code;

                    apiEndpoint += filter;
                    var request = new HttpRequestMessage(HttpMethod.Patch, new Uri(apiEndpoint));
                    var json = JsonConvert.SerializeObject(coords);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.TryAddWithoutValidation("If-Match", customer.OdataEtag);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", this.authHeaderValue);

                    var response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var customerCoord = JsonConvert.DeserializeObject<ShipToAddresses>(responseJson);
                        return customerCoord;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
