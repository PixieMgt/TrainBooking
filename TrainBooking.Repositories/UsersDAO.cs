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
    public class UsersDAO : IDAO<AspNetUser>
    {
        private readonly TrainBookingDbContext _dbContext;
        public UsersDAO()
        {
            _dbContext = new TrainBookingDbContext();
        }

        public Task Add(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<AspNetUser> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AspNetUser>> GetAll()
        {
            try
            {
                return await _dbContext.AspNetUsers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Task Update(AspNetUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
