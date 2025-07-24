using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
   public interface IQCTestContract
    {
        List<QCTestBO> GetQCTestList();
        int Save(QCTestBO QctestBO);
        QCTestBO GetQCTestDetails(int ID);

      
       
        

    }
}
