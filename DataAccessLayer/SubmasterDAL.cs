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
    public class SubmasterDAL
    {
        public List<SubmasterBO> GetMonthList()
        {
            try
            {
                using (PayrollEntities dbEntity = new PayrollEntities())
                {
                    return dbEntity.SpGetMonthList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IList<SubmasterBO> GetBloodGroupList()
        {
            try
            {
                using (MasterEntities db = new MasterEntities())
                {
                    return db.SpGetBloodeGroup().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<SubmasterBO> GetDurationList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDurationList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<SubmasterBO> GetTimePeriodList(int DurationID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetTimePeriodList(DurationID).Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<SubmasterBO> GetPatientReferenceBy()
        {
            try
            {
                using (AHCMSEntities dbEntity = new AHCMSEntities())
                {
                    return dbEntity.SpGetPatientReferedBy(GeneralBO.ApplicationID).Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name,
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<SubmasterBO> GetTreatmentGroupList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetTreatmentGroupList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.TreatmentGroup
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SubmasterBO> GetOccupationList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetOccupationList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SubmasterBO> GetMedicineCategoryGroupList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetMedicineCategoryGroupList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetEmployeeCategoryID(string Name)
        {
            ObjectParameter CategoryID = new ObjectParameter("CategoryID", typeof(int));
            try
            {

                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SPGetEmployeeCategoryID(Name, CategoryID);
                }
                return CategoryID.Value as int? ?? 0;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public int GetConfigValue(string Name)
        {
            try
            {
                using (AyurwareEntities dbEntity = new AyurwareEntities())
                {
                    ObjectParameter ConfigValue = new ObjectParameter("ConfigValue", typeof(string));
                    dbEntity.SpGetConfigValue(Name, GeneralBO.LocationID, GeneralBO.ApplicationID, ConfigValue);
                    return Convert.ToInt32(ConfigValue.Value);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SubmasterBO> GetRoomTypeList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetRoomTypeList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<SubmasterBO> GetModeOfAdministration()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPGetModeOfAdministrationList().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<SubmasterBO> GetSupplierForLabItems()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetSupplierForLabTest().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SubmasterBO> GetGeneralDiscountType()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetGeneralDiscountType(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SubmasterBO> GetDiscountType()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetDiscountType().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<SubmasterBO> GetFormList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetFormType().Select(a => new SubmasterBO
                    {
                        Id = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
