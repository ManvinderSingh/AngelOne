using AngelOne.AngelResponsePOCO;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace AngelOne;

public class WebRequestHandler : IWebRequestHandler
{
    public async Task<T> GetJsonRequest<T>(string url, Dictionary<string, string> headers) where T : class
    {
        return await CommonGetRequest<T>(url, headers);
    }

    public async Task<ResponseInfo<T>> GetRequest<T>(string url, Dictionary<string, string> headers) where T : class
    {
        return await CommonGetRequest<ResponseInfo<T>>(url, headers); 
    }

    public async Task<ResponseInfo<T>> PostRequest<T>(string url, Dictionary<string, string> headers, string payload) where T : class
    {
        ValidateInput(url, headers);
        try
        {
            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                HttpContent httpContent = payload != null ? new StringContent(payload, Encoding.UTF8, "application/json") : null;

                HttpResponseMessage response = await client.PostAsync(url, httpContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                ResponseInfo<T> obj = JsonSerializer.Deserialize<ResponseInfo<T>>(content, jsonOptions);
                return obj;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"HttpRequestException: {ex.Message}");
            throw new Exception("Error making POST request: " + ex.Message, ex);
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"JsonException: {ex.Message}");
            throw new Exception("Error deserializing the response: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
            throw new Exception("Unexpected error: " + ex.Message, ex);
        }
    }

    #region Private Methods
    private void ValidateInput(string url, Dictionary<string, string> headers)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty.");
        }

        if (headers == null || headers.Count == 0)
        {
            throw new ArgumentException("Headers cannot be null or empty.");
        }
    }

    private async Task<T> CommonGetRequest<T>(string url, Dictionary<string, string> headers) where T : class
    {
        ValidateInput(url, headers);
        try
        {
            using (var client = new HttpClient())
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return default;
                }

                T obj = JsonSerializer.Deserialize<T>(content);
                return obj;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"HttpRequestException: {ex.Message}");
            throw new Exception("Error making POST request: " + ex.Message, ex);
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"JsonException: {ex.Message}");
            throw new Exception("Error deserializing the response: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
            throw new Exception("Unexpected error: " + ex.Message, ex);
        }
    }

    #endregion
}
