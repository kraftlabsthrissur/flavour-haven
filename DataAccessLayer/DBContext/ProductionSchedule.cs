//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccessLayer.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductionSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductionSchedule()
        {
            this.ProductionScheduleTrans = new HashSet<ProductionScheduleTran>();
        }
    
        public int ID { get; set; }
        public string TransNo { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<int> ProductionGroupID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<System.DateTime> ProductionStartDate { get; set; }
        public Nullable<System.DateTime> ProductionStartTime { get; set; }
        public Nullable<decimal> StandardBatchSize { get; set; }
        public Nullable<decimal> ActualBatchSize { get; set; }
        public Nullable<int> ProductonLocationID { get; set; }
        public Nullable<int> RequestedStoreID { get; set; }
        public Nullable<bool> IsDraft { get; set; }
        public Nullable<bool> IsCompleted { get; set; }
        public string ProductionStatus { get; set; }
        public Nullable<bool> IsAborted { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> FinYear { get; set; }
        public Nullable<int> LocationID { get; set; }
        public Nullable<int> ApplicationID { get; set; }
        public Nullable<int> BatchID { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<int> MachineID { get; set; }
        public Nullable<int> MouldID { get; set; }
        public Nullable<int> ProcessID { get; set; }
        public string ProductionEndDate { get; set; }
        public string ProductionEndTime { get; set; }
        public string StartTime { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductionScheduleTran> ProductionScheduleTrans { get; set; }
    }
}
