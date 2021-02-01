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

using NUnit.Framework;
using System;

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
            row.AddCell(new Cell() {Value = new DateTime(2013,01,01), Formatted = "Now"});

            dt.AddRow(row);

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            Assert.IsTrue(json != null);

            dynamic jsonObject = JsonHelper.GetDynamicFromJson(json);


            //retrieving the ordinal position of the object
            var value = jsonObject.rows[0].c[0].v.ToString();
            var formatted = jsonObject.rows[0].c[0].f.ToString();

            var valueCell2 = jsonObject.rows[0].c[1].v.ToString();
            var formattedCell2= jsonObject.rows[0].c[1].f.ToString();

            Assert.That(value == "Year");
            Assert.That(formatted == "MyYears");
            Assert.That(valueCell2 == "Date(2013, 0, 1)");
            Assert.That(formattedCell2 == "Now");
             
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
                row.AddCell(new Cell() { Value = DateTime.Now.AddDays(-i), Formatted = "Year" });
                row.AddCell(new Cell() { Value = i });
                row.AddProperty(new Property("style", "border:1"));
                row.AddProperty(new Property("fake", "fakeValue"));
                dt.AddRow(row);
            }

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------

            var jsonObject = JsonHelper.GetDynamicFromJson(json);

            //retrieving the ordinal position of the object
            var style = jsonObject.rows[0].p.style.ToString();   
            var fake = jsonObject.rows[0].p.fake.ToString();

            Assert.That(style == "border:1");
            Assert.That(fake == "fakeValue");
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
                var cell1 = new Cell() { Value = DateTime.Now.AddDays(-i), Formatted = "Year" };
                var cell2 = new Cell() { Value = i };

                cell1.AddProperty(new Property("style", "border:1"));
                cell1.AddProperty(new Property("fake", "fakeValue"));

                cell2.AddProperty(new Property("style", "border:2"));
                cell2.AddProperty(new Property("fake", "fakeValue2"));

                row.AddCell(cell1);
                row.AddCell(cell2);

                dt.AddRow(row);
            }

            //Act -----------------
            var json = dt.GetJson();

            //Assert --------------
            var jsonObject = JsonHelper.GetDynamicFromJson(json);

            //retrieving the ordinal position of the object
            var style = jsonObject.rows[0].c[0].p.style.ToString();
            var fake = jsonObject.rows[0].c[0].p.fake.ToString();

            var style2 = jsonObject.rows[0].c[1].p.style.ToString();
            var fake2 = jsonObject.rows[0].c[1].p.fake.ToString();


            Assert.That(style == "border:1");
            Assert.That(fake == "fakeValue");

            Assert.That(style2 == "border:2");
            Assert.That(fake2 == "fakeValue2");
        }

        [Test]
        public void DataTable_WillEscapeControlCharactersInColumn()
        {
            DataTable.EnableJsonStringEscaping = true;

            const string quote = "\"";
            const string slash = "\\";
            const string colId = "Id " + quote + " " + slash;
            const string colLabel = "Label " + quote + " " + slash;

            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            dt.AddColumn(new Column(ColumnType.String, colId, colLabel));

            //Act -----------------
            var json = dt.GetJson();
 
            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));

            var jsonObject = JsonHelper.GetDynamicFromJson(json);
            Assert.That(jsonObject.cols[0].id.Value == colId);
            Assert.That(jsonObject.cols[0].label.Value == colLabel);
        }

        [Test]
        public void DataTable_WillEscapeControlCharactersInCell()
        {
            DataTable.EnableJsonStringEscaping = true;
            
            const string quote = "\"";
            const string slash = "\\";
            const string cellValue = "Value " + quote + " " + slash;
            const string cellFormattedValue = "ValueFormatted " + quote + " " + slash;

            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            dt.AddColumn(new Column(ColumnType.String, "Doesn't matter"));

            var row = dt.NewRow();
            var cell = new Cell() { Value = cellValue, Formatted = cellFormattedValue };
            row.AddCell(cell);
            dt.AddRow(row);

            //Act -----------------
            var json = dt.GetJson();
 
            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));

            var jsonObject = JsonHelper.GetDynamicFromJson(json);
            Assert.That(jsonObject.rows[0].c[0].v.Value == cellValue);
            Assert.That(jsonObject.rows[0].c[0].f.Value == cellFormattedValue);
        }

        [Test]
        public void DataTable_WillEscapeControlCharactersInCellProperties()
        {
            DataTable.EnableJsonStringEscaping = true;
            
            const string quote = "\"";
            const string slash = "\\";
            const string propName = "Prop " + quote + " " + slash;
            const string propValue = "Prop " + quote + " " + slash;

            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            dt.AddColumn(new Column(ColumnType.String, "Doesn't matter"));

            var row = dt.NewRow();
            var cell = new Cell() { Value = "Irrelevant" };
            cell.AddProperty(new Property(propName, propValue));
            row.AddCell(cell);
            dt.AddRow(row);

            //Act -----------------
            var json = dt.GetJson();
 
            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));

            var jsonObject = JsonHelper.GetDynamicFromJson(json);
            Assert.That(jsonObject.rows[0].c[0].p[propName] == propValue);
        }


        [Test]
        public void DataTable_WillEscapeControlCharactersUsingCustomCallback()
        {
            const string quote = "\"";
            const string slash = "\\";
            const string colId = "Id " + quote + " " + slash;
            const string colLabel = "Label " + quote + " " + slash;

            DataTable.EnableJsonStringEscaping = true;
            DataTable.JsonStringEscapingCallback = value => value.Replace(quote, "{quote}").Replace(slash, "{slash}");;

            //Arrange ------------
            DataTable dt = GetNewDataTableInstance();
            dt.AddColumn(new Column(ColumnType.String, colId, colLabel));

            //Act -----------------
            var json = dt.GetJson();
 
            //Assert --------------
            Assert.IsTrue(json != null);
            Assert.IsTrue(IsValidJson(json));

            var jsonObject = JsonHelper.GetDynamicFromJson(json);
            Assert.That(jsonObject.cols[0].id.Value == "Id {quote} {slash}");
            Assert.That(jsonObject.cols[0].label.Value == "Label {quote} {slash}");
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
