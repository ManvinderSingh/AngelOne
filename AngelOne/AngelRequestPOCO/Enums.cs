namespace AngelOne.AngelRequestPOCO;

public enum OrderDuration
{
    DAY,
    IOC
}

public enum Exchange
{
    NSE,
    BSE,
    NFO,
    MCX,
    BFO,
    CDS
}

public enum OrderType
{
    MARKET,
    LIMIT,
    STOPLOSS_LIMIT,
    STOPLOSS_MARKET
}

public enum ProductType
{
    DELIVERY,
    CARRYFORWARD,
    MARGIN,
    INTRADAY,
    BO
}

public enum TransactionType
{
    BUY,
    SELL
}

public enum OrderVariety
{
    NORMAL,
    STOPLOSS,
    AMO,
    ROBO
}

public enum Interval
{
    ONE_MINUTE,
    THREE_MINUTE,
    FIVE_MINUTE,
    TEN_MINUTE,
    FIFTEEN_MINUTE,
    THIRTY_MINUTE,
    ONE_HOUR,
    ONE_DAY
}

public enum SteamingMode
{
    Ltp = 1,
    Quote = 2,
    Snap_Quote = 3,
    Twenty_Depth = 4
}