using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IChartOfAccountContract
    {
        List<ChartOfAccountBO> GetChartOfAccountList();
        int Save(ChartOfAccountBO chartOfAccountBO);
        List<ChartOfAccountBO> GetAccountHeadList();
        bool IsRemovedItem(int ID);

    }
}
