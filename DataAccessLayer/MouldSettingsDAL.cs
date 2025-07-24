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
    public class MouldSettingsDAL
    {
        public List<MouldSettingsBO> GetMachinesForMouldSettings()
        {
            List<MouldSettingsBO> Machines = new List<MouldSettingsBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Machines = dbEntity.SpGetMachinesForMouldSettings(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new MouldSettingsBO
                    {
                        ID = k.ID,
                        MachineCode = k.MachineCode,
                        MouldName = k.MouldName,
                        MouldID = (int)k.LoadedMouldID,
                        MachineName = k.MachineName

                    }).ToList();
                }
                return Machines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MouldSettingsBO GetMachinesForMouldSettingsByID(int MachineID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetMachineForMouldSettingsByID(GeneralBO.LocationID, GeneralBO.ApplicationID, MachineID).Select(k => new MouldSettingsBO
                    {
                        ID = k.ID,
                        MachineCode = k.MachineCode,
                        MouldName = k.MouldName,
                        MouldID = (int)k.LoadedMouldID,
                        MachineName = k.MachineName
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Save(MouldSettingsBO mouldSettingsBO)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    dbEntity.SpCreateMouldSettings(
                        mouldSettingsBO.ID,
                        mouldSettingsBO.MouldID,
                        mouldSettingsBO.SettingTime,
                        mouldSettingsBO.Reason,
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                        );
                };
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MouldSettingsHistoryBO> GetMouldSettings(int MachineID)
        {
            List<MouldSettingsHistoryBO> MouldSettings = new List<MouldSettingsHistoryBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    MouldSettings = dbEntity.SpGetMouldSettings( GeneralBO.LocationID,GeneralBO.ApplicationID, MachineID).Select(k => new MouldSettingsHistoryBO
                    {
                    Date=(DateTime)k.CreatedDate,
                    Mould=k.MouldName,
                    SettingTime=k.SettingTime,
                    AddorRemove=k.AddorRemove,
                    Reason=k.Reason
                    }).ToList();
                }
                return MouldSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
