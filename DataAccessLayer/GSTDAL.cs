using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
  public  class GSTDAL
    {
        public List<GSTBO> GetGstList()
        {
            List<GSTBO> Gst = new List<GSTBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Gst = dEntity.GSTCategories.Select(a => new GSTBO
                {
                    ID=a.ID,
                    IGSTPercentage=(decimal)a.IGSTPercent,
                    GSTPercentage = (int)a.IGSTPercent
                }).ToList();
                return Gst;
            }

        }

    }
}
