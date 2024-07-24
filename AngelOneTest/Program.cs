using AngelOne;
using AngelOne.AngelRequestPOCO;
using AngelOne.AngelResponsePOCO;
using Ninject;
using System.ComponentModel.DataAnnotations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var program = new Program();
        var smartApi = new SmartApi();
        var login = await smartApi.Login("K123456", "1234", "YOUR AUTHENTICATOR KEY", "API KEY");

        if (login)
        {
            Console.WriteLine("Is logged in");
            await program.GetLtp(smartApi);
            await program.GetOrderBook(smartApi);
            await program.GetAllHoldins(smartApi);
            await program.GetHolding(smartApi);
            await program.GetOpenPosition(smartApi);
            //await program.CreateGTTOrder(smartApi);
            //await program.GetGTTRuleList(smartApi);
            await program.GetHistoricalData(smartApi);

        }
        else
        {
            Console.WriteLine("Is not logged in");
        }
    }

    private async Task GetLtp(ISmartApi smartApi)
    {
        var nseSymbols = new List<string>();
        nseSymbols.Add("6232");
        nseSymbols.Add("445");
        nseSymbols.Add("12533");
        var symbols = new MultipleTokensInfo
        {
            NSE = nseSymbols
        };

        //var response = await smartApi.GetMultipleSymbolLtp(symbols);
        var response = await smartApi.GetMultipleSymbolOHLC(symbols);
        //var response = await smartApi.GetMultipleSymbolFullLtp(symbols);

        //var response = await smartApi.GetLtp(new LtpRequestInfo
        //{
        //    exchange = "NSE",
        //    tradingsymbol = "SBIN-EQ",
        //    symboltoken = "3045"
        //});
    }

    private async Task<List<OrderBookResponseInfo>> GetOrderBook(ISmartApi smartApi)
    {
        var response = await smartApi.GetOrderBook();
        return response;
    }

    private async Task GetAllHoldins(ISmartApi smartApi)
    {
        var response = await smartApi.GetAllHoldings();
    }

    private async Task GetHolding(ISmartApi smartApi)
    {
        var response = await smartApi.GetHolding();

    }
    private async Task GetOpenPosition(ISmartApi smartApi)
    {
        var result = await smartApi.GetPosition();
    }

    private async Task GetHistoricalData(ISmartApi smartApi)
    {
        Console.WriteLine("Getting historical data");
        var obj = new HistoricalDataRequestInfo
        {
            exchange = "NSE",
            fromdate = new DateTime(2024, 01, 01, 12, 0, 0),
            todate = new DateTime(2024, 03, 03, 12, 0, 0),
            interval = "ONE_DAY",
            symboltoken = "3048"
        };
        var response = await smartApi.GetHistoricalData(obj);
    }

    private async Task GetGTTRuleList(ISmartApi smartApi)
    {
        var obj = new GTTOrderListRequestInfo
        {
            status = ["NEW", "ACTIVE"]
        };
        var response = await smartApi.GetGTTOrdersList(obj);
    }

    private async Task CreateGTTOrder(ISmartApi smartApi)
    {
        Console.WriteLine("Creating GTT order");
        //place gtt order
        var gttOrder = new GTTOrderRequestInfo
        {
            tradingsymbol = "GICRE-EQ",
            symboltoken = "277",
            exchange = "NSE",
            transactiontype = "SELL",
            producttype = "DELIVERY",
            price = 404.15m,
            triggerprice = 404.25m,
            qty = 46
        };

        var response = await smartApi.CreateGTTOrder(gttOrder);

    }
}