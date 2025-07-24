using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class AgeingBucketModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<AgeingBucketTransModel> Trans { get; set; }
    }

    public class AgeingBucketTransModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}