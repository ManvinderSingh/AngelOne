using AngelOne.AngelRequestPOCO;

namespace AngelOne.AngelResponsePOCO;

public class WebStreamResponseInfo
{
    public decimal ltp { get; set; }
    public string token { get; set; }
    public StreamingExchangeType exchangeType { get; set; }
    public DateTime exchangeTimeStamp { get; set; }
    public string subscriptionMode { get; set; }
}
