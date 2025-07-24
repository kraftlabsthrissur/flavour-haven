using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using DataAccessLayer;
using BusinessObject;

namespace BusinessLayer
{
    public class BuyerBL : IBuyerContract
    {
        BuyerDAL buyerDAL;
        public BuyerBL()
        {
            buyerDAL = new BuyerDAL();
        }


        public List<BuyerBO> GetBuyerList()
        {
            return buyerDAL.GetBuyerList();
        }


    }
}
