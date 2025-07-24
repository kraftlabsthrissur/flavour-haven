using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace PresentationContractLayer
{
    public interface IBankExpensesContract
    {
        List<BankExpensesBO> BankExpensesList();
        string Save(BankExpensesBO master, List<BankExpensesTransBO> details);
        List<BankExpensesBO> GetAccountAutoComplete(string AccountNameHint, string AccountCodeHint);
        List<BankExpensesBO> GetBankExpensesDetails(int bankexpensesID);
        List<BankExpensesTransBO> GetBankExpensesTransDetails(int BankExpensesID);
        List<BankExpensesBO> GetItemName();
        string Update(BankExpensesBO Master, List<BankExpensesTransBO> Details);

        DatatableResultBO GetFinancialExpensesList(string Type,string TransNoHint,string TransDateHint,string ItemNameHint,string PaymentHint,string AmountHint,string SortField,string SortOrder,int Offset,int Limit);
    }
}
