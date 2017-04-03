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

namespace Google.DataTable.Net.Wrapper
{
    /// <summary>
    /// ColumnType [Required] Data ColumnType of the data in the column. Supports the following string values (examples include the v: property, described later):
    ///'boolean'    - JavaScript boolean value ('true' or 'false'). Example value: v:'true'
    ///'number'     - JavaScript number value. Example values: v:7 , v:3.14, v:-55
    ///'string'     - JavaScript string value. Example value: v:'hello'
    ///'date'       - JavaScript Date object (zero-based month), with the time truncated. Example value: v:new Date(2008, 0, 15)
    ///'datetime'   - JavaScript Date object including the time. Example value: v:new Date(2008, 0, 15, 14, 30, 45)
    ///'timeofday'  - Array of three numbers and an optional fourth, representing hour (0 indicates midnight), minute, second, and optional millisecond. Example values: v:[8, 15, 0], v: [6, 12, 1, 144]
    /// </summary>
    [Serializable]
    public enum ColumnType
    {
        /// <summary>
        /// JavaScript string value. Example value: v:'hello'
        /// </summary>
        String,
        /// <summary>
        /// JavaScript number value. Example values: v:7 , v:3.14, v:-55
        /// </summary>
        Number,
        /// <summary>
        /// JavaScript boolean value ('true' or 'false'). Example value: v:'true'
        /// </summary>
        Boolean,
        /// <summary>
        /// JavaScript Date object (zero-based month), with the time truncated. Example value: v:new Date(2008, 0, 15)
        /// </summary>
        Date,
        /// <summary>
        /// JavaScript Date object including the time. Example value: v:new Date(2008, 0, 15, 14, 30, 45)
        /// </summary>
        Datetime,
        /// <summary>
        /// Array of three numbers and an optional fourth, representing hour (0 indicates midnight), minute, second, and optional millisecond. Example values: v:[8, 15, 0], v: [6, 12, 1, 144]
        /// </summary>
        Timeofday,
    }
}