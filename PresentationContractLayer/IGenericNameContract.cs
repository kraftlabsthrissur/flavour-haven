using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IGenericNameContract
    {
        int Save(GenericNameBO GenericName);
        List<GenericNameBO> GetGenericNameList();
        List<GenericNameBO> GetGenericNameDetails(int ID);
        int Update(GenericNameBO GenericName);
    }
}
