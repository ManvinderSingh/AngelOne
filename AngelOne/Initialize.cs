namespace AngelOne;

public class Initialize
{
    public static string UserName { get; private set; }
    public static string Pin { get; private set; }
    public static string AuthenticatorKey { get; private set; }
    public static string ApiKey { get; private set; }

    public Initialize(string userName, string pinCode, string authenticatorKey, string apiKey)
    {
        UserName = userName;
        Pin = pinCode;
        AuthenticatorKey = authenticatorKey;
        ApiKey = apiKey;
    }
}
