using AngelOne.AngelResponsePOCO;

namespace AngelOne;

public interface IWebRequestHandler
{

    Task<ResponseInfo<T>> PostRequest<T>(string url, Dictionary<string, string> headers, string payload) where T : class;
    Task<ResponseInfo<T>> GetRequest<T>(string url, Dictionary<string, string> headers) where T : class;
    Task<T> GetJsonRequest<T>(string url, Dictionary<string, string> headers) where T : class;
}
