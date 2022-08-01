using System;
using Microsoft.Extensions.Configuration;

namespace azureapp.mymapapp
{

    public partial class ConnectorConfig
    {
        public ConnectorConfig(IConfigurationRoot config)
        {
            if (config != null)
            {
                BcWebServiceUser = config["bcWebServiceUser"];
                BcWebServicePassword = config["bcWebServicePassword"];
                BcWebServiceMainUrl = config["bcWebServiceMainUrl"];
                BcWebServiceCustUrl = config["bcWebServiceCustUrl"];
                BcWebServiceCompany = config["bcWebServiceCompany"];

                NavWebServiceCompany = config["navWebServiceCompany"];
                NavWebServiceUser = config["navWebServiceUser"];
                NavWebServicePassword = config["navWebServicePassword"];
                NavWebServiceDomain = config["navWebServiceDomain"];
                NavWebServiceMainUrl = config["navWebServiceMainUrl"];

                AzureMapKey = config["azureMapKey"];
                AzureMapEndpoint = config["azureMapEndpoint"];
            }
        }

        public string BcWebServiceUser;
        public string BcWebServicePassword;
        public string BcWebServiceMainUrl;
        public string BcWebServiceCustUrl;
        public string BcWebServiceCompany;

        public string NavWebServiceCompany;
        public string NavWebServiceUser;
        public string NavWebServicePassword;
        public string NavWebServiceDomain;
        public string NavWebServiceMainUrl;

        public string AzureMapKey;
        public string AzureMapEndpoint;

    }
}
