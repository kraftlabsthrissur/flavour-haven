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
    
    public partial class GSTCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> SGSTPercent { get; set; }
        public Nullable<decimal> CGSTPercent { get; set; }
        public Nullable<decimal> IGSTPercent { get; set; }
        public Nullable<int> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> TaxTypeID { get; set; }
        public Nullable<decimal> VATPercentage { get; set; }
    }
}
