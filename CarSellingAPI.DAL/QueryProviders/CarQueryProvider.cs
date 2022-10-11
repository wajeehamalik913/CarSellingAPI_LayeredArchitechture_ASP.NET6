using CarSellingAPI.DAL.Data.Models;
using CarSellingAPI.DAL.Models;
using CarSellingAPI.DAL.QueryInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.QueryProviders
{
    public class CarQueryProvider : ICarQuery
    {
        private readonly CardbContext _context;
        public CarQueryProvider(CardbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Cars>>> GetAllcars()
        {
            var val = await _context.Cars.ToListAsync();
            return val;
        }

        public async Task<Cars> GetCarbyId(int car_id)
        {
            var val = await _context.Cars.SingleOrDefaultAsync(x => x.CarId == car_id);
            if (val == null)
            {
                return null;
            }
            //return new Cars
            //{
            //    CarId= car_id,
            //    CarName= val.CarName,
            //    BrandName= val.BrandName,
            //    Price= val.Price,
            //    MinDeposit=val.MinDeposit,
            //    Status=val.Status,
            //    BuyerId=val.BuyerId,
            //};
            return val;

        }

        public async Task UpdateCarStatus(Cars car)
        {
            var val = await _context.Cars.SingleOrDefaultAsync(x => x.CarId == car.CarId);
            val = car;
            _context.SaveChanges();

        }
    }
}
