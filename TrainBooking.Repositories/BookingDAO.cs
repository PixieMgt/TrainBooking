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
    public class BookingDAO : IDAO<Booking>
    {
        private readonly TrainBookingDbContext _dbContext;
        public BookingDAO()
        {
            _dbContext = new TrainBookingDbContext();
        }
        public async Task Add(Booking entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public Task Delete(Booking entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking> FindById(int id)
        {
            try
            {
                return await _dbContext.Bookings.Where(s => s.Id == id)
                                                .Include(s => s.Tickets)
                                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { throw new Exception("error DAO Section"); }
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            try
            {
                return await _dbContext.Bookings
                    .Include(s => s.Tickets)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
