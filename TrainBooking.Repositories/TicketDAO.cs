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
    public class TicketDAO : IDAO<Ticket>
    {

        private readonly TrainBookingDbContext _dbContext;
        public TicketDAO()
        {
            _dbContext = new TrainBookingDbContext();
        }
        public async Task Add(Ticket entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            foreach (var section in entity.Sections)
            {
                _dbContext.Entry(section).State = EntityState.Modified;
            }
            
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

        public Task Delete(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> FindById(int id)
        {
            try
            {

                return await _dbContext.Tickets.Where(s => s.Id == id)
                                                .Include(s => s.Booking)
                                                .Include(s => s.Sections)
                                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { throw new Exception("error DAO Section"); }
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            try
            {
                return await _dbContext.Tickets
                    .Include(s => s.Booking)
                    .Include(s => s.Sections)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
