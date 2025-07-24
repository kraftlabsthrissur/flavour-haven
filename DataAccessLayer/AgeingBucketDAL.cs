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
    public class AgeingBucketDAL
    {
        public bool Save(AgeingBucketBO AgeingBucketBO, List<AgeingBucketTransBO> Items)
        {
            using (MasterEntities dEntity = new MasterEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {

                        ObjectParameter BucketID = new ObjectParameter("BucketID", typeof(int));
                        var i = dEntity.SpCreateAgeingBucket(
                                         AgeingBucketBO.Name,
                                         AgeingBucketBO.Code,
                                         BucketID
                                           );

                        if (BucketID.Value != null)
                        {
                            foreach (var itm in Items)
                            {
                                dEntity.SpCreateAgeingBucketTrans(
                                Convert.ToInt32(BucketID.Value),
                                itm.Name,
                                itm.Start,
                                itm.End);
                            }

                        };
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
        public bool Update(AgeingBucketBO AgeingBucketBO, List<AgeingBucketTransBO> Items)
        {

            using (MasterEntities dEntity = new MasterEntities())
            {
                using (var transaction = dEntity.Database.BeginTransaction())
                {
                    try
                    {
                        dEntity.SpUpdateAgeingBucket(
                             AgeingBucketBO.ID,
                             GeneralBO.CreatedUserID,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID
                              );

                        foreach (var itm in Items)
                        {
                            dEntity.SpCreateAgeingBucketTrans(
                            AgeingBucketBO.ID,
                            itm.Name,
                            itm.Start,
                            itm.End);
                        }
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




        public List<AgeingBucketBO> GetAgeingBucketList()
        {
            List<AgeingBucketBO> list = new List<AgeingBucketBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetAgeingBucketList().Select(a => new AgeingBucketBO
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList();
                return list;
            }

        }
        public List<AgeingBucketBO> GetAgeingBucketDetail(int ID)
        {
            List<AgeingBucketBO> list = new List<AgeingBucketBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                try
                {
                    list = dEntity.SpGetAgeingBucketDetail(ID).Select(a => new AgeingBucketBO
                    {
                        ID = a.ID,
                        Name = a.Name


                    }).ToList();
                    return list;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }
        public List<AgeingBucketTransBO> GetAgeingBucketTrans(int ID)
        {
            List<AgeingBucketTransBO> list = new List<AgeingBucketTransBO>();
            using (MasterEntities dEntity = new MasterEntities())
            {
                list = dEntity.SpGetAgeingBucketTrans(ID).Select(a => new AgeingBucketTransBO
                {

                    ID = (int)a.ID,
                    Name = a.Name,
                    Start = (int)a.Start,
                    End = (int)a.Enddays

                }).ToList();

                return list;
            }
        }
    }
}
