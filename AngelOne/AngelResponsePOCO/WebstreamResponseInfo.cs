using AngelOne.AngelRequestPOCO;

namespace AngelOne.AngelResponsePOCO;

public class WebStreamResponseInfo
{
    public decimal ltp { get; set; }
    public string token { get; set; }
    public StreamingExchangeType exchangeType { get; set; }
    public DateTime exchangeTimeStamp { get; set; }
    public string subscriptionMode { get; set; }
    public long lastTradedQuantity { get; set; }
    public long averageTradedPrice { get; set; }
    public long volumeTradedForTheDay { get; set; }
    public double totalBuyQty { get; set; }
    public double totalSellQty { get; set; }
    public long openPrice { get; set; }
    public long highPrice { get; set; }
    public long lowPrice { get; set; }
    public long closePrice { get; set; }
    public long lastTradedTimeStamp { get; set; }
    public long openInterest { get; set; }
    public long upperCircuitLimit { get; set; }
    public long lowerCiruitLimit { get; set; }
    public long FiftyTwoWeekHighPrice { get; set; }
    public long FiftyTwoWeekLowPrice { get; set; }
    public List<BestData> BestFiveData { get; set; }
    public List<BestData> BestTwentySellData { get; set; }
    public List<BestData> BestTwentyBuyData { get; set; }
}

public class BestData
{
    public TransactionType buy_sell { get; set; }
    public long qty { get; set; }
    public long price { get; set; }
    public int noOfOrders { get; set; }
}