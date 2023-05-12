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
    public class StationDAO : IDAO<Station>
    {
        private readonly TrainBookingDbContext _dbContext;
        public StationDAO()
        {
            _dbContext = new TrainBookingDbContext();
        }
        public Task Add(Station entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Station entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Station> FindById(int id)
        {
            try
            {

                return await _dbContext.Stations.Where(s => s.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { throw new Exception("error DAO beer"); }
        }

        public async Task<IEnumerable<Station>> GetAll()
        {
            try
            {// select * from Bieren
                return await _dbContext.Stations
                    .ToListAsync(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Station entity)
        {
            throw new NotImplementedException();
        }
    }
}
