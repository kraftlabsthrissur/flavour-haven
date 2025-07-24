//file created by prama on 7-6-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPlacesContract
    {
        bool SavePlaces(PlacesBO placesBO);
        List<PlacesBO> GetPlaces(int ID);
        List<PlacesBO> GetPatientPlace(string Hint);
        PlacesBO GetPlacesDetails(int ID);
        int UpdatePlaces(PlacesBO places);
    }
}
