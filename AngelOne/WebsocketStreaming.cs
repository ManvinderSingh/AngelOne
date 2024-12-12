using AngelOne.AngelRequestPOCO;
using AngelOne.AngelResponsePOCO;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Text;

namespace AngelOne
{
    public class WebsocketStreaming
    {
        public delegate void PriceUpdate(WebStreamResponseInfo responseInfo);
        public event PriceUpdate OnPriceUpdate;
        private static readonly object _lockObject = new();
        private ClientWebSocket _socket;
        public WebStreamingRequestInfo RequestData { get; set; }
        public WebsocketStreaming()
        {
            ValidateCredentials();
            _socket = new ClientWebSocket();
        }

        public async Task StartAsync()
        {
            var requestObject = CreateRequestObject();
            if (_socket.State == WebSocketState.Open)
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                _socket.Dispose();
                _socket = new ClientWebSocket();
            }

            ConfigureSocketHeaders();
            await _socket.ConnectAsync(new Uri(URLs.WebSocketUrl), CancellationToken.None);
            if (_socket.State == WebSocketState.Open)
            {
                await SendRequestAsync(requestObject);
                await ReceiveResponseAsync();
            }
        }

        private async Task ReceiveResponseAsync()
        {
            var responseBuffer = new byte[1024];
            while (_socket.State == WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(responseBuffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    break;
                }
                lock (_lockObject)
                {
                    ProcessResponse(responseBuffer);
                }
            }
        }

