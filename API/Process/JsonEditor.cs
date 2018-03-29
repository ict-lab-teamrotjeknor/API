using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Process.Model
{
    public class JsonEditor
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
    }
}