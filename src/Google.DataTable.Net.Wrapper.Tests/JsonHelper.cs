using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Google.DataTable.Net.Wrapper.Tests
{
    public class JsonHelper
    {
        /// <summary>
        /// Checks that the returned Json string can be deserialized into an object.
        /// This can be used to check if the JSON is valid.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static bool IsValidJson(string jsonString)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            return result != null && result.Count() > 0;
        }

        public static dynamic GetDynamicFromJson(string json)
        {
            dynamic result = JObject.Parse(json);
            return result;
        }
    }
}