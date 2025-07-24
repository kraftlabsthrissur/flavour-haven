using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public  class StateBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string GstState { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string Country { get; set; }
    }
}
