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