using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class QCTestDAL
    {
        public List<QCTestBO> GetQCTestList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetQCTest().Select(a => new QCTestBO
                {
                    ID = a.ID,
                    TestName = a.Name,
                    Type = a.Type

                }).ToList();

            }
        }

        public int Save(QCTestBO QctestBO)
        {
            ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                dbEntity.SpCreateQCTest(
                QctestBO.TestName,
                QctestBO.Type,
                GeneralBO.CreatedUserID,
                GeneralBO.ApplicationID,
                ReturnValue
                 );
                if (Convert.ToInt16(ReturnValue.Value) == -1)
                {
                    throw new Exception("Already exists");
                }
            }
            return 1;
        }

        public QCTestBO GetQCTestDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetQCTestByID(ID).Select(a => new QCTestBO()
                    {
                        ID = a.ID,
                        TestName = a.Name,
                        Type = a.Type
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(QCTestBO QctestBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateQCTest(
                        QctestBO.ID,
                        QctestBO.TestName,
                        QctestBO.Type,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}
