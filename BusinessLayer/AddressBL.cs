using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;

namespace BusinessLayer
{
    public class AddressBL : IAddressContract
    {
        AddressDAL addressDAL;
        public AddressBL()
        {
            addressDAL = new AddressDAL();
        }

        public List<AddressBO> GetBillingAddress(string PartyType, int? PartyID, string Hint)
        {
            return addressDAL.GetBillingAddress(PartyType, PartyID, Hint);
        }

        public List<AddressBO> GetShippingAddress(string PartyType, int? PartyID, string Hint)
        {
            return addressDAL.GetShippingAddress(PartyType, PartyID, Hint);
        }

        public List<AddressBO> GetAddressByPartyType(int? PartyID, string PartyType)
        {
            return addressDAL.GetAddressList(PartyID, PartyType);
        }

        public AddressBO GetAddress(int AddressID)
        {
            throw new NotImplementedException();
        }

        public int CreateAddressList(List<AddressBO> _AddressList, int PartyID, string PartyType)
        {
            return addressDAL.CreateAddressList(_AddressList, PartyID, PartyType);
        }

        public int UpdateAddressList(List<AddressBO> _AddressList, int PartyID, string PartyType)
        {
            return addressDAL.UpdateAddressList(_AddressList, PartyID, PartyType);
        }

        public List<AddressBO> GetBillingAddressLocation(int? LocationID)
        {
            return addressDAL.GetShippingAddressLocation(LocationID);
        }

        public List<AddressBO> GetShippingAddressLocation(int? LocationID)
        {
            return addressDAL.GetShippingAddressLocation(LocationID);
        }

        public List<AddressBO> GetAddresses(int? AddressID)
        {
            return addressDAL.GetAddress(AddressID);
        }
    }
}
