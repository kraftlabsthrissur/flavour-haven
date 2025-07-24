using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class LaboratoryTestDAL
    {
        public int Save(LaboratoryTestBO Lab, string LabItem)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Laboratory Test";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    return dbEntity.SpCreateLaboratoryTest(Lab.Code, Lab.Name, Lab.BiologicalReference,Lab.Unit, Lab.AddedDate, Lab.Description,Lab.CategoryID
                        ,Lab.PurchaseCategoryID,Lab.QCCategoryID,Lab.GSTCategoryID,Lab.SalesCategoryID,Lab.SalesIncentiveCategoryID,
                        Lab.StorageCategoryID,Lab.ItemTypeID,Lab.AccountsCategoryID,Lab.BusinessCategoryID,Lab.ItemUnitID,Lab.Method,Lab.SpecimenID,
                        Lab.Rate,Lab.IsAlsoGroup, LabItem,GeneralBO.CreatedUserID,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public LaboratoryTestBO GetLaboratoryTestDetails()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetLaboratoryTestDetails().Select(a => new LaboratoryTestBO()
                    {
                        CategoryID=(int)a.CategoryID,
                        ItemUnitID=(int)a.UnitID                     
                    }
                    ).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LaboratoryTestBO> GetLaboratoryTestList()
        {
            try
            {
                List<LaboratoryTestBO> LabTest = new List<LaboratoryTestBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    LabTest = dbEntity.SpGetLaboratoryTestList().Select(a => new LaboratoryTestBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name=a.Name
                    }).ToList();

                    return LabTest;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<LaboratoryTestBO> GetLaboratoryTestDetailsByID(int ID)
        {
            try
            {
                List<LaboratoryTestBO> LabTest = new List<LaboratoryTestBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    LabTest = dbEntity.SpGetLaboratoryTestDetailsByID(ID).Select(a => new LaboratoryTestBO
                    {
                     ID=a.ID,
                     Code=a.Code,
                     Name=a.Name,
                     Description=a.Description,
                     BiologicalReference=a.BiologicalReference,
                     Unit=a.Unit,
                     AddedDate=(DateTime)a.Date,
                     Method=a.Method,
                     SpecimenID=a.SpecimenID,
                     Rate=a.Rate,
                     GSTCategoryID = a.GSTCategoryID,
                     Specimen=a.Specimen,
                     GSTCategory=(decimal)a.GSTCategory,
                     IsAlsoGroup=a.IsAlsoCategory
                    }).ToList();
                    return LabTest;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<LabItemBO> GetLaboratoryTestItemDetailsByID(int ID)
        {
            try
            {
                List<LabItemBO> LabTest = new List<LabItemBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    LabTest = dbEntity.SpGetLaboratoryTestItemDetailsByID(ID).Select(a => new LabItemBO
                    {
                        LabTestID=a.LabTestID,
                        LabTest=a.LabTest
                    }).ToList();
                    return LabTest;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int Update(LaboratoryTestBO lab, string LabItem)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateLaboratoryTest(lab.ID, lab.Code, lab.Name, lab.BiologicalReference,lab.Unit,
                        lab.AddedDate, lab.Description, lab.Method, lab.SpecimenID, lab.Rate, lab.GSTCategoryID,lab.IsAlsoGroup, LabItem, GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public DatatableResultBO GetLabTestList(string CodeHint, string TypeHint, string ServiceHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetLabTestForModal(CodeHint, TypeHint, ServiceHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
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
                                Code = item.Code,
                                Name = item.Name,
                                GroupName = item.GroupName,
                                ServiceName = item.ProductionGroup,
                                Type = item.Type
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
