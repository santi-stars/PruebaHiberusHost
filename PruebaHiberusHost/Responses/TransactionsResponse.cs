using PruebaHiberusHost.DTOs;
namespace PruebaHiberusHost.Responses
{
    public class TransactionsResponse
    {
        public List<TransactionDTO> TransactionDTOs { get; set; }
        public string ReturnedRows { get; set; }
    }
}
