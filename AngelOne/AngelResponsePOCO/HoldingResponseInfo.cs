namespace AngelOne.AngelResponsePOCO;
public class AllHoldingResponseInfo
{
    public List<HoldingResponseInfo> holdings { get; set; }
    public TotalHolding totalholding { get; set; }
}
public class TotalHolding
{
    public double totalholdingvalue { get; set; }
    public double totalinvvalue { get; set; }
    public double totalprofitandloss { get; set; }
    public double totalpnlpercentage { get; set; }
}

public class HoldingResponseInfo
{
    public string tradingsymbol { get; set; }
    public string exchange { get; set; }
    public string isin { get; set; }
    public int t1quantity { get; set; }
    public int realisedquantity { get; set; }
    public int quantity { get; set; }
    public int authorisedquantity { get; set; }
    public string product { get; set; }
    public double? collateralquantity { get; set; } // nullable double for null value
    public string collateraltype { get; set; }
    public double haircut { get; set; }
    public double averageprice { get; set; }
    public double ltp { get; set; }
    public string symboltoken { get; set; }
    public double close { get; set; }
    public double profitandloss { get; set; }
    public double pnlpercentage { get; set; }
}
