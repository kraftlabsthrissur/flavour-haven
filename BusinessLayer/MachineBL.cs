using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
   public class MachineBL :IMachineContract
    {
        MachineDAL machineDAL;
        public MachineBL()
        {
            machineDAL = new MachineDAL();
        }
        public  int SaveMachine(MachineBO machine)
        {
            return machineDAL.SaveMachine(machine);
        }
        public int UpdateMachineDetails(MachineBO machine)
        {
            return machineDAL.UpdateMachineDetails(machine);
        }
        public List<MachineBO> GetMachineList()
        {
            return machineDAL.GetMachineList();
        }
        public List<MachineBO> GetMachineDetails(int id)
        {
            return machineDAL.GetMachineDetails(id);
        }
        public DatatableResultBO GetAllMachineList(string MachineCodeHint,string MachineNameHint,string LoadedMouldHint, string SortField, string SortOrder,int Offset,int Limit)
        {
            return machineDAL.GetAllMachineList(MachineCodeHint, MachineNameHint, LoadedMouldHint, SortField, SortOrder, Offset, Limit);
        }


    }
}
