using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObject.FundTranferBO;

namespace BusinessLayer
{
    public class FundTransferBL : IFundTransferContract
    {
        FundTranferDAL fundtransferDAL;
        public FundTransferBL()
        {
            fundtransferDAL = new FundTranferDAL();
        }
        public List<FundTranferBO> FundTransferList()
        {
            return fundtransferDAL.FundTransferList();
        }

        public String Save(FundTranferBO Master, List<FundTranferTransBO> Details)
        {
            return fundtransferDAL.Save(Master, Details);
        }
        public bool Update(FundTranferBO Master, List<FundTranferTransBO> Details)
        {
            return fundtransferDAL.Update(Master, Details);
        }
        
        public List<FundTranferBO> GetFundTransferDetails(int FundTranferID)
        {
            return fundtransferDAL.GetFundTransferDetails(FundTranferID);
        }
        public List<FundTranferTransBO> GetFundTransferTransDetails(int FundTranferID)
        {
            return fundtransferDAL.GetFundTransferTransDetails(FundTranferID);
        }
        public List<FundTranferBO> GetLocationWiseBank(int LocationID)
        {
            return fundtransferDAL.GetLocationWiseBank(LocationID);
        }
        public List<FundTranferBO> GetFundTransferToLocation(int LocationID)
        {
            return fundtransferDAL.GetFundTransferToLocation(LocationID);
        }
        public DatatableResultBO GetFundTransferList(string Type, string FundTransferNo, string FundTransferDate, string FromLocation, string ToLocation, string ModeOfPayment, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit)
        {
            return fundtransferDAL.GetFundTransferList(Type, FundTransferNo, FundTransferDate, FromLocation, ToLocation, ModeOfPayment, TotalAmount, SortField, SortOrder, Offset, Limit);
        }
    }
}
