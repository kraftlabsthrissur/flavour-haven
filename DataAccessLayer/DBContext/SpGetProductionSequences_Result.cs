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
    
    public partial class SpGetProductionSequences_Result
    {
        public Nullable<int> ItemID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ProductionSequence { get; set; }
        public string ProcessStage { get; set; }
        public Nullable<decimal> BatchSize { get; set; }
        public Nullable<decimal> StandardOutput { get; set; }
        public Nullable<bool> IsQCRequiredForProduction { get; set; }
        public string Unit { get; set; }
    }
}
