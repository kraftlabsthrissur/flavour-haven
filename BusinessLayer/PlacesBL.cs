//file created by prama on 7-6-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationContractLayer;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;


namespace BusinessLayer
{
  public   class PlacesBL:IPlacesContract
    {
       PlacesDAL placesDAL;
        public PlacesBL()
        {
            placesDAL = new PlacesDAL();
        }
        public bool SavePlaces(PlacesBO placesBO)
        {
            return placesDAL.SavePlaces(placesBO);
        }
        public List<PlacesBO> GetPlaces(int ID)
        {
            return placesDAL.GetPlaces(ID);
        }
        public List<PlacesBO> GetPatientPlace(string Hint)
        {
            return placesDAL.GetPatientPlace(Hint);
        }
        public PlacesBO GetPlacesDetails(int ID)
        {
            return placesDAL.GetPlacesDetails(ID);
        }
        public int UpdatePlaces(PlacesBO places)
        {
            return placesDAL.UpdatePlaces(places);
        }

    }
}
