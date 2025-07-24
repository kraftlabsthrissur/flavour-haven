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
    public class UserDAL
    {
        public List<UserBO> GetUserList()
        {
            List<UserBO> itm = new List<UserBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    itm = dbEntity.SpGetUser(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new UserBO
                    {
                        ID = a.ID,
                        Name = a.Name

                    }).ToList();
                    return itm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add user Location
        /// Remove all the userrelated Locations, and add new location
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="locationIDsStr"></param>
        /// <returns></returns>
        public bool AddUserLocation(string userID, string locationIDsStr)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(locationIDsStr))
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    try
                    {
                        dbEntity.SpCreateUserLocation(userID, locationIDsStr);
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public UserBO GetLoginDetails(int UserID, string Email)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    var Data = dbEntity.SpGetLoginDetails(UserID, Email).ToList();
                    return Data.Select(a => new UserBO
                    {
                        UserID = a.UserID,
                        Email = a.Email,
                        UserName = a.UserName,
                        PhoneNumber = a.PhoneNumber,
                        EmployeeName = a.EmployeeName,
                        DepartmentID = (int)a.DepartmentID,
                        MobileNo = a.MobileNo,
                        Place = a.Place,
                        UserLocationID = (int)a.UserLocationID,
                        CurrentLocationID = (int)a.CurrentLocationID,
                        Designation = a.Designation,
                        Code = a.Code,
                        Location = a.Location,
                        LocationPlace = a.LocationPlace,
                        CompanyName = a.CompanyName,
                        OwnerName = a.OwnerName,
                        LocationGSTNo = a.LocationGSTNo,
                        LocationCINNo = a.LocationCINNo,
                        Jurisdiction = a.Jurisdiction,
                        AuthorizedSignature = a.AuthorizedSignature,
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        AddressLine4 = a.AddressLine4,
                        AddressLine5 = a.AddressLine5,
                        StateID = a.StateID.HasValue ? a.StateID.Value : 0,
                        PIN = a.PIN,
                        LandLine1 = a.LandLine1,
                        LandLine2 = a.LandLine2,
                        MobileNoInAddress = a.MobileNoInAddress,
                        ApplicationID = (int)a.ApplicationID,
                        ApplicationName = a.ApplicationName,
                        CustomerID = a.CustomerID,
                        SupplierID = a.SupplierID,
                        Logo = a.Logo,
                        ReportLogoPath=a.Reportlogopath,
                        ReportfooterPath=a.ReportfooterPath
                    }).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int UpdateSessionLog(int UserID, int SessionID)
        {
            using (AyurwareEntities dbEntity = new AyurwareEntities())
            {
                try
                {
                    ObjectParameter SessionIDOut = new ObjectParameter("SessionIDOut", typeof(int));
                    dbEntity.SpUpdateSessionLog(UserID, SessionID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SessionIDOut);
                    return Convert.ToInt16(SessionIDOut.Value.ToString());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
