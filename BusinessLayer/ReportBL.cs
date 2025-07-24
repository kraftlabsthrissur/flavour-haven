using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using BusinessLayer;
using PresentationContractLayer; 
using BusinessObject;

namespace BusinessLayer
{
   public class ReportBL:IReportContract 
    {
        ReportDAL reportDAL;
        public ReportBL()
        {
            reportDAL = new ReportDAL();
        }
        public List<SerialNumberBO> GetAutoComplete(String Term, String Table)
        {
            return reportDAL.GetAutoComplete( Term, Table);
        }
    }
}
