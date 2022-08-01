using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Linq;
using System.Net.Http.Headers;

namespace azureapp.prime365
{
    public partial class NavHelper
    {

        ConnectorConfig config;
        ILogger log;

        public NavHelper(ConnectorConfig config, ILogger log)
        {
            this.config = config;
            this.log = log;
        }

        public async Task<T> Get<T>(string apiEntity, string odataNextLink)
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            var company = System.Net.WebUtility.UrlEncode(config.NavWebServiceCompany);

            var endPoint = string.Format(config.NavWebServiceMainUrl, company, apiEntity);
            if (!string.IsNullOrEmpty(odataNextLink))
                endPoint = odataNextLink;

            var ntlmHandler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(config.NavWebServiceUser, config.NavWebServicePassword, config.NavWebServiceDomain)
            };

            using (var httpClient = new HttpClient(ntlmHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(2);

                var pageSize = 20000;
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var strPageSize = string.Format("odata.include-annotations=*, maxpagesize={0}, odata.maxpagesize={1}", pageSize, pageSize);
                httpClient.DefaultRequestHeaders.Add("Prefer", strPageSize);

                log.LogInformation(string.Format("{0} {1} {2}", "Start get", apiEntity, endPoint));

                var responseMessage = await httpClient.GetAsync(endPoint);
                if (responseMessage.IsSuccessStatusCode)
                {
                    log.LogInformation(string.Format("{0} {1} {2}", "End get", apiEntity, endPoint));
                    var value = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
                    log.LogInformation(string.Format("{0} {1} {2}", "End DeserializeObject", apiEntity, endPoint));
                    return value;
                }
                else
                {
                    log.LogInformation(string.Format("{0} {1} {2}", "Error get", apiEntity, endPoint));
                    log.LogCritical(responseMessage.StatusCode.ToString());
                    log.LogCritical(responseMessage.ReasonPhrase);
                    throw new Exception(responseMessage.ReasonPhrase);
                }
            }
        }
        public async Task<Nav_CustomerCoordinates> UpdateCustomer(string apiEntity, Nav_Customer customer, AzureMapResults azureMapResults, string filter)
        {
            try
            {
                AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
                var company = System.Net.WebUtility.UrlEncode(config.NavWebServiceCompany);

                var endPoint = string.Format(config.NavWebServiceMainUrl, company, apiEntity);

                var ntlmHandler = new HttpClientHandler
                {
                    Credentials = new NetworkCredential(config.NavWebServiceUser, config.NavWebServicePassword, config.NavWebServiceDomain)
                };

                using (var client = new HttpClient(ntlmHandler))
                {
                    var coords = new Nav_CustomerCoordinates();
                    coords.Prime365Lat = (decimal)azureMapResults.Results.ElementAt(0).Position.Lat;
                    coords.Prime365Lng = (decimal)azureMapResults.Results.ElementAt(0).Position.Lon;
                    coords.No = customer.No;

                    var apiUpdateEndpoint = endPoint + filter;
                    var request = new HttpRequestMessage(HttpMethod.Patch, new Uri(apiUpdateEndpoint));
                    var json = JsonConvert.SerializeObject(coords);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.TryAddWithoutValidation("If-Match", customer.OdataEtag);

                    var response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var customerCoord = JsonConvert.DeserializeObject<Nav_CustomerCoordinates>(responseJson);
                        return customerCoord;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}
