using System.Net.Mime;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    public class Manage
    {
        private DbManage _dbManage;
        private JsonEditor _jsonEditor;

        public Manage(ApplicationDbContext dbContext)
        {
            _dbManage = new DbManage(dbContext);
            _jsonEditor = new JsonEditor();
        } 
        
        public JObject FindAllClassrooms()
        {
            var classrooms = _dbManage.GetAllClassrooms();
            return _jsonEditor.MakeClassrooms(classrooms);
        }
    }
}