using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Accounts.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Accounts.Controllers
{
    public class BRSController : Controller
    {
        #region Private members

        private IBRSContract brsBL;
        private ITreasuryContract treasuryBL;
        private IGeneralContract generalBL;

        #endregion

        #region Constructor
        public BRSController()
        {
            brsBL = new BRSBL();
            treasuryBL = new TreasuryBL();
            generalBL = new GeneralBL();
        }
        #endregion

        #region Public methods
        // GET: Accounts/BRS
        public ActionResult Index()
        {
            List<BRSModel> BRSList = brsBL.getBRSList().Select(a => new BRSModel()
            {
                ID = a.ID,
                TransNo = a.TransNo,
                Date = General.FormatDate(a.Date, "view"),
                BankName = a.BankName,
                FromTransactionDate = General.FormatDate(a.FromTransactionDate, "view"),
                ToTransactionDate = General.FormatDate(a.ToTransactionDate, "view"),
                FileName = a.FileName,
                Status = a.IsDraft ? "draft" : "",
            }).ToList();

            return View(BRSList);
        }

        // GET: Accounts/BRS/Create
        public ActionResult Create()
        {
            BRSModel BRS = new BRSModel();
            BRS.TransNo = generalBL.GetSerialNo("BRS", "Code");
            BRS.Date = General.FormatDate(DateTime.Now);
            BRS.BankList = new SelectList(treasuryBL.GetBank(), "ID", "BankName");
            SelectList temp = new SelectList(new List<SelectListItem>{
                            new SelectListItem {Text = "UnReconciled", Value = "UnReconciled" },
                            new SelectListItem {Text = "Reconciled", Value ="Reconciled"},
            });

            ViewBag.Status = temp;

            return View(BRS);
        }
        // GET: Accounts/BRS/Details/5
        public ActionResult Details(int Id)
        {
            BRSModel brsModel = brsBL.getBRSDetails(Id).Select(m => new BRSModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(m.Date, "view"),
                BankName = m.BankName,
                FromTransactionDate = General.FormatDate(m.FromTransactionDate, "view"),
                ToTransactionDate = General.FormatDate(m.ToTransactionDate, "view"),
                FilePath = m.FilePath,
                FileName = m.FileName,
                IsDraft = m.IsDraft
                
            }).First();
            brsModel.Items = brsBL.getBRSTransDetails(Id).Select(m => new BRSItemModel()
            {
                DocumentNumber = m.DocumentNumber,
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate, "view"),
                Credit = m.Credit,
                Debit = m.Debit,
                BankCharges = m.BankCharges,
                ItemName = m.ItemName,
                EquivalentBankTransactionNumber = m.EquivalentBankTransactionNumber,
                Status = m.Status

            }).ToList();
            brsModel.Statements = brsBL.getBRSBankTransDetails(Id).Select(m => new BankStatementModel()
            {
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate, "view"),
                Credit = m.Credit,
                Debit = m.Debit,

            }).ToList();

            return View(brsModel);
        }
        // POST: Accounts/BRS/Save
        [HttpPost]
        public ActionResult Save(BRSModel model)
        {
            try
            {
                BRSBO BRS = new BRSBO()
                {
                    ID = model.ID,
                    TransNo = model.TransNo,
                    Date = General.ToDateTime(model.Date),
                    BankID = model.BankID,
                    FromTransactionDate = General.ToDateTime(model.FromTransactionDate),
                    ToTransactionDate = General.ToDateTime(model.ToTransactionDate),
                    AttachmentID = model.AttachmentID,
                    IsDraft = model.IsDraft

                };
                List<BRSTransBO> ItemList = new List<BRSTransBO>();
                BRSTransBO brsItem;
                foreach (var item in model.Items)
                {
                    brsItem = new BRSTransBO()
                    {
                        DocumentNumber = item.DocumentNumber,
                        InstrumentNumber = item.InstrumentNumber,
                        InstrumentDate = General.ToDateTime(item.InstrumentDate, "date"),
                        Credit = item.Credit,
                        Debit = item.Debit,
                        BankCharges = item.BankCharges,
                        EquivalentBankTransactionNumber = item.EquivalentBankTransactionNumber,
                        Status = item.Status
                    };
                    ItemList.Add(brsItem);
                }
                List<BankStatementBO> StatementList = new List<BankStatementBO>();
                BankStatementBO Statement;
                foreach (var item in model.Statements)
                {
                    Statement = new BankStatementBO()
                    {
                        InstrumentNumber = item.InstrumentNumber,
                        InstrumentDate = General.ToDateTime(item.InstrumentDate),
                        Debit = item.Debit,
                        Credit = item.Credit
                    };
                    StatementList.Add(Statement);
                }
                if (BRS.ID == 0)
                {
                    brsBL.Save(BRS, ItemList, StatementList);
                }
                else
                {
                    brsBL.UpdateBRS(BRS, ItemList, StatementList);
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Accounts/BRS/Edit/5
        public ActionResult Edit(int ID)
        {

            BRSModel brsModel = brsBL.getBRSDetails(ID).Select(m => new BRSModel()
            {
                ID = m.ID,
                TransNo = m.TransNo,
                Date = General.FormatDate(DateTime.Now),
                BankID = m.BankID,
                FromTransactionDate = General.FormatDate(m.FromTransactionDate),
                ToTransactionDate = General.FormatDate(m.ToTransactionDate),
                AttachmentID = m.AttachmentID,
                FilePath = m.FilePath,
                FileName = m.FileName,
                IsDraft = m.IsDraft,
            }).First();
            brsModel.Items = brsBL.getBRSTransDetails(ID).Select(m => new BRSItemModel()
            {
                DocumentNumber = m.DocumentNumber,
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate),
                Credit = m.Credit,
                Debit = m.Debit,
                BankCharges = m.BankCharges,
                ItemName = m.ItemName,
                EquivalentBankTransactionNumber = m.EquivalentBankTransactionNumber,
                Status = m.Status

            }).ToList();
            brsModel.Statements = brsBL.getBRSBankTransDetails(ID).Select(m => new BankStatementModel()
            {
                InstrumentNumber = m.InstrumentNumber,
                InstrumentDate = General.FormatDate(m.InstrumentDate),
                Credit = m.Credit,
                Debit = m.Debit,

            }).ToList();


            brsModel.BankList = new SelectList(treasuryBL.GetBank(), "ID", "BankName");
            List<object> temp = new List<object>()
            {
                new  {Text = "UnReconciled", Value = "UnReconciled" },
                new {Text = "Reconciled", Value ="Reconciled"}
            };
            ViewBag.Status = temp;

            SelectList statusTemp = new SelectList(new List<SelectListItem>{
                            new SelectListItem {Text = "UnReconciled", Value = "UnReconciled" },
                            new SelectListItem {Text = "Reconciled", Value ="Reconciled"},
            });
            ViewBag.StatusTemplate = statusTemp;

            return View(brsModel);

        }

        public JsonResult ConvertCSVtoDataTable(string FilePath)
        {
            try
            {
                BankStatementModel statement;

                DataTable table = new DataTable();
                List<BankStatementModel> data = new List<BankStatementModel>();

                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string[] headers = reader.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        table.Columns.Add(header);
                    }
                    while (!reader.EndOfStream)
                    {
                        string[] rows = reader.ReadLine().Split(',');
                        statement = new BankStatementModel();
                        if (rows.Length > 0)
                        {
                            try
                            {
                                DateTime date;
                                decimal credit;
                                decimal debit;
                                decimal.TryParse(rows[3].Trim(), out credit);
                                decimal.TryParse(rows[4].Trim(), out debit);
                                date = Convert.ToDateTime(rows[2].Trim());
                                statement.InstrumentNumber = rows[1].Trim();
                                statement.InstrumentDate = General.FormatDate(date, "view");
                                statement.Credit = credit;
                                statement.Debit = debit;

                            }
                            catch (Exception e)
                            {
                                return Json(new { Status = "failure", data = data }, JsonRequestBehavior.AllowGet);

                            }
                            data.Add(statement);
                        }
                    }
                }

                return Json(new { Status = "Success", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure" }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult ConvertXSLXtoDataTable(string FilePath, string Extention)
        {
            List<BankStatementModel> data = new List<BankStatementModel>();
            BankStatementModel statement;
            string connString = "";
            if (Extention == "xlsx")
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            else
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();

            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);
                    dt = ds.Tables[0];
                    DateTime date;
                    foreach (DataRow row in dt.Rows)
                    {
                        statement = new BankStatementModel();
                        statement.InstrumentNumber = Convert.ToString(row[0]);
                        date = Convert.ToDateTime(row[1].ToString());
                        statement.InstrumentDate = General.FormatDate(date);
                        statement.Credit = Convert.ToDecimal(row[2]);
                        statement.Debit = Convert.ToDecimal(row[3]);
                        data.Add(statement);
                    }

                }
                return Json(new { Status = "success", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", data = data }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                oledbConn.Close();
            }
        }

        public JsonResult getStatusAsPerBooks(string FromDate, string ToDate)
        {
            DateTime fromTransactionDate = General.ToDateTime(FromDate);
            DateTime toTransactionDate = General.ToDateTime(ToDate);
            List<BRSItemModel> bookstatus = brsBL.getStatusAsPerBooks(fromTransactionDate, toTransactionDate).Select(a => new BRSItemModel()
            {
                DocumentNumber = a.DocumentNumber,
                InstrumentNumber = a.InstrumentNumber,
                InstrumentDate = General.FormatDate(a.InstrumentDate),
                Credit = a.Credit,
                Debit = a.Debit,
                BankCharges = a.BankCharges,
            }).ToList();
            return Json(bookstatus, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ItemsForBankReconciliation(int BankID, string FromDate, string ToDate)
        {
            FromDate = "14-10-2020";
            ToDate = "14-10-2020";
            DateTime fromTransactionDate = General.ToDateTime(FromDate);
            DateTime toTransactionDate = General.ToDateTime(ToDate);
            //BRSModel model = new BRSModel();
            BRSModel model = brsBL.GetTotalBalanceAmountDetailsForBankReconciliation(BankID, fromTransactionDate, toTransactionDate).Select(m => new BRSModel()
            {
              BalAsPerCompanyBooks=m.BalAsPerCompanyBooks,
              CreditAmountNotReflectedInBank = m.CreditAmountNotReflectedInBank,
              DebitAmountNotReflectedInBank = m.DebitAmountNotReflectedInBank,
              BalAsPerBank=m.BalAsPerBank
            }).First();
            model.Items = brsBL.GetDataForBankReconciliation(BankID, fromTransactionDate, toTransactionDate).Select(a => new BRSItemModel()
            {

                DocumentNumber = a.DocumentNumber,
                InstrumentNumber = a.InstrumentNumber,
                InstrumentDate = General.FormatDate(a.InstrumentDate),
                ReconciledDate = General.FormatDate(a.ReconciledDate),
                Credit = a.Credit,
                Debit = a.Debit,
                BankCharges = a.BankCharges,
                DocumentType=a.DocumentType,
                AccountName = a.AccountName,
                ReferenceNo = a.ReferenceNo,
                Remarks = a.Remarks,
                DocumentID = a.DocumentID,
                
    }).ToList();
            return PartialView(model);
        }

        public ActionResult CreateV3()
        {
            BRSModel BRS = new BRSModel();
            BRS.TransNo = generalBL.GetSerialNo("BRS", "Code");
            BRS.Date = General.FormatDate(DateTime.Now);
            BRS.BankList = new SelectList(treasuryBL.GetBank(), "ID", "BankName");
            SelectList temp = new SelectList(new List<SelectListItem>{
                            new SelectListItem {Text = "UnReconciled", Value = "UnReconciled" },
                            new SelectListItem {Text = "Reconciled", Value ="Reconciled"},
            });

            ViewBag.Status = temp;

            return View(BRS);
        }

        public ActionResult IndexV3()
        {
            List<BRSModel> BRSList = new List<BRSModel>();
            return View(BRSList);
        }

        public ActionResult SaveBankReconciledDateV3(BRSModel model)
        {
            var result = new List<object>();
            try
            {
                BRSBO BRS = new BRSBO();
                List<BRSTransBO> ItemList = new List<BRSTransBO>();
                BRSTransBO brsItem;
                foreach (var item in model.Items)
                {
                    brsItem = new BRSTransBO()
                    {
                        DocumentNumber = item.DocumentNumber,
                        DocumentType = item.DocumentType,
                        ReconciledDate = General.ToDateTime(item.ReconciledDate),
                        Credit = item.Credit,
                        Debit = item.Debit,
                        DocumentID = item.DocumentID,
                        ReferenceNo=item.ReferenceNo,
                        Remarks=item.Remarks
                    };
                    ItemList.Add(brsItem);
                }

                brsBL.SaveBankReconciledDateV3(ItemList);
                return Json(new { Status = "Success", message = "Invoice Already Settled" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                result.Add(new { ErrorMessage = e.Message });
                generalBL.LogError("Accounts", "Payment", "SaveReconciledDate", 0, e);
                return Json(new { Status = "failure", data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBRSListV3(DatatableModel Datatable)
        {
            try
            {
                string DocumentType = Datatable.Columns[1].Search.Value;
                string DocumentNumber = Datatable.Columns[2].Search.Value;
                string AccountName = Datatable.Columns[3].Search.Value;
                string TransactionDate = Datatable.Columns[4].Search.Value;
                string BankName = Datatable.Columns[5].Search.Value;
                string DebitAmount = Datatable.Columns[6].Search.Value;
                string CreditAmount = Datatable.Columns[7].Search.Value;
                string ReconciledDate = Datatable.Columns[8].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = brsBL.GetBRSListV3(Type, DocumentType, DocumentNumber, TransactionDate, AccountName, BankName, DebitAmount, CreditAmount, ReconciledDate, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
