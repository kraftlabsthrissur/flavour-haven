using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class QualityCheckBL : IQualityCheckContract
    {
        QualityCheckDAL qualityCheckDAL;
        int FinYear;
        int LocationID;
        int ApplicationID;

        public QualityCheckBL()
        {
            FinYear = GeneralBO.FinYear;
            LocationID = GeneralBO.LocationID;
            ApplicationID = GeneralBO.ApplicationID;
            qualityCheckDAL = new QualityCheckDAL();
        }

        public List<QCItemBO> GetQCItemDetailsByID(int ID)
        {
            try
            {
                return qualityCheckDAL.GetQCItemDetailsByID(ID, FinYear, LocationID, ApplicationID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QCItemBO> GetQCList(string Status, int Offset, int Limit)
        {
            try
            {
                return qualityCheckDAL.GetQCList(Status, FinYear, LocationID, ApplicationID, Offset, Limit).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QCTestBO> GetQCTestDetailsByID(int ID, string Type)
        {
            try
            {
                return qualityCheckDAL.GetQCTestDetailsByID(ID, Type, FinYear, LocationID, ApplicationID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateQC(QCItemBO QCItem, List<QCTestBO> QCTestResults)
        {
            return qualityCheckDAL.UpdateQC(QCItem, QCTestResults, FinYear, LocationID, ApplicationID);
        }

        public DatatableResultBO GetQualityCheckList(string Type, string TransNoHint, string TransDateHint, string GRNNoHint, string ReceiptDateHint, string ItemNameHint, string UnitNameHint, string SupplierNameHint, string DeliveryChallanNoHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return qualityCheckDAL.GetQualityCheckList(Type, TransNoHint, TransDateHint, GRNNoHint, ReceiptDateHint, ItemNameHint, UnitNameHint, SupplierNameHint, DeliveryChallanNoHint, SortField, SortOrder, Offset, Limit);
        }
    }
}
