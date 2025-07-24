using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IXrayTestContract
    {
        XrayTestBO GetXrayCategory();
        int Save(XrayTestBO Xray);
        List<XrayTestBO> GetXrayTestList();
        List<XrayTestBO> GetXrayDetailsByID(int ID);
        int Update(XrayTestBO Xray);
    }
}
