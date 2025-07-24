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
    public class SupplierAccountsCategoryDAL
    {

        public int Save(SupplierAccountsCategoryBO SupplieraccountscategoryBO)
        {

            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));

            using (MasterEntities dbEntity = new MasterEntities())
            {

                dbEntity.SpCreateSupplierAccountsCategory(
                SupplieraccountscategoryBO.Name,
                GeneralBO.CreatedUserID,
                GeneralBO.ApplicationID,
                ReturnValue
                );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Name already exists");
                }
            }
            return 1;
        }

        public List<SupplierAccountsCategoryBO> GetAllSupplierAccountsCategory()
        {
            List<SupplierAccountsCategoryBO> item = new List<SupplierAccountsCategoryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetSupplierAccountsCategory(GeneralBO.CreatedUserID, GeneralBO.ApplicationID).Select(k => new SupplierAccountsCategoryBO
                    {
                        ID = k.ID,
                        Name = k.Name

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SupplierAccountsCategoryBO GetSupplierAccountsCategoryDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplierAccountsCategoryByID(ID).Select(a => new SupplierAccountsCategoryBO()
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(SupplierAccountsCategoryBO SupplieraccountscategoryBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateSupplierAccountsCategory(SupplieraccountscategoryBO.ID, SupplieraccountscategoryBO.Name);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
