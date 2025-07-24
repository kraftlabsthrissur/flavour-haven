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
    public class SerialNumberDAL
    {
        public int CreateSerialNumber(SerialNumberBO serialNumber)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        ObjectParameter returnvalue = new ObjectParameter("RetValue", typeof(int));
                        dbEntity.SpCreateSerialNumberNew(
                        serialNumber.FormName,
                        serialNumber.Field,
                        serialNumber.LocationPrefix,
                        serialNumber.Prefix,
                        serialNumber.SpecialPrefix,
                        serialNumber.FinYearPrefix,
                        serialNumber.Value,
                        serialNumber.IsLeadingZero,
                        serialNumber.NumberOfDigits,
                        serialNumber.Suffix,
                        serialNumber.IsMaster,
                        GeneralBO.FinYear,
                        serialNumber.LocationID,
                        GeneralBO.ApplicationID,
                        returnvalue
                   );
                        transaction.Commit();
                        return Convert.ToInt32(returnvalue.Value.ToString());
                    }
                    catch (Exception Ex)
                    {
                        transaction.Rollback();
                        return 1;
                    }
                }
            }
        }
        public DatatableResultBO GetSerialNumberList(string FormHint, string PrefixHint, string LocationPrefixHint, string FinYearPrefixHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetSerialNumberList(FormHint, PrefixHint, LocationPrefixHint, FinYearPrefixHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Form = item.Form,
                                Prefix = item.Prefix,
                                LocationPrefix = item.LocationPrefix,
                                FinYearPrefix = item.FinYearPrefix,
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
        public DatatableResultBO GetSerialNumberByFinYear(int NewFinYear, string FormHint, string LocationHint, string SortField, string SortOrder, int Offset, int Limit)

        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetSerialNoByFinYear(NewFinYear, FormHint, LocationHint, SortField, SortOrder, Offset, Limit).ToList();
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
                                Form = item.Form,
                                Prefix = item.Prefix,
                                LocationPrefix = item.LocationPrefix,
                                SpecialPrefix = item.SpecialPrefix == null ? "" : item.SpecialPrefix,
                                Value = item.value,
                                IsLeadingZero = item.IsLeadingZero,
                                NoOfDigits = item.NoOfDigits,
                                FinYear = item.FinYear,
                                LocationID = item.LocationID,
                                Suffix = item.Suffix,
                                Location = item.Location,
                                IsMaster = item.IsMaster,
                                Field = item.Field,
                                FinYearPrefix = item.FinYearPrefix
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
        public bool UpdateFinYearAndFinPrefix(List<SerialNumberBO> serialNo)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                using (var transaction = dbEntity.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var serialNumber in serialNo)
                        {

                            dbEntity.SpUpdateSerialNoByID(
                           serialNumber.ID,
                           serialNumber.FormName.Trim(),
                           serialNumber.Field,
                           serialNumber.LocationPrefix,
                           serialNumber.Prefix.Trim(),
                           serialNumber.SpecialPrefix.Trim(),
                           serialNumber.FinYearPrefix,
                           serialNumber.Value,
                           serialNumber.IsLeadingZero,
                           serialNumber.NumberOfDigits,
                           serialNumber.Suffix,
                           serialNumber.IsMaster,
                           serialNumber.NewFinYear,
                           serialNumber.LocationID,
                           GeneralBO.ApplicationID,
                           GeneralBO.CreatedUserID);

                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                        throw e;
                    }
                }
            }
        }
    }
}