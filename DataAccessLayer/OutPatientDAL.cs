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
    public class OutPatientDAL
    {
        public int Save(CustomerBO CustomerBO)
        {

            using (MasterEntities dbEntity = new MasterEntities())
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                ObjectParameter ReturnValue = new ObjectParameter("CustomerID", typeof(int));
                if (CustomerBO.Category == "Patients")
                {
                    dbEntity.SpUpdateSerialNo(
                                          "Patients",
                                          "Code",
                                          GeneralBO.FinYear,
                                          GeneralBO.LocationID,
                                          GeneralBO.ApplicationID,
                                          SerialNo);
                }
                else if (CustomerBO.Category == "ECOMMERCE")
                {
                    dbEntity.SpUpdateSerialNo(
                                            "ECOMMERCE",
                                            "Code",
                                            GeneralBO.FinYear,
                                            GeneralBO.LocationID,
                                            GeneralBO.ApplicationID,
                                            SerialNo);
                }
                var i = dbEntity.SpCreateOutPatient(
                    SerialNo.Value.ToString(),
                    CustomerBO.Name,
                    CustomerBO.AddressLine1,
                    CustomerBO.AddressLine2,
                    CustomerBO.StateID,
                    CustomerBO.CountryID,
                    CustomerBO.DistrictID,
                    CustomerBO.DOB,
                    CustomerBO.EmailID,
                    CustomerBO.MobileNumber,
                    CustomerBO.GstNo,
                    CustomerBO.Category,
                    CustomerBO.PinCode,
                    GeneralBO.CreatedUserID,
                    GeneralBO.LocationID,
                    GeneralBO.ApplicationID,
                    ReturnValue

                 );
            }
            return 1;
        }

        public List<CustomerBO> GetOutPatientList()
        {
            using (MasterEntities dEntity = new DBContext.MasterEntities())
            {
                return dEntity.SpGetOutPatient(GeneralBO.LocationID).Select(a => new CustomerBO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Code = a.Code,
                    CategoryName = a.CategoryName,
                    IsGSTRegistered = (bool)a.IsGSTRegistered
                }).ToList();

            }
        }

        public List<CustomerBO> GetOutPatientDetails(int ID)
        {
            try
            {
                List<CustomerBO> OutPatient = new List<CustomerBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    OutPatient = dbEntity.SpGetOutPatientByID(ID).Select(a => new CustomerBO()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Category = a.Category,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        State = a.State,
                        District = a.District,
                        MobileNumber = a.MobileNo,
                        Email = a.Email,
                        GstNo = a.GSTNo,
                        DistrictID = (int)a.DistrictID,
                        StateID = (int)a.StateID,
                        CategoryID = (int)a.CategoryID,
                        DOB =(DateTime) a.DOB,
                        PinCode = a.PIN
                    }
                    ).ToList();

                    return OutPatient;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(CustomerBO CustomerBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateOutPatient(
                            CustomerBO.ID,
                            CustomerBO.Name,
                            CustomerBO.AddressLine1,
                            CustomerBO.AddressLine2,
                            CustomerBO.StateID,
                            CustomerBO.CountryID,
                            CustomerBO.DistrictID,
                            CustomerBO.DOB,
                            CustomerBO.EmailID,
                            CustomerBO.MobileNumber,
                            CustomerBO.GstNo,
                            CustomerBO.Category,
                            CustomerBO.PinCode,
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

        public DatatableResultBO GetOutPatientListForPopup(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    var result = dbEntity.SpGetOutPatientListForPopUp(CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Name = item.Name,
                                Place = item.Place,
                                Mobile = item.MobileNo,
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
    }
}
