using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScheduleCrawler.Models;

namespace ScheduleCrawler
{
    public class Save
    {
        public bool SaveSchedule(Schedule currentSchedule)
        {
            var convertedSchedule = ConvertToJobject(currentSchedule);
            return SendToApi(convertedSchedule);
        }

        private JObject ConvertToJobject(Schedule currentSchedule)
        {
            var stringJson = JsonConvert.SerializeObject(currentSchedule, Formatting.Indented);
            return JObject.Parse(stringJson);
        }

        private bool SendToApi(JObject currentSchedule)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create("http://localhost:5000/schedule/uploadnewweek");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(currentSchedule);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}