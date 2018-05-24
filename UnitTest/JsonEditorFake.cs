using System.Collections.Generic;
using API.Models.Data;
using API.Process;
using API.Process.Model;
using API.Process.Model.Agenda;
using Newtonsoft.Json.Linq;

namespace UnitTest
{
    public class JsonEditorFake : IJsonEditor
    {
        public Account GetAccount(JObject account)
        {
            throw new System.NotImplementedException();
        }

        public JObject SerilizeJObject(object serilizeObject)
        {
            throw new System.NotImplementedException();
        }

        public Schedule GetSchedule(JObject schedule)
        {
            throw new System.NotImplementedException();
        }

        public Role GetRole(JObject role)
        {
            throw new System.NotImplementedException();
        }

        public NewPI GetPi(JObject newPi)
        {
            throw new System.NotImplementedException();
        }

        public NewSensorData GetSensorData(JObject newData)
        {
            throw new System.NotImplementedException();
        }

        public JObject MakeClassrooms(List<Classroom> classrooms)
        {
            throw new System.NotImplementedException();
        }

        public Sensor GetSensor(JObject sensor)
        {
            throw new System.NotImplementedException();
        }

        public NewHour GetNewHour(JObject newHour)
        {
            throw new System.NotImplementedException();
        }

        public string GetRoom(JObject sensor)
        {
            throw new System.NotImplementedException();
        }

        public JObject GetError(string errorMessage)
        {
            throw new System.NotImplementedException();
        }

        public JObject GetSucced()
        {
            throw new System.NotImplementedException();
        }
    }
}