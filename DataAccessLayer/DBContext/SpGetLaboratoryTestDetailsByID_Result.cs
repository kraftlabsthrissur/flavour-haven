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
    
    public partial class SpGetLaboratoryTestDetailsByID_Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string BiologicalReference { get; set; }
        public string Unit { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Method { get; set; }
        public int SpecimenID { get; set; }
        public string Specimen { get; set; }
        public int GSTCategoryID { get; set; }
        public decimal Rate { get; set; }
        public decimal GSTCategory { get; set; }
        public string Description { get; set; }
        public bool IsAlsoCategory { get; set; }
    }
}
