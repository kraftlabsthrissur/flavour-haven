using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class AdvancePaymentModel
    {
        public int ID { get; set; }
        public string AdvancePaymentNo { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string AdvancePaymentDate { get; set; }
        public List<KeyValuePair<string, string>> BankDetails { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentTypeName { get; set; }
        public string BankName { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public string Status { get; set; }
        public string SupplierName { get; set; }
        public decimal? NetAmount { get; set; }
        public int SupplierID { get; set; }
        public List<AdvancePaymentList> List { get; set; }
        

        public string BankDetailJsonStr
        {
            get
            {
                return BankDetails != null ?
                    Newtonsoft.Json.JsonConvert.SerializeObject(BankDetails)
                    : string.Empty;
            }
        }

        public int ModeOfPaymentID { get; set; }
        public string ModeOfPaymentName { get; set; }
        public int BankID { get; set; }
        public string BankDetail { get; set; }
        public int SelectedID { get; set; }     //SupplierID or EmployeeID 
        public string SelectedName { get; set; }     //Supplier or Employee Name
        public string AccountNo { get; set; }
        public string ReferenceNo { get; set; }
        public string Purpose { get; set; }
        public string SaveType { get; set; }
        public string Category { get; set; }
        public int PaymentTypeID { get; set; }
        public List<TreasuryModel> BankList { get; set; }
        public List<AdvancePaymentPurchaseOrderModel> PurchaseOrders { get; set; }
        public SelectList PaymentTypeList { get; set; }
        public List<AdvancePaymentList> PaymentTrans { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Address5 { get; set; }
        public string GSTNo { get; set; }
        public string NetAmountInWords { get; set; }
        public bool IsDraft { get; set; }
        public int CashPaymentLimit { get; set; }
        public decimal? AdvanceAmount { get; set; }
    }

    //created by lini on 06/04/2018
    public class AdvancePaymentList
    {
        public int ID { get; set; }
        public string AdvancePaymentNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string PurchaseOrderTerms { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public string TDSCode { get; set; }
        public Decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public string TransNo { get; set; }
        public int ItemID { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Advance { get; set; }
    }
    //
    public class AdvancePaymentPurchaseOrderModel
    {
        public int PurchaseOrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
        public int TDSID { get; set; }
        public decimal TDSAmount { get; set; }
        public string Remarks { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string PurchaseOrderDateStr { get; set; }
        public int PaymentWithin { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? POAmount { get; set; }
        public bool IsDraft { get; set; }
        public string TDS { get; set; }
        public decimal Advance { get; set; }
    }

    public class AdvancePaymentAdvanceRequestTransModel
    {
        public int EmployeeID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string EmployeeName { get; set; }
        public string ExpectedDate { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public bool IsOfficial { get; set; }
        public string Category { get; set; }
        public int ID { get; set; }
        public string AdvanceRequestNo { get; set; }
        public string AdvanceRequestDateStr { get; set; }
        public bool IsDraft { get; set; }
        public decimal AdvDetAmount { get; set; }
        public decimal Advance { get; set; }
    }


    public static partial class Mapper
    {
        public static AdvancePaymentBO MapToBo(this AdvancePaymentModel advancePaymentModel)
        {
            return new AdvancePaymentBO()
            {

                ID = advancePaymentModel.ID,
                AdvancePaymentNo = advancePaymentModel.AdvancePaymentNo,
                ModeOfPaymentID = advancePaymentModel.ModeOfPaymentID,
                BankID = advancePaymentModel.BankID,
                BankDetail = advancePaymentModel.BankDetail,
                SelectedID = advancePaymentModel.SelectedID,
                SelectedName = advancePaymentModel.SelectedName,
                AccountNo = advancePaymentModel.AccountNo,
                ReferenceNo = advancePaymentModel.ReferenceNo,
                Purpose = advancePaymentModel.Purpose,
                SaveType = advancePaymentModel.SaveType,
                Category = advancePaymentModel.Category,
                AdvancePaymentPurchaseOrders = advancePaymentModel.PurchaseOrders == null ? new List<AdvancePaymentPurchaseOrderBO>() :
                advancePaymentModel.PurchaseOrders.Select(po => new AdvancePaymentPurchaseOrderBO()
                {
                    PurchaseOrderID = po.PurchaseOrderID,
                    ItemID = po.ItemID,
                    ItemName = po.ItemName,
                    Amount = po.Amount,
                    TDSID = po.TDSID,
                    TDSAmount = po.TDSAmount,
                    Remarks = po.Remarks
                }).ToList()


            };

        }
    }

}