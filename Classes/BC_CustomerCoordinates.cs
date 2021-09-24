namespace azureapp.app365
{
    using Newtonsoft.Json;
    
    public partial class BC_CustomerCoordinates
    {
        [JsonProperty("no")]
        public string No { get; set; }

        [JsonProperty("NBLLatitude")]
        public double NblLatitude { get; set; }

        [JsonProperty("NBLLogitude")]
        public double NblLogitude { get; set; }
    }
}
