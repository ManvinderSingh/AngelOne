namespace AngelOne;

public static class URLs
{
    public static string Login => "https://apiconnect.angelbroking.com/rest/auth/angelbroking/user/v1/loginByPassword";
    public static string CreateGTTOrder => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/gtt/v1/createRule";
    public static string GetAllGTTOrders => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/gtt/v1/ruleList";
    public static string GetHistoricalData =>  "https://apiconnect.angelbroking.com/rest/secure/angelbroking/historical/v1/getCandleData";
    public static string GetOpenPosition => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/getPosition";
    public static string GetLtpData => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/getLtpData";
    public static string GetHolding => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/portfolio/v1/getHolding";
    public static string GetAllHoldings => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/portfolio/v1/getAllHolding";
    public static string CancelOrder => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/cancelOrder";
    public static string GetOrderBook => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/getOrderBook";
    public static string GetInstrumentsList = "https://margincalculator.angelbroking.com/OpenAPI_File/files/OpenAPIScripMaster.json";
    public static string GetMultipleLtpData => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/market/v1/quote/";
    public static string PlaceOrder => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/placeOrder";
    public static string ModifyOrder => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/modifyOrder";
    public static string GetTradeBook => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/getTradeBook";
    public static string GetIndividualOrderData => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/order/v1/details/";
    public static string WebSocketUrl => "wss://smartapisocket.angelone.in/smart-stream";
    public static string GetProfile => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/user/v1/getProfile";
    public static string GetFundsAndMargins => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/user/v1/getRMS";
    public static string CancelGttOrder => "https://apiconnect.angelbroking.com/rest/secure/angelbroking/gtt/v1/cancelRule";
}
