using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BillTypeDAL
    {
        public List<BillTypeBO> GetTreatmentServices(string Type)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    return dbEntity.SpGetTreatmentServices(Type, GeneralBO.LocationID).Select(
                    a => new BillTypeBO()
                    {
                        BillTypeID = a.ID,
                        BillTypeName = a.Name,
                        State = a.Selected
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public List<BillTypeBO> GetBillTypeItems(string Type)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    return dbEntity.SpGetBillTypeItems(Type, GeneralBO.LocationID).Select(
                    a => new BillTypeBO()
                    {
                        BillTypeID=(int)a.BillTypeID
                    }).ToList();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public int Save(List<BillTypeBO> OPItem, List<BillTypeBO> IPItem)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    dbEntity.SpDeleteAllBillType(
                      GeneralBO.LocationID,
                      GeneralBO.ApplicationID,
                      GeneralBO.FinYear
                      );
                    if (OPItem != null)
                    {
                        foreach (var item in OPItem)
                        {
                            dbEntity.SpCreateBillType(
                               item.BillTypeID,
                               item.Type,
                               GeneralBO.CreatedUserID,
                               GeneralBO.LocationID,
                               GeneralBO.ApplicationID,
                               GeneralBO.FinYear
                                );
                        }
                    }
                    if (IPItem != null)
                    {
                        foreach (var item in IPItem)
                        {
                            dbEntity.SpCreateBillType(
                               item.BillTypeID,
                               item.Type,
                               GeneralBO.CreatedUserID,
                               GeneralBO.LocationID,
                               GeneralBO.ApplicationID,
                               GeneralBO.FinYear
                                );
                        }
                    }

                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
