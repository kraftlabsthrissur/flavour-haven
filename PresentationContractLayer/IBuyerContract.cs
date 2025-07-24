using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using System.Web.UI.WebControls;

namespace PresentationContractLayer
{
    public interface IBuyerContract
    {
        List<BuyerBO> GetBuyerList();     
    }
}
