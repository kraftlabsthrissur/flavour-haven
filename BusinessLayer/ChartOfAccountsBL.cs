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
    public class ChartOfAccountsBL : IChartOfAccountContract
    {
        ChartOfAccountsDAL chartOfAccountsDAL;

        public ChartOfAccountsBL()
        {
            chartOfAccountsDAL = new ChartOfAccountsDAL();
        }

        public List<ChartOfAccountBO> GetChartOfAccountList()
        {
            return chartOfAccountsDAL.GetChartOfAccountList();
        }

        public int Save(ChartOfAccountBO chartOfAccountBO)
        {
            if (chartOfAccountBO.ID == 0)
            {
                return chartOfAccountsDAL.Save(chartOfAccountBO);
            }
            else
            {
                return chartOfAccountsDAL.Update(chartOfAccountBO);
            }
        }

        public List<ChartOfAccountBO> GetAccountHeadList()
        {
            return chartOfAccountsDAL.GetAccountHeadList();
        }

        public bool IsRemovedItem(int ID)
        {
            return chartOfAccountsDAL.IsRemovedItem(ID);
        }
    }
}
