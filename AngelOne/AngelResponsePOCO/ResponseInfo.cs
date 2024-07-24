namespace AngelOne.AngelResponsePOCO;

public class ResponseInfo<T> where T : class
{
    public bool status { get; set; }
    public string message { get; set; }
    public string errorcode { get; set; }
    public T data { get; set; }
}
