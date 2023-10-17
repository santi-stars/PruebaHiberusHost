namespace PruebaHiberusHost.Services
{
    public interface IExchangeRateUtility
    {
        decimal CalculateExchangeRate(string startCurrency, string endCurrency);
    }
}