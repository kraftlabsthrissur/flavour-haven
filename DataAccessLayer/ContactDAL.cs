using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class ContactDAL
    {
        public int CreateContact(ContactBO contactBO)
        {
            try
            {
                ObjectParameter IDout = new ObjectParameter("ID", typeof(int));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.spCreateContact(contactBO.Firstname, contactBO.Lastname, contactBO.ContactNo, contactBO.AlternateNo, contactBO.Email, contactBO.Address1, contactBO.Address2, contactBO.Address3, contactBO.Designation, contactBO.CustomerID, Convert.ToInt32(contactBO.IsActive), GeneralBO.CreatedUserID, IDout);
                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<ContactBO> GetContactList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SPGetContactList().Select(a => new ContactBO
                    {
                        ID = a.ID,
                        Firstname = a.Name1,
                        Lastname = a.Name2,
                        Designation = a.Designation,
                        ContactNo = a.PhoneNo
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public DatatableResultBO GetContactSearchList(int CustomerID, string firstNameHit, string lastNameHit, string phonenoHit, string alternativenoHit, string designationHit, string EmailID, string SortField, string SortOrder, int Offset, int Limit)
        {
            try
            {
                DatatableResultBO DatatableResult = new DatatableResultBO();
                DatatableResult.data = new List<object>();
                using (MasterEntities db = new MasterEntities())
                {
                    var result = db.SpGetContactSearchList(CustomerID, firstNameHit, lastNameHit, phonenoHit, alternativenoHit, designationHit, EmailID, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();

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
                                Firstname = item.Name1,
                                Lastname = item.Name2,
                                ContactNo = item.PhoneNo,
                                AlternateNo = item.AlternateNo,
                                Designation = item.Designation,
                                Email = item.EmailID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
                return DatatableResult;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ContactBO GetContactDetails(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetContactByID(ID).Select(a => new ContactBO
                    {
                        ID = a.ID,
                        Firstname = a.Name1,
                        Lastname = a.Name2,
                        ContactNo = a.PhoneNo,
                        AlternateNo = a.AlternateNo,
                        Email = a.EmailID,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        Address3 = a.Address3,
                        Designation = a.Designation,
                        Company = a.contact,
                        CustomerID = (int)a.CustomerID,
                        IsActive = Convert.ToBoolean(a.isActive)
                    }
                    ).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int UpdateContact(ContactBO contactBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateContact(contactBO.ID, contactBO.Firstname, contactBO.Lastname, contactBO.ContactNo, contactBO.AlternateNo, contactBO.Email, contactBO.Address1, contactBO.Address2, contactBO.Address3, contactBO.Designation, contactBO.CustomerID, Convert.ToInt32(contactBO.IsActive), GeneralBO.CreatedUserID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
