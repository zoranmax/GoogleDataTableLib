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
    public class ColumnTest
    {
        private const string TEST_ID = "TEST_ID";

        [Test]
        public void CanCreateColumn()
        {
            Column c = new Column(ColumnType.String);
            Assert.That(c != null);
        }

        [Test]
        public void GetJsonForSimplestPossibleColumn()
        {
            //Arrange ------------
            var c = new Column(ColumnType.String);

            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{\"type\": \"string\"}");
        }

        [Test]
        public void GetJsonForColumnWithAnId()
        {           
            //Arrange ------------
            var c = new Column(ColumnType.String, TEST_ID);

            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{\"type\": \"string\", \"id\": \""+ TEST_ID +"\"}");
        }

        [Test]
        public void GetJsonForColumnWithAnIdw()
        {
            //Arrange ------------
            var c = new Column(ColumnType.String, TEST_ID, "MY_LABEL");

            //Act -----------------
            var json = c.GetJson();

            //Assert --------------
            Assert.That(json == "{\"type\": \"string\", \"id\": \""+TEST_ID+"\", \"label\": \"MY_LABEL\"}");
        }

    }
}
