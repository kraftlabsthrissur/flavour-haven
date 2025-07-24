using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface ICustomerReturnVoucherContract
    {
        List<CustomerReturnVoucherBO> GetCustomerReturnList();
        string Save(CustomerReturnVoucherBO Master, List<CustomerReturnVoucherItemBO> Details);
        List<CustomerReturnVoucherBO> GetCustomerReturnDetails(int CustomerReturnID);
        List<CustomerReturnVoucherItemBO> GetCustomerReturnTransDetails(int CustomerReturnID);
        bool Update(CustomerReturnVoucherBO Master, List<CustomerReturnVoucherItemBO> Details);

        DatatableResultBO GetCustomerReturnVoucherList(string Type,string VoucherNoHint,string VoucherDateHint,string CustomerNameHint,string PaymentHint,string ReturnAmountHint,string SortField,string SortOrder,int Offset,int Limit);
    }
}
