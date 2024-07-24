namespace AngelOne.AngelResponsePOCO;

public class HistoricalDataResponseInfo
{
    public DateTime date { get; set; }
    public decimal open { get; set; }
    public decimal high { get; set; }
    public decimal low { get; set; }
    public decimal close { get; set; }
    public long volume { get; set; }
    public string symbol { get; set; }
}
