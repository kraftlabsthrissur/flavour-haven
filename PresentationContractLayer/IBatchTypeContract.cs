//File created by prama on 14-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IBatchTypeContract
    {
        List<BatchTypeBO> GetBatchTypeList();

    }
}
