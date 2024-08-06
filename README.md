
# AngelOne smartAPI

This is C# wrapper for smartAPI from AngelOne.


You can download the nuget package from here.
https://www.nuget.org/packages/AngelOne

## Demo

Added demo code for the wrapper [here](https://github.com/ManvinderSingh/AngelOne/blob/master/AngelOneTest/Program.cs).

## Usage/Examples

```c#
 var smartApi = new SmartApi();
 var loginResult = await smartApi.Login("K123456", "1234", "YOUR AUTHENTICATOR KEY", "API KEY");

if (loginResult)
{
    //we are in business here, we can start making the calls to smartAPI now.
    var orderBook = await smartApi.GetOrderBook();
    var holdings = await smartApi.GetAllHoldings();
}

```

For now these methods are supported.

### Methods

1. GetInstrumentList
2. CreateGTTOrder
3. GetGTTOrderList
4. GetHistoricalData
5. GetPosition
6. GetHolding
7. GetAllHoldings
8. CancelOrder
9. GetOrderBook
10. GetLtp
11. GetMultipleSymbolLtp
12. GetMultipleSymbolOHLC
13. GetMultipleSymbolFullLtp

## Roadmap

- Make it fully compatible with the smartAPI.

- Bringing all methods over here.


## Support

For support, email manvindersingh@outlook.com
I'll try to get back to you ASAP.
