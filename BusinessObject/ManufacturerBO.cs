using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class ManufacturerBO
    {
        public int ID { get; set; }
        public int StateID { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Place { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string AddedDate { get; set; }
    }
}
