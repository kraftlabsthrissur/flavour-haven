using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class DriverModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string LicenseNo { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedDate { get; set; }
        public int ApplicationID { get; set; }
        public int LocationID { get; set; }
    }
}