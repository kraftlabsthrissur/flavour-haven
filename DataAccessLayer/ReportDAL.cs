using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using BusinessObject;
using DataAccessLayer.DBContext; 
namespace DataAccessLayer
{
 public   class ReportDAL
    {
        public List<SerialNumberBO> GetAutoComplete(String Term, String Table) 
        {
            List<SerialNumberBO> code = new List<SerialNumberBO>();
            using (AyurwareEntities dEntity = new AyurwareEntities())
            {
                code = dEntity.SpGetSerialNoAutoComplete(Table,Term, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SerialNumberBO
                {
                    ID = a.ID,
                    Code = a.Code,

                }).ToList();
                return code;
            }

        }

    }
}
