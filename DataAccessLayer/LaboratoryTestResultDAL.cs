using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LaboratoryTestResultDAL
    {
        public DatatableResultBO GetInvoicedLabTestList(string Type, string InvoiceNo, string InvoiceDate, string Patient, string Doctor, string NetAmt, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    var result = dbEntity.SpGetInvoicedLabTestList(Type, InvoiceNo, InvoiceDate, Patient, Doctor, NetAmt, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                InvoiceNo = item.TransNo,
                                InvoiceDate = ((DateTime)item.InvoiceDate).ToString("dd-MMM-yyyy"),
                                GrossAmt = item.GrossAmt,
                                Patient = item.Patient,
                                PatientID = item.PatientID,
                                Doctor = item.Doctor,
                                IPID = item.IPID,
                                SalesInvoiceID = item.SalesInvoiceID,
                                OPID = item.OPID
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

        public List<LaboratoryTestResultBO> GetInvoicedLabTestItems(int SalesInvoiceID)
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {

                    return dbEntity.SpGetInvoicedLabTestItems(SalesInvoiceID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new LaboratoryTestResultBO()
                    {
                        PatientLabTestsID = a.PatientLabTestsID,
                        BillablesID = a.BillablesID,
                        ItemID = a.ItemID,
                        Item = a.Item,
                        Unit = a.Unit,
                        BiologicalReference = a.BiologicalReference,
                        IsProcessed = (bool)a.IsProcessed,
                        Status = a.Status,
                        NormalHighLevel = a.HighReference,
                        NormalLowLevel = a.LowReference,
                        ObservedValue = a.ObservedValue,
                        SpecimenID = a.SpecimenID,
                        Specimen = a.Specimen,
                        PatientLabTestTransID = a.PatientLabTestTransID,
                        DocumentID = a.DocumentID,
                        ReportedTime = a.ReportedTime,
                        CollectedTime = a.CollectedTime,
                        CollectedDate=a.CollectedDate,
                        ReportedDate=a.ReportedDate,
                        Type=a.Type,
                        ReportTime =(DateTime) a.ReportTime,
                        CollectTime =(DateTime) a.CollectTime,
                        Method =a.Method,
                        Test=a.Test
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Save(List<LaboratoryTestResultBO> Items)
        {
            using (AHCMSEntities dbEntity = new AHCMSEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in Items)
                        {
                            dbEntity.SpUpdateLabTestResult(
                            item.PatientLabTestsID,
                            item.PatientLabTestTransID,
                            item.Status,
                            item.ObservedValue,
                            item.DocumentID,
                            item.CollectedTime,
                            item.ReportedTime
                                );
                        }
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

    }
}
