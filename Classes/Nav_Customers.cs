namespace azureapp.prime365
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class Nav_Customers
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<Nav_Customer> Value { get; set; }

        [JsonProperty("@odata.nextLink")]
        public Uri OdataNextLink { get; set; }
    }

    public partial class Nav_Customer
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("No")]
        public string No { get; set; }


        [JsonProperty("Prime365Lat")]
        public decimal Prime365Lat { get; set; }

        [JsonProperty("Prime365Lng")]
        public decimal Prime365Lng { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("Post_Code")]
        public string PostCode { get; set; }


        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("County")]
        public string County { get; set; }

        [JsonProperty("Country_Region_Code")]
        public string CountryRegionCode { get; set; }



        [JsonProperty("ETag")]
        public string ETag { get; set; }
    }
}
