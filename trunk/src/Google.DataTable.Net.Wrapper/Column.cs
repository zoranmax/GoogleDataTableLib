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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Google.DataTable.Net.Wrapper.Common;
using System.IO;

namespace Google.DataTable.Net.Wrapper
{
    [Serializable]
    public class Column : ISerializable, IPropertyMap
    {
        private readonly List<Property> _propertyMap;

        public Column()
        {
            this.ColumnType = ColumnType.String;
            this._propertyMap = new List<Property>();
        }

        public Column(ColumnType columnType): this()
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

        public Column(ColumnType columnType, string id, string label, string pattern)
            : this(columnType, id, label)
        {
            Pattern = pattern;
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
        /// A column role describes the purpose of the data in that column: 
        /// for example, a column might hold data describing tooltip text, 
        /// data point annotations, or uncertainty indicators.
        /// </summary>
        /// <remarks>
        /// More info about the role can be read here
        /// https://developers.google.com/chart/interactive/docs/roles
        /// </remarks>
        public string Role { get; set; }

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
        /// Returns the Json string as expected by the Google Api
        /// </summary>
        /// <returns></returns>
        internal void GetJson(StreamWriter sw)
        {
            sw.Write("{");

            sw.AppendIfNotNullOrEmpty("type", this.ColumnType.ToString().ToLower(CultureInfo.InvariantCulture));

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

            List<Property> propertyList = this.PropertyMap.ToList();
            if (!string.IsNullOrEmpty(this.Role))
            {
                propertyList.Add(new Property("role", this.Role));
            }

            Helper.JsonizeProperties(sw, propertyList);
           
            sw.Write("}");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("columnType", this.ColumnType.ToString().ToLower(CultureInfo.InvariantCulture));
            info.AddValue("id", this.Id);
            info.AddValue("label", this.Label);
            info.AddValue("pattern", this.Pattern);
            info.AddValue("p", this.PropertyMap);
            info.AddValue("role", this.Role);
        }

        protected Column(SerializationInfo info, StreamingContext context)
        {
            ColumnType = (ColumnType) info.GetValue("columnType", typeof (ColumnType));
            Id = (string) info.GetValue("id", typeof (string));
            Label = (string) info.GetValue("label", typeof (string));
            Pattern = (string) info.GetValue("pattern", typeof (string));
            _propertyMap = (List<Property>)info.GetValue("p", typeof(List<Property>));
            Role = (string) info.GetValue("role", typeof (string));
        }

        /// <summary>
        /// p [Optional] An object that is a map of custom values applied to the cell. 
        /// These values can be of any JavaScript ColumnType. 
        /// If your visualization supports any cell-level properties, 
        /// it will describe them; otherwise, this property will be ignored. 
        /// <example>
        /// Example: p:{style: 'border: 1px solid green;'}.
        /// </example>
        /// </summary>
        /// <remarks>
        /// Returns a list of currently assigned properties to the Cell
        /// </remarks>
        public IEnumerable<Property> PropertyMap
        {
            get { return _propertyMap; }
        }

        /// <summary>
        /// Adds a new property to the list of properties.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Property AddProperty(Property p)
        {
            _propertyMap.Add(p);
            return p;
        }

        /// <summary>
        /// Removes a property from the Property Map
        /// </summary>
        /// <param name="p"></param>
        public void RemoveProperty(Property p)
        {
            _propertyMap.Remove(p);
        }

        /// <summary>
        /// Removes a property from the Property Map by an index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveProperty(int index)
        {
            _propertyMap.RemoveAt(index);
        }
    }
}