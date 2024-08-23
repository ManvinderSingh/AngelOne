using AngelOne.AngelRequestPOCO;
using AngelOne.AngelResponsePOCO;
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
                exchangeTimeStamp = GetExchangeDateTime(responseBuffer.Skip(35).Take(8).ToArray()),
                ltp = GetLtpData(responseBuffer.Skip(43).Take(8).ToArray())
            };

            OnPriceUpdate?.Invoke(responseObject);
        }

        private decimal GetLtpData(byte[] ltpBytes)
        {
            var ltpPaise = BitConverter.ToInt64(ltpBytes);
            return ltpPaise / 100m;
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
                        mode = 1, //1 is for ltp, hardcoding this for now
                        tokenList = lstOfTokens.ToArray()
                    }
                };

                var requestObject = HelperMethods.Serialize(obj);
                return requestObject;
            }
        }
    }
}
