using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccessLayer;
using PresentationContractLayer;

namespace BusinessLayer
{
    public class DamageTypeBL : IDamageTypeContract
    {
        DamageTypeDAL damagetypeDAl;
        public DamageTypeBL()
        {
            damagetypeDAl = new DamageTypeDAL();

        }
        public List<DamageTypeBO> GetDamageTypeList()
        {
            return damagetypeDAl.GetDamageTypeList();
        }
    }
}
