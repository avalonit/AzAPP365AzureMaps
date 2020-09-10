namespace azureapp.app365
{
    using System;
    using Newtonsoft.Json;
    
    public partial class CustomerCoordinates
    {
        [JsonProperty("no")]
        public string No { get; set; }

        [JsonProperty("NBLLatitude")]
        public double NblLatitude { get; set; }

        [JsonProperty("NBLLogitude")]
        public double NblLogitude { get; set; }
    }
}
