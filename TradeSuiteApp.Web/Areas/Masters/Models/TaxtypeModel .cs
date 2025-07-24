using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class TaxtypeModel
    {

        public int? Id { get; set; }       
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Createddate { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserID { get; set; }
        public int IsActive { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }

    }
}