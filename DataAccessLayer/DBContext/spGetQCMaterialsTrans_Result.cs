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
    
    public partial class spGetQCMaterialsTrans_Result
    {
        public int ID { get; set; }
        public int QCID { get; set; }
        public Nullable<int> QCTestID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
        public string DefinedResult { get; set; }
        public Nullable<decimal> ActualValue { get; set; }
        public string ActualResult { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsMandatory { get; set; }
    }
}
