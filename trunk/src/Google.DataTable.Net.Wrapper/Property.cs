/*
   Copyright 2012 Zoran Maksimovic (zoran.maksimovich@gmail.com 
   http://www.agile-code.com)

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

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    /// Reserved for future usages.
    /// P values in reality should be a name -> value map
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Property()
        {
            
        }

        /// <summary>
        /// Constructor that has as input name and value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
