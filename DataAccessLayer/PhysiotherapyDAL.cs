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
   public class PhysiotherapyDAL
    {
        public PhysiotherapyTestBO GetPhysiotherapyCategory()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPhysiotherapyCategory().Select(a => new PhysiotherapyTestBO()
                    {
                        CategoryID = (int)a.CategoryID,
                        ItemUnitID = (int)a.UnitID
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Save(PhysiotherapyTestBO Physiotherapy)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Physiotherapy";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    return dbEntity.SpCreatePhysiotherapy(Physiotherapy.Code, Physiotherapy.Name, Physiotherapy.AddedDate, Physiotherapy.Description, Physiotherapy.CategoryID
                        , Physiotherapy.PurchaseCategoryID, Physiotherapy.QCCategoryID, Physiotherapy.GSTSubCategoryID, Physiotherapy.SalesCategoryID, Physiotherapy.SalesIncentiveCategoryID,
                        Physiotherapy.StorageCategoryID, Physiotherapy.ItemTypeID, Physiotherapy.AccountsCategoryID, Physiotherapy.BusinessCategoryID, Physiotherapy.ItemUnitID,
                        GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<PhysiotherapyTestBO> GetPhysiotherapyList()
        {
            try
            {
                List<PhysiotherapyTestBO> physiotherapy = new List<PhysiotherapyTestBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    physiotherapy = dbEntity.SpGetPhysiotherapyList().Select(a => new PhysiotherapyTestBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name
                    }).ToList();

                    return physiotherapy;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PhysiotherapyTestBO> GetPhysiotherapyDetailsByID(int ID)
        {
            try
            {
                List<PhysiotherapyTestBO> physiotherapy = new List<PhysiotherapyTestBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    physiotherapy = dbEntity.SpGetPhysiotherapyDetailsByID(ID).Select(a => new PhysiotherapyTestBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Description = a.Description,
                        AddedDate = (DateTime)a.Date
                    }).ToList();
                    return physiotherapy;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public int Update(PhysiotherapyTestBO physiotherapy)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePhysiotherapy(physiotherapy.ID, physiotherapy.Code, physiotherapy.Name,
                        physiotherapy.AddedDate, physiotherapy.Description, GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
