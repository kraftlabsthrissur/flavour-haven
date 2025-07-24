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
   public class AppBL : IAppContract
    {
        AppDAL appDAL;

        public AppBL()
        {
            appDAL = new AppDAL();
        }

        public DatatableResultBO GetActionList(string Type, string NameHint, string AreaHint, string ControllerHint, string ActionHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return appDAL.GetActionList(Type, NameHint, AreaHint, ControllerHint, ActionHint, SortField, SortOrder, Offset, Limit);
        }

        public int EnableItems(List<AppBO> EnableItems)
        {
            return appDAL.EnableItems(EnableItems);
        }
    }
}
