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
    }
}