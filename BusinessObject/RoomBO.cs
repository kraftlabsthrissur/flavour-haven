using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessObject
{
   public class RoomBO
    {
        public int ID { get; set; }
        public int RoomTypeID { get; set; }
        public int StoreID { get; set; }

        public string Code { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string RoomType { get; set; }
        public string Store { get; set; }
        public decimal Rate { get; set; }
        public SelectList RoomTypeList { get; set; }
    }
}
