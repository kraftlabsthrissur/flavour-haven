//File created by prama
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace PresentationContractLayer
{
    public interface IMilkSupplierContract
    {
        int SaveMilkSupplier(MilkSupplierBO milkSupplier);
         }
}
