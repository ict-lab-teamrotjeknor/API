using System.Collections.Generic;
using System.Linq;

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
    }
}