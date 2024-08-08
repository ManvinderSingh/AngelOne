using AngelOne.AngelRequestPOCO;
using AngelOne.AngelResponsePOCO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AngelOne;

public class SmartApi : ISmartApi
{
    public SmartApi()
    {
        InitializeHeaders();
        _webRequestHandler = new WebRequestHandler();
    }
    #region Public Methods

    public async Task<InstrumentInfo[]> GetInstrumentList()
    {
        try
        {
            var result = await _webRequestHandler.GetJsonRequest<InstrumentInfo[]>(URLs.GetInstrumentsList, _headers);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving instrument list", ex);
        }
    }

    public async Task<List<OrderBookResponseInfo>> GetOrderBook()
    {
        try
        {
            var response = await _webRequestHandler.GetRequest<List<OrderBookResponseInfo>>(URLs.GetOrderBook, _headers);
            return response.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error executing GetOrderBook", ex);
        }
    }

    public async Task<CancelOrderResponseInfo> CancelOrder(CancelOrderRequestInfo cancelOrderRequestInfo)
    {
        try
        {
            var response = await _webRequestHandler.PostRequest<CancelOrderResponseInfo>(URLs.CancelOrder, _headers, HelperMethods.Serialize(cancelOrderRequestInfo));
            return response.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error executing CancelOrder", ex);
        }
    }

    public async Task<List<HoldingResponseInfo>> GetHolding()
    {
        try
        {
            var response = await _webRequestHandler.GetRequest<List<HoldingResponseInfo>>(URLs.GetHolding, _headers);
            return response.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving GetHolding", ex);
        }
    }

    public async Task<AllHoldingResponseInfo> GetAllHoldings()
    {
        try
        {
            var response = await _webRequestHandler.GetRequest<AllHoldingResponseInfo>(URLs.GetAllHoldings, _headers);
            return response.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving GetAllHoldings", ex);
        }
    }

    public async Task<int> CreateGTTOrder(GTTOrderRequestInfo gttorderRequestInfo)
    {
        try
        {
            var response = await _webRequestHandler.PostRequest<GttOrderResponseInfo>(URLs.CreateGTTOrder, _headers, HelperMethods.Serialize(gttorderRequestInfo));
            return response.data.id;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving CreateGTTOrder.", ex);
        }
    }

    public async Task<List<GttOrderListResponseInfo>> GetGTTOrdersList(GTTOrderListRequestInfo gttOrderListInfo)
    {
        try
        {
            var response = await _webRequestHandler.PostRequest<List<GttOrderListResponseInfo>>(URLs.GetAllGTTOrders, _headers, HelperMethods.Serialize(gttOrderListInfo));
            return response.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retriving Get GTTOrderList", ex);
        }
    }

