using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Google.DataTable.Net.Wrapper.Tests
{
    [TestFixture]
    public class PropertyTest
    {
        [Test]
        public void CanCreateAnInstanceByDefaultConstructor()
        {
            var property = new Property();
            Assert.That(property != null);
        }

        [Test]
        public void CanCreateAnInstanceByConstructorThatTakesNameAndValue()
        {
            var property = new Property("name", "value");

            Assert.That(property.Name == "name");
            Assert.That(property.Value == "value");
            Assert.That(property != null);
        }
    }
}
