/*
   Copyright 2012 Zoran Maksimovic

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Google.DataTable.Net.Wrapper.Common
{
    internal class Helper
    {
        /// <summary>
        /// Wraps the string around double quotes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DoubleQuoteString2(string value)
        {
            //if (string.IsNullOrEmpty(value))
            if (value.Length == 0)
            {
                return "\"\"";
            }

            return string.Format("\"{0}\"", value);
        }

        public static void JsonizeProperties(StreamWriter sw, IEnumerable<Property> properties)
        {
            if (properties != null && properties.Any())
            {
                var propertiesCount = properties.Count();
                var lastProperty = propertiesCount - 1;

                sw.Write(", \"p\": {");
                for (int p = 0; p < propertiesCount; p++)
                {
                    Property currentProperty = properties.ElementAt(p);

                    sw.Write("\"" + EscapeJsonString(currentProperty.Name) + "\" : \"" + EscapeJsonString(currentProperty.Value) + "\"");

                    if (p != lastProperty)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write("}");
            }
        }

        public static string EscapeJsonString(string text)
        {
            // This doesn't cover all the fancy escaping stuff,
            // but it covers the two potentially most common characters,
            // and provides a place to easily include more.

            // JSON RFC, string section: https://tools.ietf.org/html/rfc7159#section-7

            // make sure the \ -> \\ replacement is the first one, or it
            // will screw up the rest.
            // Don't ask me how I know that.
            return text?
                .Replace("\\", "\\\\")  // replace \ with \\
                .Replace("\"", "\\\""); // replace " with \"
        }
    }
}