    public async Task<List<HistoricalDataResponseInfo>> GetHistoricalData(HistoricalDataRequestInfo historicalDataRequestInfo)
    {
        try
        {
            var response = await _webRequestHandler.PostRequest<List<List<object>>>(URLs.GetHistoricalData, _headers, HelperMethods.Serialize(historicalDataRequestInfo));
            var result = new List<HistoricalDataResponseInfo>();
            foreach (var item in response.data)
            {
                if (DateTime.TryParse(item[0].ToString(), out DateTime date) &&
                    decimal.TryParse(item[1].ToString(), out decimal open) &&
                    decimal.TryParse(item[2].ToString(), out decimal high) &&
                    decimal.TryParse(item[3].ToString(), out decimal low) &&
                    decimal.TryParse(item[4].ToString(), out decimal close) &&
                    long.TryParse(item[5].ToString(), out long volume))
                {
                    result.Add(new HistoricalDataResponseInfo
                    {
                        date = date,
                        open = open,
                        high = high,
                        low = low,
                        close = close,
                        volume = volume
                    });
                }
                else
                {
                    throw new Exception("Faild to parse one of more data fields");
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving historical data.", ex);
        }
    }

    public async Task<List<PositionInfo>> GetPosition()
    {
        try
        {
            var result = await _webRequestHandler.GetRequest<List<PositionInfo>>(URLs.GetOpenPosition, _headers);
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving Get Position.", ex);
        }
    }

    public async Task<MultipleSymbolLtpResponseInfo<FullLtpInfo>> GetMultipleSymbolFullLtp(MultipleTokensInfo multipleSymbolRequestInfo)
    {
        try
        {
            var obj = new MultipleSymbolRequestInfo
            {
                mode = "FULL",
                exchangeTokens = multipleSymbolRequestInfo
            };
            var result = await GetMultipleLtp<MultipleSymbolLtpResponseInfo<FullLtpInfo>>(obj);
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving Get Multiple Symbol Full LTP.", ex);
        }
    }

    public async Task<MultipleSymbolLtpResponseInfo<OHLCResponseInfo>> GetMultipleSymbolOHLC(MultipleTokensInfo multipleSymbolRequestInfo)
    {
        try
        {
            var obj = new MultipleSymbolRequestInfo
            {
                mode = "OHLC",
                exchangeTokens = multipleSymbolRequestInfo
            };
            var result = await GetMultipleLtp<MultipleSymbolLtpResponseInfo<OHLCResponseInfo>>(obj);
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving GetMultipleSymbolOHLC.", ex);
        }
    }

    public async Task<MultipleSymbolLtpResponseInfo<LtpInfo>> GetMultipleSymbolLtp(MultipleTokensInfo multipleSymbolRequestInfo)
    {
        try
        {
            var obj = new MultipleSymbolRequestInfo
            {
                mode = "LTP",
                exchangeTokens = multipleSymbolRequestInfo
            };
            var result = await GetMultipleLtp<MultipleSymbolLtpResponseInfo<LtpInfo>>(obj);
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving GetMultipleSymbolLtp.", ex);
        }
    }

    public async Task<OHLCResponseInfo> GetLtp(LtpRequestInfo ltpRequestInfo)
    {
        try
        {
            var result = await _webRequestHandler.PostRequest<OHLCResponseInfo>(URLs.GetLtpData, _headers, HelperMethods.Serialize(ltpRequestInfo));
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving Get LTP.", ex);
        }
    }

    public async Task<PlaceOrderResponseInfo> PlaceOrder(PlaceOrderRequestInfo requestInfo)
    {
        try
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            var result = await _webRequestHandler.PostRequest<PlaceOrderResponseInfo>
                (URLs.PlaceOrder, _headers, HelperMethods.SerializeWithOptions(requestInfo, jsonSerializerOptions));

            if (result.data == null)
            {
                throw new Exception($"Unable to place order because {result.message}");
            }
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to place new order because {ex.Message}");
        }
    }

    public async Task<PlaceOrderResponseInfo> ModifyOrder(ModifyOrderRequestInfo requestInfo)
    {
        try
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            var result = await _webRequestHandler.PostRequest<PlaceOrderResponseInfo>
                (URLs.ModifyOrder, _headers, HelperMethods.SerializeWithOptions(requestInfo, jsonSerializerOptions));

            if (result.data == null)
            {
                throw new Exception($"Unable to place order because {result.message}");
            }
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to modify order because {ex.Message}");
        }
    }

    public async Task<List<TradeBookResponseInfo>> GetTradeBook()
    {
        try
        {
            var result = await _webRequestHandler.GetRequest<List<TradeBookResponseInfo>>(URLs.GetTradeBook, _headers);
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to get trade book for the day because {ex.Message}");
        }
    }

    public async Task<IndividualOrderResponseInfo> GetIndividualOrderStatus(string uniqueOrderId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(uniqueOrderId))
            {
                throw new Exception("Invalid unique order id passed");
            }
            string url = $"{URLs.GetIndividualOrderData}{uniqueOrderId}";
            var result = await _webRequestHandler.GetRequest<IndividualOrderResponseInfo>(url, _headers);
            return result.data;
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to get trade book for the day because {ex.Message}");
        }
    }
    #endregion

    #region Public Properties

    //public bool IsLoggedIn { get; set; }

    #endregion

    #region private variables

    Dictionary<string, string> _headers;
    private readonly IWebRequestHandler _webRequestHandler;

    #endregion

    #region Private Methods

    public async Task<bool> Login(string userName, string pin, string authenticatorKey, string apiKey)
    {
        _headers.Add("X-PrivateKey", apiKey);
        var loginInfo = new
        {
            clientcode = userName,
            password = pin,
            totp = HelperMethods.GetTotpFromAuthenticator(authenticatorKey)
        };

        try
        {
            var response = await _webRequestHandler.PostRequest<LoginResponseInfo>
                (URLs.Login, _headers, HelperMethods.Serialize(loginInfo));

            if (response.status)
            {
                if (!_headers.ContainsKey("Authorization"))
                    _headers.TryAdd("Authorization", $"Bearer {response.data.jwtToken}");
                {
                   // IsLoggedIn = true;
                    return true;
                }
            }
            else
            {
                if (response.message == "Invalid totp")
                {
                    return await Login(userName, pin, authenticatorKey, apiKey);
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
        return false;
    }


    private void InitializeHeaders()
    {
        _headers = new Dictionary<string, string>
        {
            { "X-UserType", "USER" },
            { "X-SourceID", "WEB" },
            { "X-ClientLocalIP", HelperMethods.LocalIpAddress },
            { "X-ClientPublicIP", HelperMethods.PublicIpAddress },
            { "X-MACAddress", HelperMethods.MacAddress },
        };
    }

    private async Task<ResponseInfo<T>> GetMultipleLtp<T>(MultipleSymbolRequestInfo multipleSymbolRequestInfo) where T : class
    {
        var result = await _webRequestHandler.PostRequest<T>(URLs.GetMultipleLtpData, _headers, HelperMethods.Serialize(multipleSymbolRequestInfo));
        return result;
    }

    #endregion
}
