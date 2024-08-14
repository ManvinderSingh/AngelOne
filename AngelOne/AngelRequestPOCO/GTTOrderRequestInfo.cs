namespace AngelOne.AngelRequestPOCO;

public class GTTOrderRequestInfo
{
    public string tradingsymbol { get; set; }
    public string symboltoken { get; set; }
    public Exchange exchange { get; set; }
    public TransactionType transactiontype { get; set; }
    public ProductType producttype { get; set; }
    public decimal price { get; set; }
    public int qty { get; set; }
    public decimal triggerprice { get; set; }
    public int disclosedqty { get; set; }
    public int timeperiod { get; set; }
}
