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
   public  class IncentiveCalculationDAL
    {
        public List<IncentiveCalculationBO> GetCalculatedIncentives(int DurationID, int TimePeriodID, string PartyType)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetCalculatedIncentives(DurationID, TimePeriodID, PartyType, GeneralBO.FinYear,GeneralBO.ApplicationID).Select(a => new IncentiveCalculationBO()
                    {
                        PartyID=(int)a.PartyID,
                        PartyName=a.PartyName,
                        TotalClassicalTarget=(decimal)a.TotalClassicalTarget,
                        TotalPatentTarget=(decimal)a.TotalPatentTarget,
                        TotalTarget=(decimal)a.TotalTarget,
                        TotalAchievedClassicalTarget=(decimal)a.TotalAchievedClassicalTarget,
                        TotalAchievedPatentTarget=(decimal)a.TotalAchievedPatentTarget,
                        TotalAchievedTarget=(decimal)a.TotalAchievedTarget,
                        TotalAchievedClassicalPercent=(decimal)a.TotalAchievedClassicalPercent,
                        TotalAchievedPatentPercent=(decimal)a.TotalAchievedPatentPercent,
                        TotalAchievedPercent=(decimal)a.TotalAchievedPercent,
                        ClassicalIncentiveAmount=(decimal)a.ClassicalIncentiveAmount,
                        PatentIncentiveAmount=(decimal)a.PatentIncentiveAmount,
                        TotalIncentiveAmount=(decimal)a.TotalIncentiveAmount,
                        IncentiveAboveLimit=(decimal)a.IncentiveAbove105percent,
                        TotalEligableAmount=(decimal)a.TotalEligableAmount,

                    }).ToList();
                    
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
