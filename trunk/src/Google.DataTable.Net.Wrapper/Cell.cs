/*
   Copyright 2012 Zoran Maksimovic (zoran.maksimovich@gmail.com)

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

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using Google.DataTable.Net.Wrapper.Common;

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    /// Cells in the row array should be in the same order as their column descriptions in cols
    /// 
    /// v [Optional] The cell value. The data ColumnType should match the column data ColumnType.
    ///  If null, the whole object should be empty and have neither v nor f properties.
    /// 
    /// 
    /// </summary>
    public class Cell: ISerializable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Cell()
        {
            
        }

        /// <summary>
        /// Constructor that accepts a value
        /// </summary>
        /// <param name="value"></param>
        public Cell(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor that accepts a value and formatted properties.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatted"></param>
        public Cell(object value, string formatted) : this(value)
        {
            Formatted = formatted;
        }

        /// <summary>
        /// Column type represents the type of value that the current cell holds.
        /// </summary>
        public ColumnType ColumnType { get;  internal set; }

        /// <summary>
        ///  [Optional] The cell value. The data ColumnType should match the column data ColumnType.
        ///  If null, the whole object should be empty and have neither v nor f properties.
        /// </summary>
        public object Value { get; set; }

        ///<summary> [Optional] A string version of the v value, formatted for display. 
        /// The values should match, so if you specify Date(2008, 0, 1) for v, 
        /// you should specify "January 1, 2008" or some such string for this property. 
        /// This value is not checked against the v value. The visualization will not use this value 
        /// for calculation, only as a label for display. If omitted, a string version of v will be used.       
        /// </summary>
        public string Formatted { get; set; }

        /// <summary>
        /// p [Optional] An object that is a map of custom values applied to the cell. 
        /// These values can be of any JavaScript ColumnType. If your visualization supports 
        /// any cell-level properties, it will describe them; otherwise, this property will be ignored. 
        /// <example>Example: p:{style: 'border: 1px solid green;'}.</example>
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// Returns the Json string as expected by the Google Api
        /// <example>
        /// c:[{v: 'Mike'}, {v: new Date(2008, 1, 28), f:'February 28, 2008'}]
        /// </example>
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            var sb = new StringBuilder();

            sb.AppendIfNotNullOrEmpty("v", GetFormattedValue(this.ColumnType, this.Value));

            sb.AppendIfNotNullOrEmpty("f", this.Formatted);
            sb.AppendIfNotNullOrEmpty("p", this.Properties, true);         

            string json = sb.ToString();
            return "{" + json.Trim().TrimEnd(',') + "}";
        }

        private string GetFormattedValue(ColumnType columnType, object value)
        {
            if (value == null)
            {
                return null;
            }

            string returnValue;

            switch (columnType)
            {
                case ColumnType.Number:
                    switch (value.GetType().FullName.ToString(CultureInfo.InvariantCulture))
                    {
                        case "System.Decimal":
                            returnValue = ((Decimal) value).ToString(CultureInfo.InvariantCulture);
                            break;
                        case "System.Double":
                            returnValue = ((Double) value).ToString(CultureInfo.InvariantCulture);
                            break;
                        case "System.Single":
                            returnValue = ((Single) value).ToString(CultureInfo.InvariantCulture);
                            break;
                        default:
                            returnValue = value.ToString();
                            break;
                    }
                    break;
                case ColumnType.String:
                    returnValue = value.ToString();
                    break;
                case ColumnType.Boolean:
                    returnValue = (bool) value ? "true" : "false";
                    break;
                case ColumnType.Date:
                    var d = (DateTime) value;
                    returnValue = string.Format("new Date({0}, {1}, {2})", d.Year, d.Month - 1, d.Day);
                    break;
                case ColumnType.Datetime:
                    var dt = (DateTime) value;
                    returnValue = string.Format("new Date({0}, {1}, {2}, {3}, {4}, {5})", dt.Year, dt.Month - 1, dt.Day,
                                                dt.Hour, dt.Minute, dt.Second);
                    break;
                case ColumnType.Timeofday:
                     var tod = (DateTime)value;
                     returnValue = string.Format("[{0}, {1}, {2}]", tod.Hour, tod.Minute, tod.Second);
                     break;
                default:
                    returnValue = value.ToString();
                    break;
            }
            return returnValue;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("columnType", this.ColumnType.ToString().ToLower());
            info.AddValue("v", this.Value);
            info.AddValue("f", this.Formatted);
            info.AddValue("p", this.Properties);
        }

        protected Cell (SerializationInfo info, StreamingContext context)
        {
            ColumnType = (ColumnType) info.GetValue("columnType", typeof (ColumnType));
            Value = info.GetValue("v", typeof (object));
            Formatted = (string)info.GetValue("f", typeof (string));
            Properties = (string) info.GetValue("p", typeof (string));           
        }
    }
}