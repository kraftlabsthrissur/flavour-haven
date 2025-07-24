using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AddressDAL
    {
        public List<AddressBO> GetBillingAddress(string PartyType, int? PartyID, string Hint)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetBillingAddress(PartyType, PartyID, Hint).Select(a => new AddressBO
                    {
                        LocationID = a.LocationID,
                        LocationCode = a.LocationCode,
                        Location = a.Location,
                        SupplierID = a.SupplierID,
                        SupplierCode = a.SupplierCode,
                        Supplier = a.Supplier,
                        AddressID = a.AddressID,
                        Place = a.Place,
                        IsDefault = a.IsDefault,
                        StateID = a.StateID,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        PIN = a.PIN,
                        MobileNo = a.MobileNo,
                        State = a.State,
                        District = a.District,
                        Email = a.Email,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AddressBO> GetShippingAddress(string PartyType, int? PartyID, string Hint)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetShippingAddress(PartyType, PartyID, Hint).Select(a => new AddressBO
                    {
                        LocationID = a.LocationID,
                        LocationCode = a.LocationCode,
                        Location = a.Location,
                        SupplierID = a.SupplierID,
                        SupplierCode = a.SupplierCode,
                        Supplier = a.Supplier,
                        AddressID = a.AddressID,
                        Place = a.Place,
                        IsDefault = a.IsDefault,
                        StateID = a.StateID,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        IsDefaultShipping = (bool)a.IsDefaultShipping,
                        State = a.State,
                        District = a.District,
                        PIN = a.PIN,
                        MobileNo = a.MobileNo,
                        Email = a.Email,
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AddressBO> GetAddressList(int? PartyID, string PartyType)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    var AddressList = dbEntity.SpGetAddressByPartyID(PartyID, PartyType).Select(a => new AddressBO
                    {
                        AddressID = a.ID,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        ContactPerson = a.ContactPerson,
                        Place = a.Place,
                        StateID = (int)a.StateID,
                        State = a.StateName,
                        DistrictID = (int)a.DistrictID,
                        District = a.DistrictName,
                        PIN = a.PIN,
                        LandLine1 = a.LandLine1,
                        LandLine2 = a.LandLine2,
                        MobileNo = a.MobileNo,
                        Fax = a.Fax,
                        Email = a.Email,
                        IsBilling = (bool)a.IsBilling,
                        IsShipping = (bool)a.IsShipping,
                        IsDefault = (bool)a.IsDefault,
                        IsDefaultShipping = (bool)a.IsDefaultShipping

                    }).ToList();
                    return AddressList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateAddressList(List<AddressBO> _AddressList, int PartyID, string PartyType)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    if (PartyID != 0)
                    {

                        foreach (var item in _AddressList)
                        {
                            dbEntity.SpCreateAddress(PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3,0, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping, GeneralBO.CreatedUserID, DateTime.Now, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        }
                    }
                    return PartyID;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }


        public int UpdateAddressList(List<AddressBO> _AddressList, int PartyID, string PartyType)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    if (PartyID != 0)
                    {

                        foreach (var item in _AddressList)
                        {
                            dbEntity.SpUpdateAddress(item.AddressID, PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping);
                        }
                    }
                    return PartyID;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<AddressBO> GetBillingAddressLocation(int? LocationID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetBillingAddressLocation(LocationID).Select(a => new AddressBO
                    {
                        LocationID = a.LocationID,
                        LocationCode = a.LocationCode,
                        Location = a.Location,
                        SupplierID = a.SupplierID,
                        SupplierCode = a.SupplierCode,
                        Supplier = a.Supplier,
                        AddressID = a.AddressID,
                        Place = a.Place,
                        IsDefault = a.IsDefault,
                        StateID = a.StateID,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        PIN = a.PIN,
                        MobileNo = a.MobileNo,
                        State = a.State
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AddressBO> GetShippingAddressLocation(int? LocationID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetShippingAddressLocation(LocationID).Select(a => new AddressBO
                    {
                        LocationID = a.LocationID,
                        LocationCode = a.LocationCode,
                        Location = a.Location,
                        SupplierID = a.SupplierID,
                        SupplierCode = a.SupplierCode,
                        Supplier = a.Supplier,
                        AddressID = a.AddressID,
                        Place = a.Place,
                        IsDefault = a.IsDefault,
                        StateID = a.StateID,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        IsDefaultShipping = (bool)a.IsDefaultShipping
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AddressBO> GetAddress(int? AddressID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetAddress(AddressID).Select(a => new AddressBO
                    {
                        LocationID = a.LocationID,
                        LocationCode = a.LocationCode,
                        Location = a.Location,
                        SupplierID = a.SupplierID,
                        SupplierCode = a.SupplierCode,
                        Supplier = a.Supplier,
                        AddressID = a.AddressID,
                        Place = a.Place,
                        IsDefault = a.IsDefault,
                        StateID = a.StateID,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        PIN = a.PIN,
                        MobileNo = a.MobileNo,
                        State = a.State,
                        District = a.District,
                        Email = a.Email,
                        LocationGSTNo = a.LocationGSTNo,
                        SupplierGSTNo = a.SupplierGSTNo,
                        CustomerGSTNo = a.CustomerGSTNo,
                        
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
