namespace azureapp.prime365
{
    using Newtonsoft.Json;
    
    public partial class BC_ShipToAddressCoordinates
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }
}
