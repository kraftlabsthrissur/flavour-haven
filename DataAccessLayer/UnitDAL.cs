using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class UnitDAL
    {
        public List<UnitBO> GetUnitList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetUnitList().Select(a => new UnitBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        QOM = a.QOM,
                        UOM = a.UOM,
                        CF = a.CF,
                        Value = a.ID + "#" + a.QOM,
                        PackSize = (decimal)a.PackSize
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<UnitBO> GetUnitGroupList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetUnitGroupList().Select(a => new UnitBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public UnitBO GetUnitDetails(int UnitID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetUnit(UnitID).Select(a => new UnitBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        QOM = a.QOM,
                        UOM = a.UOM,
                        CF = a.CF,
                        PackSize = (decimal)a.PackSize
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<UnitBO> GetUnitsByItemID(int ItemID, string Type)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetUnitByItemID(ItemID, Type, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new UnitBO
                    {
                        ID = (int)a.UnitID,
                        Name = a.Unit
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CreateUnit(UnitBO unitBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SPCreateUnit(
                unitBO.Name,
                unitBO.QOM,
                unitBO.UOM,
                unitBO.CF,
                unitBO.PackSize,
                GeneralBO.CreatedUserID,
                DateTime.Now,
                ReturnValue
         );

                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }



        public int UpdateUnit(UnitBO unitBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateUnit(unitBO.ID, unitBO.Name, unitBO.QOM, unitBO.UOM, unitBO.CF, unitBO.PackSize, GeneralBO.CreatedUserID, DateTime.Now,
                        GeneralBO.LocationID, GeneralBO.ApplicationID);

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<UnitBO> GetUnitsList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetUnitsList().Select(a => new UnitBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        QOM = a.QOM,
                        UOM = a.UOM,
                        CF = a.CF,
                        Value = a.ID + "#" + a.QOM,
                        PackSize = (decimal)a.packsize
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UnitBO> GetUnitListForAPI(int ItemID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetUnitListForAPI(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new UnitBO
                    {
                        UnitID = a.UnitID,
                        Unit = a.Unit,
                        PackSize = (decimal)a.PackSize,
                        Description = a.Description
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SecondaryUnitBO> GeSecondarytUnitListByUnitID(int UnitID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetSecondarytUnitListByUnitID(UnitID).Select(a => new SecondaryUnitBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        PackSize = (decimal)a.PackSize
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SecondaryUnitBO> GetSecondaryUnitList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetSecondarytUnitList().Select(a => new SecondaryUnitBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        UnitID = a.UnitID,
                        UnitGroup = a.UnitGroupName,
                        Unit = a.UnitName,
                        UnitGroupID = a.UnitGroupID,
                        PackSize = (decimal)a.PackSize
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public SecondaryUnitBO GetSecondaryUnitDetails(int SecondaryUnitID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSecondaryUnit(SecondaryUnitID).Select(a => new SecondaryUnitBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        UnitGroupID = a.UnitGroupID,
                        UnitGroup = a.UnitGroupName,
                        UnitID = a.UnitID,
                        Unit = a.UnitName,
                        PackSize = (decimal)a.PackSize
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int CreateSecondaryUnit(SecondaryUnitBO secondaryUnitBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SPCreateSecondaryUnit(
                secondaryUnitBO.Name,
                secondaryUnitBO.UnitID,
                secondaryUnitBO.PackSize,
                secondaryUnitBO.UnitGroupID,
                GeneralBO.CreatedUserID,
                DateTime.Now,
                ReturnValue
                );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

        public int UpdateSecondaryUnit(SecondaryUnitBO unitBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateSecondaryUnit(unitBO.ID, unitBO.Name, unitBO.UnitID, unitBO.UnitGroupID,unitBO.PackSize, GeneralBO.CreatedUserID, DateTime.Now,
                        GeneralBO.LocationID, GeneralBO.ApplicationID);

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
