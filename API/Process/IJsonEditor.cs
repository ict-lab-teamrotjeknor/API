using System;
using System.Collections.Generic;
using API.Models.Data;
using API.Process.Model;
using API.Process.Model.Agenda;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    public interface IJsonEditor
    {
        Account GetAccount(JObject account);
        JObject SerilizeJObject(Object serilizeObject);
        Schedule GetSchedule(JObject schedule);
        Role GetRole(JObject role);
        NewPI GetPi(JObject newPi);
        NewSensorData GetSensorData(JObject newData);
        JObject MakeClassrooms(List<Classroom> classrooms);
        Sensor GetSensor(JObject sensor);
        NewHour GetNewHour(JObject newHour);
        string GetRoom(JObject sensor);
        JObject GetError(string errorMessage);
        JObject GetSucced();
    }
}