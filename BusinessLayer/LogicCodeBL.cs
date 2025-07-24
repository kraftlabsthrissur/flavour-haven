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
    public class LogicCodeBL:ILogicCodeContract
    {
        LogicCodeDAL LogicCodeDAL;

        public LogicCodeBL()
        {
            LogicCodeDAL = new LogicCodeDAL();
        }

        public int Save(LogicCodeBO LogicCodeBO)
        {
            if (LogicCodeBO.ID == 0)
            {
                return LogicCodeDAL.Save(LogicCodeBO);
            }
            else
            {
                return LogicCodeDAL.Update(LogicCodeBO);
            }
           
        }

        public DatatableResultBO GetLogicCodeList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return LogicCodeDAL.GetLogicCodeList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
        }

        public LogicCodeBO GetLogicCodeDetails(int ID)
        {
            return LogicCodeDAL.GetLogicCodeDetails(ID);
        }
    }
}
