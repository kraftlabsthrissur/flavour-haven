using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class AgeingBucketBO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<AgeingBucketTransBO> Trans { get; set; }
    }

    public class AgeingBucketTransBO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}