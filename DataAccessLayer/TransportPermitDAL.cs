using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class TransportPermitDAL
    {
        public List<TransportPermitItemBO> PendingPermitList(TransportPermitBO permitBO)
        {

            List<TransportPermitItemBO> item = new List<TransportPermitItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    item = dbEntity.SpGetItemForTransportPermit(permitBO.StockTransferNoFromID, permitBO.StockTransferNoToID, permitBO.SalesInvoiceNoFromID, permitBO.SalesInvoiceNoToID,
                        permitBO.Type, permitBO.FromDate, permitBO.ToDate, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(x => new TransportPermitItemBO
                        {
                            TransNo = x.TransNo,
                            TransDate = (DateTime)x.TransDate,
                            TransID = x.ID,
                            CustomerName = x.CustomerName,
                            CustomerID = x.CustomerID,
                            LocationID = x.LocationID,

                            District = x.District == null ? "" : x.District,
                            DistrictID = (int)x.DistrictID,
                            Type = x.Type,

                        }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }

        public int SaveTransportPermit(TransportPermitBO permitBO, List<TransportPermitItemBO> itemBO)
        {
            using (SalesEntities entities = new SalesEntities())
            {
                using (var transaction = entities.Database.BeginTransaction())
                {

                    try
                    {
                        ObjectParameter id = new ObjectParameter("RetVal", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        var j = entities.SpUpdateSerialNo("Transport Permit", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                        var i = entities.SpCreateTransportPermit(
                           SerialNo.Value.ToString(),
                            permitBO.DriverName,
                            permitBO.VehicleNumber,
                            permitBO.ValidFromDate,
                            permitBO.ValidToDate,
                            permitBO.FromDate,
                            permitBO.ToDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,
                             id);
                        if (id.Value != null)
                        {
                            foreach (var itm in itemBO)
                            {
                                entities.SpCreateTransportPermitTrans(
                                    Convert.ToInt32(id.Value),
                                    itm.Type,
                                    itm.TransNo,
                                    itm.TransDate,
                                    itm.TransID,
                                    itm.LocationID,
                                    itm.CustomerID,
                                    itm.DistrictID,
                                    itm.ItemID,
                                    itm.BatchTypeID,
                                    itm.Quantity,
                                    GeneralBO.FinYear,
                                    GeneralBO.LocationID,
                                    GeneralBO.ApplicationID
                                        );
                            }

                        };
                        transaction.Commit();
                        return Convert.ToInt32(id.Value.ToString());
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

        public List<TransportPermitBO> GetTransportPermitList()
        {
            List<TransportPermitBO> item = new List<TransportPermitBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    item = dbEntity.SpGetTransportPermitList(
                        GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID)
                        .Select(x => new TransportPermitBO
                        {
                            TransNo = x.TransNo,

                            ID = x.ID,
                            ValidToDate = (DateTime)x.ValidToDate,
                            ValidFromDate = (DateTime)x.ValidFromDate

                        }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }

        public List<TransportPermitItemBO> GetTransportPermitTrans(int ID)
        {
            List<TransportPermitItemBO> item = new List<TransportPermitItemBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    item = dbEntity.SpGetTransportPermitTrans(
                        ID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID)
                        .Select(x => new TransportPermitItemBO
                        {
                            TransNo = x.TransNo,
                            TransDate = (DateTime)x.TransDate,
                            CustomerName = x.CustomerName,
                            District = x.DistrictName == null ? "" : x.DistrictName,
                            Type = x.Type,

                        }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }

        public List<TransportPermitBO> GetTransportPermit(int ID)
        {
            List<TransportPermitBO> item = new List<TransportPermitBO>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    item = dbEntity.SpGetTransportPermitDetail(
                        ID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID)
                        .Select(x => new TransportPermitBO
                        {
                            TransNo = x.TransNo,
                            ValidFromDate = (DateTime)x.ValidFromDate,
                            ValidToDate = (DateTime)x.ValidToDate,
                            DriverName = x.driver,
                            VehicleNumber = x.VehicleNo

                        }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }

        public DatatableResultBO GetTransportPermitListForDataTable(string TransNo, string ValidFromdate, string ValidTodate, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetTransportPermitListForDataTable(TransNo, ValidFromdate, ValidTodate, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                ValidFromdate = ((DateTime)item.ValidFromDate).ToString("dd-MMM-yyyy"),
                                ValidTodate = ((DateTime)item.ValidToDate).ToString("dd-MMM-yyyy")
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
    }
}

