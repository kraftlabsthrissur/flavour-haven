using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IAppContract
    {
        DatatableResultBO GetActionList(string Type, string NameHint, string AreaHint, string ControllerHint, string ActionHint, string SortField, string SortOrder, int Offset, int Limit);

        int EnableItems(List<AppBO> EnableItems);
    }
}
