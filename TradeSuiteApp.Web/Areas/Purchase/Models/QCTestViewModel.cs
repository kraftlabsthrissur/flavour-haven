using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeSuiteApp.Web.Areas.Masters.Models;

namespace TradeSuiteApp.Web.Areas.Purchase.Models
{
    public class QCTestViewModel
    {
        public QCItemModel QCItem { get; set; }
        public List<QCTestModel> physicalTestDetails { get; set; }
        public List<QCTestModel> chemicalTestDetails { get; set; }
        public List<WareHouseModel> wareHouse { get; set; }
        public List<QCTestModel> testResults { get; set; }
    }
}