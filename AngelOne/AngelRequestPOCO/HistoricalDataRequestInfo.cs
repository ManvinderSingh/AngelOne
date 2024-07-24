using System.Text.Json.Serialization;

namespace AngelOne.AngelRequestPOCO;

public class HistoricalDataRequestInfo
{
    public string exchange { get; set; }
    public string symboltoken { get; set; }
    public string interval { get; set; }
    [JsonIgnore]
    public DateTime fromdate { get; set; }
    [JsonIgnore]
    public DateTime todate { get; set; }
    [JsonPropertyName("fromdate")]
    public string from_date_string => fromdate.ToString("yyyy-MM-dd hh:mm");
    [JsonPropertyName("todate")]
    public string to_date_string => todate.ToString("yyyy-MM-dd hh:mm");
}
