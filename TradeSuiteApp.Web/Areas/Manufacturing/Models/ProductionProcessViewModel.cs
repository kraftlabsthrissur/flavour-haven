using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class ProductionProcessViewModel
    {
        public string SlNo { get; set; }
        public string Date { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public string BatchNo { get; set; }
        public decimal BatchSize { get; set; }
        public string BatchName { get; set; }
        public int BatchID { get; set; }
         public SelectList BatchList { get; set; }
        public int ID { get; set; }




        public List<ProductionProcessItemModel> Items { get; set; }
    }

    public class ProductionProcessItemModel
    {
        public string stage { get; set; }
        public string ProcessName { get; set; }
        public string StartTime { get; set; }
        public int InputQuantity { get; set; }
        public string EndTime { get; set; }
        public int OutputQty { get; set; }
        public int SkilledLabours { get; set; }
        public int UnSkilledLabours { get; set; }
        public decimal MachineHours { get; set; }
        public String DoneBy { get; set; }
        public int StatusId { get; set; }
        public SelectList StatusList { get; set; }





    }



}