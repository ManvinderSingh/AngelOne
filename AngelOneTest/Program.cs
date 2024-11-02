using AngelOne;
using AngelOne.AngelRequestPOCO;
using AngelOne.AngelResponsePOCO;
using System.Diagnostics;

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
            await program.WebsocketStreamind(smartApi);
            Console.ReadLine();
            await program.GetTradeBook(smartApi);
            var orderObj = await program.PlaceOrder(smartApi);
            if (orderObj != null) 
            {
                //get order status
                await program.GetIndividualOrderStatus(smartApi, orderObj.uniqueorderid);
                await program.ModifyOrder(smartApi, orderObj.orderid);
                await program.CancelOrder(smartApi, orderObj.orderid);
            }
            await program.GetLtp(smartApi);
            await program.GetOrderBook(smartApi);
            await program.GetAllHoldins(smartApi);
            await program.GetHolding(smartApi);
            await program.GetOpenPosition(smartApi);
            //var gttOrder = await program.CreateGTTOrder(smartApi);
            ////await program.CancelGttOrder(smartApi, gttOrder);
            //await program.GetGTTRuleList(smartApi);
            await program.GetHistoricalData(smartApi);

        }
        else
        {
            Console.WriteLine("Is not logged in");
        }
    }

    private async Task CancelGttOrder(ISmartApi smartApi, int orderId)
    {
        var response = await smartApi.CancelGttOrder(new CancelGttOrderRequestInfo
        {
            exchange = Exchange.NSE,
            symboltoken = "277",
            id = orderId
        });
    }

    public async Task CancelOrder(ISmartApi smartApi, string orderId)
    {
        var response = await smartApi.CancelOrder(new CancelOrderRequestInfo
        {
            orderid = orderId,
            variety = OrderVariety.NORMAL
        });
    }

    private async Task WebsocketStreamind(ISmartApi smartApi)
    {
        var tokenList = new List<string> { "13868", "17438", "14366", "11915" };
        var obj = new WebsocketStreaming();
        obj.RequestData = new WebStreamingRequestInfo
        {
            exchange = StreamingExchangeType.NSE,
            tokens = tokenList
        };
        obj.OnPriceUpdate += Obj_OnPriceUpdate;
        await obj.StartAsync();
    }
    private void Obj_OnPriceUpdate(WebStreamResponseInfo response)
    {
        Debug.WriteLine($"{DateTime.Now.TimeOfDay.ToString()} Message update for {response.token}, ltp is {response.ltp} at {response.exchangeTimeStamp} for exchnage {response.exchangeType}");
    }

    private async Task GetIndividualOrderStatus(ISmartApi smartApi, string uniqueOrderId)
    {
        var response = await smartApi.GetIndividualOrderStatus(uniqueOrderId);
    }

    private async Task GetTradeBook(ISmartApi smartApi)
    {
        var response = await smartApi.GetTradeBook();
    }

    private async Task<PlaceOrderResponseInfo> PlaceOrder(ISmartApi smartApi)
    {
        var requestInfo = new PlaceOrderRequestInfo
        {
            tradingsymbol = "ADANIENT-EQ",
            quantity = 10,
            symboltoken = "25",
            price = 1m,
            duration = OrderDuration.DAY,
            exchange = Exchange.NSE,
            ordertype = OrderType.LIMIT,
            producttype = ProductType.DELIVERY,
            disclosedquantity = 5
        };
        var response = await smartApi.PlaceOrder(requestInfo);
        return response;
    }

    public async Task ModifyOrder(ISmartApi smartApi, string orderId)
    {
        var requestInfo = new ModifyOrderRequestInfo
        {
            orderid = orderId,
            tradingsymbol = "ADANIENT-EQ",
            quantity = 10,
            symboltoken = "25",
            price = 1m,
            duration = OrderDuration.DAY,
            exchange = Exchange.NSE,
            ordertype = OrderType.LIMIT,
            producttype = ProductType.DELIVERY,
            disclosedquantity = 5
        };
        var response = await smartApi.ModifyOrder(requestInfo);
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

        //var multipleResponse = await smartApi.GetMultipleSymbolLtp(symbols);
        //var ohlcResponse = await smartApi.GetMultipleSymbolOHLC(symbols);
        //var fullResponse = await smartApi.GetMultipleSymbolFullLtp(symbols);

        var response = await smartApi.GetLtp(new LtpRequestInfo
        {
            exchange = Exchange.NSE,
            tradingsymbol = "SBIN-EQ",
            symboltoken = "3045"
        });
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
            exchange = Exchange.NSE,
            fromdate = new DateTime(2024, 01, 01, 12, 0, 0),
            todate = new DateTime(2024, 03, 03, 12, 0, 0),
            interval = Interval.ONE_DAY,
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

    private async Task<int> CreateGTTOrder(ISmartApi smartApi)
    {
        Console.WriteLine("Creating GTT order");
        //place gtt order
        var gttOrder = new GTTOrderRequestInfo
        {
            tradingsymbol = "GICRE-EQ",
            symboltoken = "277",
            exchange = Exchange.NSE,
            transactiontype = TransactionType.SELL,
            producttype = ProductType.DELIVERY,
            price = 404.15m,
            triggerprice = 404.25m,
            qty = 46
        };

        var response = await smartApi.CreateGTTOrder(gttOrder);
        return response;
    }
}