using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class LaboratoryTestResultBL : ILaboratoryTestResultContract
    {
        LaboratoryTestResultDAL labTestResultDAL;

        public LaboratoryTestResultBL()
        {
            labTestResultDAL = new LaboratoryTestResultDAL();
        }
        public DatatableResultBO GetInvoicedLabTestList(string Type, string InvoiceNo, string InvoiceDate, string Patient, string Doctor, string NetAmt, string SortField, string SortOrder, int Offset, int Limit)
        {
            return labTestResultDAL.GetInvoicedLabTestList(Type, InvoiceNo, InvoiceDate, Patient, Doctor, NetAmt, SortField, SortOrder, Offset, Limit);
        }
        public List<LaboratoryTestResultBO> GetInvoicedLabTestItems(int SalesInvoiceID)
        {
            return labTestResultDAL.GetInvoicedLabTestItems(SalesInvoiceID);
        }
        public int Save(List<LaboratoryTestResultBO> Items)
        {
            return labTestResultDAL.Save(Items);
        }
    }
}
