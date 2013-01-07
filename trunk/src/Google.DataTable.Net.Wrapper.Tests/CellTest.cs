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

using System.Linq;
using NUnit.Framework;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class CellTest
    {
        [Test]
        public void Cell_CheckDefaultValuesUpponInstantiation()
        {
            //Arrange ------------            
            var c = new Cell();
            
            //Act -----------------

            //Assert --------------
            Assert.That(c!=null);
            Assert.That(c.PropertyMap.Any() == false);
            Assert.That(c.Value == null);
            Assert.That(c.Formatted == null);
            Assert.That(c.ColumnType == ColumnType.String);
        }

        [Test]
        public void Cell_CanAddProperty()
        {
            //Arrange ------------            
            var c = new Cell();

            //Act -----------------
            c.AddProperty(new Property("style", "border:1"));
            c.AddProperty(new Property("style2", "border:2"));

            //Assert --------------
            Assert.That(c.PropertyMap.Count() == 2);
            Assert.That(c.PropertyMap.ElementAt(0).Name == "style");
            Assert.That(c.PropertyMap.ElementAt(0).Value == "border:1");
        }

        [Test]
        public void Cell_CanRemoveProperty()
        {
            //Arrange ------------            
            var c = new Cell();

            var property1 = c.AddProperty(new Property("style", "border:1"));
            var property2 = c.AddProperty(new Property("style2", "border:2"));

            //Act -----------------
            c.RemoveProperty(property1);

            //Assert --------------
            Assert.That(c.PropertyMap.Count() == 1);
            Assert.That(c.PropertyMap.ElementAt(0).Name == "style2");
            Assert.That(c.PropertyMap.ElementAt(0).Value == "border:2");
        }

        [Test]
        public void Cell_CanRemovePropertyByIndex()
        {
            //Arrange ------------            
            var c = new Cell();

            var property1 = c.AddProperty(new Property("style", "border:1"));
            var property2 = c.AddProperty(new Property("style2", "border:2"));

            //Act -----------------
            c.RemoveProperty(0);

            //Assert --------------
            Assert.That(c.PropertyMap.Count() == 1);
            Assert.That(c.PropertyMap.ElementAt(0).Name == "style2");
            Assert.That(c.PropertyMap.ElementAt(0).Value == "border:2");
        }
    }
}
