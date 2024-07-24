using System.Text.Json.Serialization;

namespace AngelOne.AngelResponsePOCO;

public class DepthEntry
{
    public double price { get; set; }
    public int quantity { get; set; }
    public int orders { get; set; }
}

public class Depth
{
    public List<DepthEntry> buy { get; set; }
    public List<DepthEntry> sell { get; set; }
}

public class FullLtpInfo
{
    public string exchange { get; set; }
    public string tradingSymbol { get; set; }
    public string symbolToken { get; set; }
    public decimal ltp { get; set; }
    public decimal open { get; set; }
    public decimal high { get; set; }
    public decimal low { get; set; }
    public decimal close { get; set; }
    public int lastTradeQty { get; set; }
    public string exchFeedTime { get; set; }
    public string exchTradeTime { get; set; }
    public decimal netChange { get; set; }
    public decimal percentChange { get; set; }
    public decimal avgPrice { get; set; }
    public long tradeVolume { get; set; }
    public long opnInterest { get; set; }
    public decimal lowerCircuit { get; set; }
    public decimal upperCircuit { get; set; }
    public int totBuyQuan { get; set; }
    public int totSellQuan { get; set; }
    [JsonPropertyName("52WeekLow")]
    public decimal _52WeekLow { get; set; }
    [JsonPropertyName("52WeekHigh")]
    public decimal _52WeekHigh { get; set; }
    public Depth depth { get; set; }
}

