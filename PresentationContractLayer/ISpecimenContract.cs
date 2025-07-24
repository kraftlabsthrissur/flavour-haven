using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ISpecimenContract
    {
        List<SpecimenBO> GetSpecimenList();
    }
}
