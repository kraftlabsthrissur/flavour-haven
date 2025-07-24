using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DBContext;
using BusinessObject;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class DistrictDAL
    {
        public List<DistrictBO> GetDistrictList(int StateID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDistrictList(StateID).Select(a => new DistrictBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        StateID = (int)a.StateID,
                        PIN = a.PIN,
                        OfficeName = a.OfficeName,
                        Taluk = a.Taluk,
                        StateName = a.StateName,
                        CountryID = (int)a.CountryID
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DistrictBO> GetDistrict()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.spGetDistrict().Select(a => new DistrictBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateDistrict(DistrictBO districtBO)
        {
            try
            {
                ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpCreateDistrict(
                       districtBO.Name,
                       districtBO.StateID,
                       districtBO.PIN,
                       districtBO.OfficeName,
                       districtBO.Taluk,
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

        public List<StateBO> GetStateName()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetStateList().Select(a => new StateBO   ///TO DO
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

        public DistrictBO GetDistrictDetails(int DistrictID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDistrictByID(DistrictID).Select(a => new DistrictBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        StateID = (int)a.StateID,
                        StateName = a.StateName,
                        OfficeName = a.OfficeName,
                        PIN = a.PIN,
                        Taluk = a.Taluk
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateDistrict(DistrictBO districtBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateDistrict(districtBO.ID, districtBO.Name, districtBO.StateID, districtBO.OfficeName, districtBO.PIN, districtBO.Taluk,
                        GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
