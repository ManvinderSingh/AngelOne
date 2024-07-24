namespace AngelOne.AngelResponsePOCO;

public class LtpInfo
{
    public string exchange { get; set; }
    public string tradingSymbol { get; set; }
    public string symbolToken { get; set; }
    public double ltp { get; set; }
}

public class OHLCResponseInfo
{
    public string exchange { get; set; }
    public string tradingSymbol { get; set; }
    public string symbolToken { get; set; }
    public double open { get; set; }
    public double high { get; set; }
    public double low { get; set; }
    public double close { get; set; }
    public double ltp { get; set; }
}

public class MultipleSymbolLtpResponseInfo<T> where T : class
{
    public List<T> fetched { get; set; }
    public List<T> unfetched { get; set; }
}
