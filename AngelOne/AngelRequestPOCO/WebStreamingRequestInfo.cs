using System.Text.Json.Serialization;

namespace AngelOne.AngelRequestPOCO;

public class WebStreamingRequestInfo
{
    public StreamingExchangeType exchange { get; set; }
    public List<string> tokens { get; set; }
}

internal class StreamingRequestModel
{
    public int action { get; set; }
    [JsonPropertyName("params")]
    public StreamingParamsInfo someParameters { get; set; }

}

internal class StreamingParamsInfo
{
    public int mode { get; set; }
    public StreamingTokenListInfo[] tokenList { get; set; }
}

internal class StreamingTokenListInfo
{
    public StreamingExchangeType exchangeType { get; set; }
    public List<string> tokens { get; set; }
}

public enum StreamingExchangeType
{
    NSE = 1,
    NSE_FO = 2,
    BSE = 3,
    BSE_FO = 4,
    MCX_FO = 5,
    NCX_FO = 7,
    CDE_FO = 13
}