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

using System.Web.Script.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class DataTableJsonTest
    {
        [Test]
        public void DataTable_SimplestJsonReturned()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.That(json != null);
            Assert.IsTrue(IsValidJson(json));
        }


        [Test]
        public void DataTable_GetSimpleJsonWithOnlyColumnsDefined()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, "column1");
            Column col2 = new Column(ColumnType.Number, "column2");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));
        }

        [Test]
        public void DataTable_GetSimpleJsonWithColumnsAndOneRow()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, "Year");
            Column col2 = new Column(ColumnType.Number, "Count");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            var row = dt.NewRow();
            row.AddCell(new Cell() {Value = "Year"});
            row.AddCell(new Cell() {Value = 10});

            dt.AddRow(row);
            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));
        }

        [Test]
        public void DataTable_GetSimpleJsonWithColumnsAndOneRowWithFormatting()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, "Year");
            Column col2 = new Column(ColumnType.Number, "Count");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            var row = dt.NewRow();
            row.AddCell(new Cell() {Value = "Year", Formatted = "MyYears"});
            row.AddCell(new Cell() {Value = 10, Formatted = "Ten"});

            dt.AddRow(row);
            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(json.Contains("Ten"));
            Assert.IsTrue(json.Contains("MyYears"));
            Assert.IsTrue(IsValidJson(json));
        }

        [Test]
        public void DataTable_GetSimpleJsonWithColumnsAndOneRowWithFormattingAndProperties()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, "Year");
            Column col2 = new Column(ColumnType.Number, "Count");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            var row = dt.NewRow();

            var cell1 = new Cell() {Value = "Year", Formatted = "MyYears"};
            cell1.AddProperty(new Property("style", "backgroundColor: red"));

            var cell2 = new Cell() {Value = 10, Formatted = "Ten"};
            cell2.AddProperty(new Property("style", "backgroundColor: red"));

            row.AddCell(cell1);
            row.AddCell(cell2);

            dt.AddRow(row);

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(json.Contains("Ten"));
            Assert.IsTrue(json.Contains("MyYears"));
            Assert.IsTrue(IsValidJson(json));
        }

        [Test]
        public void DataTable_GetSimpleJsonWithColumnsWithDateTimeFormat()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.String, "Year");
            Column col2 = new Column(ColumnType.Date, "DateTime");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            var row = dt.NewRow();
            row.AddCell(new Cell() {Value = "Year", Formatted = "MyYears"});
            row.AddCell(new Cell() {Value = DateTime.Now, Formatted = "Now"});

            dt.AddRow(row);

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(json.Contains("MyYears"));
            Assert.IsTrue(json.Contains("Now"));
            Assert.IsTrue(IsValidJson(json));
        }

        [Test]
        public void DataTable_ExchangeRateExample()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            Column col = new Column(ColumnType.Date, "Year", "Year");
            Column col2 = new Column(ColumnType.Number, "End Of Day Rate", "End Of Day Rate");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            for (int i = 0; i < 365; i++)
            {
                var row = dt.NewRow();
                row.AddCell(new Cell() {Value = DateTime.Now.AddDays(-i), Formatted = "Year"});
                row.AddCell(new Cell() {Value = i});
                dt.AddRow(row);
            }


            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));
        }
        
        [Test(Description="Checks that the properties assigned to the Row can be properly serialized")]
        public void DataTable_CanSerializeRowPropertyMap()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            var col = new Column(ColumnType.Date, "Year", "Year");
            var col2 = new Column(ColumnType.Number, "End Of Day Rate", "End Of Day Rate");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            for (int i = 0; i < 1; i++)
            {
                var row = dt.NewRow();
                row.AddCell(new Cell() {Value = DateTime.Now.AddDays(-i), Formatted = "Year"});
                row.AddCell(new Cell() {Value = i});
                row.AddProperty(new Property("style", "border:1"));
                row.AddProperty(new Property("fake", "fakeValue"));
                dt.AddRow(row);
            }

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.That(json.Contains("\"p\": {\"style\" : \"border:1\",\"fake\" : \"fakeValue\"}"));
            Assert.IsTrue(IsValidJson(json));
        }

        [Test]
        public void DataTable_CanSerializeCellPropertyMap()
        {
            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            var col = new Column(ColumnType.Date, "Year", "Year");
            var col2 = new Column(ColumnType.Number, "End Of Day Rate", "End Of Day Rate");

            dt.AddColumn(col);
            dt.AddColumn(col2);

            for (int i = 0; i < 1; i++)
            {
                var row = dt.NewRow();
                var cell1 = new Cell() {Value = DateTime.Now.AddDays(-i), Formatted = "Year"};
                var cell2 = new Cell() {Value = i};

                cell1.AddProperty(new Property("style", "border:1"));
                cell1.AddProperty(new Property("fake", "fakeValue"));

                cell2.AddProperty(new Property("style", "border:1"));
                cell2.AddProperty(new Property("fake", "fakeValue"));

                row.AddCell(cell1);
                row.AddCell(cell2);  
              
                dt.AddRow(row);
            }

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.That(json.Contains("\"p\": {\"style\" : \"border:1\",\"fake\" : \"fakeValue\"}"));
            Assert.IsTrue(IsValidJson(json));
        }

        /// <summary>
        /// Checks that the returned Json string can be deserialized into an object.
        /// This can be used to check if the JSON is valid.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private bool IsValidJson(string jsonString)
        {
            return JsonHelper.IsValidJson(jsonString);
        }

        /// <summary>
        /// Returns a new instance of the DataTable.
        /// </summary>
        /// <returns></returns>
        public DataTable GetNewDataTableInstance()
        {
            return new DataTable();
        }
    }
}
