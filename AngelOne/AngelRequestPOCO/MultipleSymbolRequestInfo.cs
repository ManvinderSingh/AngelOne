using System.Text.Json.Serialization;

namespace AngelOne.AngelRequestPOCO;

public class MultipleTokensInfo
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> NSE { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> NFO { get; set; }
}

public class MultipleSymbolRequestInfo
{
    public string mode { get; set; }
    public MultipleTokensInfo exchangeTokens { get; set; }
}

