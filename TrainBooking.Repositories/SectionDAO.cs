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
    public class SectionDAO : IDAO<Section>
    {
        private readonly TrainBookingDbContext _dbContext;
        public SectionDAO()
        {
            _dbContext = new TrainBookingDbContext();
        }
        public Task Add(Section entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Section entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Section> FindById(int id)
        {
            try
            {

                return await _dbContext.Sections.Where(s => s.Id == id)
                                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { throw new Exception("error DAO Section"); }
        }

        public async Task<IEnumerable<Section>> GetAll()
        {
            try
            {
                return await _dbContext.Sections
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Section entity)
        {
            throw new NotImplementedException();
        }
    }
}
