namespace azureapp.mymapapp
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class AzureMapResults
    {
        [JsonProperty("summary")]
        public Summary Summary { get; set; }

        [JsonProperty("results")]
        public List<AzureMapResult> Results { get; set; }
    }

    public partial class AzureMapResult
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("streetName")]
        public string StreetName { get; set; }

        [JsonProperty("municipality")]
        public string Municipality { get; set; }

        [JsonProperty("countrySecondarySubdivision")]
        public string CountrySecondarySubdivision { get; set; }

        [JsonProperty("countrySubdivision")]
        public string CountrySubdivision { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryCodeISO3")]
        public string CountryCodeIso3 { get; set; }

        [JsonProperty("freeformAddress")]
        public string FreeformAddress { get; set; }

        [JsonProperty("localName")]
        public string LocalName { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }

    public partial class Viewport
    {
        [JsonProperty("topLeftPoint")]
        public Position TopLeftPoint { get; set; }

        [JsonProperty("btmRightPoint")]
        public Position BtmRightPoint { get; set; }
    }

    public partial class Summary
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("queryType")]
        public string QueryType { get; set; }

        [JsonProperty("queryTime")]
        public long QueryTime { get; set; }

        [JsonProperty("numResults")]
        public long NumResults { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("fuzzyLevel")]
        public long FuzzyLevel { get; set; }
    }
}
