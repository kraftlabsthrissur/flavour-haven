using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class DistrictBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> PIN { get; set; }
        public string OfficeName { get; set; }
        public string Taluk { get; set; }
        public int CountryID { get; set; }
    }
}
