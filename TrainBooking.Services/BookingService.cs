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
    public class BookingService : IService<Booking>
    {
        private readonly IDAO<Booking> _bookingDao;
        public BookingService(IDAO<Booking> bookingDao)
        {
            _bookingDao = bookingDao;
        }

        public async Task Add(Booking entity)
        {
            await _bookingDao.Add(entity);
        }

        public Task Delete(Booking entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking> FindById(int id)
        {
            return await _bookingDao.FindById(id);
        }

        public async Task<IEnumerable<Booking>> GetAll()
        {
            return await _bookingDao.GetAll();
        }

        public Task Update(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
