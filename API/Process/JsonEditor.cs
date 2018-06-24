using System;
using System.Collections.Generic;
using System.Transactions;
using API.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Process.Model.Agenda;
using Microsoft.Extensions.Logging;


namespace API.Process.Model
{
    //This class handles every json files
    //The get methodes set Json to Objects
    public class JsonEditor : IJsonEditor
    {
        private ILogger _logger;
        private bool Deployment;

        public JsonEditor(ILogger logger)
        {
            Deployment = true;
            _logger = logger;
        }

        public JsonEditor()
        {
            Deployment = false;
        }

        public Account GetAccount(JObject account)
        {
            try
            {
                return JsonConvert.DeserializeObject<Account>(account.ToString());
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Account");
            }

            return new Account();
        }

        //Sets every object to json
        public JObject SerilizeJObject(Object serilizeObject)
        {
            try
            {
                var stringJson = JsonConvert.SerializeObject(serilizeObject);
                var sendback = JObject.Parse(stringJson);
                return sendback;
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with SerilizeObject");
                return this.GetError("Object bad");
            }
        }

        public Schedule GetSchedule(JObject schedule)
        {
            try
            {
                return JsonConvert.DeserializeObject<Schedule>(schedule.ToString());
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Schedule");
                return new Schedule();
            }
        }

        public Role GetRole(JObject role)
        {
            try
            {
                return JsonConvert.DeserializeObject<Role>(role.ToString());
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Role");
                return new Role();
            }
        }

        public NewPI GetPi(JObject newPi)
        {
            try
            {
                return JsonConvert.DeserializeObject<NewPI>(newPi.ToString());
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Schedule");
                return new NewPI();
            }
        }

        public NewSensorData GetSensorData(JObject newData)
        {
            try
            {
                return JsonConvert.DeserializeObject<NewSensorData>(newData.ToString());
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with NewSensorData");
                return new NewSensorData();
            }
        }


        public JObject MakeClassrooms(List<Classroom> classrooms)
        {
            try
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
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Classroom");
                return this.GetError("Classroom is wrong");
            }

        }

        public Sensor GetSensor(JObject sensor)
        {
            try
            {
                var newSensor = JsonConvert.DeserializeObject<Sensor>(sensor.ToString());
                newSensor.Id = Guid.NewGuid().ToString();
                return newSensor;
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Sensor");
                return new Sensor();
            }
        }

        public NewHour GetNewHour(JObject hour)
        {
            try
            {
                var newHour = JsonConvert.DeserializeObject<NewHour>(hour.ToString());
                return newHour;
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with NewHour");
                return new NewHour();
            }
        }

        public string GetRoom(JObject sensor)
        {
            return sensor["Room"].ToString();
        }
        
        //Create an error in json format
        public JObject GetError(string errorMessage)
        {
            var error = new ErrorMessage();
            error.Succeed = false;
            error.Error = errorMessage;
            return SerilizeJObject(error);
        }

        //Create an succeed in json format
        public JObject GetSucced()
        {
            var error = new ErrorMessage();
            error.Succeed = true;
            error.Error = string.Empty;
            return SerilizeJObject(error);
        }

        public Notification GetNotification(JObject notification)
        {
            try
            {
                var newNotification = JsonConvert.DeserializeObject<Notification>(notification.ToString());
                return newNotification;
            }
            catch (Exception e)
            {
                if (Deployment) _logger.LogInformation("Something went wrong with Sensor");
                return new Notification();
            }
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