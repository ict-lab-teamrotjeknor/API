using System;
using System.Collections.Generic;
using System.Linq;
using API.Process.Model;

namespace API.Models.Data.Query
{
    public class DbManage
    {
        private ApplicationDbContext _dbContext;
        
        public DbManage(ApplicationDbContext newDbContext)
        {
            _dbContext = newDbContext;
        }

        public List<Classroom> GetAllClassrooms()
        {
            var classrooms = _dbContext.Classrooms.ToList();
            return classrooms;
        }

        public bool SaveRoom()
        {
            return true;
        }

        public string GetRoomId(string roomName)
        {
            var room = _dbContext.Classrooms
                .SingleOrDefault(c => c.Name.Equals(roomName));
            if (room != null) return room.Id;
            return string.Empty;
        }

        public PI GetPi(NewPI searchPi)
        {
            var pi = _dbContext.PI
                .SingleOrDefault(p => p.MacAdress.Equals(searchPi.MacAdress));
            return pi;
        }

        public bool SavePi(NewPI newPi, string roomId)
        {
            var room = _dbContext.Classrooms
                .SingleOrDefault(c => c.Id.Equals(roomId));
            
            
            var pi = new PI();
            pi.Id = Guid.NewGuid().ToString();
            pi.MacAdress = newPi.MacAdress;

            room.PiID = pi.Id;

            _dbContext.Add(pi);
            _dbContext.Update(room);
            _dbContext.SaveChanges();
            return true;
        }
    }
}