using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ILogicCodeContract
    {
        int Save(LogicCodeBO LogicCodeBO);
        DatatableResultBO GetLogicCodeList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);
        LogicCodeBO GetLogicCodeDetails(int ID);
    }
}
