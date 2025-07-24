using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;


namespace DataAccessLayer
{
    public class MaterialPurificationDAL
    {
        public List<MaterialPurificationBO> GetMaterialPurificationProcessList()
        {
            List<MaterialPurificationBO> MaterialPurificationProcess = new List<MaterialPurificationBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    MaterialPurificationProcess = dbEntity.SpGetMaterialPurificationProcess().Select(a => new MaterialPurificationBO
                    {
                        ProcessID = a.ID,
                        ProcessName = a.Name
                    }).ToList();
                }
                return MaterialPurificationProcess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(MaterialPurificationBO materialPurificationBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateMaterialPurification(
                        materialPurificationBO.ItemID,
                        materialPurificationBO.UnitID,
                        materialPurificationBO.ProcessID,
                        materialPurificationBO.PurificationItemID,
                        materialPurificationBO.PurificationUnitID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(MaterialPurificationBO materialPurificationBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateMaterialPurification(
                        materialPurificationBO.ID,
                        materialPurificationBO.ItemID,
                        materialPurificationBO.UnitID,
                        materialPurificationBO.ProcessID,
                        materialPurificationBO.PurificationItemID,
                        materialPurificationBO.PurificationUnitID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        GeneralBO.CreatedUserID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<MaterialPurificationBO> GetMaterialPurificationList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetMaterialPurificationList(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MaterialPurificationBO()
                    {
                        ID = (int)a.ID,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        ProcessID = (int)a.ProcessID,
                        ProcessName = a.ProcessName,
                        PurificationItemID = (int)a.PurificationItemID,
                        PurificationItemName = a.PurificationItemName,
                        PurificationUnitID = (int)a.PurificationUnitID,
                        PurificationUnit = a.PurificationUnit,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MaterialPurificationBO> GetMaterialPurificationDetail(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetMaterialPurificationDetail(ID,GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new MaterialPurificationBO()
                    {
                        ID = (int)a.ID,
                        ItemCategoryID = (int)a.CategoryID,
                        CategoryName = a.CategoryName,
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        UnitID = (int)a.UnitID,
                        Unit = a.Unit,
                        ProcessID = (int)a.ProcessID,
                        ProcessName = a.ProcessName,
                        PurificationItemID = (int)a.PurificationItemID,
                        PurificationItemName = a.PurificationItemName,
                        PurificationUnitID = (int)a.PurificationUnitID,
                        PurificationUnit = a.PurificationUnit,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
