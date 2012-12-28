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

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Google.DataTable.Net.Wrapper.Common;
using System.IO;

namespace Google.DataTable.Net.Wrapper
{
    [Serializable]
    public class Column : ISerializable
    {
        public Column()
        {
            this.ColumnType = ColumnType.String;
        }

        public Column(ColumnType columnType)
        {
            ColumnType = columnType;
        }

        public Column(ColumnType columnType, string id)
            : this(columnType)
        {
            Id = id;
        }

        public Column(ColumnType columnType, string id, string label)
            : this(columnType, id)
        {
            Label = label;
        }

        public Column(ColumnType columnType, string id, string label, string pattern, string p)
            : this(columnType, id, label)
        {
            Pattern = pattern;
            Properties = p;
        }

        /// <summary>
        /// id [Optional] String ID of the column. Must be unique in the table. 
        /// Use basic alphanumeric characters, so the host page does not require 
        /// fancy escapes to access the column in JavaScript. Be careful not to 
        /// choose a JavaScript keyword. Example: id:'col_1'
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// label [Optional] String value that some visualizations display for this column. 
        /// <example>Example: label:'Height'</example>
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ColumnType [Required] Data ColumnType of the data in the column. 
        /// Supports the following string values (examples include the v: property, described later):
        /// </summary>
        public ColumnType ColumnType { get; set; }

        /// <summary>
        /// pattern [Optional] String pattern that was used by a data source 
        /// to format numeric, date, or time column values. 
        /// This is for reference only; you probably won't need to read 
        /// the pattern, and it isn't required to exist. 
        /// The Google Visualization client does not use this value 
        /// (it reads the cell's formatted value). 
        /// If the DataTable has come from a data source in response to a query 
        /// with a format clause, the pattern you specified in that clause 
        /// will probably be returned in this value. 
        /// The recommended pattern standards are the ICU DecimalFormat and SimpleDateFormat.
        /// <remarks>
        ///     <see cref="http://icu-project.org/apiref/icu4j/com/ibm/icu/text/SimpleDateFormat.html"/>
        ///     <see cref="http://icu-project.org/apiref/icu4j/com/ibm/icu/text/DecimalFormat.html"/>
        /// </remarks>
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// p [Optional] An object that is a map of custom values applied to the cell. 
        /// These values can be of any JavaScript ColumnType. 
        /// If your visualization supports any cell-level properties, 
        /// it will describe them; otherwise, this property will be ignored. 
        /// <example>
        /// Example: p:{style: 'border: 1px solid green;'}.
        /// </example>
        /// </summary>
        public string Properties { get; set; }

        /// <summary>
        /// Returns the Json string as expected by the Google Api
        /// </summary>
        /// <returns></returns>
        internal void GetJson(StreamWriter sw)
        {
            sw.Write("{");

            
            sw.AppendIfNotNullOrEmpty("type", this.ColumnType.ToString().ToLower());
            

            if (this.Id != null)
            {
                sw.Write(",");
                sw.AppendIfNotNullOrEmpty("id", this.Id);
            }
            if (this.Label != null)
            {
                sw.Write(",");
                sw.AppendIfNotNullOrEmpty("label", this.Label);
            }
            if (this.Pattern != null)
            {
                sw.Write(",");
                sw.AppendIfNotNullOrEmpty("pattern", this.Pattern);
            }
            if (this.Properties != null)
            {
                sw.Write(",");
                sw.AppendIfNotNullOrEmpty("p", this.Properties);
            }
            sw.Write("}");
        }      

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("columnType", this.ColumnType.ToString().ToLower());
            info.AddValue("id", this.Id);
            info.AddValue("label", this.Label);
            info.AddValue("pattern", this.Pattern);
            info.AddValue("p", this.Properties);
        }

        protected Column(SerializationInfo info, StreamingContext context)
        {
            ColumnType = (ColumnType) info.GetValue("columnType", typeof (ColumnType));
            Id = (string) info.GetValue("id", typeof (string));
            Label = (string) info.GetValue("label", typeof (string));
            Pattern = (string) info.GetValue("pattern", typeof (string));
            Properties = (string) info.GetValue("p", typeof (string));
        }
    }
}