using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class LocationBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int PlaceID { get; set; }    
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CompanyName { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlaces { get; set; }
        public string OwnerName { get; set; }
        public string GSTNo { get; set; }
        public string Jurisdiction { get; set; }
        public string AuthorizedSignature { get; set; }
        public string URL { get; set; }
        public int StateID { get; set; }
        public int LocationStateID { get; set; }
        public string LocationState { get; set; }
        public string State { get; set; }    
        public int LocationHeadID { get; set; }
        public string LocationHead{ get; set; } 
        public int LocationTypeID { get; set; }
        public string LocationType { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int SupplierID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public int CountryID { get; set; }
        public int CurrencyID { get; set; }
        public string TaxType { get; set; }
        public int TaxTypeID { get; set; }
        public int IsGST { get; set; }
        public int IsVat { get; set; }
        public string VatRegNo { get; set; }
    }
    public class LocationAddressBO
    {
        public int AddressID { get; set; }
        public string LandLine1 { get; set; }
        public string LandLine2 { get; set; }
    
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PIN { get; set; }
        public string MobileNo { get; set; }
        public string ContactPerson { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultShipping { get; set; }
    }
}
