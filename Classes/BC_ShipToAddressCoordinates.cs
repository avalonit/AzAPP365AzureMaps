namespace azureapp.mymapapp
{
    using Newtonsoft.Json;
    
    public partial class BC_ShipToAddressCoordinates
    {
        [JsonProperty("customerNo")]
        public string CustomerNo { get; set; }
        
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
    }
}
