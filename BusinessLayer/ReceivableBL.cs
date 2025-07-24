using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class ReceivableBL : IReceivablesContract
    {
        ReceivableDAL receivableDAL;
        public ReceivableBL()
        {
            receivableDAL = new ReceivableDAL();
        }
        public List<ReceivablesBO> GetReceivables(int CustomerID)
        {
            return receivableDAL.GetReceivables(CustomerID);
        }

        public int SaveReceivables(ReceivablesBO Receivable)
        {
            return receivableDAL.SaveReceivables(Receivable);
        }
        public List<ReceivablesBO> GetReceivablesV3(int AccountHeadID)
        {
            return receivableDAL.GetReceivablesV3(AccountHeadID);
        }

    }
}
