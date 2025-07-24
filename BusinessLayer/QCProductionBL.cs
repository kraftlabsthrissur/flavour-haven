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
    public class QCProductionBL : IQCProductionContract
    {
        QCProductionDAL qcProductionDAL;
      
        public QCProductionBL()
        {
            qcProductionDAL = new QCProductionDAL();
        }

        public List<QCItemBO> GetQCItemDetails(int ID)
        {
            try
            {
                return qcProductionDAL.GetQCItemDetails(ID);
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
                return qcProductionDAL.GetQCList(Status, Offset, Limit);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QCTestBO> GetQCTestDetails(int ID, string Type)
        {
            try
            {
                return qcProductionDAL.GetQCTestDetails(ID, Type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateQC( QCItemBO QCItem, List<QCTestBO> QCTestResults)
        {
            return qcProductionDAL.UpdateQC(QCItem, QCTestResults);
        }
        public DatatableResultBO GetQCList(string Type, string ProductionNoHint, string ReceiptDateHint, string ItemHint, string BatchNoHint, string UnitHint, string AcceptedQuantityHint, string ApprovedQuantityHint, string BatchsizeHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return qcProductionDAL.GetQCList(Type, ProductionNoHint, ReceiptDateHint, ItemHint, BatchNoHint, UnitHint, AcceptedQuantityHint, ApprovedQuantityHint, BatchsizeHint, SortField, SortOrder, Offset, Limit);
        }

    }
}
