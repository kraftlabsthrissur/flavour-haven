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
    public  class StateDAL
    {
        public List<StateBO> GetStateList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetStateList().Select(a => new StateBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        GstState = a.GstState,
                        CountryID = a.CountryID
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<StateBO> GetStateListCountryWise(int CountryID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetStateByCountry(CountryID).Select(a => new StateBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        GstState = a.GstState,
                        CountryID = a.CountryID
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public StateBO GetStateDetails(int StateID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetState(StateID).Select(a => new StateBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        GstState = a.GstState,
                        CountryID = a.CountryID,
                        Country  = a.Country
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int UpdateState(StateBO stateBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateState(stateBO.ID, stateBO.Name,stateBO.GstState,stateBO.CountryID,GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);
                   
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int CreateState(StateBO stateBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                 dbEntity.SpCreateState(
                      stateBO.Name,
                      stateBO.GstState,
                      stateBO.CountryID,
                      GeneralBO.CreatedUserID,
                      ReturnValue
                      );

                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

       
    }
}
