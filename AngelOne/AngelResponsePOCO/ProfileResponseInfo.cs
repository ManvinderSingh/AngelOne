namespace AngelOne.AngelResponsePOCO;

public class ProfileResponseInfo
{
    public string clientcode { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string mobileno { get; set; }
    public string[] exchanges    { get; set; }
    public string[] products { get; set; }
    public string lastlogintime { get; set; }
    public string brokerid { get; set; }
}
