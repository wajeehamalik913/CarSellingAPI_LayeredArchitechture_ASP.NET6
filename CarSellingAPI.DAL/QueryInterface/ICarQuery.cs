using CarSellingAPI.DAL.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.QueryInterface
{
    public interface ICarQuery
    {
        public Task<ActionResult<IEnumerable<Cars>>> GetAllcars();
        public Task<Cars> GetCarbyId(int car_id);

        public Task UpdateCarStatus(Cars car);
    }
}
