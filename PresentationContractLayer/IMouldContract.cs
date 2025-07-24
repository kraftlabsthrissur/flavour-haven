using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;


namespace PresentationContractLayer
{
   public interface IMouldContract
    {
        int Save(MouldBO mouldBO, List<MouldItemBO> mouldItems,List<MouldMachinesBO>mouldmachines);
        List<MouldBO> GetMould();
        MouldBO GetMouldDetails(int MouldID);
        List<MouldItemBO> GetMouldItems(int MouldID);
        List<MouldMachinesBO> GetMachines(int MouldID);
        DatatableResultBO GetMouldList(string CodeHint, string MouldNameHint, string ItemNameHint, string MachineNameHint, string SortField, string SortOrder, int Offset, int Limit);
    }
}
