using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PackingBO
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }
        public string UOM { get; set; }
        public string BatchName { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchType { get; set; }
        public decimal BatchSize { get; set; }
        public string CurrentStage { get; set; }
        public decimal PackedQty { get; set; }
        public int IsBatchSuspended { get; set; }
        public bool IsAborted { get; set; }
        public bool IsCancelled { get; set; }
        public string Remarks { get; set; }
        public int UnitID { get; set; }


    }
}
