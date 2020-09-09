using System;
using Microsoft.Extensions.Configuration;

namespace varprime.app365
{

    public partial class ConnectorConfig
    {
        public ConnectorConfig(IConfigurationRoot config)
        {
            if (config != null)
            {
                WebServiceUser = config["webServiceUser"];
                WebServicePassword = config["webServicePassword"];
                WebServiceAltImageUrl = config["webServiceAltImageUrl"];
                WebServiceMainImageUrl = config["webServiceMainImageUrl"];

                WebServiceCompany = config["webServiceCompany"];

                StorageAccountName = config["storageAccountName"];
                StorageAccountKey = config["storageAccountKey"];
                StorageEndpoint = config["storageEndpoint"];
            }
            // If you are customizing here it means you
            //  should give a look on how use
            //  azure configuration file
            if (String.IsNullOrEmpty(WebServiceUser))
                WebServiceUser = "WEBUSER";
            if (String.IsNullOrEmpty(WebServicePassword))
                WebServicePassword = "Yj9GRfVmbOHK0fXyj6N/hXgiKG1kOneDe5PzJfaNr6I=";
            if (String.IsNullOrEmpty(WebServiceAltImageUrl))
                WebServiceAltImageUrl = "https://api.businesscentral.dynamics.com/v1.0/3c9c4b25-e43a-4050-ba5a-9f850281fe47/sandbox/api/VRP/APP365DEAPI/v2.0/companies(e0a970ca-d7b9-ea11-9ab4-000d3aaaf5fe)/APP365DEDocumentAttachmentsBase64({0})";
            if (String.IsNullOrEmpty(WebServiceMainImageUrl))
                WebServiceMainImageUrl = "https://api.businesscentral.dynamics.com/v1.0/3c9c4b25-e43a-4050-ba5a-9f850281fe47/sandbox/api/v1.0/companies(e0a970ca-d7b9-ea11-9ab4-000d3aaaf5fe)/items({0})/picture({0})/content";
        }

        public String WebServiceUser;
        public String WebServicePassword;
        public String WebServiceAltImageUrl;
        public String WebServiceMainImageUrl;
        public String WebServiceCompany;

        public String StorageAccountName;
        public String StorageAccountKey;
        public String StorageEndpoint;

    }
}
