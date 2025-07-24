using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class SupplierBL : ISupplierContract
    {
        SupplierDAL supplierDAL;

        public SupplierBL()
        {
            supplierDAL = new SupplierDAL();
        }

        public int CreateSupplier(SupplierBO supplierBO, List<AddressBO> _AddressList, List<SupplierBO> _SupplierItemCategoryList, List<SupplierBO> _SupplierLocationList, List<RelatedSupplierBO> RelatedSuppliers)
        {
            int result;
            if (supplierBO.ID == 0)
            {
                result = supplierDAL.CreateSupplier(supplierBO, _AddressList, _SupplierItemCategoryList, _SupplierLocationList, RelatedSuppliers);
            }
            else
            {
                result = supplierDAL.UpdateSupplier(supplierBO, _AddressList, _SupplierItemCategoryList, _SupplierLocationList, RelatedSuppliers);
            }

            return result;
        }

        public int DeleteSupplier(SupplierBO supplerBO)
        {
            throw new NotImplementedException();
        }

        public int EditSupplier(SupplierBO supplierBO)
        {
            throw new NotImplementedException();
        }

        public string CheckSupplierAlradyExist(int ID, string Name, string GstNo, string PanCardNo, string AdhaarCardNo, string Mobile, string LandLine1, string landline2, string AcNo)
        {
            return supplierDAL.CheckSupplierAlradyExist( ID,Name,GstNo,PanCardNo,AdhaarCardNo,Mobile,LandLine1,landline2,AcNo);
        }
        public List<SupplierBO> GetSupplierAutoComplete(string term)
        {
            return supplierDAL.GetSupplierAutoComplete(term, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }
        public List<SupplierBO> GetAllSupplierList()
        {
            return supplierDAL.GetAllSupplierList();
        }
        public List<SupplierBO> GetSupplierAutoCompleteForService(string term)
        {
            return supplierDAL.GetSupplierAutoCompleteForService(term, GeneralBO.LocationID, GeneralBO.ApplicationID);
        }

        public DatatableResultBO GetSupplierList(string Type, string CodeHint, string NameHint, string LocationHint, string SupplierCategoryHint, string ItemCategoryHint,string OldCodeHint, string GSTRegisteredHint,string LandLine,string MobileNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            return supplierDAL.GetSupplierList(Type, CodeHint, NameHint, LocationHint, SupplierCategoryHint, ItemCategoryHint,OldCodeHint, GSTRegisteredHint, LandLine, MobileNo, SortField, SortOrder, Offset, Limit);
        }


        public DatatableResultBO GetSupplierListForPaymentReturn(string Type, string CodeHint, string NameHint, string LocationHint, string SupplierCategoryHint, string ItemCategoryHint, string OldCodeHint, string GSTRegisteredHint,string LandLine,string MobileNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            return supplierDAL.GetSupplierListForPaymentReturn(Type, CodeHint, NameHint, LocationHint, SupplierCategoryHint, ItemCategoryHint, OldCodeHint, GSTRegisteredHint, LandLine, MobileNo, SortField, SortOrder, Offset, Limit);
        }

        public List<SupplierBO> GetGRNWiseSupplierForAutoComplete(string term)
        {
            return supplierDAL.GetGRNWiseSupplierForAutoComplete(term);
        }

        //code below by prama on 23-4-18 for payment voucher
        public List<SupplierBO> getCreditorsForAutoComplete(string term)
        {
            return supplierDAL.getCreditorsForAutoComplete(term);
        }

        public List<SupplierBO> GetAllSupplierAutoComplete(string Term)
        {
            return supplierDAL.GetAllSupplierAutoComplete(Term);
        }
        public List<SupplierBO> InterCompanySupplier(string Term)
        {
            return supplierDAL.InterCompanySupplier(Term);
        }
        public List<SupplierBO> InterCompanySupplierListForLocation(string Term)
        {
            return supplierDAL.InterCompanySupplierListForLocation(Term);
        }
        public List<SupplierBO> GetCreditDaysgroup()
        {
            return supplierDAL.GetCreditDaysgroup();
        }
        public List<SupplierBO> PaymentGroupList()
        {
            return supplierDAL.PaymentGroupList();
        }

        public SupplierBO GetSupplierDetails(int SupplierID)
        {
            return supplierDAL.GetSupplierDetails(SupplierID);
        }

        public bool IsInterCompanySupplier(int SupplierID)
        {
            return supplierDAL.IsInterCompanySupplier(SupplierID);
        }

        public int GetCustomerID(int SupplierID)
        {
            return supplierDAL.GetCustomerID(SupplierID);
        }
        public int DeleteSupplier(int id)
        {
            return supplierDAL.DeleteSupplier(id);
        }
        public int GetCustomerID()
        {
            return supplierDAL.GetCustomerID();
        }

        public int GetSupplierLocationID(int SupplierID)
        {
            return supplierDAL.GetSupplierLocationID(SupplierID);
        }
        public List<SupplierBO> GetDoctorList()
        {
            return supplierDAL.GetDoctorList();
        }

        public List<RelatedSupplierBO> GetRelatedSupplier(int SupplierID)
        {
            return supplierDAL.GetRelatedSupplier(SupplierID);
        }
        public List<SupplierDescriptionBO> GetDescription(int SupplierID, string Type)
        {
            return supplierDAL.GetDescription(SupplierID, Type);
        }
    }
}
