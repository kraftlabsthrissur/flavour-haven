using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IContactContract
    {
      
        int CreateContact(ContactBO contactBO);
        List<ContactBO> GetContactList();
        ContactBO GetContactDetails(int ID);
        int EditContact(ContactBO contactBO);
        DatatableResultBO GetContactSearchList(int CustomerID,string firstNameHit, string lastNameHit, string phonenoHit, string alternativenoHit, string designationHit, string EmailIDHit, string SortField, string SortOrder, int Offset, int Limit);
    }
}
