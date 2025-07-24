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
    public class DiscountCategoryDAL
    {
        public int Save(DiscountCategoryBO discountCategoryBO)
        {
            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCreateDiscountCategory(
                    discountCategoryBO.DiscountPercentage,
                    discountCategoryBO.DiscountType,
                    discountCategoryBO.Days,
                    GeneralBO.ApplicationID,
                    ReturnValue
                     );
                    if (Convert.ToInt16(ReturnValue.Value) == -1)
                    {
                        throw new Exception("Already exists");
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(DiscountCategoryBO discountCategoryBO)
        {
            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                     dbEntity.SpUpdateDiscountCategory(
                        discountCategoryBO.ID,
                        discountCategoryBO.DiscountPercentage,
                        discountCategoryBO.DiscountType,
                        discountCategoryBO.Days,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID,
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DiscountCategoryBO> GetDiscountCategoryList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetDiscountCategoryList(GeneralBO.ApplicationID).Select(a => new DiscountCategoryBO
                {
                    ID = (int)a.ID,
                    DiscountPercentage = (decimal)a.DiscountPercentage,
                    DiscountType = a.DiscountType,
                    Days = (int)a.DiscountDays
                }).ToList();

            }
        }
        public List<DiscountCategoryBO> GetDiscountList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetDiscountCategoryLists().Select(a => new DiscountCategoryBO
                {
                    DiscountPercentage = (decimal)a.DiscountPercentage,
                    DiscountType = a.DiscountType,
                    Days = (int)a.DiscountDays,
                    DiscountCategory=a.DiscountCategory,
                    ID=a.ID
                }).ToList();

            }
        }

        public DiscountCategoryBO GetDiscountCategoryDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDiscountCategoryByID(ID,GeneralBO.ApplicationID).Select(a => new DiscountCategoryBO()
                    {
                       ID = (int)a.ID,
                       DiscountPercentage= (decimal)a.DiscountPercentage,
                       DiscountType=a.DiscountType,
                       Days= (int)a.DiscountDays
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
