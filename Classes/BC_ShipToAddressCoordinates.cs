namespace azureapp.mymapapp
{
    using Newtonsoft.Json;
    
    public partial class BC_ShipToAddressCoordinates
    {
        [JsonProperty("NBLLatitude")]
        public double NBLLatitude { get; set; }

        [JsonProperty("NBLLogitude")]
        public double NBLLogitude { get; set; }
    }
}
