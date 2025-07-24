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
    public class TreasuryController : Controller
    {

        #region Private Declarations

        private ITreasuryContract treasuryBL;
        private ILocationContract locationBL;
        private IGeneralContract generalBL;

        #endregion
        public TreasuryController()
        {
            treasuryBL = new TreasuryBL();
            locationBL = new LocationBL();
            generalBL = new GeneralBL();
        }



        // GET: Masters/Treasury
        public ActionResult Index()
        {
            List<TreasuryModel> treasuryList = new List<TreasuryModel>();
            //treasuryList = treasuryBL.GetTreasuryList().Select(a => new TreasuryModel
            //{
            //    ID = a.ID,
            //    Type = a.Type,
            //    AccountCode = a.AccountCode,
            //    BankName = a.BankName,
            //    AliasName = a.AliasName,
            //    CoBranchName = a.CoBranchName,
            //    BankBranchName = a.BankBranchName,
            //    AccountType1 = a.AccountType1
            //}).ToList();


            return View(treasuryList);
        }

        // GET: Masters/State/Details/5
        public ActionResult Details(int id)
        {
            var obj = treasuryBL.GetTreasuryDetails(id);

            TreasuryModel treasuryModel = new TreasuryModel();

            treasuryModel.ID = obj.ID;
            treasuryModel.Type = obj.Type;
            treasuryModel.BankCode = obj.BankCode;
            treasuryModel.AccountCode = obj.AccountCode;
            treasuryModel.AccountName = obj.AccountName;
            treasuryModel.BankName = obj.BankName;
            treasuryModel.AliasName = obj.AliasName;
            treasuryModel.CoBranchName = obj.CoBranchName;
            treasuryModel.BankBranchName = obj.BankBranchName;
            treasuryModel.AccountType1 = obj.AccountType1;
            treasuryModel.AccountType2 = obj.AccountType2;
            treasuryModel.AccountNo = obj.AccountNo;
            treasuryModel.IFSC = obj.IFSC;
            treasuryModel.LocationMapping = obj.LocationMapping;
            treasuryModel.StartDate = General.FormatDate(obj.StartDate);
            treasuryModel.EndDate = General.FormatDate(obj.EndDate);
            treasuryModel.remarks = obj.remarks;
            treasuryModel.IsPayment = obj.IsPayment;
            treasuryModel.IsReceipt = obj.IsReceipt;
            return View(treasuryModel);
        }

        //GET: Masters/Treasury/Create
        public ActionResult Create()
        {
            TreasuryModel treasuryModel = new TreasuryModel();
            treasuryModel.TypeList = new SelectList(treasuryBL.GetTreasuryType(), "Name", "Name");
            treasuryModel.StartDate = General.FormatDate(DateTime.Now);
            treasuryModel.EndDate = General.FormatDate(Convert.ToDateTime(generalBL.GetConfig("DefaultDate")));
            treasuryModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            treasuryModel.Type = "Cash";
            return View(treasuryModel);
        }

        // POST: Masters/Treasury/Create
        [HttpPost]
        public ActionResult Create(TreasuryModel model)
        {

            try
            {
                TreasuryBO treasuryBO = new TreasuryBO()
                {
                    Type = model.Type,
                    BankCode = model.BankCode,
                    AccountCode = model.AccountCode,
                    BankName = model.BankName,
                    AliasName = model.AliasName,
                    CoBranchName = model.CoBranchName,
                    BankBranchName = model.BankBranchName,
                    AccountType1 = model.AccountType1,
                    AccountType2 = model.AccountType2,
                    AccountNo = model.AccountNo,
                    IFSC = model.IFSC,
                    LocationMappingID = model.LocationMappingID,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                    remarks = model.remarks,
                    IsPayment = model.IsPayment,
                    IsReceipt = model.IsReceipt,
                    OpeningAmount = model.OpeningAmount
                };

                treasuryBL.CreateTreasury(treasuryBO);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: Masters/Treasury/Edit/5
        public ActionResult Edit(int id)
        {
            var obj = treasuryBL.GetTreasuryDetails(id);
            TreasuryModel treasuryModel = new TreasuryModel();

            treasuryModel.ID = obj.ID;
            treasuryModel.Type = obj.Type;
            treasuryModel.BankCode = obj.BankCode;
            treasuryModel.AccountCode = obj.AccountCode;
            treasuryModel.AccountName = obj.AccountName;
            treasuryModel.BankName = obj.BankName;
            treasuryModel.AliasName = obj.AliasName;
            treasuryModel.CoBranchName = obj.CoBranchName;
            treasuryModel.BankBranchName = obj.BankBranchName;
            treasuryModel.AccountType1 = obj.AccountType1;
            treasuryModel.AccountType2 = obj.AccountType2;
            treasuryModel.AccountNo = obj.AccountNo;
            treasuryModel.IFSC = obj.IFSC;
            treasuryModel.LocationMappingID = obj.LocationMappingID;
            treasuryModel.LocationMapping = obj.LocationMapping;
            treasuryModel.StartDate = General.FormatDate(obj.StartDate);
            treasuryModel.EndDate = General.FormatDate(obj.EndDate);
            treasuryModel.remarks = obj.remarks;
            treasuryModel.IsPayment = obj.IsPayment;
            treasuryModel.IsReceipt = obj.IsReceipt;
            treasuryModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            treasuryModel.TypeList = new SelectList(treasuryBL.GetTreasuryType(), "Name", "Name");
            return View(treasuryModel);

        }

        // POST: Masters/Treasury/Edit/5
        [HttpPost]
        public ActionResult Edit(TreasuryModel model)
        {
            try
            {
                TreasuryBO treasuryBO = new TreasuryBO()
                {
                    ID = (int)model.ID,
                    Type = model.Type,
                    BankCode = model.BankCode,
                    AccountCode = model.AccountCode,
                    BankName = model.BankName,
                    AliasName = model.AliasName,
                    CoBranchName = model.CoBranchName,
                    BankBranchName = model.BankBranchName,
                    AccountType1 = model.AccountType1,
                    AccountType2 = model.AccountType2,
                    AccountNo = model.AccountNo,
                    IFSC = model.IFSC,
                    LocationMappingID = model.LocationMappingID,
                    StartDate = General.ToDateTime(model.StartDate),
                    EndDate = General.ToDateTime(model.EndDate),
                    remarks = model.remarks,
                    IsPayment = model.IsPayment,
                    IsReceipt = model.IsReceipt,
                };
                treasuryBL.EditTreasury(treasuryBO);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBank(string ModuleName,string Mode)
        {
            List<TreasuryBO> BankName = treasuryBL.GetBank(ModuleName, Mode).ToList();
            return Json(new { Status = "success", data = BankName }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBankList()
        {
            List<TreasuryBO> BankName = treasuryBL.GetBankList().ToList();
            return Json(new { Status = "success", data = BankName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankForCounterSales(string Mode)
        {
            List<TreasuryBO> BankName = treasuryBL.GetBankForCounterSales(Mode).ToList();
            return Json(new { Status = "success", data = BankName }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountCodeAutoComplete(string CodeHint)
        {
            List<TreasuryModel> itemList = new List<TreasuryModel>();
            itemList = treasuryBL.GetAccountCodeAutoComplete(CodeHint).Select(a => new TreasuryModel()
            {
                ID = a.ID,
                Code = a.Code,
                AccountName = a.AccountName
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(int id)
        {
            return null;
        }

        public JsonResult GetBankDetails(int LocationID)
        {
            List<TreasuryBO> BankName = treasuryBL.GetBank(LocationID).ToList();
            return Json(new { Status = "success", data = BankName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTreasuryList(DatatableModel Datatable)
        {
            try
            {
                string Type = Datatable.Columns[1].Search.Value;
                string AccountCode = Datatable.Columns[2].Search.Value;
                // BankName = Datatable.Columns[3].Search.Value;
                string AliasName = Datatable.Columns[3].Search.Value;
                //string CoBranchName = Datatable.Columns[5].Search.Value;
                //string BankBranchName = Datatable.Columns[6].Search.Value;
                //string AccountType = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = treasuryBL.GetTreasuryList(Type, AccountCode, null, AliasName, null, null, null, SortField, SortOrder, Offset, Limit);
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