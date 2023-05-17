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
            List<Section> sections = new List<Section>();
            foreach (var section in entity.Sections)
            {
                sections.Add(_dbContext.Sections.FirstOrDefault(s => s.Id == section.Id));
            }
            entity.Sections = sections;
            _dbContext.Entry(entity).State = EntityState.Added;
            
            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task Delete(Ticket entity)
        {
            await _dbContext.Entry(entity).Collection(s => s.Sections).LoadAsync();

            entity.Sections.Clear();

            _dbContext.Entry(entity).State = EntityState.Deleted;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
