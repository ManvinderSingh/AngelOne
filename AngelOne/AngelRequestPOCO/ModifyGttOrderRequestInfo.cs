namespace AngelOne.AngelRequestPOCO;

public class ModifyGttOrderRequestInfo
{
    public string id { get; set; }
    public string symboltoken { get; set; }
    public Exchange exchange { get; set; }
    public decimal price { get; set; }
    public int qty { get; set; }
    public decimal triggerprice { get; set; }
    public int disclosedqty { get; set; }
    public string timeperiod { get; set; }
}
