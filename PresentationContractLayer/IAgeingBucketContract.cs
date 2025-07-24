using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface  IAgeingBucketContract
    {
        bool Save(AgeingBucketBO schemeallocationBO, List<AgeingBucketTransBO> Items);

        List<AgeingBucketBO> GetAgeingBucketList();

        List<AgeingBucketBO> GetAgeingBucketDetail(int ID);

        List<AgeingBucketTransBO> GetAgeingBucketTrans(int ID);

     
    }
}
