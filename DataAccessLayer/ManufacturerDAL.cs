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
   public class ManufacturerDAL
    {
        public int Save(ManufacturerBO manufacturer)
        {
            try
            {
                ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    string FormName = "Manufacturer";
                    var j = dbEntity.SpUpdateSerialNo(FormName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    return dbEntity.SpCreateManufacturer(manufacturer.Code, manufacturer.Name, manufacturer.AddressLine1, manufacturer.AddressLine2,
                        manufacturer.StateID, manufacturer.Place, manufacturer.Phone, manufacturer.Description,GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        public List<ManufacturerBO> GetManufacturerList()
        {
            try
            {
                List<ManufacturerBO> manufacturer = new List<ManufacturerBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    manufacturer = dbEntity.SpGetManufacturerList().Select(a => new ManufacturerBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        Phone=a.PhoneNumber
                    }).ToList();

                    return manufacturer;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ManufacturerBO> GetManufacturerDetails(int ID)
        {
            try
            {
                List<ManufacturerBO> manufacturer = new List<ManufacturerBO>();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    manufacturer = dbEntity.SpGetManufacturerDetails(ID,GeneralBO.ApplicationID).Select(a => new ManufacturerBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2=a.AddressLine2,
                        Place=a.Place,
                        State=a.State,
                        Description = a.Description,
                        Phone=a.PhoneNumber,
                        StateID=(int)a.StateID
                    }).ToList();
                    return manufacturer;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public int Update(ManufacturerBO manufacturer)
        {
            try
            {
                GeneralDAL generalDAL = new GeneralDAL();
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpUpdateManufacturer(manufacturer.ID, manufacturer.Code, manufacturer.Name, manufacturer.AddressLine1,
                        manufacturer.AddressLine2, manufacturer.StateID,
                        manufacturer.Place, manufacturer.Phone, manufacturer.Description, GeneralBO.CreatedUserID, GeneralBO.ApplicationID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


    }
}
