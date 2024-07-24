using AngelOne.AngelRequestPOCO;
using AngelOne.AngelResponsePOCO;

namespace AngelOne;

public interface ISmartApi
{
    Task<bool> Login(string userName, string pin, string authenticatorKey, string apiKey);
    Task<InstrumentInfo[]> GetInstrumentList();
    Task<int> CreateGTTOrder(GTTOrderRequestInfo gttorderRequestInfo);
    Task<List<GttOrderListResponseInfo>> GetGTTOrdersList(GTTOrderListRequestInfo gttOrderListInfo);
    Task<List<HistoricalDataResponseInfo>> GetHistoricalData(HistoricalDataRequestInfo historicalDataRequestInfo);
    Task<List<PositionInfo>> GetPosition();
    Task<List<HoldingResponseInfo>> GetHolding();
    Task<AllHoldingResponseInfo> GetAllHoldings();
    Task<CancelOrderResponseInfo> CancelOrder(CancelOrderRequestInfo cancelOrderRequestInfo);
    Task<List<OrderBookResponseInfo>> GetOrderBook();
    Task<OHLCResponseInfo> GetLtp(LtpRequestInfo ltpRequestInfo);
    Task<MultipleSymbolLtpResponseInfo<LtpInfo>> GetMultipleSymbolLtp(MultipleTokensInfo multipleSymbolRequestInfo);
    Task<MultipleSymbolLtpResponseInfo<OHLCResponseInfo>> GetMultipleSymbolOHLC(MultipleTokensInfo multipleSymbolRequestInfo);
    Task<MultipleSymbolLtpResponseInfo<FullLtpInfo>> GetMultipleSymbolFullLtp(MultipleTokensInfo multipleSymbolRequestInfo);
}
