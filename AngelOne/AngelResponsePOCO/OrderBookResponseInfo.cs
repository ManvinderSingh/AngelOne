namespace AngelOne.AngelResponsePOCO
{
    public class OrderBookResponseInfo
    {
        public string variety { get; set; }
        public string ordertype { get; set; }
        public string producttype { get; set; }
        public string duration { get; set; }
        public double price { get; set; }
        public double triggerprice { get; set; }
        public string quantity { get; set; }
        public string disclosedquantity { get; set; }
        public double squareoff { get; set; }
        public double stoploss { get; set; }
        public double trailingstoploss { get; set; }
        public string tradingsymbol { get; set; }
        public string transactiontype { get; set; }
        public string exchange { get; set; }
        public string symboltoken { get; set; }
        public string ordertag { get; set; }
        public string instrumenttype { get; set; }
        public double strikeprice { get; set; }
        public string optiontype { get; set; }
        public string expirydate { get; set; }
        public string lotsize { get; set; }
        public string cancelsize { get; set; }
        public double averageprice { get; set; }
        public string filledshares { get; set; }
        public string unfilledshares { get; set; }
        public string orderid { get; set; }
        public string text { get; set; }
        public string status { get; set; }
        public string orderstatus { get; set; }
        public string updatetime { get; set; }
        public string exchtime { get; set; }
        public string exchorderupdatetime { get; set; }
        public string fillid { get; set; }
        public string filltime { get; set; }
        public string parentorderid { get; set; }
        public string uniqueorderid { get; set; }
        public string exchangeorderid { get; set; }
    }
}
