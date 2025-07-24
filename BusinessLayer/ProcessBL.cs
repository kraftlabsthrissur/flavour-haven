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
    public class ProcessBL : IProcessContract
    {
        ProcessDAL processDAL;

        public ProcessBL()
        {
            processDAL = new ProcessDAL();
        }

        public int Save(ProcessBO processBO)
        {
            if (processBO.ID == 0)
            {
                return processDAL.Save(processBO);
            }
            else
            {
                return processDAL.Update(processBO);
            }
        }

        public List<ProcessBO> GetProcessList()
        {
            return processDAL.GetProcessList();
        }
        public ProcessBO GetProcessDetails(int ID)
        {
            return processDAL.GetProcessDetails(ID);
        }
    }
}
