using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class GSTCategoryDAL
    {
        public List<GSTCategoryBO> GetGSTCategoryList()
        {
            try
            {
                List<GSTCategoryBO> gstCategory = new List<GSTCategoryBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    gstCategory = dbEntity.SpGetGSTCategorylist().Select(a => new GSTCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,

                    }).ToList();

                    return gstCategory;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<GSTCategoryBO> GetTaxCategoryList()
        {
            try
            {
                List<GSTCategoryBO> gstCategory = new List<GSTCategoryBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    gstCategory = dbEntity.SPGetTaxCategory().Select(a => new GSTCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        CGSTPercent = a.CGSTPercent.HasValue ? a.CGSTPercent.Value : 0,
                        SGSTPercent = a.SGSTPercent.HasValue ? a.SGSTPercent.Value : 0,
                        IGSTPercent = a.IGSTPercent.HasValue ? a.IGSTPercent.Value : 0,
                        VATPercent = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0

                    }).ToList();

                    return gstCategory;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<GSTCategoryBO> GetTaxCategoryListByTaxType(int TaxTypeID)
        {
            try
            {
                List<GSTCategoryBO> gstCategory = new List<GSTCategoryBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    gstCategory = dbEntity.SPGetTaxCategoryByTaxTypeID(TaxTypeID).Select(a => new GSTCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        VATPercent = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,

                    }).ToList();

                    return gstCategory;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Save(GSTCategoryBO gstCategory)
        {
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {

                    return dbEntity.SpCreateGSTCategory(gstCategory.Name, gstCategory.SGSTPercent, gstCategory.CGSTPercent, gstCategory.IGSTPercent,
                        GeneralBO.CreatedUserID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public int Update(GSTCategoryBO gstCategory)
        {
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {

                    return dbEntity.SpUpdateGSTCategory(gstCategory.ID, gstCategory.Name, gstCategory.SGSTPercent, gstCategory.CGSTPercent, gstCategory.IGSTPercent,
                        GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<GSTCategoryBO> GetGSTCategoryDetails(int GSTCategoryID)
        {
            try
            {
                List<GSTCategoryBO> GSTCategory = new List<GSTCategoryBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    GSTCategory = dbEntity.SpGetGSTCategoryDetails(GSTCategoryID).Select(a => new GSTCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        IGSTPercent = (decimal)a.IGSTPercent,
                        CGSTPercent = (decimal)a.CGSTPercent,
                        SGSTPercent = (decimal)a.SGSTPercent
                    }).ToList();
                    return GSTCategory;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<GSTCategoryBO> GetGSTList()
        {
            List<GSTCategoryBO> GST = new List<GSTCategoryBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                GST = dEntity.SpGetGSTListForPurchaseReturnOrder().Select(a => new GSTCategoryBO
                {
                    ID = a.ID,
                    IGSTPercent = (decimal)a.IGSTPercent

                }).ToList();
                return GST;
            }
        }

        public List<GSTCategoryBO> GetVatPercentage()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetVatPercentage(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GSTCategoryBO
                    {
                        ID = a.ID,
                        VATPercent = (decimal)a.VATPercentage
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
