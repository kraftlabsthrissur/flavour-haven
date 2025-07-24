//File crteated by prama on 7-6-18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class PlacesDAL
    {
        public bool SavePlaces(PlacesBO placesBO)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {

                try
                {
                    dEntity.SpCreatePlaces(
                       placesBO.Code, 
                       placesBO.Name, placesBO.Address, placesBO.DistrictID, placesBO.StateID, placesBO.CountryID);
                    return true;

                }
                catch (Exception e)
                {
                    return false;

                }
            }
        }

        public List<PlacesBO> GetPlaces(int ID)
        {
            List<PlacesBO> Batch = new List<PlacesBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Batch = dEntity.spGetPlaces(ID).Select(a => new PlacesBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name
                }).ToList();
                return Batch;
            }

        }

        public List<PlacesBO> GetPatientPlace(string Hint)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.spGetPatientPlaces(Hint).Select(a => new PlacesBO
                    {
                        ID = a.ID,
                        Place=a.Place

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PlacesBO GetPlacesDetails(int ID)
        {
            PlacesBO Places = new PlacesBO();
            using (MasterEntities dEntity = new MasterEntities())
            {
                return dEntity.spGetPlaces(ID).Select(a => new PlacesBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    DistrictID = (int)a.DistrictID,
                    StateID = (int)a.StateID,
                    CountryID = (int)a.CountryID,
                    Address = a.Address,
                    District = a.District,
                    State = a.State,
                    Country = a.Country
                }).FirstOrDefault();
            }

        }
        public int UpdatePlaces(PlacesBO placesBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePlaces(placesBO.ID, placesBO.Code, placesBO.Name, placesBO.Address, placesBO.DistrictID, placesBO.StateID, placesBO.CountryID,
                        GeneralBO.CreatedUserID,GeneralBO.LocationID,GeneralBO.ApplicationID);

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}

