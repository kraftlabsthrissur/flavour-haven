using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPeriodClosingContract
    {
        List<PeriodClosingDaysBO> GetPeriodClosingList();
        int Save(List<PeriodClosingDaysBO> items);
        string IsMonthClosed(string Type, string Month,int Year);
        DateTime GetFirstOpenMonth(string Type);
    }
}
