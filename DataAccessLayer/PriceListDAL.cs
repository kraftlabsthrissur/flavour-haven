using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PriceListDAL
    {
        public List<PriceListBO> GetPriceList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetPriceListDetails().Select(a => new PriceListBO
                {
                    ID = a.ID,
                    Name = a.Name,
                    FromDate = (DateTime)a.FromDate,
                    ToDate = (DateTime)a.ToDate
                }).ToList();

            }
        }

        public List<PriceListBO> GetPriceListDetails(int ID)
        {
            try
            {
                List<PriceListBO> PriceList = new List<PriceListBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    PriceList = dEntity.SpGetPriceListByID(ID).Select(a => new PriceListBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        FromDate = (DateTime)a.FromDate,
                        ToDate = (DateTime)a.ToDate

                    }).ToList();
                    return PriceList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PriceListItemBO> GetPriceListTransDetails(int ID)
        {
            try
            {
                List<PriceListItemBO> PriceList = new List<PriceListItemBO>();

                using (MasterEntities dEntity = new MasterEntities())
                {
                    PriceList = dEntity.SpGetPriceListTransDetails(ID).Select(a => new PriceListItemBO
                    {
                        //ID = a.BasePriceListID,
                        ItemCode = a.ItemCode.TrimStart().TrimEnd(),
                        ItemID = a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = a.UnitID,
                        Unit = a.Unit,
                        ISKMRP=(decimal)a.ISKMRP,
                        ISKLoosePrice=(decimal)a.ISKLoosePrice

                    }).ToList();
                    return PriceList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save(PriceListBO priceListBO, string Items)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter BasePriceListID = new ObjectParameter("BasePriceListID", typeof(int));
                    ObjectParameter ReturnValueID = new ObjectParameter("ReturnValue", typeof(int));


                    var i = dbEntity.SpCreatePriceList(
                       priceListBO.Name,
                       priceListBO.FromDate,
                       priceListBO.ToDate,
                       GeneralBO.CreatedUserID,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID,
                       BasePriceListID,
                       ReturnValueID

                       );

                    int item = Convert.ToInt16(ReturnValueID.Value);
                    if (item == -1)
                    {
                        throw new Exception("PriceList Already exists");
                    }

                    dbEntity.SpCreatePriceListDetails(
                       Convert.ToInt32(BasePriceListID.Value),
                       Items,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID
                    );
                }
                return 1;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(PriceListBO priceListBO,string Items)
        {
            try
            {


                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var i = dbEntity.SpUpdatePriceList(
                        priceListBO.ID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID

                  );
                   
                       dbEntity.SpCreatePriceListDetails(
                            priceListBO.ID,
                            Items,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                       );

                    
                    return 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

}
