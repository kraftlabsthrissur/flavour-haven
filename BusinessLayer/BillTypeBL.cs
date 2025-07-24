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
   public class BillTypeBL:IBillTypeContract
    {
        BillTypeDAL billTypeDAL;
        public BillTypeBL()
        {
            billTypeDAL = new BillTypeDAL();
        }

        public List<BillTypeBO> GetTreatmentServices(string Type)
        {
            return billTypeDAL.GetTreatmentServices(Type);
        }
        public List<BillTypeBO> GetBillTypeItems(string Type)
        {
            return billTypeDAL.GetBillTypeItems(Type);
        }
        public int Save(List<BillTypeBO> OPItem, List<BillTypeBO> IPItem)
        {
            return billTypeDAL.Save(OPItem,IPItem);
        }
    }
}
