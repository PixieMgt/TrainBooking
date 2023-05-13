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
    public class TrainService : IService<Train>
    {
        private IDAO<Train> _trainDao;
        public TrainService(IDAO<Train> trainDao)
        {
            _trainDao = trainDao;
        }
        public Task Add(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Train entity)
        {
            throw new NotImplementedException();
        }

        public Task<Train> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Train>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Train entity)
        {
            throw new NotImplementedException();
        }
    }
}
