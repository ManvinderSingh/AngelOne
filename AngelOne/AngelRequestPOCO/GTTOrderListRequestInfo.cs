namespace AngelOne.AngelRequestPOCO;

public class GTTOrderListRequestInfo
{
    public GTTOrderListRequestInfo()
    {
        page = 1;
        count = 20;
    }
    public string[] status { get; set; }
    /// <summary>
    /// default for page is 1
    /// </summary>
    public int page { get; set; }
    /// <summary>
    /// default for count is 20
    /// </summary>
    public int count { get; set; }
}
