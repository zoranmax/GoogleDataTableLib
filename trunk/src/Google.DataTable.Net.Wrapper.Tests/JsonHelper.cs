using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<Dictionary<string, object>>(jsonString);

            return result != null && result.Count > 0;
        }

        public static Dictionary<string, object> GetDictionaryFromJson(string jsonString)
        {
            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<Dictionary<string, object>>(jsonString);
            return result;
        }

        public static dynamic GetDynamicFromJson(string json)
        {
            var serializer = new JavaScriptSerializer();
            dynamic result = serializer.DeserializeObject(json);
            return result;
        }
    }
}
