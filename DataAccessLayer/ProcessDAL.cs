using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class ProcessDAL
    {
        public int Save(ProcessBO processBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateProcess(
                processBO.Code,
                processBO.Process,
                GeneralBO.CreatedUserID,
                GeneralBO.ApplicationID,
                GeneralBO.LocationID,
                ReturnValue
                 );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

        public List<ProcessBO> GetProcessList()
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.SpGetProcess().Select(a => new ProcessBO
                {
                    ID = a.ID,
                    Process = a.Process,
                    Code = a.Code
                }).ToList();

            }
        }

        public ProcessBO GetProcessDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetProcessByID(ID).Select(a => new ProcessBO()
                    {
                        ID = a.ID,
                        Process = a.Process,
                        Code = a.Code
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(ProcessBO processBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                   return dbEntity.SpUpdateProcess(
                    processBO.ID,
                    processBO.Process,
                    processBO.Code
                    
                     );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
