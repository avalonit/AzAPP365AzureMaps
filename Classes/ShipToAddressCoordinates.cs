namespace azureapp.app365
{
    using System;
    using Newtonsoft.Json;
    
    public partial class ShipToAddressCoordinates
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("NBLLatitude")]
        public double NblLatitude { get; set; }

        [JsonProperty("NBLLogitude")]
        public double NblLogitude { get; set; }
    }
}
