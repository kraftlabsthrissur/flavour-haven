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
    public class QCTestDefinitionBL : IQcTestDefinitionContract
    {
        QCTestDefinitionDAL QCTestDefinitionDAL;

        public QCTestDefinitionBL()
        {
            QCTestDefinitionDAL = new QCTestDefinitionDAL();
        }

        public List<QCTestBO> GetQCTestDefinitionList()
        {
            return QCTestDefinitionDAL.GetQCTestDefinitionList();
        }

        public List<QCTestBO> GetQCList()
        {
            return QCTestDefinitionDAL.GetQCList();
        }

        public int Save(List<QCTestItemBO> Items, QCTestBO QCTestBO)
        {
            return QCTestDefinitionDAL.Save(Items, QCTestBO);
        }

        public List<QCTestItemBO> GetTestForItemList(int ItemID)
        {
            return QCTestDefinitionDAL.GetTestForItemList(ItemID);
        }

        public List<QCTestBO> GetQCDefinitionDetails(int ID)
        {
            return QCTestDefinitionDAL.GetQCDefinitionDetails(ID);
        }

        public List<QCTestItemBO> GetQCDefinitionTransDetails(int ID)
        {
            return QCTestDefinitionDAL.GetQCDefinitionTransDetails(ID);
        }

        public bool IsDeletable(int QCTestID, int ID, int ItemID)
        {
            return QCTestDefinitionDAL.IsDeletable(QCTestID, ID, ItemID);
        }

        public DatatableResultBO GetQcTestDefinitionList(string Code,string ItemName, string TestName, string SortField, string SortOrder, int Offset, int Limit)
        {
            return QCTestDefinitionDAL.GetQcTestDefinitionList(Code,ItemName, TestName,SortField,SortOrder,Offset,Limit);
        }
    }
}

