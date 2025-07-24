using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AccountBO
    {
        public int ID { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string GroupClassification { get; set; }
        public decimal OpeningAmt { get; set; }
    }
}
