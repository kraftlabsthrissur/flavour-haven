using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class GSTSubCategoryDAL
    {
        public List<GSTSubCategoryBO> GetGSTSubCategoryList()
        {
            try
            {
                List<GSTSubCategoryBO> gstCategory = new List<GSTSubCategoryBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetGSTSubCategoryList().Select(a => new GSTSubCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Description = a.Description,
                        Percentage = a.Percentage
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<GSTSubCategoryBO> GetGSTSubCategoryDetails(int ID)
        {
            try
            {
                List<GSTSubCategoryBO> gstCategory = new List<GSTSubCategoryBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetGSTSubCategoryDetails(ID).Select(a => new GSTSubCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Description = a.Description,
                        Percentage = a.Percentage
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int CreateGSTSubCategory(GSTSubCategoryBO subCategory)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCreateGSTSubCategory(
                        subCategory.Name,
                        subCategory.Description,
                        subCategory.Percentage
                        );
                    return 1;
                }

            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }
        public int UpdateGSTSubCategory(GSTSubCategoryBO subCategory)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpUpdateGSTSubCategory(
                        subCategory.ID,
                        subCategory.Name,
                        subCategory.Description,
                        subCategory.Percentage,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                    return 1;
                }

            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }
    }
}
