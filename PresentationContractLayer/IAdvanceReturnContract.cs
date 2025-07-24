using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IAdvanceReturnContract
    {
           bool Save(AdvanceReturnBO advanceReturn,List< AdvanceReturnTransBO> advancereturntrans);
        List<AdvanceReturnBO> GetAdvanceReturnList();
        List<AdvanceReturnBO> GetAdvanceReturnDetails(int AdvanceReturnID);
        List<AdvanceReturnTransBO> GetAdvanceReturnTransDetails(int AdvanceReturnID);

        DatatableResultBO GetAdvanceReturnList(string Type,string ARNoHint,string ARDateHint,string NameHint,string CategoryHint,string NetAmountHint,string SortField,string SortOrder,int Offset,int Limit);

    }
}
