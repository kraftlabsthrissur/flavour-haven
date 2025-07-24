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
   public class IncentiveCalculationBL : IIncentiveCalculationContract
    {
        IncentiveCalculationDAL incentiveCalculationDAL;
        public IncentiveCalculationBL()
        {
            incentiveCalculationDAL = new IncentiveCalculationDAL();
        }
        public List<IncentiveCalculationBO> GetCalculatedIncentives(int DurationID, int TimePeriodID, string PartyType)
        {
            return incentiveCalculationDAL.GetCalculatedIncentives(DurationID, TimePeriodID, PartyType);
        }
    }
}
