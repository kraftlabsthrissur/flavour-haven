using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class PatientDAL
    {
        public DatatableResultBO GetPatientList(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetPatientList(CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.PatientName,
                                Place=item.Place,
                                Mobile=item.Mob,
                                DoctorName=item.DoctorName
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public int Save(PatientBO patientBO)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        ObjectParameter PatientId = new ObjectParameter("PatientID", typeof(int));
                        var j = dEntity.SpUpdateSerialNo(
                                        "Patient",
                                        "Code",
                                        GeneralBO.FinYear,
                                        GeneralBO.LocationID,
                                        GeneralBO.ApplicationID,
                                        SerialNo);

                        var i = dEntity.SpCreatePatient(
                            SerialNo.Value.ToString(),
                            patientBO.Name,
                            patientBO.Age,
                            patientBO.Sex,
                            patientBO.DOB,
                            patientBO.Address1,
                            patientBO.Address2,
                            patientBO.Place,
                            patientBO.Email,
                            patientBO.Mobile,
                            patientBO.PinCode,
                            patientBO.DoctorID,
                            patientBO.DoctorName,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            PatientId
                             );
                        transaction.Commit();
                        return (int)PatientId.Value;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<PatientBO> GetPatientDetails(int ID)
        {
            try
            {
                List<PatientBO> Patient= new List<PatientBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Patient = dbEntity.SpGetPatientByID(ID).Select(a => new PatientBO()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.PatientName,
                        Age=(int)a.Age,
                        Sex=a.Sex,
                        DOB=a.DOB,
                        Address1=a.Address1,
                        Address2=a.Address2,
                        Place=a.Place,
                        Email=a.Email,
                        Mobile=a.Mob,
                        PinCode=a.PinCode,
                        DoctorID=(int)a.DoctorId,
                        DoctorName = a.DoctorName
                    }
                    ).ToList();

                    return Patient;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(PatientBO patientBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdatePatient(
                            patientBO.ID,
                            patientBO.Code,
                            patientBO.Name,
                            patientBO.Age,
                            patientBO.Sex,
                            patientBO.DOB,
                            patientBO.Address1,
                            patientBO.Address2,
                            patientBO.Place,
                            patientBO.Email,
                            patientBO.Mobile,
                            patientBO.PinCode,
                            patientBO.DoctorID,
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
