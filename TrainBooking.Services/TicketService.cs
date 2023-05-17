using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainBooking.Models.Entities;
using TrainBooking.Repositories.Interfaces;
using TrainBooking.Services.Interfaces;

namespace TrainBooking.Services
{
    public class TicketService : IService<Ticket>
    {
        private IDAO<Ticket> _ticketDao;
        public TicketService(IDAO<Ticket> ticketDao)
        {
            _ticketDao = ticketDao;
        }
        public async Task Add(Ticket entity)
        {
            await _ticketDao.Add(entity);
        }

        public async Task Delete(Ticket entity)
        {
            await _ticketDao.Delete(entity);
        }

        public async Task<Ticket> FindById(int id)
        {
            return await _ticketDao.FindById(id);
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            return await _ticketDao.GetAll();
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
