//File created by prama on 14-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using BusinessLayer;

namespace BusinessLayer
{
    public class BatchTypeBL : IBatchTypeContract
    {
        BatchTypeDAL batchtypeDAl;
        public BatchTypeBL()
        {
            batchtypeDAl = new BatchTypeDAL();

        }
        public List<BatchTypeBO> GetBatchTypeList()
        {
            return batchtypeDAl.GetBatchTypeList();
        }

    }
}
