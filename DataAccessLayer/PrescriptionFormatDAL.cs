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
    public class PrescriptionFormatDAL
    {
        public int Save(List<PrescriptionFormatItemBO> Items, PrescriptionFormatBO PrescriptionFormatBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ObjectParameter Id = new ObjectParameter("TransID", typeof(int));
                    foreach (var item in Items)
                    {

                        var i = dbEntity.SpCreatePrescriptionFormat(
                                item.MedicineCategoryID,
                                GeneralBO.CreatedUserID,
                                GeneralBO.ApplicationID,
                                    Id
                                );
                    }

                    foreach (var item in Items)
                    {

                        dbEntity.SpCreatePrescriptionFormatTrans(
                              Convert.ToInt32(Id.Value),
                              item.Prescription,
                              GeneralBO.CreatedUserID,
                              GeneralBO.ApplicationID
                          );

                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PrescriptionFormatItemBO> GetPrescriptionFormatItemList(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPrescriptionFormatItemList(ID, GeneralBO.FinYear, GeneralBO.ApplicationID).Select(a => new PrescriptionFormatItemBO()
                    {

                        MedicineCategoryID = a.MedicineCategoryID,
                        MedicineCategory = a.MedicineCategory,
                        Prescription = a.Prescription
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PrescriptionFormatBO> GetPrescriptionFormatList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetPrescriptionFormatList().Select(a => new PrescriptionFormatBO
                {
                    MedicineCategoryID = a.ID,
                    MedicineCategory = a.Name
                }).ToList();

            }
        }

        public List<PrescriptionFormatBO> GetPrescriptionFormatDetails(int ID)
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetPrescriptionFormatDetails(ID).Select(a => new PrescriptionFormatBO
                {
                    MedicineCategory = a.Name,
                    Prescription = a.Prescription
                }).ToList();

            }
        }
        public List<PrescriptionFormatItemBO> GetPrescriptionFormatDetailsTrans(int ID)
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetPrescriptionFormatDetails(ID).Select(a => new PrescriptionFormatItemBO
                {
                    MedicineCategory = a.Name,
                    Prescription = a.Prescription,

                }).ToList();

            }
        }

        public List<PrescriptionFormatBO> GetPrescription(int CategoryID)
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetPrescriptionFormat(CategoryID).Select(a => new PrescriptionFormatBO
                {
                    MedicineCategory = a.Name,
                    ID = a.ID,

                }).ToList();

            }
        }
    }
}