        private async Task SendRequestAsync(string requestObject)
        {
            var requestBuffer = Encoding.UTF8.GetBytes(requestObject);
            await _socket.SendAsync(requestBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void ProcessResponse(byte[] responseBuffer)
        {
            var responseObject = new WebStreamResponseInfo
            {
                subscriptionMode = GetSubscriptionMode(responseBuffer[0]),
                exchangeType = GetExchange(responseBuffer[1]),
                token = GetToken(responseBuffer.Skip(2).Take(25).ToArray()),
                exchangeTimeStamp = GetExchangeDateTime(responseBuffer.Skip(35).Take(8).ToArray())
            };

            if (responseObject.subscriptionMode == "20Depth")
            {
                responseObject.BestTwentyBuyData = BestTwentyData(responseBuffer.Skip(43).ToArray(), TransactionType.BUY);
                responseObject.BestTwentySellData = BestTwentyData(responseBuffer.Skip(243).ToArray(), TransactionType.SELL);
            }
            else
            {
                responseObject.ltp = GetLtpData(responseBuffer.Skip(43).Take(8).ToArray());

                if (responseObject.subscriptionMode == "Quote" || responseObject.subscriptionMode == "SnapQuote")
                {
                    GetQuoteData(responseBuffer.Skip(51).ToArray(), responseObject);
                }
                if (responseObject.subscriptionMode == "SnapQuote")
                {
                    GetSnapData(responseBuffer.Skip(123).ToArray(), responseObject);
                }
            }
           

            OnPriceUpdate?.Invoke(responseObject);
        }

        private decimal GetLtpData(byte[] ltpBytes)
        {
            var ltpPaise = BitConverter.ToInt64(ltpBytes);
            return ltpPaise / 100m;
        }

        private void GetSnapData(byte[] responseBuffer, WebStreamResponseInfo responseObject)
        {
            var lastTradedTimeStamp = responseBuffer.Take(8).ToArray();
            responseObject.lastTradedTimeStamp = BitConverter.ToInt64(lastTradedTimeStamp);

            var openInterest = responseBuffer.Skip(8).Take(8).ToArray();
            responseObject.openInterest = BitConverter.ToInt64(openInterest);

            BestFiveData(responseBuffer.Skip(24).Take(200).ToArray(), responseObject);

            var uppererCircuitLimit = responseBuffer.Skip(224).Take(8).ToArray();
            responseObject.upperCircuitLimit = BitConverter.ToInt64(uppererCircuitLimit);

            var lowerCircuitLimit = responseBuffer.Skip(232).Take(8).ToArray();
            responseObject.lowerCiruitLimit = BitConverter.ToInt64(lowerCircuitLimit);

            var fiftyTwoWeekHighPrice = responseBuffer.Skip(240).Take(8).ToArray();
            responseObject.FiftyTwoWeekHighPrice = BitConverter.ToInt64(fiftyTwoWeekHighPrice);

            var fiftyTwoWeekLowPrice = responseBuffer.Skip(248).Take(8).ToArray();
            responseObject.FiftyTwoWeekLowPrice = BitConverter.ToInt64(fiftyTwoWeekLowPrice);
        }

        private List<BestData> BestTwentyData(byte[] responseBuffer, TransactionType transactionType)
        {
            var lstOfBestData = new List<BestData>();
            for (int i = 0; i < 200; i = i + 20)
            {
                var subBuffer = responseBuffer.Skip(i);
                var bestData = new BestData();

                bestData.buy_sell = transactionType;
                var qtyBytes = subBuffer.Take(4).ToArray();
                bestData.qty = BitConverter.ToInt32(qtyBytes);

                var priceBytes = subBuffer.Skip(4).Take(4).ToArray();
                var priceInPaise = BitConverter.ToInt32(priceBytes);
                bestData.price = priceInPaise / 100;

                var noOfOrders = subBuffer.Skip(8).Take(2).ToArray();
                bestData.noOfOrders = BitConverter.ToInt16(noOfOrders);

                lstOfBestData.Add(bestData);
            }
            return lstOfBestData;
        }

        private void BestFiveData(byte[] responseBuffer, WebStreamResponseInfo responseObject)
        {
            responseObject.BestFiveData = new List<BestData>();
            for (int i = 0; i < 200; i = i + 20)
            {
                var subBuffer = responseBuffer.Skip(i);
                var bestData = new BestData();
                var buySell = subBuffer.Take(2).ToArray();
                var transactionType = BitConverter.ToInt16(buySell);

                bestData.buy_sell = transactionType == 0 ? TransactionType.SELL : TransactionType.BUY;
                var qtyBytes = subBuffer.Skip(2).Take(8).ToArray();
                bestData.qty = BitConverter.ToInt64(qtyBytes);

                var priceBytes = subBuffer.Skip(10).Take(8).ToArray();
                var priceInPaise = BitConverter.ToInt64(priceBytes);
                bestData.price = priceInPaise / 100;

                var noOfOrders = subBuffer.Skip(18).Take(2).ToArray();
                bestData.noOfOrders = BitConverter.ToInt16(noOfOrders);

                responseObject.BestFiveData.Add(bestData);
            }
        }

        private void GetQuoteData(byte[] responseBuffer, WebStreamResponseInfo responseObject)
        {
            var lastTradedQtyBytes = responseBuffer.Take(8).ToArray();
            responseObject.lastTradedQuantity = BitConverter.ToInt64(lastTradedQtyBytes);

            var averageTradedPriceBytes = responseBuffer.Skip(8).Take(8).ToArray();
            var averageTradedPrice  = BitConverter.ToInt64(averageTradedPriceBytes);
            responseObject.averageTradedPrice = averageTradedPrice / 100;

            var volumeTradedForTheDay = responseBuffer.Skip(16).Take(8).ToArray();
            responseObject.volumeTradedForTheDay = BitConverter.ToInt64(volumeTradedForTheDay);

            var totalBuyQty = responseBuffer.Skip(24).Take(8).ToArray();
            responseObject.totalBuyQty = BitConverter.ToDouble(totalBuyQty);

            var totalSellQty = responseBuffer.Skip(32).Take(8).ToArray();
            responseObject.totalSellQty = BitConverter.ToDouble(totalSellQty);

            var openPriceBytes = responseBuffer.Skip(40).Take(8).ToArray();
            var openPrice = BitConverter.ToInt64(openPriceBytes);
            responseObject.openPrice = openPrice / 100;

            var highPriceBytes = responseBuffer.Skip(48).Take(8).ToArray();
            var highPrice = BitConverter.ToInt64(highPriceBytes);
            responseObject.highPrice = highPrice / 100;

            var lowPriceBytes = responseBuffer.Skip(56).Take(8).ToArray();
            var lowPrice = BitConverter.ToInt64(lowPriceBytes);
            responseObject.lowPrice = lowPrice / 100;

            var closePriceBytes = responseBuffer.Skip(64).Take(8).ToArray();
            var closePrice = BitConverter.ToInt64(closePriceBytes);
            responseObject.closePrice = closePrice / 100;
        }

        private DateTime GetExchangeDateTime(byte[] exchangeDateTimeBytes)
        {
            var exchangeTimeStampEpoch = BitConverter.ToInt64(exchangeDateTimeBytes, 0);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(exchangeTimeStampEpoch);
            DateTime dateTime = dateTimeOffset.UtcDateTime;
            // If you want to convert to local time
            DateTime localDateTime = dateTimeOffset.LocalDateTime;
            return localDateTime;
        }

        private string GetToken(byte[] tokenBytes)
        {
            var tokenString = Encoding.UTF8.GetString(tokenBytes);
            return tokenString.TrimEnd('\0');
        }

        private StreamingExchangeType GetExchange(byte exchangeTypeByte)
        {
            return exchangeTypeByte switch
            {
                1 => StreamingExchangeType.NSE,
                2 => StreamingExchangeType.NSE_FO,
                3 => StreamingExchangeType.BSE,
                4 => StreamingExchangeType.BSE_FO,
                5 => StreamingExchangeType.MCX_FO,
                7 => StreamingExchangeType.NCX_FO,
                13 => StreamingExchangeType.CDE_FO,
                _ => throw new InvalidOperationException("Invalid exchange type from web service response")
            };
        }

        private static string GetSubscriptionMode(byte subscriptionByte)
        {
            return subscriptionByte switch
            {
                1 => "LTP",
                2 => "Quote",
                3 => "SnapQuote",
                4 => "20Depth",
                _ => throw new InvalidOperationException("Invalid response from web service")
            };
        }

        private void ValidateCredentials()
        {
            if (string.IsNullOrWhiteSpace(GlobalData.FeedToken) ||
                string.IsNullOrWhiteSpace(GlobalData.AuthToken))
            {
                throw new InvalidOperationException("You need to login to smartAPI first.");
            }
        }

        private void ConfigureSocketHeaders()
        {
            string authorizationToken = $"Bearer {GlobalData.AuthToken}";
            _socket.Options.SetRequestHeader("Authorization", $"Bearer {GlobalData.AuthToken}");
            _socket.Options.SetRequestHeader("x-api-key", GlobalData.ApiKey);
            _socket.Options.SetRequestHeader("x-client-code", "M652954");
            _socket.Options.SetRequestHeader("x-feed-token", GlobalData.FeedToken);
        }

        private string CreateRequestObject()
        {
            if (RequestData == null || RequestData.tokens == null || !RequestData.tokens.Any())
            {
                throw new Exception("Request data is not passed, please set the property properly");
            }
            else
            {
                var lstOfTokens = new List<StreamingTokenListInfo>
                {
                    new StreamingTokenListInfo
                    {
                        exchangeType = RequestData.exchange,
                        tokens = RequestData.tokens
                    }
                };
                var obj = new StreamingRequestModel
                {
                    action = 1, //1 is for subscribe
                    someParameters = new StreamingParamsInfo
                    {
                        mode = (int)RequestData.Mode, //1 is for ltp, hardcoding this for now
                        tokenList = lstOfTokens.ToArray()
                    }
                };

                var requestObject = HelperMethods.Serialize(obj);
                return requestObject;
            }
        }
    }
}
