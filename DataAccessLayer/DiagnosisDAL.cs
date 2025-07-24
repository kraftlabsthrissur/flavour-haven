using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DiagnosisDAL
    {
        public int Save(DiagnosisBO diagnosisBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpCreateDiagnosis(
                        diagnosisBO.Name,
                        diagnosisBO.Description,
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

        public DatatableResultBO GetDiagnosisList(string NameHint, string DescriptionHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetDiagnosisList(NameHint, DescriptionHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Description = item.Description,
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

        public List<DiagnosisBO> GetDiagnosisDetails(int ID)
        {
            try
            {
                List<DiagnosisBO> diagnosis = new List<DiagnosisBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    diagnosis = dbEntity.SpGetDiagnosisByID(ID, GeneralBO.ApplicationID).Select(a => new DiagnosisBO()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Description = a.Description
                    }
                    ).ToList();

                    return diagnosis;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(DiagnosisBO diagnosisBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateDiagnosis(
                        diagnosisBO.ID,
                        diagnosisBO.Name,
                        diagnosisBO.Description,
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
    }
}
