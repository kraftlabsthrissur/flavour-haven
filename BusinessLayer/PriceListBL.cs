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
    public class PriceListBL : IPriceListContract
    {
        PriceListDAL priceListDAL;

        public PriceListBL()
        {
            priceListDAL = new PriceListDAL();
        }

        public List<PriceListBO> GetPriceList()
        {
            return priceListDAL.GetPriceList();
        }

        public List<PriceListBO> GetPriceListDetails(int ID)
        {
            return priceListDAL.GetPriceListDetails(ID);
        }

        public List<PriceListItemBO> GetPriceListTransDetails(int ID)
        {
            return priceListDAL.GetPriceListTransDetails(ID);
        }

        public int Save(List<PriceListItemBO> Items, PriceListBO priceListBO)
        {
           

            string StringItems = XMLHelper.Serialize(Items);
           

            if (priceListBO.ID == 0)
            {
                return priceListDAL.Save(priceListBO, StringItems);
            }
            else
            {
                return priceListDAL.Update(priceListBO, StringItems);
            }

        }

        public List<PriceListItemBO> ReadExcel(string Path)
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(0, "ItemCode");
            dict.Add(1, "ItemName");
            dict.Add(2, "ISKMRP");
            dict.Add(3, "ISKLoosePrice");

            PriceListItemBO UploadPriceList = new PriceListItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<PriceListItemBO> PriceList;
            try
            {
                PriceList = generalBL.ReadExcel(Path, UploadPriceList, dict);
            }
            catch (Exception e)
            {
                throw e;
            }
            return PriceList;
        }
    }
}
