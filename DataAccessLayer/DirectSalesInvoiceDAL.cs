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
    public class DirectSalesInvoiceDAL
    {
        public JSONOutputBO Save(string XMLInvoice, string XMLItems, string XMLAmountDetails, string XMLPackingDetails, SalesInvoiceBO Invoice)
        {
            JSONOutputBO output = new JSONOutputBO();
            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter AccountHeadID = new ObjectParameter("AccountHeadID", typeof(int));
                        ObjectParameter RetDiscountValue = new ObjectParameter("RetDiscountValue", typeof(int));
                        ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(int));
                        int ReceivablesID = 0;
                        dbEntity.SpCreateSalesOrderDirect(
                           "",
                           Invoice.InvoiceDate,
                           Invoice.CustomerID,
                           Invoice.ItemCategoryID,
                           Invoice.SchemeAllocationID,
                           Invoice.InvoiceDate,
                           Invoice.FreightAmount,
                           Invoice.GrossAmount,
                           Invoice.DiscountAmount,
                           Invoice.TaxableAmount,
                           Invoice.SGSTAmount,
                           Invoice.CGSTAmount,
                           Invoice.IGSTAmount,
                           Invoice.CessAmount,
                           Invoice.RoundOff,
                           Invoice.NetAmount,
                           Invoice.PurchaseOrderID,
                           Invoice.FsoID,
                           Invoice.Source,
                           Invoice.BillingAddressID,
                           Invoice.ShippingAddressID,
                           Invoice.IsDraft,
                           Invoice.IsApproved,
                           Invoice.IPID,
                           //Invoice.AdditionalDiscount,
                           Invoice.OPID,
                           Invoice.SalesTypeID,
                           GeneralBO.CreatedUserID,
                           GeneralBO.FinYear,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID,
                           XMLItems
                           );

                        var i = dbEntity.SpCreateDirectSalesInvoice(
                            XMLInvoice,
                            XMLItems,
                            XMLAmountDetails,
                            XMLPackingDetails,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SalesInvoiceID,
                            RetValue,
                            RetDiscountValue,
                            SerialNo,
                            AccountHeadID
                            );
                        if (!Invoice.IsDraft && (Invoice.SalesType == "Cash Sale" || (Invoice.PatientType == "OP" && Invoice.SalesType == "Credit Sale")))
                        {
                            ReceivableDAL receivableBL = new ReceivableDAL();
                            ReceivablesBO receivableBO = new ReceivablesBO()
                            {
                                PartyID = Invoice.CustomerID,
                                TransDate = Invoice.InvoiceDate,
                                ReceivableType = "INVOICE",
                                ReferenceID = Convert.ToInt32(SalesInvoiceID.Value),
                                DocumentNo = SerialNo.Value.ToString(),
                                ReceivableAmount = Invoice.NetAmount,
                                Description = "Sales Invoice ",
                                ReceivedAmount = 0,
                                Status = "",
                                Discount = 0,
                            };
                            ReceivablesID = receivableBL.SaveReceivables(receivableBO);
                        }
                        if (!Invoice.IsDraft && Invoice.SalesType == "Cash Sale")
                        {
                            ReceiptVoucherDAL receivableDAL = new ReceiptVoucherDAL();
                            ReceiptVoucherBO receiptVoucherBO = new ReceiptVoucherBO()
                            {
                                ReceiptDate = Invoice.InvoiceDate,
                                CustomerID = Invoice.CustomerID,
                                AccountHeadID = Convert.ToInt32(AccountHeadID.Value),
                                BankID = Invoice.BankID,
                                ReceiptAmount = Invoice.NetAmount,
                                PaymentTypeID = Invoice.PaymentModeID,
                                Date = Invoice.InvoiceDate,
                                BankReferanceNumber = "",
                                Remarks = "",
                                IsDraft = false,
                                DiscountTypeID = 0,
                                DiscountAmount = 0
                            };
                            ReceiptItemBO receiptItemBO = new ReceiptItemBO()
                            {
                                CreditNoteID = 0,
                                DebitNoteID = 0,
                                AdvanceReceivedAmount = 0,
                                ReceivableID = ReceivablesID,
                                AdvanceID = 0,
                                DocumentType = "INVOICE",
                                DocumentNo = SerialNo.Value.ToString(),
                                ReceivableDate = Invoice.InvoiceDate,
                                Amount = Invoice.NetAmount,
                                Balance = Invoice.NetAmount,
                                AmountToBeMatched = Invoice.NetAmount,
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
                                ReceivableID = ReceivablesID,
                                SalesReturnID = 0,
                                SettlementFrom = "DirectReceipt",
                                DocumentNo = SerialNo.Value.ToString(),
                                DocumentType = "INVOICE",
                                Amount = Invoice.NetAmount,
                                SettlementAmount = Invoice.NetAmount

                            };
                            Settlements.Add(SettlementBO);
                            XmlSerializer xmlSerializer = new XmlSerializer(Settlements.GetType());
                            using (StringWriter textWriter = new StringWriter())
                            {
                                xmlSerializer.Serialize(textWriter, Settlements);
                                StringSettlements = textWriter.ToString();
                            }
                            receivableDAL.SaveV3(receiptVoucherBO, item, StringSettlements);

                        }
                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new Exception("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new Exception("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new Exception("Cancelled  sales orders / proforma invoices are selected");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new Exception("Some items quantity already met");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -5)
                        {
                            throw new Exception("Credit Limit exceeded");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -6)
                        {
                            throw new Exception("Net amount is below minimum billing amount");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -7)
                        {
                            throw new Exception("Credit days exceeded");
                        }
                        if (Convert.ToInt32(RetDiscountValue.Value) == -1)
                        {
                            throw new Exception("Discount exceeded");
                        }
                        InvoiceID = Convert.ToInt32(SalesInvoiceID.Value);
                        output.Data = new OutputDataBO
                        {
                            ID = InvoiceID,
                            TransNo = SerialNo.Value.ToString()
                        };
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return output;
        }

        public DatatableResultBO GetDirectSalesInvoiceList(string Type, string CodeHint, string DateHint, string SalesTypeHint, string CustomerNameHint, string LocationHint, string DoctorHint, string NetAmountHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetDirectSalesInvoiceList(Type, CodeHint, DateHint, SalesTypeHint, CustomerNameHint, LocationHint, DoctorHint, NetAmountHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                TransNo = item.TransNo,
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                SalesType = item.SalesType,
                                CustomerName = item.CustomerName,
                                Location = item.Location,
                                Doctor = item.Doctor,
                                NetAmount = (decimal)item.NetAmount,
                                Status = (bool)item.IsCancelled ? "cancelled" : (bool)item.IsProcessed ? "processed" : (bool)item.IsDraft ? "draft" : ""
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public SalesInvoiceBO GetDirectSalesInvoice(int SalesInvoiceID, int LocationID)
        {
            SalesInvoiceBO Invoice = new SalesInvoiceBO();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {

                    Invoice = dbEntity.SpGetDirectSalesInvoice(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(a => new SalesInvoiceBO()
                    {
                        ID = a.ID,
                        InvoiceNo = a.TransNo,
                        InvoiceDate = (DateTime)a.InvoiceDate,
                        CustomerID = (int)a.CustomerID,
                        SalesOrderNos = a.SalesOrders,
                        SchemeID = (int)a.SchemeID,
                        GrossAmount = (decimal)a.GrossAmt,
                        DiscountAmount = (decimal)a.DiscountAmt,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        PaymentModeID = (int)a.PaymentModeID,
                        PaymentMode = a.PaymentMode,
                        FreightAmount = (decimal)a.FreightAmount,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TaxableAmount = (decimal)a.TaxableAmt,
                        SGSTAmount = (decimal)a.SGSTAmt,
                        CGSTAmount = (decimal)a.CGSTAmt,
                        IGSTAmount = (decimal)a.IGSTAmt,
                        RoundOff = (decimal)a.RoundOff,
                        NetAmount = (decimal)a.NetAmt,
                        IsProcessed = (bool)a.IsProcessed,
                        IsDraft = (bool)a.IsDraft,
                        IsCancelled = (bool)a.IsCancelled,
                        CheckStock = (bool)a.HoldStock,
                        SalesTypeID = (int)a.SalesTypeID,
                        SalesTypeName = a.SalesTypeName,
                        CustomerName = a.CustomerName,
                        CustomerCategory = a.CustomerCategory,
                        CustomerCategoryID = (int)a.CustomerCategoryID,
                        PriceListID = (int)a.PriceListID,
                        StateID = (int)a.StateID,
                        BillingAddressID = (int)a.BillingAddressID,
                        ShippingAddressID = (int)a.ShippingAddressID,
                        BillingAddress = a.BillingAddress,
                        ShippingAddress = a.ShippingAddress,
                        NoOfBags = (int)a.NoOfBags,
                        NoOfBoxes = (int)a.NoOfBoxes,
                        NoOfCans = (int)a.NoOfCans,
                        CashDiscount = (decimal)a.CashDiscount,
                        CessAmount = (decimal)a.CessAmount,
                        CreditAmount = (decimal)a.CreditBalance,
                        MinCreditLimit = (decimal)a.MinimumCreditLimit,
                        MaxCreditLimit = (decimal)a.MaxCreditLimit,
                        CashDiscountPercentage = (decimal)a.CashDiscountPercentage,
                        OutstandingAmount = (decimal)a.OutstandingAmount,
                        VehicleNo = a.VehicleNo,
                        CustomerGSTNo = a.CustomerGSTNo,
                        Remarks = a.Remarks,
                        IsGSTRegistered = (bool)a.IsGSTRegistered,
                        BankID = (int)a.BankID,
                        BankName = a.BankName,
                        WarehouseID = (int)a.WareHouseID,
                        WarehouseName = a.WarehouseName,
                        DoctorID = (int)a.DoctorID,
                        DoctorName = a.DoctorName,
                        DiscountCategoryID = a.DiscountCategoryID,
                        DiscountCategory = a.DiscountCategory,
                        IPID = a.IPID,
                        OPID = a.OPID,
                        PaymentTypeID = (int)a.PatientTypeID,
                        CustomerCode = a.CustomerCode,
                        Form = a.Form
                    }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Invoice;
        }

        public List<SalesItemBO> GetDirectSalesInvoiceItems(int SalesInvoiceID, int LocationID)
        {
            List<SalesItemBO> Items = new List<SalesItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    Items = dbEntity.SpGetDirectSalesInvoiceItems(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesInvoiceTransID = a.ID,
                        ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                        SalesOrderItemID = (int)a.SalesOrderTransID,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        ItemID = (int)a.ItemID,
                        Qty = (decimal)a.Quantity,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        BatchName = a.BatchName,
                        Stock = (decimal)a.Stock,
                        MRP = (decimal)a.MRP,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.IGSTPercentage,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        NetAmount = (decimal)a.NetAmt,
                        StoreID = (int)a.WareHouseID,
                        Name = a.ItemName,
                        UnitName = a.UnitName,
                        Code = a.Code,
                        UnitID = (int)a.UnitID,
                        LooseRate = (decimal)a.LooseRate,
                        Rate = (decimal)a.Rate,
                        SalesUnitID = (int)a.SalesUnitID,
                        CashDiscount = (decimal)a.CashDiscount,
                        POID = a.PurchaseOrderID,
                        POTransID = a.POTransID,
                        SalesInvoiceID = (int)a.SalesInvoiceID,
                        PORate = (decimal)a.PORate,
                        POQuantity = (decimal)a.POQuantity,
                        CessAmount = (decimal)a.CessAmount,
                        CessPercentage = (decimal)a.CessPercentage,
                        ExpiryDate = a.ExpiryDate,
                        PackSize = (decimal)a.PackSize,
                        BatchTypeName = a.BatchType,
                        PrimaryUnit = a.PrimaryUnit,
                        ItemCategoryID = a.CategoryID,
                        Form = a.Form,
                        MedicineIssueID = (int)a.MedicineIssueID,
                        MedicineIssueTransID = (int)a.MedicineIssueTransID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public List<SalesItemBO> GetMedicineIssueItemsForDirectSalesInvoice(string MedicineIssueType, int MedicineIssuedToID)
        {
            List<SalesItemBO> Items = new List<SalesItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    Items = dbEntity.SpGetMedicineIssueItemsForDirectSalesInvoice(MedicineIssueType,MedicineIssuedToID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesItemBO()
                    {
                        SalesInvoiceTransID = a.ID,
                        ProformaInvoiceTransID = (int)a.ProformaInvoiceTransID,
                        SalesOrderItemID = (int)a.SalesOrderTransID,
                        BatchID = (int)a.BatchID,
                        BatchTypeID = (int)a.BatchTypeID,
                        ItemID = (int)a.ItemID,
                        Qty = (decimal)a.Quantity,
                        OfferQty = (decimal)a.OfferQty,
                        InvoiceQty = (decimal)a.InvoiceQty,
                        InvoiceOfferQty = (decimal)a.InvoiceOfferQty,
                        BatchName = a.BatchName,
                        Stock = (decimal)a.Stock,
                        MRP = (decimal)a.MRP,
                        BasicPrice = (decimal)a.BasicPrice,
                        GrossAmount = (decimal)a.GrossAmount,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        DiscountAmount = (decimal)a.DiscountAmount,
                        AdditionalDiscount = (decimal)a.AdditionalDiscount,
                        TurnoverDiscount = (decimal)a.TurnoverDiscount,
                        TaxableAmount = (decimal)a.TaxableAmount,
                        SGSTPercentage = (decimal)a.SGSTPercentage,
                        CGSTPercentage = (decimal)a.CGSTPercentage,
                        IGSTPercentage = (decimal)a.IGSTPercentage,
                        GSTPercentage = (decimal)a.IGSTPercentage,
                        SGST = (decimal)a.SGSTAmt,
                        CGST = (decimal)a.CGSTAmt,
                        IGST = (decimal)a.IGSTAmt,
                        NetAmount = (decimal)a.NetAmt,
                        StoreID = (int)a.WareHouseID,
                        Name = a.ItemName,
                        UnitName = a.UnitName,
                        Code = a.Code,
                        UnitID = (int)a.UnitID,
                        LooseRate = (decimal)a.LooseRate,
                        Rate = (decimal)a.Rate,
                        SalesUnitID = (int)a.SalesUnitID,
                        CashDiscount = (decimal)a.CashDiscount,
                        POID = a.PurchaseOrderID,
                        POTransID = a.POTransID,
                        SalesInvoiceID = (int)a.SalesInvoiceID,
                        PORate = (decimal)a.PORate,
                        POQuantity = (decimal)a.POQuantity,
                        CessAmount = (decimal)a.CessAmount,
                        CessPercentage = (decimal)a.CessPercentage,
                        ExpiryDate = a.ExpiryDate,
                        PackSize = (decimal)a.PackSize,
                        BatchTypeName = a.BatchType,
                        PrimaryUnit = a.PrimaryUnit,
                        ItemCategoryID = a.CategoryID,
                        Form = a.Form,
                        MedicineIssueID = a.MedicineIssueID,
                        MedicineIssueTransID = a.MedicineIssueTransID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return Items;
        }

        public List<SalesAmountBO> GetDirectSalesInvoiceAmountDetails(int SalesInvoiceID, int LocationID)
        {
            List<SalesAmountBO> AmountDetails = new List<SalesAmountBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    AmountDetails = dbEntity.SpGetDirectSalesInvoiceAmountDetails(SalesInvoiceID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID).Select(a => new SalesAmountBO()
                    {
                        Amount = (decimal)a.Amount,
                        Particulars = a.Particulars,
                        Percentage = (decimal)a.Percentage,
                        TaxableAmount = (decimal)a.TaxableAmount
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return AmountDetails;
        }

        public List<SalesPackingDetailsBO> GetDirectSalesInvoicePackingDetails(int SalesInvoiceID, int LocationID)
        {
            List<SalesPackingDetailsBO> PackingDetails = new List<SalesPackingDetailsBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    PackingDetails = dbEntity.SpGetPackingDetails(SalesInvoiceID, "SalesInvoice", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SalesPackingDetailsBO()
                    {
                        UnitName = a.Unit,
                        UnitID = (int)a.UnitID,
                        PackSize = a.PackSize,
                        Quantity = (decimal)a.Quantity

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return PackingDetails;
        }

        public int Update(string XMLInvoice, string XMLItems, string XMLAmountDetails, int SalesInvoiceID, string XMLPackingDetails, SalesInvoiceBO Invoice)
        {

            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter AccountHeadID = new ObjectParameter("AccountHeadID", typeof(int));
                        ObjectParameter SalesInvoiceNo = new ObjectParameter("SalesInvoiceNo", typeof(string));
                        int ReceivablesID = 0;
                        dbEntity.SpSUpdateSalesOrder(
                       Invoice.ID,
                       "",
                       DateTime.Now,
                       Invoice.CustomerID,
                       Invoice.SchemeAllocationID,
                       Invoice.InvoiceDate,
                       Invoice.FreightAmount,
                       Invoice.GrossAmount,
                       Invoice.DiscountAmount,
                       Invoice.TaxableAmount,
                       Invoice.SGSTAmount,
                       Invoice.CGSTAmount,
                       Invoice.IGSTAmount,
                       Invoice.CessAmount,
                       Invoice.RoundOff,
                       "",
                       null,
                       "",
                       Invoice.NetAmount,
                       Invoice.BillingAddressID,
                       Invoice.ShippingAddressID,
                       Invoice.IsDraft,
                       Invoice.SalesTypeID,
                       0,
                       0,
                       0,
                       0,
                       Invoice.EnquiryDate,
                        Invoice.Remarks,
                       0,
                       0,0,0,
                       //Invoice.AdditionalDiscount,
                       GeneralBO.CreatedUserID,
                       GeneralBO.FinYear,
                       GeneralBO.LocationID,
                       GeneralBO.ApplicationID,
                       Invoice.IsApproved,
                       XMLItems
                       );
                        dbEntity.SpUpdateDirectSalesInvoice(SalesInvoiceID, XMLInvoice, XMLItems, XMLAmountDetails, XMLPackingDetails, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, RetValue, AccountHeadID, SalesInvoiceNo);

                        if (!Invoice.IsDraft && (Invoice.SalesType == "Cash Sale" || (Invoice.PatientType == "OP" && Invoice.SalesType == "Credit Sale")))
                        {
                            ReceivableDAL receivableBL = new ReceivableDAL();
                            ReceivablesBO receivableBO = new ReceivablesBO()
                            {
                                PartyID = Invoice.CustomerID,
                                TransDate = Invoice.InvoiceDate,
                                ReceivableType = "INVOICE",
                                ReferenceID = Invoice.ID,
                                DocumentNo = Invoice.InvoiceNo,
                                ReceivableAmount = Invoice.NetAmount,
                                Description = "Sales Invoice ",
                                ReceivedAmount = 0,
                                Status = "",
                                Discount = 0,
                            };
                            ReceivablesID = receivableBL.SaveReceivables(receivableBO);
                        }
                        if (!Invoice.IsDraft && Invoice.SalesType == "Cash Sale")
                        {
                            ReceiptVoucherDAL receivableDAL = new ReceiptVoucherDAL();
                            ReceiptVoucherBO receiptVoucherBO = new ReceiptVoucherBO()
                            {
                                ReceiptDate = Invoice.InvoiceDate,
                                CustomerID = Invoice.CustomerID,
                                AccountHeadID = Convert.ToInt32(AccountHeadID.Value),
                                BankID = Invoice.BankID,
                                ReceiptAmount = Invoice.NetAmount,
                                PaymentTypeID = Invoice.PaymentModeID,
                                Date = Invoice.InvoiceDate,
                                BankReferanceNumber = "",
                                Remarks = "",
                                IsDraft = false,
                                DiscountTypeID = 0,
                                DiscountAmount = 0
                            };
                            ReceiptItemBO receiptItemBO = new ReceiptItemBO()
                            {
                                CreditNoteID = 0,
                                DebitNoteID = 0,
                                AdvanceReceivedAmount = 0,
                                ReceivableID = ReceivablesID,
                                AdvanceID = 0,
                                DocumentType = "INVOICE",
                                DocumentNo = SalesInvoiceNo.Value.ToString(),
                                ReceivableDate = Invoice.InvoiceDate,
                                Amount = Invoice.NetAmount,
                                Balance = Invoice.NetAmount,
                                AmountToBeMatched = Invoice.NetAmount,
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
                                ReceivableID = ReceivablesID,
                                SalesReturnID = 0,
                                SettlementFrom = "DirectReceipt",
                                DocumentNo = SalesInvoiceNo.Value.ToString(),
                                DocumentType = "INVOICE",
                                Amount = Invoice.NetAmount,
                                SettlementAmount = Invoice.NetAmount

                            };
                            Settlements.Add(SettlementBO);
                            XmlSerializer xmlSerializer = new XmlSerializer(Settlements.GetType());
                            using (StringWriter textWriter = new StringWriter())
                            {
                                xmlSerializer.Serialize(textWriter, Settlements);
                                StringSettlements = textWriter.ToString();
                            }
                            receivableDAL.SaveV3(receiptVoucherBO, item, StringSettlements);

                        }

                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new Exception("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new Exception("Some items quantity already met");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new Exception("Cancelled  sales orders / proforma invoices are selected");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new Exception("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -5)
                        {
                            throw new Exception("Credit Limit exceeded");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -6)
                        {
                            throw new Exception("Net amount is below minimum billing amount");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -7)
                        {
                            throw new Exception("Credit days exceeded");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -8)
                        {
                            throw new Exception("Invoice already Processed in Sevice Bill");
                        }
                        InvoiceID = SalesInvoiceID;
                        dbEntity.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return InvoiceID;
        }

        public List<SalesInvoiceBO> GetSalesType()
        {
            try
            {
                List<SalesInvoiceBO> SalesType = new List<SalesInvoiceBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSalesTypeList().Select(a => new SalesInvoiceBO
                    {
                        ID = a.ID,
                        SalesType = a.Name
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SalesInvoiceBO> GetPatientTypeList()
        {
            try
            {
                List<SalesInvoiceBO> SalesType = new List<SalesInvoiceBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPatientTypeList().Select(a => new SalesInvoiceBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JSONOutputBO SaveV3(string XMLInvoice, string XMLItems, string XMLAmountDetails, string XMLPackingDetails, SalesInvoiceBO Invoice)
        {
            JSONOutputBO output = new JSONOutputBO();
            int InvoiceID = 0;

            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(int));
                        ObjectParameter AccountHeadID = new ObjectParameter("AccountHeadID", typeof(int));
                        ObjectParameter SalesInvoiceID = new ObjectParameter("SalesInvoiceID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(int));
                        ObjectParameter RetDiscountValue = new ObjectParameter("RetDiscountValue", typeof(int));
                        int ReceivablesID = 0;
                        dbEntity.SpCreateSalesOrderDirect(
                           "",
                           Invoice.InvoiceDate,
                           Invoice.CustomerID,
                           Invoice.ItemCategoryID,
                           Invoice.SchemeAllocationID,
                           Invoice.InvoiceDate,
                           Invoice.FreightAmount,
                           Invoice.GrossAmount,
                           Invoice.DiscountAmount,
                           Invoice.TaxableAmount,
                           Invoice.SGSTAmount,
                           Invoice.CGSTAmount,
                           Invoice.IGSTAmount,
                           Invoice.CessAmount,
                           Invoice.RoundOff,
                           Invoice.NetAmount,
                           Invoice.PurchaseOrderID,
                           Invoice.FsoID,
                           Invoice.Source,
                           Invoice.BillingAddressID,
                           Invoice.ShippingAddressID,
                           Invoice.IsDraft,
                           Invoice.IsApproved,
                           Invoice.IPID,
                           //Invoice.AdditionalDiscount,
                           Invoice.OPID,
                           Invoice.SalesTypeID,
                           GeneralBO.CreatedUserID,
                           GeneralBO.FinYear,
                           GeneralBO.LocationID,
                           GeneralBO.ApplicationID,
                           XMLItems
                           );

                        var i = dbEntity.SpCreateDirectSalesInvoice(
                            XMLInvoice,
                            XMLItems,
                            XMLAmountDetails,
                            XMLPackingDetails,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            SalesInvoiceID,
                            RetValue,
                            RetDiscountValue,
                            SerialNo,
                            AccountHeadID
                            );


                        if (!Invoice.IsDraft && (Invoice.SalesType == "Cash Sale" || (Invoice.PatientType == "OP" && Invoice.SalesType == "Credit Sale")))
                        {
                            ReceivableDAL receivableBL = new ReceivableDAL();
                            ReceivablesBO receivableBO = new ReceivablesBO()
                            {
                                PartyID = Invoice.CustomerID,
                                TransDate = Invoice.InvoiceDate,
                                ReceivableType = "INVOICE",
                                ReferenceID = Convert.ToInt32(SalesInvoiceID.Value),
                                DocumentNo = SerialNo.Value.ToString(),
                                ReceivableAmount = Invoice.NetAmount,
                                Description = "Sales Invoice ",
                                ReceivedAmount = 0,
                                Status = "",
                                Discount = 0,
                            };
                            ReceivablesID = receivableBL.SaveReceivables(receivableBO);
                        }
                        if (!Invoice.IsDraft && Invoice.SalesType == "Cash Sale")
                        {
                            ReceiptVoucherDAL receivableDAL = new ReceiptVoucherDAL();
                            ReceiptVoucherBO receiptVoucherBO = new ReceiptVoucherBO()
                            {
                                ReceiptDate = Invoice.InvoiceDate,
                                CustomerID = Invoice.CustomerID,
                                AccountHeadID = Convert.ToInt32(AccountHeadID.Value),
                                BankID = Invoice.BankID,
                                ReceiptAmount = Invoice.NetAmount,
                                PaymentTypeID = Invoice.PaymentModeID,
                                Date = Invoice.InvoiceDate,
                                BankReferanceNumber = "",
                                Remarks = "",
                                IsDraft = false,
                                DiscountTypeID = 0,
                                DiscountAmount = 0
                            };
                            ReceiptItemBO receiptItemBO = new ReceiptItemBO()
                            {
                                CreditNoteID = 0,
                                DebitNoteID = 0,
                                AdvanceReceivedAmount = 0,
                                ReceivableID = ReceivablesID,
                                AdvanceID = 0,
                                DocumentType = "INVOICE",
                                DocumentNo = SerialNo.Value.ToString(),
                                ReceivableDate = Invoice.InvoiceDate,
                                Amount = Invoice.NetAmount,
                                Balance = Invoice.NetAmount,
                                AmountToBeMatched = Invoice.NetAmount,
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
                                ReceivableID = ReceivablesID,
                                SalesReturnID = 0,
                                SettlementFrom = "DirectReceipt",
                                DocumentNo = SerialNo.Value.ToString(),
                                DocumentType = "INVOICE",
                                Amount = Invoice.NetAmount,
                                SettlementAmount = Invoice.NetAmount

                            };
                            Settlements.Add(SettlementBO);
                            XmlSerializer xmlSerializer = new XmlSerializer(Settlements.GetType());
                            using (StringWriter textWriter = new StringWriter())
                            {
                                xmlSerializer.Serialize(textWriter, Settlements);
                                StringSettlements = textWriter.ToString();
                            }
                            receivableDAL.SaveV3(receiptVoucherBO, item, StringSettlements);

                        }

                        if (Convert.ToInt32(RetValue.Value) == -2)
                        {
                            throw new Exception("Item out of stock");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -1)
                        {
                            throw new Exception("Database error");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -3)
                        {
                            throw new Exception("Cancelled  sales orders / proforma invoices are selected");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -4)
                        {
                            throw new Exception("Some items quantity already met");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -5)
                        {
                            throw new Exception("Credit Limit exceeded");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -6)
                        {
                            throw new Exception("Net amount is below minimum billing amount");
                        }
                        if (Convert.ToInt32(RetValue.Value) == -7)
                        {
                            throw new Exception("Credit days exceeded");
                        }
                        InvoiceID = Convert.ToInt32(SalesInvoiceID.Value);
                        output.Data = new OutputDataBO
                        {
                            ID = InvoiceID,
                            TransNo = SerialNo.Value.ToString()
                        };
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            return output;
        }
    }
}
