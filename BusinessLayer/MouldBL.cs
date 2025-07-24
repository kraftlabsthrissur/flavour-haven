using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;


namespace BusinessLayer
{
   public class MouldBL:IMouldContract
    {
        MouldDAL mouldDAL;

        public MouldBL()
        {
            mouldDAL = new MouldDAL();
        }
        public int Save(MouldBO mouldBO, List<MouldItemBO> mouldItems, List<MouldMachinesBO> mouldmachines)
        {
            if (mouldBO.ID == 0)
            {
                return mouldDAL.Save(mouldBO, mouldItems, mouldmachines);
            }
            else
            {
                return mouldDAL.Update(mouldBO, mouldItems, mouldmachines);
            }

        }
        public List<MouldBO> GetMould()
        {
            return mouldDAL.GetMould();
        }
        public MouldBO GetMouldDetails(int MouldID)
        {
            return mouldDAL.GetMouldDetails(MouldID);
        }

        public List<MouldItemBO> GetMouldItems(int MouldID)
        {
            return mouldDAL.GetMouldItems(MouldID);
        }

        public List<MouldMachinesBO> GetMachines(int MouldID)
        {
            return mouldDAL.GetMachines(MouldID);
        }

        public DatatableResultBO GetMouldList(string CodeHint, string MouldNameHint, string ItemNameHint, string MachineNameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return mouldDAL.GetMouldList(CodeHint, MouldNameHint, ItemNameHint, MachineNameHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
