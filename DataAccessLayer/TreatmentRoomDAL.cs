using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class TreatmentRoomDAL
    {
        public int Save(TreatmentRoomBO treatmentRoomBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateTreatmentRoom(
                        treatmentRoomBO.Name,
                        treatmentRoomBO.Remarks,
                        GeneralBO.CreatedUserID,
                        GeneralBO.ApplicationID
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Update(TreatmentRoomBO treatmentRoomBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateTreatmentRoom(
                        treatmentRoomBO.ID,
                        treatmentRoomBO.Name,
                        treatmentRoomBO.Remarks,
                        GeneralBO.CreatedUserID,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DatatableResultBO GetTreatmentRoomList(string NameHint, string RemarkHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetTreatmentRoomList(NameHint, RemarkHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Remark = item.Remark,
                                Name = item.Name
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

        public List<TreatmentRoomBO> GetTreatmentRoomDetails(int ID)
        {
            try
            {
                List<TreatmentRoomBO> treatmentroom = new List<TreatmentRoomBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    treatmentroom = dbEntity.SpGetTreatmentRoomByID(ID, GeneralBO.ApplicationID).Select(a => new TreatmentRoomBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Remarks = a.Remark
                    }
                    ).ToList();

                    return treatmentroom;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TreatmentRoomBO> GetTreatmentRoomDetailsList()
        {
            List<TreatmentRoomBO> item = new List<TreatmentRoomBO>();
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    item = dbEntity.SpGetTreatmentRoomDetails().Select(a => new TreatmentRoomBO
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DatatableResultBO GetTreatmentRoomAutoComplete(string Hint)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetAllTreatmentRoomList(Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DatatableResult;
        }
    }
}
