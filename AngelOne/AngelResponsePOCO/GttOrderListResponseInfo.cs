namespace AngelOne.AngelResponsePOCO;

public class GttOrderListResponseInfo
{
    public string status { get; set; }

    // Use DateTimeOffset for date-time values with timezone information
    public DateTimeOffset createddate { get; set; }
    public DateTimeOffset updateddate { get; set; }
    public DateTimeOffset expirydate { get; set; }

    public string clientid { get; set; }
    public string tradingsymbol { get; set; }
    public string symboltoken { get; set; }
    public string exchange { get; set; }
    public string transactiontype { get; set; }
    public string producttype { get; set; }

    public decimal price { get; set; }
    public int qty { get; set; }
    public decimal triggerprice { get; set; }
    public int disclosedqty { get; set; }
}
