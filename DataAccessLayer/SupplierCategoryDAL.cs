using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class SupplierCategoryDAL
    {
        public int Save(SupplierCategoryBO SuppliercategoryBO)
        {

            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));

            using (MasterEntities dbEntity = new MasterEntities())
            {

                dbEntity.SpCreateSupplierCategory(
                SuppliercategoryBO.Name,
                SuppliercategoryBO.Remarks,
                GeneralBO.CreatedUserID,
                GeneralBO.ApplicationID,
                ReturnValue
                 );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Category Name already exists");
                }
            }
            return 1;
        }

        public List<SupplierCategoryBO> GetAllSupplierCategory()
        {
            List<SupplierCategoryBO> item = new List<SupplierCategoryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetSupplierCategory(GeneralBO.CreatedUserID, GeneralBO.ApplicationID).Select(k => new SupplierCategoryBO
                    {
                        ID = k.ID,
                        Name = k.Name,
                        Remarks = k.Remarks

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SupplierCategoryBO GetSupplierCategoryDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplierCategoryByID(ID).Select(a => new SupplierCategoryBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Remarks = a.Remarks
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(SupplierCategoryBO SuppliercategoryBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateSupplierCategory(SuppliercategoryBO.ID, SuppliercategoryBO.Name, SuppliercategoryBO.Remarks);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
