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
    public class XrayTestDAL
    {
        public XrayTestBO GetXrayCategory()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetXrayCategory().Select(a => new XrayTestBO()
                    {
                        CategoryID = (int)a.CategoryID,
                        ItemUnitID = (int)a.UnitID
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Save(XrayTestBO xray)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Xray";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    return dbEntity.SpCreateXray(xray.Code, xray.Name, xray.AddedDate, xray.Description, xray.CategoryID
                        , xray.PurchaseCategoryID, xray.QCCategoryID, xray.GSTSubCategoryID, xray.SalesCategoryID, xray.SalesIncentiveCategoryID,
                        xray.StorageCategoryID, xray.ItemTypeID, xray.AccountsCategoryID, xray.BusinessCategoryID, xray.ItemUnitID,
                        GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<XrayTestBO> GetXrayTestList()
        {
            try
            {
                List<XrayTestBO> xray = new List<XrayTestBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    xray = dbEntity.SpGetXrayTestList().Select(a => new XrayTestBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name
                    }).ToList();

                    return xray;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<XrayTestBO> GetXrayDetailsByID(int ID)
        {
            try
            {
                List<XrayTestBO> xray = new List<XrayTestBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    xray = dbEntity.SpGetXrayDetailsByID(ID).Select(a => new XrayTestBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Description = a.Description,
                        AddedDate = (DateTime)a.Date
                    }).ToList();
                    return xray;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int Update(XrayTestBO xray)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateXray(xray.ID, xray.Code, xray.Name,
                        xray.AddedDate, xray.Description, GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


    }
}
