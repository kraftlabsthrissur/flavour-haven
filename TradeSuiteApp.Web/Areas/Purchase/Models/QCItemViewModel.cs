using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class QCItemViewModel
    {
        public List<QCItemModel> pendingQCList { get; set; }
        public List<QCItemModel> onGoingQCList { get; set; }
        public List<QCItemModel> completedQCList { get; set; }
    }
}