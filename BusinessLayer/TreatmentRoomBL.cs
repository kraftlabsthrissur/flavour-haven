using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public class TreatmentRoomBL : ITreatmentRoomContract
    {
        TreatmentRoomDAL treatmentRoomDAL;

        public TreatmentRoomBL()
        {
            treatmentRoomDAL = new TreatmentRoomDAL();
        }

        public int Save(TreatmentRoomBO treatmentRoomBO)
        {
            if (treatmentRoomBO.ID == 0)
            {
                return treatmentRoomDAL.Save(treatmentRoomBO);
            }
            else
            {
                return treatmentRoomDAL.Update(treatmentRoomBO);
            }
        }

        public DatatableResultBO GetTreatmentRoomList(string NameHint, string RemarkHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return treatmentRoomDAL.GetTreatmentRoomList(NameHint, RemarkHint, SortField, SortOrder, Offset, Limit);
        }

        public List<TreatmentRoomBO> GetTreatmentRoomDetails(int ID)
        {
            return treatmentRoomDAL.GetTreatmentRoomDetails(ID);
        }

        public List<TreatmentRoomBO> GetTreatmentRoomDetailsList()
        {
            return treatmentRoomDAL.GetTreatmentRoomDetailsList();
        }
        public DatatableResultBO GetTreatmentRoomAutoComplete(string Hint)
        {
            return treatmentRoomDAL.GetTreatmentRoomAutoComplete(Hint);
        }
    }
}
