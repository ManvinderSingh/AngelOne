namespace AngelOne.AngelResponsePOCO;

public class InstrumentInfo
{
    public string token { get; set; }

    public string symbol { get; set; }

    public string name { get; set; }

    public string expiry { get; set; }
    public DateTime expiry_date
    {
        get
        {
            if (string.IsNullOrWhiteSpace(expiry)) return DateTime.Now;
            return DateTime.ParseExact(expiry, "ddMMMyyyy", null).AddHours(15).AddMinutes(30);
        }
    }

    public string strike { get; set; }
    public decimal strike_decimal => Math.Round(decimal.Parse(strike) / 100, 1); //because the value it returned is in paise not rupees
    public string lotsize { get; set; }
    public string option_type => symbol.Substring(symbol.Length - 2);
    public string instrumenttype { get; set; }

    public string exch_seg { get; set; }

    public string tick_size { get; set; }
    public string strike_type { get; set; }

    public decimal ltp { get; set; }
}
