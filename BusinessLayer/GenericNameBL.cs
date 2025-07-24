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
   public class GenericNameBL:IGenericNameContract
    {
        GenericNameDAL genericNameDAL;
        public GenericNameBL()
        {
            genericNameDAL = new GenericNameDAL();
        }
        public int Save(GenericNameBO GenericName)
        {
            return genericNameDAL.Save(GenericName);
        }
        public List<GenericNameBO> GetGenericNameList()
        {
            return genericNameDAL.GetGenericNameList();
        }
        public List<GenericNameBO> GetGenericNameDetails(int ID)
        {
            return genericNameDAL.GetGenericNameDetails(ID);
        }
        public int Update(GenericNameBO GenericName)
        {
            return genericNameDAL.Update(GenericName);
        }
    }
}
