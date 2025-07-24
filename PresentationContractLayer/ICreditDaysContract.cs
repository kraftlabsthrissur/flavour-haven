using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ICreditDaysContract
    {
        List<CreditDaysBO> GetCreditDaysList();
        List<CreditDaysBO> GetCreditDaysDetails(int ID);
        int Save(CreditDaysBO CreditDays);
    }
}
