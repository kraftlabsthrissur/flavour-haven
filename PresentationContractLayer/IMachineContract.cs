using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
   public interface IMachineContract
    {
        int SaveMachine(MachineBO machine);
        int UpdateMachineDetails(MachineBO machine);
        List<MachineBO> GetMachineList();
        List<MachineBO> GetMachineDetails(int id);
        DatatableResultBO GetAllMachineList(string MachineCodeHint, string MachineNameHint, string LoadedMouldHint, string SortField, string SortOrder, int Offset, int Limit);

    }
}
