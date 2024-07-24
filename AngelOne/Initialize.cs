using Ninject;

namespace AngelOne;

public class IoC
{
    public static IKernel Kernel { get; set; }
}

public class Initialize
{
    public static string UserName { get; private set; }
    public static string Pin { get; private set; }
    public static string AuthenticatorKey { get; private set; }
    public static string ApiKey { get; private set; }

    public Initialize(string userName, string pinCode, string authenticatorKey, string apiKey)
    {
        IoC.Kernel = new StandardKernel();
        IoC.Kernel.Bind<IWebRequestHandler>().To<WebRequestHandler>();
        IoC.Kernel.Bind<ISmartApi>().To<SmartApi>();

        UserName = userName;
        Pin = pinCode;
        AuthenticatorKey = authenticatorKey;
        ApiKey = apiKey;
    }
}
