namespace AngelOne.AngelRequestPOCO;

public class PlaceOrderRequestInfo
{
    public OrderVariety variety { get; set; }
    public string tradingsymbol { get; set; }
    public string symboltoken { get; set; }
    public TransactionType transactiontype { get; set; }
    public Exchange exchange { get; set; }
    public OrderType ordertype { get; set; }
    public ProductType producttype { get; set; }
    public OrderDuration duration { get; set; }
    public decimal price { get; set; }
    public string squareoff { get; set; } = "0";
    public string stoploss { get; set; } = "0";
    public int quantity { get; set; }
    public int disclosedquantity { get; set; }
}
