using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TaxTypeBL : ITaxTypeContract
    {
        TaxTypeDAL taxTypeDAL;
        public TaxTypeBL()
        {
            taxTypeDAL = new TaxTypeDAL();

        }
        CurrencyDAL currencyDAL;

       
        public List<TaxTypeBO> GetTaxTypeDDLList()
        {
            return taxTypeDAL.GetTaxTypeDDLList().ToList();
        }
        public List<TaxTypeBO> GetTaxTypeListByLocation(int LocationID)
        {
            return taxTypeDAL.GetTaxTypeListByLocation(LocationID).ToList();
        }
        public List<TaxTypeBO> GetTaxTypeList()
        {
            return taxTypeDAL.GetTaxTypeList().ToList();
        }
        public int CreateTaxtype(TaxTypeBO TaxTypeBO)
        {
            return taxTypeDAL.CreateTaxtype(TaxTypeBO);
        }
        public TaxTypeBO GetTaxtypeDetails(int LocationID)
        {
            return taxTypeDAL.GetTaxtypeDetails(LocationID);
        }
        public int EditTaxtype(TaxTypeBO TaxTypeBO)
        {
            return taxTypeDAL.UpdateTaxtype(TaxTypeBO);
        }

    }
}
