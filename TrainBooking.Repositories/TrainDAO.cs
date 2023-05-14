using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Models.Data;
using TrainBooking.Models.Entities;
using TrainBooking.Repositories.Interfaces;

namespace TrainBooking.Repositories
{
    public class TrainDAO : IDAO<Train>
    {
        private readonly TrainBookingDbContext _dbContext;
        public TrainDAO()
        {
            _dbContext = new TrainBookingDbContext();
        }
        public Task Add(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Train entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Train> FindById(int id)
        {
            try
            {

                return await _dbContext.Trains.Where(s => s.Id == id)
                                              .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("error DAO Train"); 
            }
        }

        public async Task<IEnumerable<Train>> GetAll()
        {
            try
            {
                return await _dbContext.Trains
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO: " + ex.Message);
                throw;
            }
        }

        public Task Update(Train entity)
        {
            throw new NotImplementedException();
        }
    }
}
