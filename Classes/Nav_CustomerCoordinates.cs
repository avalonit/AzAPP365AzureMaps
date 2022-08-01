namespace azureapp.mymapapp
{
    using Newtonsoft.Json;

    public partial class Nav_CustomerCoordinates
    {
        [JsonProperty("No")]
        public string No { get; set; }

        [JsonProperty("Prime365Lat")]
        public decimal Prime365Lat { get; set; }

        [JsonProperty("Prime365Lng")]
        public decimal Prime365Lng { get; set; }
    }
}
