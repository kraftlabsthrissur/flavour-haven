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
  public class TreatmentDAL
    {
        public int Save(TreatmentListBO treatment)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Treatment";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        return dbEntity.SpCreateTreatment(treatment.TreatmentCode,treatment.TreatmentName,treatment.TreatmentGroupID,treatment.AddedDate,
                            treatment.Description,GeneralBO.CreatedUserID,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<TreatmentListBO> GetAllTreatment()
        {
            try
            {
                List<TreatmentListBO> Treatment = new List<TreatmentListBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Treatment = dbEntity.SpGetTreatmentList().Select(a => new TreatmentListBO
                    {
                        ID = a.ID,
                        TreatmentName = a.Name,
                        TreatmentCode = a.Code
                    }).ToList();

                    return Treatment;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TreatmentListBO> GetTreatmentDetails(int ID)
        {
            try
            {
                List<TreatmentListBO> Treatment = new List<TreatmentListBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Treatment = dbEntity.SpGetTreatmentDetails(ID).Select(a => new TreatmentListBO
                    {
                        ID = a.ID,
                        TreatmentCode=a.Code,
                        TreatmentName=a.Name,
                        AddedDate=(DateTime)a.AddedDate,
                        Description=a.Description
                    }).ToList();
                    return Treatment;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int Update(TreatmentListBO treatment)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                        return dbEntity.SpUpdateTreatment(treatment.ID, treatment.TreatmentCode, treatment.TreatmentName, treatment.TreatmentGroupID, treatment.AddedDate, treatment.Description,
                             GeneralBO.CreatedUserID,GeneralBO.LocationID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public DatatableResultBO GetTreatmentAutoComplete(string Hint)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetAllTreatmentList(Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DatatableResult;
        }
    }
}
