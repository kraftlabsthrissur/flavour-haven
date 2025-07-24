using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using static BusinessObject.FundTranferBO;

namespace PresentationContractLayer
{
    public interface IFundTransferContract
    {
        List<FundTranferBO> FundTransferList();
        string Save(FundTranferBO Master, List<FundTranferTransBO> Details);       
        List<FundTranferBO> GetFundTransferDetails(int FundTranferID);
        List<FundTranferBO> GetLocationWiseBank(int LocationID);
        List<FundTranferBO> GetFundTransferToLocation(int LocationID);
        List<FundTranferTransBO> GetFundTransferTransDetails(int FundTranferID);
        bool Update(FundTranferBO Master, List<FundTranferTransBO> Details);
        DatatableResultBO GetFundTransferList(string Type, string FundTransferNo, string FundTransferDate, string FromLocation, string ToLocation, string ModeOfPayment, string TotalAmount, string SortField, string SortOrder, int Offset, int Limit);
    }
}
