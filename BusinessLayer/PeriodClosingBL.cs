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
    public class PeriodClosingBL : IPeriodClosingContract
    {
        PeriodClosingDAL periodClosingDAL;

        public PeriodClosingBL()
        {
            periodClosingDAL = new PeriodClosingDAL();
        }

        public List<PeriodClosingDaysBO> GetPeriodClosingList()
        {
            return periodClosingDAL.GetPeriodClosingList();
        }

        public int Save(List<PeriodClosingDaysBO> items)
        {
            return periodClosingDAL.Save(items);
        }
        public string IsMonthClosed(string Type, string Month, int Year)
        {
            return periodClosingDAL.IsMonthClosed(Type, Month, Year);
        }
        public DateTime GetFirstOpenMonth(string Type)
        {
            return periodClosingDAL.GetFirstOpenMonth(Type);
        }
    }
}
