using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AddressBO
    {
        public string Name { get; set; }
        public Nullable<int> LocationID { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string Supplier { get; set; }
        public int AddressID { get; set; }
        public string Place { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int StateID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string ContactPerson { get; set; }
        public string LandLine1 { get; set; }
        public string LandLine2 { get; set; }
        public string MobileNo { get; set; }
        public int DistrictID { get; set; }
        public string Email { get; set; }
        public string PIN { get; set; }
        public string Fax { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefaultShipping { get; set; }   
        public string State { get; set; }
        public string District { get; set; }
        public string LocationGSTNo { get; set; }
        public string SupplierGSTNo { get; set; }
        public string CustomerGSTNo { get; set; }

        public List<BillingAddressBO> BillingTo { get; set; }
        public List<ShippingAddressBO> ShippingTo { get; set; }
    }

    public class BillingAddressBO
    {
        public Nullable<int> LocationID { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string Supplier { get; set; }
        public int AddressID { get; set; }
        public string Place { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int StateID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string MobileNo { get; set; }
        public int DistrictID { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }

    public class ShippingAddressBO
    {
        public Nullable<int> LocationID { get; set; }
        public string LocationCode { get; set; }
        public string Location { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string Supplier { get; set; }
        public int AddressID { get; set; }
        public string Place { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public int StateID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string MobileNo { get; set; }
        public int DistrictID { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }
}
