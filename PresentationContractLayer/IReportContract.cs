using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IReportContract 
    {
        List<SerialNumberBO> GetAutoComplete(String Term, String Table);
    }
}
