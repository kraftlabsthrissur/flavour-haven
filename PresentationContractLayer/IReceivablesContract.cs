using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IReceivablesContract
    {
        List<ReceivablesBO> GetReceivables(int CustomerID);
        int SaveReceivables(ReceivablesBO Receivable);
        List<ReceivablesBO> GetReceivablesV3(int AccountHeadID);
    }
}
