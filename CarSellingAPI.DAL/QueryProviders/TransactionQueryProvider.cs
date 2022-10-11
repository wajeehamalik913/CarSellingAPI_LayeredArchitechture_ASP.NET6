using CarSellingAPI.DAL.Data.Models;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using CarSellingAPI.DAL.QueryInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.QueryProviders
{
    public class TransactionQueryProvider : ITransactionQuery
    {
        private readonly CardbContext _context;
        public TransactionQueryProvider(CardbContext context)
        {
            _context = context;
        }
        public async Task AddTransaction(Transaction transaction)
        {
            await _context.Transaction.AddAsync(transaction);
            _context.SaveChanges();
        }
        public async Task UpdateTransaction(Transaction transaction)
        {
            var val = await _context.Transaction.SingleOrDefaultAsync(x => x.TranId == transaction.TranId);
            val = transaction;
            _context.SaveChanges();

        }
    }
}
