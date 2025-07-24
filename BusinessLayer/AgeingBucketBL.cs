using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;

namespace BusinessLayer
{
    public class AgeingBucketBL : IAgeingBucketContract
    {
        AgeingBucketDAL bucketDAL;
        public AgeingBucketBL()
        {
            bucketDAL = new AgeingBucketDAL();
        }


        public bool Save(AgeingBucketBO BucketBO, List<AgeingBucketTransBO> Items)
        {
            if (BucketBO.ID == 0)
            {
                return bucketDAL.Save(BucketBO, Items);
            }
            else
            {
                return bucketDAL.Update(BucketBO, Items);
            }

        }
        public List<AgeingBucketBO> GetAgeingBucketList()
        {
            return bucketDAL.GetAgeingBucketList();
        }
        public List<AgeingBucketBO> GetAgeingBucketDetail(int ID)
        {
            return bucketDAL.GetAgeingBucketDetail(ID);
        }
        public List<AgeingBucketTransBO> GetAgeingBucketTrans(int ID)
        {
            return bucketDAL.GetAgeingBucketTrans(ID);
        }



    }
}
