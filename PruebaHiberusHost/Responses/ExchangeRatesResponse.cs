using PruebaHiberusHost.Entities;

namespace PruebaHiberusHost.Responses
{
    public class ExchangeRatesResponse
    {
        public List<ExchangeRate> ExchangeRates { get; set; }
        public string ReturnedRows { get; set; }
    }
}
