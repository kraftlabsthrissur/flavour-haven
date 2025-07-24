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
    public class ContactBL : IContactContract
    {
        ContactDAL contactDAL;

        public ContactBL()
        {
            contactDAL = new ContactDAL();
        }
        public int CreateContact(ContactBO contactBO)
        {
            return contactDAL.CreateContact(contactBO);
        }
        public List<ContactBO> GetContactList()
        {
            try
            {
                return contactDAL.GetContactList().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ContactBO GetContactDetails(int ID)
        {
            return contactDAL.GetContactDetails(ID);
        }
        public DatatableResultBO GetContactSearchList(int CustomerID, string firstNameHit, string lastNameHit, string phonenoHit, string alternativenoHit, string designationHit, string emailIDHit, string SortField, string SortOrder, int Offset, int Limit)
        {
            return contactDAL.GetContactSearchList(CustomerID, firstNameHit, lastNameHit, phonenoHit, alternativenoHit, designationHit, emailIDHit, SortField, SortOrder, Offset, Limit);
        }
        public int EditContact(ContactBO contactBO)
        {
            return contactDAL.UpdateContact(contactBO);
        }
    }
}
