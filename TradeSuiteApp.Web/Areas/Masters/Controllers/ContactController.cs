using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{

    public class ContactController : Controller
    {
        private IContactContract iContactContract;
        private IGeneralContract generalBL;
        public ContactController()
        {
            generalBL = new GeneralBL();
            iContactContract = new ContactBL();
        }
        public ActionResult Index()
        {

            List<ContactModel> contactList = new List<ContactModel>();
            contactList = iContactContract.GetContactList().Select(a => new ContactModel()
            {
                ID = a.ID,
                Firstname = a.Firstname,
                Lastname = a.Lastname,
                Designation = a.Designation,
                ContactNo = a.ContactNo,

            }).ToList();
            return View(contactList);

        }
        public ActionResult Create()
        {


            return View();
        }
        public ActionResult Save(ContactModel model)
        {

            try
            {
                ContactBO contactBO = new ContactBO()
                {
                    ID = model.ID,
                    Firstname = model.Firstname,
                    IsActive = model.IsActive,
                    Lastname = model.Lastname,
                    ContactNo = model.ContactNo,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Address3 = model.Address3,
                    Company = model.Company,
                    CustomerID = model.CustomerID,
                    AlternateNo = model.AlternateNo,
                    Designation = model.Designation,
                    Email = model.Email,
                };
                iContactContract.CreateContact(contactBO);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Details(int id)
        {

            //try
            //{
            var obj = iContactContract.GetContactDetails(id);
            ContactModel contactModel = new ContactModel();
            contactModel.ID = obj.ID;
            contactModel.Firstname = obj.Firstname;
            contactModel.Lastname = obj.Lastname;
            contactModel.ContactNo = obj.ContactNo;
            contactModel.AlternateNo = obj.AlternateNo;
            contactModel.Email = obj.Email;
            contactModel.Address1 = obj.Address1;
            contactModel.Address2 = obj.Address2;
            contactModel.Address3 = obj.Address3;
            contactModel.Designation = obj.Designation;
            contactModel.Company = obj.Company;
            contactModel.IsActive = obj.IsActive;
            return View(contactModel);
            //}
            //catch (Exception e)
            //{
            //    return RedirectToAction("Index");
            //}



        }
        public ActionResult Edit(int id)
        {
            try
            {
                var obj = iContactContract.GetContactDetails(id);
                ContactModel contactModel = new ContactModel();
                contactModel.ID = obj.ID;
                contactModel.Firstname = obj.Firstname;
                contactModel.Lastname = obj.Lastname;
                contactModel.ContactNo = obj.ContactNo;
                contactModel.AlternateNo = obj.AlternateNo;
                contactModel.Email = obj.Email;
                contactModel.Address1 = obj.Address1;
                contactModel.Address2 = obj.Address2;
                contactModel.Address3 = obj.Address3;
                contactModel.Designation = obj.Designation;
                contactModel.Company = obj.Company;
                contactModel.CustomerID = obj.CustomerID;
                contactModel.IsActive = obj.IsActive;

                return View(contactModel);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit1(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContactBO countryBO = new ContactBO()
                    {
                        ID = model.ID,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        ContactNo = model.ContactNo,
                        AlternateNo = model.AlternateNo,
                        Email = model.Email,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Address3 = model.Address3,
                        Designation = model.Designation,
                        CustomerID = model.CustomerID,
                        IsActive = model.IsActive,
                    };
                    iContactContract.EditContact(countryBO);
                    return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var res = new List<object>();
                    return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetContactList(DatatableModel Datatable)
        {
            try
            {
                int CustomerID = Convert.ToInt32(Datatable.GetValueFromKey("CustomerID", Datatable.Params));
                string firstNameHit = Datatable.Columns[2].Search.Value;
                string lastNameHit = Datatable.Columns[3].Search.Value;
                string phonenoHit = Datatable.Columns[4].Search.Value;
                string alternativenoHit = Datatable.Columns[5].Search.Value;
                string designationHit = Datatable.Columns[6].Search.Value;
                string emailIDHit = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = iContactContract.GetContactSearchList(CustomerID, firstNameHit, lastNameHit, phonenoHit, alternativenoHit, designationHit, emailIDHit, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                generalBL.LogError("Masters", "Contact", "GetContactList", 0, e);
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
