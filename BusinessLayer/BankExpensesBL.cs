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
    public class BankExpensesBL :IBankExpensesContract
    {
       BankExpensesDAL bankexpensesDAL;
        public BankExpensesBL()
        {
            bankexpensesDAL = new BankExpensesDAL();
        }
        public List<BankExpensesBO> BankExpensesList()
        {
            return bankexpensesDAL.BankExpensesList();
        }
        public String Save(BankExpensesBO master, List<BankExpensesTransBO> details)
        {
            return bankexpensesDAL.Save(master, details);
        }
        public List<BankExpensesBO> GetAccountAutoComplete(string AccountNameHint, string AccountCodeHint)
        {
            return bankexpensesDAL.GetAccountAutoComplete(AccountNameHint, AccountCodeHint);
        }
        public List<BankExpensesBO> GetBankExpensesDetails(int BankExpensesID)
        {
            return bankexpensesDAL.GetBankExpensesDetails(BankExpensesID);
        }
        public List<BankExpensesTransBO> GetBankExpensesTransDetails(int BankExpensesID)
        {
            return bankexpensesDAL.GetBankExpensesTransDetails(BankExpensesID);
        }
        public List<BankExpensesBO> GetItemName()
        {
            return bankexpensesDAL.GetItemName();
        }
        public string Update(BankExpensesBO Master, List<BankExpensesTransBO> Details)
        {
            return bankexpensesDAL.Update(Master, Details);
        }

        public DatatableResultBO GetFinancialExpensesList(string Type, string TransNoHint, string TransDateHint, string ItemNameHint, string PaymentHint, string AmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return bankexpensesDAL.GetFinancialExpensesList(Type, TransNoHint, TransDateHint, ItemNameHint, PaymentHint, AmountHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
