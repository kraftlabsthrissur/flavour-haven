using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface ITreatmentRoomContract
    {
        int Save(TreatmentRoomBO treatmentRoomBO);
        DatatableResultBO GetTreatmentRoomList(string NameHint, string RemarkHint, string SortField, string SortOrder, int Offset, int Limit);
        List<TreatmentRoomBO> GetTreatmentRoomDetails(int ID);
        List<TreatmentRoomBO> GetTreatmentRoomDetailsList();
        DatatableResultBO GetTreatmentRoomAutoComplete(string Hint);

    }
}
