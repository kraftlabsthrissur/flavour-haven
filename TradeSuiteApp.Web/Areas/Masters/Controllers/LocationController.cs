using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class LocationController : Controller
    {
        private ILocationContract locationBL;
        private IPlacesContract placesBL;
        private IStateContract stateBL;
        private ILocationGroupContract locationGroupBL;
        private ILocationHeadContract locationHeadBL;
        private IDistrictContract disrictBL;
        private IAddressContract addressBL;
        private IGeneralContract generalBL;
        public LocationController(IDropdownContract dropdown)
        {
            locationBL = new LocationBL();
            placesBL = new PlacesBL();
            locationGroupBL = new LocationGroupBL();
            locationHeadBL = new LocationHeadBL();
            stateBL = new StateBL();
            disrictBL = new DistrictBL();
            addressBL = new AddressBL();
            generalBL = new GeneralBL();
        }
        // GET: Masters/Location
        public ActionResult Index()
        {
            List<LocationModel> LocationList = new List<LocationModel>();
            LocationList = locationBL.GetLocationList().Select(a => new LocationModel
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                CountryName = a.CountryName,
                CurrencyName = a.CurrencyName
            }).ToList();

            return View(LocationList);
        }
        public ActionResult Save(LocationModel model)
        {
            try
            {
                LocationBO Location = new LocationBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    Place = model.Place,
                    LocationStateID = model.LocationStateID,
                    LocationTypeID = model.LocationTypeID,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                    CompanyName = model.CompanyName,
                    OwnerName = model.OwnerName,
                    GSTNo = model.GSTNo,
                    Jurisdiction = model.Jurisdiction,
                    AuthorizedSignature = model.AuthorizedSignature,
                    URL = model.URL,
                    LocationHeadID = model.LocationHeadID,
                    SupplierID = model.SupplierID,
                    CustomerID = model.CustomerID,
                    CurrencyID = model.CurrencyID,
                    CountryID = model.CountryID,
                    VatRegNo=model.VatRegNo

                };
                List<LocationAddressBO> AddressList = new List<LocationAddressBO>();
                LocationAddressBO AddressItem;
                foreach (var item in model.AddressList)
                {
                    AddressItem = new LocationAddressBO()
                    {
                        AddressID = item.AddressID,
                        AddressLine1 = item.AddressLine1,
                        AddressLine2 = item.AddressLine2,
                        AddressLine3 = item.AddressLine3,
                        Place = item.AddressPlace,
                        ContactPerson = item.ContactPerson,
                        LandLine1 = item.LandLine1,
                        LandLine2 = item.LandLine2,
                        MobileNo = item.MobileNo,
                        StateID = item.StateID,
                        PIN = item.PIN,
                        Fax = item.Fax,
                        DistrictID = item.DistrictID,
                        Email = item.Email,
                        IsBilling = item.IsBilling,
                        IsShipping = item.IsShipping,
                        IsDefault = item.IsDefault,
                        IsDefaultShipping = item.IsDefaultShipping,
                    };
                    AddressList.Add(AddressItem);
                }
                if (Location.ID == 0)
                {
                    locationBL.Save(Location, AddressList);
                }
                else
                {
                    locationBL.UpdateLocation(Location, AddressList);
                }
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

            }
            catch (CodeAlreadyExistsException e)
            {
                return Json(new { Status = "Failure", Message = "Location Code already exists" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failure", Message = "Save Location failed" }, JsonRequestBehavior.AllowGet);
            }

        }
        // GET: Masters/Location/Create
        public ActionResult Create()
        {
            LocationModel Location = new LocationModel();


            Location.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Location.StartDate = General.FormatDate(DateTime.Now);
            Location.EndDate = General.FormatDate(DateTime.Now);
            Location.LocationTypeList = new SelectList(locationGroupBL.GetLocationGroup(), "ID", "Name");
            Location.LocationHeadList = new SelectList(locationHeadBL.GetLocationHeadList(), "ID", "Name");
            Location.DisitrictList = new SelectList(disrictBL.GetDistrictList(0), "ID", "Name");
            Location.EndDate = General.FormatDate(Convert.ToDateTime(generalBL.GetConfig("DefaultDate")));
            return View(Location);
        }
        public ActionResult Details(int Id)
        {
            LocationModel Location = locationBL.GetLocationDetails(Id).Select(a => new LocationModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place,
                LocationStateID = a.LocationStateID,
                LocationState = a.LocationState,
                LocationType = a.LocationType,
                StartDate = General.FormatDate(a.StartDate, "view"),
                EndDate = General.FormatDate(a.EndDate, "view"),
                CompanyName = a.CompanyName,
                OwnerName = a.OwnerName,
                GSTNo = a.GSTNo,
                Jurisdiction = a.Jurisdiction,
                AuthorizedSignature = a.AuthorizedSignature,
                URL = a.URL,
                LocationHead = a.LocationHead,
                SupplierID = a.SupplierID,
                CustomerID = a.CustomerID,
                SupplierName = a.SupplierName,
                CustomerName = a.CustomerName,
                CurrencyName = a.CurrencyName,
                CountryName = a.CountryName,
                VatRegNo= a.VatRegNo

            }).First();
            Location.AddressList = addressBL.GetAddressByPartyType(Id, "Location").Select(a => new LocationAddressModel()
            {
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                AddressPlace = a.Place,
                PIN = a.PIN,
                ContactPerson = a.ContactPerson,
                LandLine1 = a.LandLine1,
                LandLine2 = a.LandLine2,
                MobileNo = a.MobileNo,
                Fax = a.Fax,
                Email = a.Email,
                District = a.District,
                State = a.State,
                IsBilling = a.IsBilling,
                IsShipping = a.IsShipping,
                IsDefault = (bool)a.IsDefault,
                IsDefaultShipping = (bool)a.IsDefaultShipping
            }).ToList();

            return View(Location);
        }

        // GET: Masters/Department/Edit
        public ActionResult Edit(int Id)
        {
            LocationModel Location = locationBL.GetLocationDetails(Id).Select(a => new LocationModel()
            {
                ID = a.ID,
                Code = a.Code,
                Name = a.Name,
                Place = a.Place,
                LocationStateID = a.LocationStateID,
                LocationTypeID = a.LocationTypeID,
                StartDate = General.FormatDate(a.StartDate),
                EndDate = General.FormatDate(a.EndDate),
                CompanyName = a.CompanyName,
                OwnerName = a.OwnerName,
                GSTNo = a.GSTNo,
                Jurisdiction = a.Jurisdiction,
                AuthorizedSignature = a.AuthorizedSignature,
                URL = a.URL,
                LocationHeadID = a.LocationHeadID,
                SupplierID = a.SupplierID,
                CustomerID = a.CustomerID,
                SupplierName = a.SupplierName,
                CustomerName = a.CustomerName,
                CountryID = a.CountryID,
                CountryName = a.CountryName,
                CurrencyID = a.CurrencyID,
                CurrencyName = a.CurrencyName,
                VatRegNo = a.VatRegNo
            }).First();

            Location.StateList = new SelectList(stateBL.GetStateList(), "ID", "Name");
            Location.LocationTypeList = new SelectList(locationGroupBL.GetLocationGroup(), "ID", "Name");
            Location.LocationHeadList = new SelectList(locationHeadBL.GetLocationHeadList(), "ID", "Name");

            return View(Location);
        }
        public JsonResult GetAddressList(int? locationID = 0)
        {
            List<LocationAddressModel> AddressList = addressBL.GetAddressByPartyType(locationID, "Location").Select(a => new LocationAddressModel()
            {
                AddressID = a.AddressID,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                AddressLine3 = a.AddressLine3,
                AddressPlace = a.Place,
                PIN = a.PIN,
                ContactPerson = a.ContactPerson,
                LandLine1 = a.LandLine1,
                LandLine2 = a.LandLine2,
                MobileNo = a.MobileNo,
                Fax = a.Fax,
                Email = a.Email,
                District = a.District,
                DistrictID = a.DistrictID,
                StateID = a.StateID,
                State = a.State,
                IsBilling = a.IsBilling,
                IsShipping = a.IsShipping,
                IsDefault = (bool)a.IsDefault,
                IsDefaultShipping = (bool)a.IsDefaultShipping
            }).ToList();
            return Json(AddressList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getInterCompanyLocation(int locationID)
        {
            List<InterCompanyLocationModel> list = locationBL.getInterCompanyLocation(locationID).Select(a => new InterCompanyLocationModel()
            {
                ID = a.ID,
                Name = a.Name
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCurrencyLocationList(DatatableModel Datatable, int locationID)
        {
            try
            {
                string Name = Datatable.Columns[2].Search.Value;
                string Country = Datatable.Columns[3].Search.Value;
                string Currency = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = locationBL.GetCurrencyLocationSearchList(locationID, Name, Country, Currency, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}