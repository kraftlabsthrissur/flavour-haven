using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Accounts.Models
{
    public class ChequeStatusModel
    {
        public int ID { get; set; }
        public string TransNo { get; set; }
        public string Date { get; set; }
        public string ReceiptDateFrom { get; set; }
        public string ReceiptDateTo { get; set; }
        public int InstrumentStatusID { get; set; }
        public string InstrumentStatus { get; set; }
        public SelectList InstrumentStatusList { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public int UserStateID { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public List<ChequeStatusTransModel> Items { get; set; }
        public string CustomerName { get; set; }
        public int GstCategoryID { get; set; }

    }
    public class ChequeStatusTransModel
    {
        public int ID { get; set; }
        public int ChequeStatusID { get; set; }
        public string VoucherNo { get; set; }
        public string InstrumentNumber { get; set; }
        public string InstrumentDate { get; set; }
        public string StatusChangeDate { get; set; }
        public string ChequeReceivedDate { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal InstrumentAmount { get; set; }
        public decimal BankCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ChargesToCustomer { get; set; }
        public string VoucherDate { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string ChequeStatus { get; set; }
        public string EditStatus { get; set; }
        public int VoucherID { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public int StateID { get; set; }       
        

        public List<SelectListItem> StatusReceived = new List<SelectListItem>
             {
                new SelectListItem { Text = "Received", Value = "Received"},
                new SelectListItem { Text = "Deposited", Value = "Deposited"},
             };

        public List<SelectListItem> StatusDeposited = new List<SelectListItem>
            {
                new SelectListItem { Text = "Deposited", Value = "Deposited"},
                new SelectListItem { Text = "Collected", Value = "Collected"},
                new SelectListItem { Text = "Pending Error", Value = "PendingError"},
                new SelectListItem { Text = "Bounced", Value = "Bounced"},
                new SelectListItem { Text = "Cancelled", Value = "Cancelled"},
            };

        public List<SelectListItem> StatusCollected = new List<SelectListItem>
            {
                new SelectListItem { Text = "Collected", Value = "Collected"},
             };
        public List<SelectListItem> StatusPendingError = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pending Error", Value = "PendingError"},
                new SelectListItem { Text = "Deposited", Value = "Deposited"},
                new SelectListItem { Text = "Cancelled", Value = "Cancelled"},
             };

        public List<SelectListItem> StatusBounced = new List<SelectListItem>
            {
                new SelectListItem { Text = "Bounced", Value = "Bounced"},
             };
        public List<SelectListItem> StatusCancelled = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cancelled", Value = "Cancelled"},
             };


        public SelectList ChequeStatusList
        {
            get
            {
                SelectList chequeStatusList = new SelectList(StatusReceived, "Value", "Text");

                switch (EditStatus)
                {
                    case "Received":
                        chequeStatusList = new SelectList(StatusReceived, "Value", "Text");
                        break;

                    case "Deposited":
                        chequeStatusList = new SelectList(StatusDeposited, "Value", "Text");
                        break;

                    case "Collected":
                        chequeStatusList = new SelectList(StatusCollected, "Value", "Text"); 
                        break;

                    case "PendingError":
                        chequeStatusList = new SelectList(StatusPendingError, "Value", "Text");
                        break;
                    case "Bounced":
                        chequeStatusList = new SelectList(StatusBounced, "Value", "Text"); 
                        break;
                    case "Cancelled":
                        chequeStatusList = new SelectList(StatusCancelled, "Value", "Text");
                        break;
                }
                return chequeStatusList;
            }


        }



    }

}
