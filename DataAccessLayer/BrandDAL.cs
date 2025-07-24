using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class BrandDAL
    {
        public IList<BrandBO> GetBrandList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SPGetBrandList().Select(a => new BrandBO
                    {
                        Id = a.ID,
                        BrandId = a.BrandId,
                        Code = a.Code,
                        BrandName=a.BrandName,
                       //Path=a.Path

                        

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int CreateBrand(BrandBO brandBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.spCreateBrand(brandBO.Id, brandBO.BrandId,brandBO.Code, brandBO.BrandName, brandBO.Path,brandBO.image);
                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BrandBO GetBrandDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetBrandByID(ID).Select(a => new BrandBO
                    {
                        Id = a.ID,
                        BrandId=a.BrandID,
                        Code = a.Code,
                        BrandName=a.BrandName,
                        Path=a.Path,
                        image=a.image
                      
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int UpdateBrand(BrandBO BrandBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateBrand(BrandBO.Id, BrandBO.BrandId, BrandBO.Code, BrandBO.BrandName, BrandBO.Path,BrandBO.image, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
