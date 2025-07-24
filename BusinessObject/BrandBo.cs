using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BrandBO
    {
        public int? Id { get; set; }
        public int? BrandId { get; set; }

        public string Code { get; set; }
        public string BrandName { get; set; }
        public string Path { get; set; }
        public string image { get; set; }
    }
}
