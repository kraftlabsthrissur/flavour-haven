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
    
    public partial class SpGetPriviledgeCardsByID_Result
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ValidDays { get; set; }
        public int DiscountCategoryID { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public decimal Rate { get; set; }
    }
}
