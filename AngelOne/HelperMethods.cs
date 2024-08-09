using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using OtpNet;
using System.Text.Json;

namespace AngelOne;

public class HelperMethods
{
    public static string LocalIpAddress { get; }
    public static string PublicIpAddress { get; }
    public static string MacAddress { get; }

    static HelperMethods()
    {
        LocalIpAddress = GetLocalIP();
        PublicIpAddress = GetPublicIp().Result;
        MacAddress = GetMacAddress();
    }

    private static string GetLocalIP()
    {
        string localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }
        return localIP;
    }

    private static async Task<string> GetPublicIp()
    {
        using (var client = new HttpClient())
        {
            string externalIpString = await client.GetStringAsync("http://icanhazip.com").ConfigureAwait(false);
            externalIpString = externalIpString.Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);
            return externalIp.ToString();
        }
    }

    private static string GetMacAddress()
    {
        var result = NetworkInterface.GetAllNetworkInterfaces()
                                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                .Select(nic => nic.GetPhysicalAddress().ToString())
                                .FirstOrDefault();
        if (result == null) return "";
        return result;
    }

    public static string GetTotpFromAuthenticator(string authenticator)
    {
        var arr = Base32Encoding.ToBytes(authenticator);
        var totp = new Totp(arr);
        return totp.ComputeTotp();
    }

    public static string Serialize<T>(T obj)
    {
        var payload = System.Text.Json.JsonSerializer.Serialize(obj);
        return payload;
    }

    public static string SerializeWithOptions<T>(T obj, JsonSerializerOptions options)
    {
        var payload = System.Text.Json.JsonSerializer.Serialize(obj, options);
        return payload;
    }

    public static DateTime FromEpochToDateTime(long epochMilliseconds)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(epochMilliseconds);
        DateTime dateTime = dateTimeOffset.UtcDateTime;
        // If you want to convert to local time
        DateTime localDateTime = dateTimeOffset.LocalDateTime;
        return localDateTime;
    }

}
