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
    public class ServiceItemIssueBL : IServiceItemIssueContract
    {
        ServiceItemIssueDAL serviceItemIssueDAL;

        public ServiceItemIssueBL()
        {
            serviceItemIssueDAL = new ServiceItemIssueDAL();
        }
        public decimal GetTradeDiscountPercent()
        {
            return serviceItemIssueDAL.GetTradeDiscountPercent();
        }
        public bool Save(StockIssueBO stockIssueBO, List<StockIssueItemBO> stockIssueItems)
        {
            if (stockIssueBO.ID > 0)
            {
                return serviceItemIssueDAL.Update(stockIssueBO, stockIssueItems);

            }
            else
            {
                return serviceItemIssueDAL.Save(stockIssueBO, stockIssueItems);
            }
        }

        public DatatableResultBO GetServiceItemIssueList(string Type, string TransNoHint, string TransDateHint, string IssueLocationHint, string IssuePremiseHint, string ReceiptLocationHint, string ReceiptPremiseHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return serviceItemIssueDAL.GetServiceItemIssueList(Type, TransNoHint, TransDateHint, IssueLocationHint, IssuePremiseHint, ReceiptLocationHint, ReceiptPremiseHint, SortField, SortOrder, Offset, Limit);
        }

        public List<StockIssueBO> GetServiceItemIssueDetail(int ID)
        {
            return serviceItemIssueDAL.GetServiceItemIssueDetail(ID);
        }

        public List<StockIssueItemBO> GetServiceItemIssueTrans(int ID)
        {
            return serviceItemIssueDAL.GetServiceItemIssueTrans(ID);

        }

        public bool IsServiceOrStockIssue(int ID, string Type)
        {
            return serviceItemIssueDAL.IsServiceOrStockIssue(ID, Type);
        }

        public List<StockIssueBO> GetUnProcessedServiceItemIssueList(int IssueLocationID, int IssuePremiseID, int ReceiptLocationID, int ReceiptPremiseID)
        {
            return serviceItemIssueDAL.GetUnProcessedServiceItemIssueList(IssueLocationID, IssuePremiseID, ReceiptLocationID, ReceiptPremiseID);

        }

    }
}
