using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;
using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class GatePassDAL
    {
        public bool SaveGatePass(GatePassBO gatePassBO, List<GatePassItemsBO> ListItem)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        string FormName = "GatePass";
                        ObjectParameter GpId = new ObjectParameter("GatePassID", typeof(int));
                        ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                        if (gatePassBO.IsDraft)
                        {
                            FormName = "DraftGatePass";
                        }
                        var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                        dbEntity.SaveChanges();
                        var i = dbEntity.SpCreateGatePass(
                            SerialNo.Value.ToString(),
                            gatePassBO.TransDate,
                            gatePassBO.FromDate,
                            gatePassBO.ToDate,
                            gatePassBO.Salesman,
                            gatePassBO.VehicleNoID,
                            gatePassBO.DespatchDateTime,
                            gatePassBO.Time,
                            gatePassBO.DriverID,
                            gatePassBO.DrivingLicense,
                            gatePassBO.VehicleOwner,
                            gatePassBO.TransportingAgency,
                            gatePassBO.HelperName,
                            gatePassBO.Area,
                            gatePassBO.StartingKilometer,
                            gatePassBO.IssuedBy,
                            gatePassBO.BagCount,
                            gatePassBO.CanCount,
                            gatePassBO.BoxCount,
                            gatePassBO.TotalAmount,
                            gatePassBO.IsDraft,
                            gatePassBO.VehicleNo,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            GpId);
                        if (GpId.Value != null)
                        {
                            foreach (var item in ListItem)
                            {
                                dbEntity.SpCreateGatePassTrans(
                                Convert.ToInt16(GpId.Value),
                                item.ID,
                                item.Amount,
                                item.Area,
                                item.PPSNO,
                                item.Type,
                                item.NoOfBags,
                                item.NoOfboxes,
                                item.NoOfCans,
                                GeneralBO.FinYear,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                                );
                            }
                        };
                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public bool UpdateGatePass(GatePassBO gatePassBO, List<GatePassItemsBO> ListItem)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {

                        dbEntity.SaveChanges();
                        var i = dbEntity.SpUpdateGatePass(
                            gatePassBO.ID,
                            gatePassBO.TransDate,
                            gatePassBO.Salesman,
                            gatePassBO.VehicleNoID,
                            gatePassBO.DespatchDateTime,
                            gatePassBO.Time,
                            gatePassBO.DriverID,
                            gatePassBO.DrivingLicense,
                            gatePassBO.VehicleOwner,
                            gatePassBO.TransportingAgency,
                            gatePassBO.HelperName,
                            gatePassBO.Area,
                            gatePassBO.StartingKilometer,
                            gatePassBO.IssuedBy,
                            gatePassBO.BagCount,
                            gatePassBO.CanCount,
                            gatePassBO.BoxCount,
                            gatePassBO.TotalAmount,
                            gatePassBO.IsDraft,
                            gatePassBO.VehicleNo,
                            GeneralBO.CreatedUserID,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            
                            );
                        dbEntity.SaveChanges();
                        foreach (var item in ListItem)
                        {
                            dbEntity.SpCreateGatePassTrans(
                            gatePassBO.ID,
                            item.ID,
                            item.Amount,
                            item.Area,
                            item.PPSNO,
                            item.Type,
                            item.NoOfBags,
                            item.NoOfboxes,
                            item.NoOfCans,
                            GeneralBO.FinYear,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID
                            );
                        };
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }
        public List<GatePassBO> GetGatePassList()
        {
            try
            {
                List<GatePassBO> GatePassBO = new List<GatePassBO>();
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return GatePassBO = dbEntity.SpGetGatePass(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GatePassBO()
                    {
                        ID = m.ID,
                        TransNo = m.TransNo,
                        TransDate = (DateTime)m.TransDate,
                        FromDate = (DateTime)m.InvoiceFromDate,
                        ToDate = (DateTime)m.InvoiceToDate,
                        Salesman = m.Salesman,
                        DespatchDateTime = (DateTime)m.DespatchDateTime,
                        VehicleNoID = (int)m.vehicleID,
                        VehicleNo = m.VehicleNo,
                        DriverName = m.DriverName,
                        DriverID = (int)m.DriverID,
                        Area = m.Area,
                        TotalAmount = (decimal)m.TotalInvoiceAmount,
                        IsDraft = (bool)m.IsDraft,
                        IsCancelled = (bool)m.IsCancelled

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GatePassBO> GetGatePassDetails(int ID)
        {
            try
            {
                List<GatePassBO> GatePassBO = new List<GatePassBO>();
                using (SalesEntities dbEntity = new SalesEntities())
                {

                    return GatePassBO = dbEntity.SpGetGatePassDetail(ID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GatePassBO()
                    {
                        ID = m.ID,
                        TransNo = m.TransNo,
                        FromDate = (DateTime)m.InvoiceFromDate,
                        ToDate = (DateTime)m.InvoiceToDate,
                        Salesman = m.Salesman,
                        VehicleNo = m.VehicleNo,
                        VehicleNoID = (int)m.VehicleID,
                        DespatchDateTime = (DateTime)m.DespatchDateTime,
                        Time = m.Time,
                        DriverName = m.DriverName,
                        DriverID = (int)m.DriverID,
                        DrivingLicense = m.DrivingLicenseNo,
                        VehicleOwner = m.VehicleOwner,
                        TransportingAgency = m.TransportingAgency,
                        HelperName = m.Helper,
                        StartingKilometer = (decimal)m.StartingKilometer,
                        IssuedBy = m.IssuedBy,
                        IsDraft = (bool)m.IsDraft,
                        BagCount = (int)m.BagCount,
                        CanCount = (int)m.CanCount,
                        BoxCount = (int)m.BoxCount,
                        Area = m.Area,
                        TransDate = (DateTime)m.TransDate,
                        TotalAmount = (decimal)m.TotalInvoiceAmount
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GatePassItemsBO> GetGatePassTransDetails(int id)
        {
            try
            {
                List<GatePassItemsBO> salesinvoice = new List<GatePassItemsBO>();
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    return salesinvoice = dbEntity.SpGetGatePassTransDetails(id, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new GatePassItemsBO()
                    {
                        ID = (int)m.TransID,
                        Amount = (decimal)m.NetAmount,
                        Area = m.Area,
                        DeliveryDate = m.DeliveryDate,
                        Name = m.Name,
                        TransDate = (DateTime)m.TransDate,
                        TransNo = m.TransNo,
                        Type = m.Type,
                        GatePassTransID = (int)m.ID,
                        NoOfBags = (int)m.NoOfBags,
                        NoOfCans = (int)m.NoOfCans,
                        NoOfboxes = (int)m.NoOfboxes
                    }).ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool SaveDeliveryDate(List<GatePassItemsBO> GatePassItems)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var item in GatePassItems)
                        {
                            dbEntity.SpUpdateGatePassDeliveryDate(
                            item.GatePassTransID,
                            item.DeliveryDate,
                            GeneralBO.FinYear,
                            GeneralBO.ApplicationID,
                            GeneralBO.LocationID
                            );
                        }

                        dbEntity.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
        }


        public List<GatePassItemsBO> getGatePassItems(DateTime FromDate, DateTime ToDate, string Type)
        {
            List<GatePassItemsBO> salesInvoice = new List<GatePassItemsBO>();
            using (SalesEntities dbEntity = new SalesEntities())
                try
                {
                    salesInvoice = dbEntity.SpGetSalesInvoiceBteweenDate(FromDate, ToDate, Type, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new GatePassItemsBO()
                    {
                        ID = a.ID,
                        TransNo = a.TransNo,
                        TransDate = Convert.ToDateTime(a.TransDate),
                        Name = a.Name,
                        Amount = Convert.ToDecimal(a.NetAmount),
                        Area = a.District,
                        Type = a.Type,
                        NoOfBags = (int)a.NoOfBags,
                        NoOfboxes = (int)a.NoOfboxes,
                        NoOfCans = (int)a.NoOfCans,

                    }).ToList();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            return salesInvoice;
        }

        public DatatableResultBO GetGatePassListForDataTable(string Type, string TransNo, string TransDate, string VehicleNo, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (SalesEntities dbEntity = new SalesEntities())
                {
                    var result = dbEntity.SpGetGatePassListForDataTable(Type,TransNo,TransDate, VehicleNo,SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                TransDate = ((DateTime)item.TransDate).ToString("dd-MMM-yyyy"),
                                VehicleNo = item.VehicleNo,
                                Status = item.Status,

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

    }
}