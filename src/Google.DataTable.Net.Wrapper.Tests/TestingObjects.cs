using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.DataTable.Net.Wrapper.Tests
{
    public class ColumnTested
    {
        public List<Col> cols { get; set; }
        public List<Row> rows { get; set; }

        public string type { get; set; }
        public P p { get; set; }
    }

    public class Col
    {
        public string type { get; set; }
        public string id { get; set; }
        public string label { get; set; }
    }

    public class C
    {
        public string v { get; set; }
        public string f { get; set; }
        public P p { get; set; }
    }

    public class Row
    {
        public P p { get; set; }
       public List<C> c { get; set; }
    }
    

    public class P
    {
        public string fake { get; set; }
        public string style { get; set;  }
        public string property1 { get; set; }
        public string role { get; set; }
    }
}
