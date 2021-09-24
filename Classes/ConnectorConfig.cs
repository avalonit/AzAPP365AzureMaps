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
                BcWebServiceUser = config["bcBcWebServiceUser"];
                BcWebServicePassword = config["bcBcWebServicePassword"];
                BcWebServiceMainUrl = config["bcWebServiceMainUrl"];
                BcWebServiceCustUrl = config["bcWebServiceCustUrl"];
                BcWebServiceCompany = config["bcBcWebServiceCompany"];

                AzureMapKey = config["azureMapKey"];
                AzureMapEndpoint = config["azureMapEndpoint"];
            }
        }

        public String BcWebServiceUser;
        public String BcWebServicePassword;
        public String BcWebServiceMainUrl;
        public String BcWebServiceCustUrl;
        public String BcWebServiceCompany;
        public String AzureMapKey;
        public String AzureMapEndpoint;

    }
}
