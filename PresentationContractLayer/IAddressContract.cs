using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IAddressContract
    {
        List<AddressBO> GetShippingAddress(string PartyType, int? PartyID, string Hint);
        List<AddressBO> GetBillingAddress(string PartyType, int? PartyID, string Hint);
        List<AddressBO> GetAddressByPartyType(int? PartyID, string PartyType);
        AddressBO GetAddress(int AddressID);
        int CreateAddressList(List<AddressBO> _AddressList, int PartyID, string PartyType);
        int UpdateAddressList(List<AddressBO> _AddressList, int PartyID, string PartyType);
        List<AddressBO> GetShippingAddressLocation(int? LocationID);
        List<AddressBO> GetBillingAddressLocation(int? LocationID);
        List<AddressBO> GetAddresses(int? AddressID);
    }
}
