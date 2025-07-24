using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PrivilegeCardDAL
    {
        public List<PrivilegeCardBO> GetPriviledgeCards()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPriviledgeCards().Select(a => new PrivilegeCardBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        DiscountCategoryID = a.DiscountCategoryID,
                        DiscountPercentage=(decimal)a.DiscountPercentage,
                        ValidDays = a.ValidDays,
                        Rate = a.Rate
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }
        public List<PrivilegeCardBO> GetPriviledgeCardbyID(int id)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetPriviledgeCardsByID(id).Select(a => new PrivilegeCardBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Type = a.Type,
                        DiscountCategoryID = a.DiscountCategoryID,
                        DiscountPercentage = (decimal)a.DiscountPercentage,
                        ValidDays = a.ValidDays,
                        Rate = a.Rate
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }
    }
}
