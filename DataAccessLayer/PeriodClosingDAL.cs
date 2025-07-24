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
    public class PeriodClosingDAL
    {
        public List<PeriodClosingDaysBO> GetPeriodClosingList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPeriodClosing().Select(a => new PeriodClosingDaysBO
                    {
                        ID = a.ID,
                        Month = a.Month,
                        JournalStatus = a.JournalStatus,
                        SDNStatus = a.SDNStatus,
                        SCNStatus = a.SCNStatus,
                        CDNStatus = a.CDNStatus,
                        CCNStatus = a.CCNStatus,
                        FinYear = (int)a.FinYear
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save(List<PeriodClosingDaysBO> items)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    var i = 0;
                    foreach (var item in items)
                    {
                       i = dbEntity.SpUpdatePeriodClosing(
                                               item.ID,
                                               item.Month,
                                               item.JournalStatus,
                                               item.SDNStatus,
                                               item.SCNStatus,
                                               item.CDNStatus,
                                               item.CCNStatus,
                                               GeneralBO.FinYear,
                                               GeneralBO.LocationID,
                                               GeneralBO.ApplicationID);
                    }
                    return i;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        public string IsMonthClosed(string Type, string Month, int Year)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(string));
                    dbEntity.SpGetClosedMonths(
                            Type,
                            Month,
                            Year,
                            GeneralBO.ApplicationID,
                            ReturnValue
                        );
                    string status = ReturnValue.Value.ToString();
                    return status;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DateTime GetFirstOpenMonth(string Type)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(DateTime));
                    dbEntity.SpGetFirstOpenMonth(
                            Type,
                            ReturnValue
                        );
                    DateTime Dateval = Convert.ToDateTime(ReturnValue.Value);
                    return Dateval;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


    }

}

