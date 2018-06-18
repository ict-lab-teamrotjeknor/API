using System;
using API.Models.Data;
using API.Process;
using API.Process.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit;

namespace UnitTest
{
    public class SensorTest
    {
        private SensorHandler _sensor;
        
        public SensorTest()
        {
            _sensor = new SensorHandler(new DbSensorFake(), new JsonEditorFake());
        }
        
        [Fact]
        public void AddSensor()
        {
            var newSensor = JObject.Parse(@"{    
                'Name':'Temperature',
                'Type':'C'    ,
                'Room':'H.1.308'
            }
            ");
            
            var result = _sensor.AddSensor(newSensor);
            
            var expected = JObject.Parse(@"{'Succeed':true}");
            
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void AddData()
        {
            var data = new NewSensorData();
            data.Name = "Room";
            data.Room = "Name";
            data.Value = "Something";
            
            var result = _sensor.AddData(data);
            
            var expect = JObject.Parse(@"{'Succeed':true}");
            
            Assert.Equal(expect.ToString(), result.ToString());
        }

        [Fact]
        public void GetSensors()
        {
            var roomName = "TestRoom";
            
            var result = _sensor.GetSensors(roomName);

            var expect = JObject.Parse(@"{
             'RoomName': 'TestRoom',
            'AllSensors': [
            {
                'Id': '1',
                'Name': 'Test',
                'Type': 'Nothing'
            }
            ]
            }
            ");

            Assert.Equal(expect.ToString(), result.ToString());
        }

        [Fact]
        public void GetData()
        {
            var roomName = "TestRoom";
            var SensorName = "TestSensor";
            var result = _sensor.GetData(roomName, SensorName);

            var expect = JObject.Parse(@"{
            'Name': 'TestSensor',
            'Type': 'Nothing',
            'Data': [
            {
                'Value': '50',
                'Date': '2018-10-10T10:40:00'
            }
            ]
        }
        ");
            Assert.Equal(expect.ToString(), result.ToString());
        }

    }
}