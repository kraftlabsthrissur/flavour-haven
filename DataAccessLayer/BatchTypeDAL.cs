//File created by prama on 14-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class BatchTypeDAL
    {
        public List<BatchTypeBO> GetBatchTypeList()
        {
            List<BatchTypeBO> batchType = new List<BatchTypeBO>();
            using (AyurwareEntities dEntity = new AyurwareEntities())
            {
                batchType = dEntity.viBatchTypes.Select(a => new BatchTypeBO
                {
                    ID = a.id,
                    Name = a.name

                }).ToList();
                return batchType;
            }

        }
    }
}
