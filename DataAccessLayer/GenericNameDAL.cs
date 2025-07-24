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
   public class GenericNameDAL
    {
        public int Save(GenericNameBO GenericName)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "GenericName";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    return dbEntity.SpCreateGenericName(GenericName.Name, GenericName.Code, GenericName.Description,
                    GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public List<GenericNameBO> GetGenericNameList()
        {
            try
            {
                List<GenericNameBO> GenericName = new List<GenericNameBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    GenericName = dbEntity.SpGetGenericNameList().Select(a => new GenericNameBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code
                    }).ToList();

                    return GenericName;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<GenericNameBO> GetGenericNameDetails(int ID)
        {
            try
            {
                List<GenericNameBO> GenericName = new List<GenericNameBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    GenericName = dbEntity.SpGetGenericNameDetails(ID,GeneralBO.ApplicationID).Select(a => new GenericNameBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Description = a.Description
                    }).ToList();
                    return GenericName;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int Update(GenericNameBO GenericName)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateGenericName(GenericName.ID, GenericName.Name, GenericName.Code, GenericName.Description);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
