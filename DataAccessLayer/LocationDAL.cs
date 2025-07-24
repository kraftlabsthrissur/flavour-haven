//File created by prama on 14-4-18
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

    public class LocationDAL
    {
        public List<LocationBO> GetLocationList()
        {
            List<LocationBO> Location = new List<LocationBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Location = dEntity.SpGetLocationList().Select(a => new LocationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    LocationType = a.LocationType,
                    LocationHead = a.LocationHead,
                    Place = a.Place,
                    LocationHeadID = (int)a.LocationHeadID,
                    CountryName = a.CountryName,
                    CurrencyName = a.CurrencyName,
                }).ToList();
                return Location;
            }
        }

        public List<LocationBO> GetTransferableLocationList()
        {
            List<LocationBO> Location = new List<LocationBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Location = dEntity.SpGetTransferableLocationList(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new LocationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    LocationType = a.LocationType,
                    LocationHead = a.LocationHead,
                    Place = a.Place,
                    LocationHeadID = (int)a.LocationHeadID,
                    StateID = (int)a.StateID
                }).ToList();
                return Location;
            }

        }

        public List<LocationBO> GetLocationListByUser(int UserID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetLocationsByUser(UserID).Select(Location => new LocationBO
                    {
                        ID = Location.ID,
                        LocationType = Location.LocationType,
                        Code = Location.Code,
                        Name = Location.Name,
                        Place = Location.Place
                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetCurrentLocation(int UserID, int LocationID)
        {

            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpSetCurrentLocation(UserID, LocationID);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsBranchLocation(int LocationID)
        {
            bool IsBranch = false;
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    IsBranch = dbEntity.SpIsBranchLocation(LocationID, GeneralBO.ApplicationID).First().Value;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsBranch;
        }

        public List<LocationBO> GetLocationDetails(int LocationID)
        {
            List<LocationBO> Location = new List<LocationBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Location = dEntity.SpGetLocationDetails(LocationID).Select(a => new LocationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    Place = a.Place,
                    LocationStateID = (int)a.StateID,
                    LocationState = a.State,
                    LocationTypeID = (int)a.LocationGroupID,
                    LocationType = a.LocationType,
                    StartDate = (DateTime)a.StartDate,
                    EndDate = (DateTime)a.EndDate,
                    CompanyName = a.CompanyName,
                    OwnerName = a.OwnerName,
                    GSTNo = a.GSTNo,
                    Jurisdiction = a.Jurisdiction,
                    AuthorizedSignature = a.AuthorizedSignature,
                    URL = a.URl,
                    LocationHeadID = (int)a.LocationHeadID,
                    LocationHead = a.LocationHead,
                    SupplierID = (int)a.SupplierID,
                    CustomerID = (int)a.CustomerID,
                    CustomerName = a.CustomerName,
                    SupplierName = a.SupplierName,
                    CurrencyName = a.CurrencyName,
                    CountryName = a.CountryName,
                    CurrencyID = a.CurrencyID,
                    CountryID = a.CountryID,
                    VatRegNo = a.VatRegNo

                }).ToList();
                return Location;
            }

        }

        public int Save(LocationBO Location, List<LocationAddressBO> Address)
        {
            GeneralDAL generalDAL = new GeneralDAL();
            ObjectParameter LocationId = new ObjectParameter("LocationID", typeof(int));
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {

                        if (!generalDAL.IsCodeAlreadyExists("Location", "Code", Location.Code))
                        {
                            var i = dbEntity.SpCreateLocation(Location.Code, Location.Name, Location.Place, Location.LocationStateID, Location.LocationTypeID, Location.StartDate, Location.EndDate,
                                Location.CompanyName, Location.OwnerName, Location.GSTNo, Location.Jurisdiction, Location.AuthorizedSignature, Location.URL, Location.LocationHeadID, Location.SupplierID, 
                                Location.CustomerID, Location.CurrencyID, Location.CountryID,
                                GeneralBO.ApplicationID, LocationId,Location.VatRegNo);

                            dbEntity.SaveChanges();

                            string PartyType = "Location";

                            int PartyID = Convert.ToInt32(LocationId.Value);
                            if (LocationId.Value != null)
                            {

                                foreach (var item in Address)
                                {
                                    dbEntity.SpCreateAddress(PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, 0, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping, GeneralBO.CreatedUserID, DateTime.Now, GeneralBO.LocationID, GeneralBO.ApplicationID);

                                }
                            };
                            transaction.Commit();
                            return PartyID;
                        }
                        else
                        {
                            throw new CodeAlreadyExistsException("Location code already exists");
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public int Update(LocationBO Location, List<LocationAddressBO> Address)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    if (Location.ID != 0)
                    {


                        if (!generalDAL.IsCodeAlreadyExists("Location", "Code", Location.Code, Location.ID))
                        {
                            var i = dbEntity.SpUpdateLocation(Location.ID, Location.Code, Location.Name, Location.Place, Location.LocationStateID, Location.LocationTypeID, Location.StartDate, Location.EndDate,
                                Location.CompanyName, Location.OwnerName, Location.GSTNo, Location.Jurisdiction, Location.AuthorizedSignature, Location.URL, Location.LocationHeadID, Location.SupplierID, Location.CustomerID, Location.CurrencyID, Location.CountryID,
                                GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID,Location.VatRegNo);

                            string PartyType = "Location";
                            int PartyID = Convert.ToInt32(Location.ID);

                            dbEntity.SpUpdateSupplierItemCategory(PartyID);
                            foreach (var item in Address)
                            {

                                if (item.AddressID == 0)
                                {
                                    dbEntity.SpCreateAddress(PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, 0, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping, GeneralBO.CreatedUserID, DateTime.Now, GeneralBO.LocationID, GeneralBO.ApplicationID);

                                }
                                else
                                {
                                    dbEntity.SpUpdateAddress(item.AddressID, PartyType, PartyID, item.AddressLine1, item.AddressLine2, item.AddressLine3, item.ContactPerson, item.Place, item.DistrictID, item.StateID, item.PIN, item.LandLine1, item.LandLine2, item.MobileNo, item.Fax, item.Email, item.IsBilling, item.IsShipping, item.IsDefault, item.IsDefaultShipping);
                                }
                            }
                        }
                        else
                        {
                            throw new CodeAlreadyExistsException("Location code already exists");
                        }

                    };
                    return Location.ID;
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<LocationBO> GetSupplierLocationBySupplierID(int SupplierID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetSupplierLocation(SupplierID).Select(a => new LocationBO
                    {
                        ID = a.ID,
                        LocationName = a.LocationName,
                        LocationID = a.LocationID
                    }).ToList();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LocationBO> GetItemLocationByItemID(int ItemID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetItemLocation(ItemID).Select(a => new LocationBO
                    {
                        ID = a.ID,
                        LocationName = a.LocationName,
                        LocationID = (int)a.LocationID
                    }).ToList();
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LocationBO> GetCustomerLocationMappingByCustomerID(int CustomerID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetCustomerLocationMapping(CustomerID).Select(a => new LocationBO
                    {
                        ID = a.ID,
                        LocationName = a.LocationName,
                        LocationID = (int)a.LocationID
                    }).ToList();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LocationBO> GetFreeMedicineLocationsByEmployeeID(int EmployeeID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetFreeMedicineLocationMapping(EmployeeID).Select(a => new LocationBO
                    {
                        ID = a.ID,
                        LocationName = a.LocationName,
                        LocationID = (int)a.LocationID
                    }).ToList();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public LocationBO GetHeadLocation(int LocationID)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                LocationBO Location = new LocationBO();
                Location = dEntity.SpGetHeadLocation(LocationID, GeneralBO.ApplicationID).Select(a => new LocationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    Place = a.Place,
                    StateID = (int)a.StateID
                }).FirstOrDefault();
                return Location;
            }
        }

        public List<LocationBO> GetProductionLocationList()
        {
            List<LocationBO> Location = new List<LocationBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Location = dEntity.SpGetProductionLocationList().Select(a => new LocationBO
                {
                    ID = a.ID,
                    Code = a.Code,
                    Name = a.Name,
                    Place = a.Place,
                }).ToList();
                return Location;
            }
        }

        public List<LocationBO> GetLocationListByLocationHead()
        {
            List<LocationBO> Location = new List<LocationBO>();
            using (MasterEntities entity = new MasterEntities())
            {
                Location = entity.SpGetLocationByLocationHead(GeneralBO.LocationID).Select(a => new LocationBO
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();
                return Location;
            }
        }
        public List<LocationBO> getInterCompanyLocation(int LocationID)
        {
            List<LocationBO> Location = new List<LocationBO>();
            using (MasterEntities entity = new MasterEntities())
            {
                Location = entity.SpGetLocationByLocationHead(LocationID).Select(a => new LocationBO
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();
                return Location;
            }
        }
        public List<LocationBO> GetProductionLocationMapping(int ProductionGroupID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetProductionLocationMapping(ProductionGroupID).Select(a => new LocationBO
                    {
                        ID = a.ID,
                        LocationName = a.LocationName,
                        LocationID = (int)a.LocationID
                    }).ToList();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LocationBO> GetBranchList()
        {
            List<LocationBO> Branch = new List<LocationBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                Branch = dEntity.SpGetBranchList().Select(a => new LocationBO
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();
                return Branch;
            }
        }
        public List<LocationBO> GetCurrentLocationTaxDetails(int locationID)
        {
            List<LocationBO> Locationlist = new List<LocationBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCurrentLocationTaxDetails(locationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            var obj = new LocationBO
                            {
                                ID = item.ID,
                                Name = item.Name.Trim(),
                                CurrencyCode = item.CurrencyCode,
                                DecimalPlaces = item.DecimalPlaces,
                                CountryName = item.Country.Trim(),
                                CountryID = item.CountryID.HasValue ? item.CountryID.Value : 0,
                                CurrencyName = item.Currency.Trim(),
                                CurrencyID = item.CurrencyID.HasValue ? item.CurrencyID.Value : 0,
                                TaxType = item.TaxType,
                                TaxTypeID = item.TaxTypeID,
                                IsGST = item.IsGST,
                                IsVat = item.IsVat
                            };
                            Locationlist.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return Locationlist;
        }
        public DatatableResultBO GetCurrencyLocationSearchList(int locationID, string Name, string Country, string Currency, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetCurrencyLocationSearchList(locationID, Name, Country, Currency, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name.Trim(),
                                Country = item.Country.Trim(),
                                CountryID = item.CountryID,
                                Currency = item.Currency.Trim(),
                                CurrencyID = item.CurrencyID,
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

    }

}


