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
    public class SectionService : IService<Section>
    {
        private IDAO<Section> _sectionDao;
        public SectionService(IDAO<Section> stationDao)
        {
            _sectionDao = stationDao;
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
            return await _sectionDao.FindById(id);
        }

        public async Task<IEnumerable<Section>> GetAll()
        {
            return await _sectionDao.GetAll();
        }

        public Task Update(Section entity)
        {
            throw new NotImplementedException();
        }

        
    }
}
