using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ITaxTypeContract
    {
        List<TaxTypeBO> GetTaxTypeDDLList();

        List<TaxTypeBO> GetTaxTypeListByLocation(int LocationID);
        List<TaxTypeBO> GetTaxTypeList();
        int CreateTaxtype(TaxTypeBO TaxTypeBO);
        TaxTypeBO GetTaxtypeDetails(int TaxtypeID);

        int EditTaxtype(TaxTypeBO TaxTypeBO);
       


    }
}
