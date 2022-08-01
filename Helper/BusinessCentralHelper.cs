using System.Diagnostics;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace azureapp.prime365
{
    public partial class BusinessCentralHelper
    {

        string apiEndpoint;
        string apiEndpoint4C;
        string authHeaderValue;
        ConnectorConfig config;
        ILogger log;

        public BusinessCentralHelper(ConnectorConfig config, string entity, ILogger log)
        {
            this.config = config;
            this.log = log;
            this.apiEndpoint = String.Format(config.BcWebServiceMainUrl, config.BcWebServiceCompany, entity);
            this.apiEndpoint4C = String.Format(config.BcWebServiceCustUrl, config.BcWebServiceCompany, entity);

            var authData = string.Format("{0}:{1}", config.BcWebServiceUser, config.BcWebServicePassword);
            this.authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
        }

        public BC_Customers GetCustomers()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var uri = new Uri(this.apiEndpoint);

                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeaderValue;

                    var response = client.DownloadString(uri).ToString();
                    var entities = JsonConvert.DeserializeObject<BC_Customers>(response.ToString());
                    return entities;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
                return null;
            }

        }

        public BC_ShipToAddresses GetShipToAddress()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var uri = new Uri(this.apiEndpoint4C);

                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + authHeaderValue;

                    var response = client.DownloadString(uri).ToString();
                    var entities = JsonConvert.DeserializeObject<BC_ShipToAddresses>(response.ToString());
                    return entities;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
                return null;
            }


        }
        public async Task<BC_CustomerCoordinates> UpdateCustomer(BC_Customer customer, AzureMapResults azureMapResults, string filter)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var coords = new BC_CustomerCoordinates();
                    coords.NblLatitude = azureMapResults.Results.ElementAt(0).Position.Lat;
                    coords.NblLogitude = azureMapResults.Results.ElementAt(0).Position.Lon;
                    coords.No = customer.No;

                    var apiUpdateEndpoint = apiEndpoint + filter;
                    var request = new HttpRequestMessage(HttpMethod.Patch, new Uri(apiUpdateEndpoint));
                    var json = JsonConvert.SerializeObject(coords);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.TryAddWithoutValidation("If-Match", customer.OdataEtag);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", this.authHeaderValue);

                    var response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var customerCoord = JsonConvert.DeserializeObject<BC_CustomerCoordinates>(responseJson);
                        return customerCoord;
                    }
                    else
                    {
                        Debugger.Break();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
                return null;
            }


        }

        public async Task<BC_ShipToAddresses> UpdateShipToAddress(BC_ShipToAddress customer, AzureMapResults azureMapResults, string filter)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var coords = new BC_ShipToAddressCoordinates();
                    coords.Latitude = azureMapResults.Results.ElementAt(0).Position.Lat;
                    coords.Longitude = azureMapResults.Results.ElementAt(0).Position.Lon;
                    coords.Code = customer.Code;

                    var apiUpdateEndpoint = apiEndpoint4C + filter;
                    var request = new HttpRequestMessage(HttpMethod.Patch, new Uri(apiUpdateEndpoint));
                    var json = JsonConvert.SerializeObject(coords);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.TryAddWithoutValidation("If-Match", customer.OdataEtag);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", this.authHeaderValue);

                    var response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var customerCoord = JsonConvert.DeserializeObject<BC_ShipToAddresses>(responseJson);
                        return customerCoord;
                    }
                    else
                    {
                        Debugger.Break();
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
                return null;
            }


        }
    }
}
