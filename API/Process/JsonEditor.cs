using System;
using System.Collections.Generic;
using API.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Process.Model.Agenda;

namespace API.Process.Model
{
    public class JsonEditor : IJsonEditor
    {
        public Account GetAccount(JObject account)
        {
            return JsonConvert.DeserializeObject<Account>(account.ToString());
        }

        public JObject SerilizeJObject(Object serilizeObject)
        {
            var stringJson = JsonConvert.SerializeObject(serilizeObject);
            var sendback = JObject.Parse(stringJson);
            return sendback;
        }

        public Schedule GetSchedule(JObject schedule)
        {
            return JsonConvert.DeserializeObject<Schedule>(schedule.ToString());
        }

        public Role GetRole(JObject role)
        {
            return JsonConvert.DeserializeObject<Role>(role.ToString());
        }

        public NewPI GetPi(JObject newPi)
        {
            return JsonConvert.DeserializeObject<NewPI>(newPi.ToString());
        }
        
        public NewSensorData GetSensorData(JObject newData)
        {
            return JsonConvert.DeserializeObject<NewSensorData>(newData.ToString());
        }


        public JObject MakeClassrooms(List<Classroom> classrooms)
        {
            var newClassrooms = new JObject();
            newClassrooms.Add("Classroom", new JObject());

            var getClass = newClassrooms["Classroom"] as JObject;

            var totalRooms = 1;
            
            foreach (var classroom in classrooms)
            {
                var smallRoom = MakeSmallRoom(classroom);
                getClass.Add(totalRooms.ToString(), smallRoom);
                totalRooms++;
            }

            return newClassrooms;
        }

        public Sensor GetSensor(JObject sensor)
        {
            var newSensor = JsonConvert.DeserializeObject<Sensor>(sensor.ToString());
            newSensor.Id = Guid.NewGuid().ToString();
            return newSensor;
        }

        public NewHour GetNewHour(JObject hour)
        {
            var newHour = JsonConvert.DeserializeObject<NewHour>(hour.ToString());
            return newHour;
        }

        public string GetRoom(JObject sensor)
        {
            return sensor["Room"].ToString();
        }
        
        public JObject GetError(string errorMessage)
        {
            var error = new ErrorMessage();
            error.Succeed = false;
            error.Error = errorMessage;
            return SerilizeJObject(error);
        }

        public JObject GetSucced()
        {
            var error = new ErrorMessage();
            error.Succeed = true;
            error.Error = string.Empty;
            return SerilizeJObject(error);
        }

        private JObject MakeSmallRoom(Classroom classroom)
        {
            var newClassroom = new JObject();
            newClassroom.Add("Id", classroom.Id);
            newClassroom.Add("Name", classroom.Name);
            return newClassroom;
        }
    }
}