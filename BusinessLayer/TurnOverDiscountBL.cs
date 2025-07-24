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
    public class TurnOverDiscountBL : ITurnOverDiscountContract
    {
        TurnOverDiscountsDAL discountDAL;

        public TurnOverDiscountBL()
        {
            discountDAL = new TurnOverDiscountsDAL();
        }

        public List<DiscountItemBO> ReadExcel(string Path)
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(0, "Code");
            dict.Add(1, "CustomerName");
            dict.Add(2, "TurnOverDiscount");
            dict.Add(3, "FromDate");
            dict.Add(4, "ToDate");
            dict.Add(5, "Month");
            dict.Add(6, "Location");

            DiscountItemBO UploadDiscountList = new DiscountItemBO();
            GeneralBL generalBL = new GeneralBL();
            List<DiscountItemBO> DiscountList;
            try
            {
                DiscountList = generalBL.ReadExcel(Path, UploadDiscountList, dict);
            }
            catch (Exception e)
            {
                throw e;
            }
            return DiscountList;
        }

        public DatatableResultBO GetCustomerListForLocation(int CustomerLocationID, string CodeHint, string NameHint, string LocationHint, string CustomerCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return discountDAL.GetCustomerListForLocation(CustomerLocationID, CodeHint, NameHint, LocationHint, CustomerCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public int Save(List<DiscountItemBO> Items, TurnOverDiscountsBO turnOverDiscountBO)
        {
            if (turnOverDiscountBO.ID == 0)
            {
                return discountDAL.Save(Items, turnOverDiscountBO);
            }
            else
            {
                return discountDAL.Update(Items, turnOverDiscountBO);
            }
           
        }

        public List<TurnOverDiscountsBO> GetTurnOverDiscountList()
        {
            return discountDAL.GetTurnOverDiscountList();
        }

        public List<TurnOverDiscountsBO> GetTurnOverDiscountDetails(int ID)
        {
            return discountDAL.GetTurnOverDiscountDetails(ID);
        }

        public List<DiscountItemBO> GetTurnOverDiscountTransDetails(int ID)
        {
            return discountDAL.GetTurnOverDiscountTransDetails(ID);
        }

    }
}

