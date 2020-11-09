namespace azureapp.app365
{
    using System;
    using Newtonsoft.Json;
    
    public partial class ShipToAddressCoordinates
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }
}
