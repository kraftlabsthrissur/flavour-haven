using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class XrayTestBL:IXrayTestContract
    {
        XrayTestDAL xrayTestDAL;

        public XrayTestBL()
        {
            xrayTestDAL = new XrayTestDAL();
        }
        public XrayTestBO GetXrayCategory()
        {
            return xrayTestDAL.GetXrayCategory();
        }
        public int Save(XrayTestBO xray)
        {
            return xrayTestDAL.Save(xray);
        }
        public List<XrayTestBO> GetXrayTestList()
        {
            return xrayTestDAL.GetXrayTestList();
        }
        public List<XrayTestBO> GetXrayDetailsByID(int ID)
        {
            return xrayTestDAL.GetXrayDetailsByID(ID);
        }
        public int Update(XrayTestBO xray)
        {
            return xrayTestDAL.Update(xray);
        }
    }
}
