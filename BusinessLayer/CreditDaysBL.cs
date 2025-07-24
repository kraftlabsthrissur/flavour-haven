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
    public class CreditDaysBL : ICreditDaysContract
    {
        CreditDaysDAL creditDaysDAL;

        public CreditDaysBL()
        {
            creditDaysDAL = new CreditDaysDAL();
        }

        public List<CreditDaysBO> GetCreditDaysList()
        {
            return creditDaysDAL.GetCreditDaysList();
        }

        public List<CreditDaysBO> GetCreditDaysDetails(int ID)
        {
            return creditDaysDAL.GetCreditDaysDetails(ID);
        }

        public int Save(CreditDaysBO creditDaysBO)
        {
            if (creditDaysBO.ID == 0)
            {
                return creditDaysDAL.Save(creditDaysBO);
            }
            else
            {
                return creditDaysDAL.Update(creditDaysBO);
            }
        }
    }
}
