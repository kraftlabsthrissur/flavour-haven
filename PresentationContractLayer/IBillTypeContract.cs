using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IBillTypeContract
    {
        List<BillTypeBO> GetTreatmentServices(string Type);
        List<BillTypeBO> GetBillTypeItems(string Type);
        int Save(List<BillTypeBO> OPItem, List<BillTypeBO> IPItem);
    }
}
