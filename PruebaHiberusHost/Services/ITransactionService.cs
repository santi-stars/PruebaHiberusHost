using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaHiberusHost.DTOs;

namespace PruebaHiberusHost.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetTransactionsFromUrlAsync(string url);
        Task<ActionResult> CreateTransaction(TransactionDTO transactionDTO, ITransactionService _transactionService, IMapper mapper);
    }
}
