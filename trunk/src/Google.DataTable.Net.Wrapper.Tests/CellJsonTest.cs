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

using NUnit.Framework;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class CellJsonTest
    {
        [Test]
        public void GetJsonForNumericCellWithValueAndFormattedValue()
        {
            //Arrange ------------
            var c = new Cell();
            c.Value = 100;
            c.Formatted = "100";
            c.ColumnType = ColumnType.Number;
           
            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{\"v\": \"100\", \"f\": \"100\"}");
        }

        [Test]
        public void GetJsonForAnEmptyCell()
        {
            //Arrange ------------
            var c = new Cell();            

            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{}");
        }

        [Test]
        public void GetJsonWhenOnlyValueIsSpecified()
        {
            //Arrange ------------
            var c = new Cell();
            c.Value = 100;            
            c.ColumnType = ColumnType.Number;

            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{\"v\": \"100\"}");
        }

        [Test]
        public void GetJsonForNumericCellWithAllAttributesSpecified()
        {
            //Arrange ------------
            var c = new Cell();
            c.Value = 100;
            c.Formatted = "100";
            c.Properties = "style: 'border: 1px solid green;'";

            c.ColumnType = ColumnType.Number;

            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{\"v\": \"100\", \"f\": \"100\", \"p\":{style: 'border: 1px solid green;'}}");
        }
    }
}
