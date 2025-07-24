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
    public class CustomerReturnVoucherBL : ICustomerReturnVoucherContract
    {
        CustomerReturnVoucherDAL customerReturnVoucherDAL;

        public CustomerReturnVoucherBL()
        {
            customerReturnVoucherDAL = new CustomerReturnVoucherDAL();
        }

        public List<CustomerReturnVoucherBO> GetCustomerReturnList()
        {
            return customerReturnVoucherDAL.GetCustomerReturnList();
        }

        public String Save(CustomerReturnVoucherBO Master, List<CustomerReturnVoucherItemBO> Details)
        {
            return customerReturnVoucherDAL.Save(Master, Details);
        }

        public bool Update(CustomerReturnVoucherBO Master, List<CustomerReturnVoucherItemBO> Details)
        {
            return customerReturnVoucherDAL.Update(Master, Details);
        }

        public List<CustomerReturnVoucherBO> GetCustomerReturnDetails(int CustomerReturnID)
        {
            return customerReturnVoucherDAL.GetCustomerReturnDetails(CustomerReturnID);
        }

        public List<CustomerReturnVoucherItemBO> GetCustomerReturnTransDetails(int CustomerReturnID)
        {
            return customerReturnVoucherDAL.GetCustomerReturnTransDetails(CustomerReturnID);
        }

        public DatatableResultBO GetCustomerReturnVoucherList(string Type, string VoucherNoHint, string VoucherDateHint, string CustomerNameHint, string PaymentHint, string ReturnAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return customerReturnVoucherDAL.GetCustomerReturnVoucherList(Type, VoucherNoHint, VoucherDateHint, CustomerNameHint, PaymentHint, ReturnAmountHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
