namespace azureapp.app365
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class BC_ShipToAddresses
    {
        [JsonProperty("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonProperty("value")]
        public List<BC_ShipToAddress> Value { get; set; }
    }

    public partial class BC_ShipToAddress
    {
        [JsonProperty("@odata.etag")]
        public string OdataEtag { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("customerNo")]
        public string customerNo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vatRegistrationNo")]
        public string VatRegistrationNo { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("territoryCode")]
        public string TerritoryCode { get; set; }

        [JsonProperty("postCode")]
        public string PostCode { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("eMail")]
        public string EMail { get; set; }

        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty("salespersonCode")]
        public string SalespersonCode { get; set; }

        [JsonProperty("customerPriceGroup")]
        public string CustomerPriceGroup { get; set; }

        [JsonProperty("paymentTermsCode")]
        public string PaymentTermsCode { get; set; }

        [JsonProperty("shipmentMethodCode")]
        public string ShipmentMethodCode { get; set; }

        [JsonProperty("customerDiscGroup")]
        public string CustomerDiscGroup { get; set; }

        [JsonProperty("vrpBusinessAreaCode")]
        public string VrpBusinessAreaCode { get; set; }

        [JsonProperty("vrpBusinessZoneCode")]
        public string VrpBusinessZoneCode { get; set; }

        [JsonProperty("vrpSalespersonCode2")]
        public string VrpSalespersonCode2 { get; set; }

        [JsonProperty("Latitude")]
        public long Latitude { get; set; }

        [JsonProperty("Logitude")]
        public long NblLogitude { get; set; }

        [JsonProperty("comment")]
        public bool Comment { get; set; }
    }
}
