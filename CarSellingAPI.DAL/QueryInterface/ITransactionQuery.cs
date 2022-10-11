using CarSellingAPI.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.QueryInterface
{
    public interface ITransactionQuery
    {
        public Task AddTransaction(Transaction transaction);
        public Task UpdateTransaction(Transaction transaction);
    }
}
