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
    
    public partial class SpGetAllCustomerList_Result
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<bool> IsGSTRegistered { get; set; }
        public Nullable<int> PriceListID { get; set; }
        public string CustomerCategory { get; set; }
        public Nullable<int> SchemeID { get; set; }
        public Nullable<int> recordsFiltered { get; set; }
        public Nullable<int> totalRecords { get; set; }
        public string Address { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public Nullable<int> CustomerCategoryID { get; set; }
        public string OldCode { get; set; }
        public string PropratorName { get; set; }
        public Nullable<bool> IsDraftCustomer { get; set; }
        public string Status { get; set; }
    }
}
