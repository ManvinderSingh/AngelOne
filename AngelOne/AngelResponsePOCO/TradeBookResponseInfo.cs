namespace AngelOne.AngelResponsePOCO;

public class TradeBookResponseInfo
{
    public string exchange { get; set; }
    public string producttype { get; set; }
    public string tradingsymbol { get; set; }
    public string instrumenttype { get; set; }
    public string symbolgroup { get; set; }
    public decimal strikeprice { get; set; }
    public string optiontype { get; set; }
    public string expirydate { get; set; }
    public string marketlot { get; set; }
    public string precision { get; set; }
    public string multiplier { get; set; }
    public decimal tradevalue { get; set; }
    public string transactiontype { get; set; }
    public decimal fillprice { get; set; }
    public string fillsize { get; set; }
    public string orderid { get; set; }
    public string fillid { get; set; }
    public string filltime { get; set; }
    public string uniqueorderid { get; set; }
}
