using System;
using System.Linq;

namespace API.Models.Data.Query
{
    public class DbSensor
    {
        private ApplicationDbContext _dbContext;

        public DbSensor(ApplicationDbContext newDbContext)
        {
            _dbContext = newDbContext;
        }

        public bool SaveSensor(Sensor newSensor, string roomName)
        {
            try
            {
                var room = _dbContext.Classrooms
                    .SingleOrDefault(c => c.Name.Equals(roomName));

                newSensor.RoomId = room.Id;
                
                _dbContext.Add(newSensor);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}