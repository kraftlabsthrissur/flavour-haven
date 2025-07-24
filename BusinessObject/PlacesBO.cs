//file created by prama on 7-6-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class PlacesBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Place { get; set; }
        public String State { get; set; }
        public int StateID { get; set; }
        public String District { get; set; }
        public int DistrictID { get; set; }
        public String Country { get; set; }
        public int CountryID { get; set; }
    }
}
