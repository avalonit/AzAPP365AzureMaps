using System;
using Microsoft.Extensions.Configuration;

namespace azureapp.app365
{

    public partial class ConnectorConfig
    {
        public ConnectorConfig(IConfigurationRoot config)
        {
            if (config != null)
            {
                WebServiceUser = config["webServiceUser"];
                WebServicePassword = config["webServicePassword"];
                WebServiceMainUrl = config["webServiceMainUrl"];
                WebServiceCompany = config["webServiceCompany"];

                AzureMapKey = config["azureMapKey"];
                AzureMapEndpoint = config["azureMapEndpoint"];
            }
        }

        public String WebServiceUser;
        public String WebServicePassword;
        public String WebServiceMainUrl;
        public String WebServiceCompany;
        public String AzureMapKey;
        public String AzureMapEndpoint;

    }
}
