using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class LabTestDAL
    {
        public DatatableResultBO GetLabTestList(string Type, string TransNo, string Date, string PatientCode, string Patient, string LabTest, string Doctor, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetLabTestList(Type,TransNo, Date, PatientCode, Patient, LabTest, Doctor, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                AppointmentProcessID = item.AppointmentProcessID,
                                Date = ((DateTime)item.Date).ToString("dd-MMM-yyyy"),
                                PatientCode = item.PatientCode,
                                Patient = item.Patient,
                                PatientID = item.PatientID,
                                Doctor = item.Doctor,
                                IPID = item.IPID,
                                TestName = item.TestName,
                                PatientLabTestMasterID = item.PatientLabTestMasterID,
                                TransNo = item.TransNo,
                                InvoiceID = item.InvoiceID,
                                Status = item.Status
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

        public List<LabTestBO> GetPatientDetails(int ID, int PatientID)
        {
            try
            {
                List<LabTestBO> LabTest = new List<LabTestBO>();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    LabTest = dEntity.SpGetPatientDetailForLabTest(ID, PatientID).Select(a => new LabTestBO
                    {
                        PatientID = a.PatientID,
                        Patient = a.Patient,
                        PatientCode = a.PatientCode,
                        Age = (int)a.Age,
                        Sex = a.Gender,
                        Mobile = a.MobileNo,
                        Doctor = a.Doctor,
                        PatientLabTestID = a.PatientLabTestID,
                        IPID = a.IPID,
                        OPID = ID,
                        PatientTypeID = (int)a.PatientTypeID,
                        PaymentModeID=a.PaymentModeID,
                        BankID=a.BankID,
                        InvoiceID = a.InvoiceID,
                        DiscountAmount = (decimal)a.DiscountAmount
                    }).ToList();
                    return LabTest;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LabTestItemBO> GetLabTestItems(int ID, int PatientLabTestMasterID,int IPID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetItemsForLabTest(ID, PatientLabTestMasterID,IPID).Select(a => new LabTestItemBO()
                    {
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        BiologicalReference = a.BiologicalReference,
                        ID = a.ID,
                        Unit = a.Unit,
                        ObserveValue = a.ObservedValue,
                        Status = a.Status,
                        Price = a.MRP,
                        Type = a.TestType,
                        IsBillGenerated = a.IsBillGenerated,
                        SalesInvoiceID=(int)a.SalesInvoiceID                    
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(LabTestBO labtestBO, List<LabTestItemBO> Items)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    int ReceivableID = 0;
                    ObjectParameter BillablesID = new ObjectParameter("BillablesID", typeof(int));
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter SalesOrderID = new ObjectParameter("SalesOrderID", typeof(int));
                    ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));         
                    ObjectParameter AccountHeadID = new ObjectParameter("AccountHeadID", typeof(int));
                    try
                    {
                        var j = dbEntity.SpUpdateSerialNo(
                                "ServiceSalesInvoice",
                                "Code",
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID,
                                SerialNo);
                        dbEntity.SpCreateLabTestInvoice(
                            SerialNo.Value.ToString(),
                            labtestBO.PatientID,
                            labtestBO.OPID,
                            labtestBO.IPID,
                            labtestBO.SalesTypeID,
                            labtestBO.PaymentModeID,
                            labtestBO.BankID,
                            labtestBO.DiscountAmount,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SalesOrderID,
                            SalesInvoiceID,                          
                            AccountHeadID
                            );
                        if (labtestBO.IPID>0 && labtestBO.SalesType != "Cash Sale")
                        {
                            dbEntity.SpCreateLabTestBillablesForIP(
                                   labtestBO.PatientID,
                                   labtestBO.OPID,
                                   labtestBO.IPID,
                                   Convert.ToInt32(SalesInvoiceID.Value),
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID,
                                   BillablesID
                              );

                        }
                        foreach (var item in Items)
                        {
                            //if (labtestBO.IPID <= 0)
                            //{
                            //    dbEntity.SpCreateLabTestBillables(
                            //       item.ItemID,
                            //       labtestBO.PatientID,
                            //       labtestBO.OPID,
                            //       labtestBO.IPID,
                            //       item.ID,
                            //       item.Price,
                            //       GeneralBO.CreatedUserID,
                            //       GeneralBO.FinYear,
                            //       GeneralBO.LocationID,
                            //       GeneralBO.ApplicationID,
                            //       BillablesID
                            //  );
                            //}
                            dbEntity.SpCreateLabTestInvoiceTrans(
                                       item.ItemID,
                                       labtestBO.PatientID,
                                       labtestBO.OPID,
                                       labtestBO.IPID,
                                       item.ID,
                                       item.Price,
                                       Convert.ToInt32(SalesOrderID.Value),
                                       Convert.ToInt32(SalesInvoiceID.Value),
                                       Convert.ToInt32(BillablesID.Value),
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.LocationID,
                                       GeneralBO.ApplicationID

                                  );
                            dbEntity.SpUpdateLabTestType(
                                   item.ID,
                                   Convert.ToInt32(SalesInvoiceID.Value),                                
                                   item.Type,
                                   item.IssueDate,
                                   item.SupplierID
                             );
                        }
                        dbEntity.SpUpdateLabTestInvoiceAmount
                            (
                             Convert.ToInt32(SalesOrderID.Value),
                                   Convert.ToInt32(SalesInvoiceID.Value),
                                   Convert.ToInt32(BillablesID.Value),
                                    ReceivableID,
                                   labtestBO.IPID,
                                   labtestBO.SalesTypeID,
                                   labtestBO.PaymentModeID,
                                   labtestBO.BankID,
                                   GeneralBO.CreatedUserID,
                                   GeneralBO.FinYear,
                                   GeneralBO.LocationID,
                                   GeneralBO.ApplicationID
                            );
                        ReceivableDAL receivableDAL = new ReceivableDAL();
                        ReceivablesBO receivableBO = new ReceivablesBO()
                        {
                            PartyID = labtestBO.PatientID,
                            TransDate = labtestBO.Date,
                            ReceivableType = "INVOICE",
                            ReferenceID = Convert.ToInt32(SalesInvoiceID.Value),
                            DocumentNo = SerialNo.Value.ToString(),
                            ReceivableAmount = labtestBO.NetAmount,
                            Description = "Service Sales Invoice ",
                            ReceivedAmount = 0,
                            Status = "",
                            //Discount = 0,
                            Discount = labtestBO.DiscountAmount

                        };
                        //ReceivableID = receivableDAL.SaveReceivables(receivableBO);
                        if (labtestBO.SalesType == "Cash Sale")
                        {
                            ReceivableID = receivableDAL.SaveReceivables(receivableBO);
                            ReceiptVoucherDAL ReceiptDAL = new ReceiptVoucherDAL();
                            ReceiptVoucherBO receiptVoucherBO = new ReceiptVoucherBO()
                            {
                                ReceiptDate = labtestBO.Date,
                                CustomerID = labtestBO.PatientID,
                                AccountHeadID = Convert.ToInt32(AccountHeadID.Value),
                                BankID = labtestBO.BankID,
                                ReceiptAmount = labtestBO.NetAmount,
                                PaymentTypeID = labtestBO.PaymentModeID,
                                Date = labtestBO.Date,
                                BankReferanceNumber = "",
                                Remarks = "",
                                IsDraft = false,
                                DiscountTypeID = 0,
                               // DiscountAmount = 0
                               DiscountAmount = labtestBO.DiscountAmount
                            };
                            ReceiptItemBO receiptItemBO = new ReceiptItemBO()
                            {
                                CreditNoteID = 0,
                                DebitNoteID = 0,
                                AdvanceReceivedAmount = 0,
                                ReceivableID = ReceivableID,
                                AdvanceID = 0,
                                DocumentType = "INVOICE",
                                DocumentNo = SerialNo.Value.ToString(),
                                ReceivableDate = labtestBO.Date,
                                Amount = labtestBO.NetAmount,
                                Balance = labtestBO.NetAmount,
                                AmountToBeMatched = labtestBO.NetAmount,
                                Status = "Settled",
                                PendingDays = 0,
                                SalesReturnID = 0,
                                CustomerReturnVoucherID = 0
                            };
                            List<ReceiptItemBO> item = new List<ReceiptItemBO>();
                            item.Add(receiptItemBO);

                            string StringSettlements;
                            List<ReceiptSettlementBO> Settlements = new List<ReceiptSettlementBO>();
                            ReceiptSettlementBO SettlementBO = new ReceiptSettlementBO
                            {
                                AdvanceID = 0,
                                CreditNoteID = 0,
                                DebitNoteID = 0,
                                ReceivableID = ReceivableID,
                                SalesReturnID = 0,
                                SettlementFrom = "DirectReceipt",
                                DocumentNo = SerialNo.Value.ToString(),
                                DocumentType = "INVOICE",
                                Amount = labtestBO.NetAmount,
                                SettlementAmount = labtestBO.NetAmount

                            };
                            Settlements.Add(SettlementBO);
                            XmlSerializer xmlSerializer = new XmlSerializer(Settlements.GetType());
                            using (StringWriter textWriter = new StringWriter())
                            {
                                xmlSerializer.Serialize(textWriter, Settlements);
                                StringSettlements = textWriter.ToString();
                            }
                            ReceiptDAL.SaveV3(receiptVoucherBO, item, StringSettlements);

                        }
                        transaction.Commit();
                        return Convert.ToInt32(SalesInvoiceID.Value);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }

        }
        public List<LabTestBO> GetLabTestDetailsForBilling(int ID, int SalesInvoiceID)
        {
            try
            {
                List<LabTestBO> LabTest = new List<LabTestBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    LabTest = dbEntity.SpGetBillableDetailsOfLabTest(ID, SalesInvoiceID, GeneralBO.FinYear, GeneralBO.ApplicationID, GeneralBO.LocationID).Select(a => new LabTestBO()
                    {
                        TransNo = a.TransNo,
                        InvoiceDate = (DateTime)a.CreatedDate,
                        Patient = a.PatientName,
                        PatientCode = a.PatientCode,
                        ItemName = a.Name,
                        NetAmount = (decimal)a.NetAmt,
                        price = (decimal)a.Price
                    }
                    ).ToList();

                    return LabTest;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LabTestBO> GetLabTestDetailsForPrint(int ID, int patientLabTestMasterID,int IPID)
        {
            try
            {
                List<LabTestBO> LabTest = new List<LabTestBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    LabTest = dbEntity.SpGetItemsForLabTestPrint(ID,patientLabTestMasterID,IPID).Select(a => new LabTestBO()
                    {
                        TransNo = a.TransNo,
                        PatientID = a.PatientID,                       
                        Patient = a.Patient,
                        PatientCode = a.PatientCode,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        Place = a.Place,
                        ItemID = (int)a.ItemID,
                        CreatedDate = a.CreatedDate,
                        ItemName = a.ItemName,
                        MRP = a.MRP
                    }
                    ).ToList();

                    return LabTest;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public LabTestItemBO GetLabTestItemsDetails(int ID, int patientID)
        {
            try
            {
                LabTestItemBO LabTest = new LabTestItemBO();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    LabTest = dbEntity.SpGetLabTestItemsDetails(ID, patientID).Select(a => new LabTestItemBO()
                    {
                        ItemID = (int)a.ID,
                        ItemName = a.ItemName,
                        BiologicalReference = a.BiologicalReference,
                        Unit = a.Unit,
                        ObserveValue = Convert.ToString(a.ObservedValue),
                        Price = (decimal)a.MRP
                    }
                    ).FirstOrDefault();

                    return LabTest;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<LabTestBO> GetPatientLabTestID(int appoinmentprocessID)
        {
            try
            {
                List<LabTestBO> LabTest = new List<LabTestBO>();
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    LabTest = dbEntity.SpGetNewPrescribedLabtest(appoinmentprocessID).Select(a => new LabTestBO()
                    {
                        ItemID = (int)a.ItemID,
                        PatientLabTestID = a.LabTestID,
                        AppointmentProcessID = a.AppointmentProcessID
                    }
                    ).ToList();

                    return LabTest;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int SaveLabTestItems(List<LabtestsBO> LabTestItems)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                try
                {
                    foreach (var item in LabTestItems)
                    {

                        dbEntity.SpCreateLabTestInInvoice(
                                       item.TestDate,
                                       item.OPID,
                                       item.IPID,
                                       item.LabTestID,
                                       item.PatientID,
                                       GeneralBO.CreatedUserID,
                                       GeneralBO.FinYear,
                                       GeneralBO.ApplicationID,
                                       GeneralBO.LocationID
                                        );
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return 1;
            }
        }
        public List<LabTestBO> GetLaboratoryInvoice(int InvoiceID)
        {
            try
            {
                List<LabTestBO> LabTest = new List<LabTestBO>();

                using (AHCMSEntities dEntity = new AHCMSEntities())
                {
                    LabTest = dEntity.SpGetLaboratoryInvoice(InvoiceID).Select(a => new LabTestBO
                    {
                        PatientID = a.PatientID,
                        Patient = a.Patient,
                        PatientCode = a.PatientCode,
                        Age = (int)a.Age,
                        Sex = a.Gender,
                        Mobile = a.MobileNo,
                        Doctor = a.Doctor,
                        PatientLabTestID = a.PatientLabTestID,
                        IPID = a.IPID,
                        //OPID = ID,
                        PatientTypeID = (int)a.PatientTypeID,
                        PaymentModeID = a.PaymentModeID,
                        BankID = a.BankID,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        NetAmount = (decimal)a.NetAmount
                    }).ToList();
                    return LabTest;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LabTestItemBO> GetLaboratoryInvoiceItems(int InvoiceID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetLaboratoryInvoiceItems(InvoiceID).Select(a => new LabTestItemBO()
                    {
                        ItemID = (int)a.ItemID,
                        ItemName = a.ItemName,
                        BiologicalReference = a.BiologicalReference,
                        ID = a.ID,
                        Unit = a.Unit,
                        ObserveValue = a.ObservedValue,
                        Status = a.Status,
                        Price = a.MRP,
                        Type = a.TestType,
                        IsBillGenerated = a.IsBillGenerated,
                        SalesInvoiceID = (int)a.SalesInvoiceID
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
