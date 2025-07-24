using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;
namespace BusinessLayer
{
    public class SchemeAllocationBL : ISchemeAllocationContract
    {
        SchemeAllocationDAL schemeAllocationDAL;

        public SchemeAllocationBL()
        {
            schemeAllocationDAL = new SchemeAllocationDAL();
        }        

        public int Save(SchemeAllocationBO schemeallocationBO)
        {
            string StringItems = XMLHelper.Serialize(schemeallocationBO.Items);
            string StringCustomers = XMLHelper.Serialize(schemeallocationBO.Customers);
            string StringStates = XMLHelper.Serialize(schemeallocationBO.States);
            string StringCategories = XMLHelper.Serialize(schemeallocationBO.CustomerCategory);
            string StringDistricts = XMLHelper.Serialize(schemeallocationBO.Districts);

            schemeAllocationDAL.Save(schemeallocationBO, StringItems, StringCustomers, StringCategories, StringStates, StringDistricts);
            return 0;
        }

        public DatatableResultBO GetSchemeAllocationList(string CodeHint, string NameHint, string CustomerNameHint, string CustomerCategoryHint, string CustomerStateHint, string CustomerDistrictHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return schemeAllocationDAL.GetSchemeAllocationList(CodeHint, NameHint, CustomerNameHint, CustomerCategoryHint, CustomerStateHint, CustomerDistrictHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetSchemeAllocationTransList(int ID, string NameHint, string SalesCategoryHint, string StartDate, string EndDate, string SortField, string SortOrder, int Offset, int Limit)
        {
            return schemeAllocationDAL.GetSchemeAllocationTransList(ID, NameHint, SalesCategoryHint, StartDate, EndDate, SortField, SortOrder, Offset, Limit);
        }

        public SchemeAllocationBO GetSchemeAllocationDetails(int ID)
        {
            return schemeAllocationDAL.GetSchemeAllocationDetails(ID);
        }

        public List<SchemeItemBO> GetSchemeItemList(int ID)
        {
            return schemeAllocationDAL.GetSchemeItemList(ID);
        }

        public List<SchemeCustomerBO> GetSchemeCustomerList(int ID)
        {
            return schemeAllocationDAL.GetSchemeCustomerList(ID);
        }

        public List<SchemeCustomerCategoryBO> GetSchemeCategoryList(int ID)
        {
            return schemeAllocationDAL.GetSchemeCategoryList(ID);
        }

        public List<SchemeStateBO> GetSchemeStateList(int ID)
        {
            return schemeAllocationDAL.GetSchemeStateList(ID);
        }

        public List<SchemeDistrictBO> GetSchemeDistrictList(int ID)
        {
            return schemeAllocationDAL.GetSchemeDistrictList(ID);
        }

    }
}
