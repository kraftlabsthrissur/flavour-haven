using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;


namespace PresentationContractLayer
{
    public interface ISupplierContract
    {
        DatatableResultBO GetSupplierList(string Type, string CodeHint, string NameHint, string LocationHint, string SupplierCategoryHint, string ItemCategoryHint,string OldeCodeHint, string GSTRegisteredHint,string LandLine,string MobileNo, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetSupplierListForPaymentReturn(string Type, string CodeHint, string NameHint, string LocationHint, string SupplierCategoryHint, string ItemCategoryHint, string OldeCodeHint, string GSTRegisteredHint,string LandLine,string MobileNo, string SortField, string SortOrder, int Offset, int Limit);
        int CreateSupplier(SupplierBO supplierBO, List<AddressBO> _AddressList, List<SupplierBO> _SupplierItemCategoryList, List<SupplierBO> _SupplierLocationList, List<RelatedSupplierBO> RelatedSuppliers);
        int EditSupplier(SupplierBO supplierBO);
        List<SupplierBO> GetSupplierAutoComplete(string term);
        List<SupplierBO> GetAllSupplierList();
        List<SupplierBO> GetSupplierAutoCompleteForService(string term);
        List<SupplierBO> GetGRNWiseSupplierForAutoComplete(string term);
        List<SupplierBO> getCreditorsForAutoComplete(string term);
        List<SupplierBO> GetAllSupplierAutoComplete(string term);
        List<SupplierBO> InterCompanySupplier(string Term);
        List<SupplierBO> InterCompanySupplierListForLocation(string Term);
        List<SupplierBO> GetCreditDaysgroup();
        List<SupplierBO> PaymentGroupList();
        SupplierBO GetSupplierDetails(int SupplierID);
        string CheckSupplierAlradyExist(int ID,string  Name,string  GstNo,string  PanCardNo,string  AdhaarCardNo,string  Mobile,string  LandLine1,string  landline2,string  AcNo);
        List<SupplierBO> GetDoctorList();
        List<RelatedSupplierBO> GetRelatedSupplier(int SupplierID);
        int DeleteSupplier(int id);
        List<SupplierDescriptionBO> GetDescription(int SupplierID, string Type);




    }
}
