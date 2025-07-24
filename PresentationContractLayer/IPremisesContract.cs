//File created by prama on 14-4-18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IPremisesContract
    {
        List<PremisesBO> GetPremisesList(int ID);

        List<PremisesBO> GetPremisesWithItemID(int ID);

    }
}
