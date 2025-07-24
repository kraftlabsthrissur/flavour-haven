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
  public class QCTestBL: IQCTestContract
    {
        QCTestDAL QctestDAL;

        public QCTestBL()
        {
            QctestDAL = new QCTestDAL();
        }

        public List<QCTestBO> GetQCTestList()
        {
            return QctestDAL.GetQCTestList();
        }

        public int Save(QCTestBO QctestBO)
        {
            if (QctestBO.ID == 0)
            {
                return QctestDAL.Save(QctestBO);
            }
            else
            {
                return QctestDAL.Update(QctestBO);
            }
        }

        public QCTestBO GetQCTestDetails(int ID)
        {
            return QctestDAL.GetQCTestDetails(ID);
        }

     
    }
}
