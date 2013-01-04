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

using System.IO;
using NUnit.Framework;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class ColumnTest
    {
        [Test(Description = "Checks that the column object can be created with the default constructor")]
        public void Column_CanInstantiateColumn()
        {
            var c = new Column(ColumnType.String);
            Assert.That(c != null);
        }

        [Test(Description = "Checks that the Role field get's retruend in the json string")]
        public void Column_Role_GetsProperlySerialized()
        {
            //Arrange ------------------
            string columnJson;

            var column = new Column(ColumnType.String)
                {
                    Role = ColumnRole.Annotation
                };

            //Act ----------------------
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);

                column.GetJson(sw);

                sw.Flush();
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    columnJson = sr.ReadToEnd();
                }
            }

            //Assert -------------------
            Assert.That(columnJson != null);
            Assert.That(columnJson == "{\"type\": \"string\" , \"p\": { \"role\": \"annotation\"}}");
        }

        [Test(Description = "If the property and role are specified, this tests that the output in json gets generated properly")]
        public void Column_Property_And_Role_Specified()
        {
            //Arrange ------------------
            string columnJson;

            var column = new Column(ColumnType.String);
            column.Role = ColumnRole.Annotation;
            column.Properties = "\"property1\": \"value\"";

            //Act ----------------------
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);

                column.GetJson(sw);

                sw.Flush();
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    columnJson = sr.ReadToEnd();
                }
            }

            //Assert -------------------
            Assert.That(columnJson != null);
            Assert.That(columnJson == "{\"type\": \"string\" , \"p\": { \"role\": \"annotation\", \"property1\": \"value\"}}");
        }
    }
}
