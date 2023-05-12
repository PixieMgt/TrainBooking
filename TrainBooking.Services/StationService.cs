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
    public class StationService : IService<Station>
    {
        private IDAO<Station> _stationDao;
        public StationService(IDAO<Station> stationDao)
        {
            _stationDao = stationDao;
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
            return await _stationDao.FindById(id);
        }

        public async Task<IEnumerable<Station>> GetAll()
        {
            return await _stationDao.GetAll();
        }

        public Task Update(Station entity)
        {
            throw new NotImplementedException();
        }
    }
}
