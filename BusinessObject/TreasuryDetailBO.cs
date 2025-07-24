using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class TreasuryDetailBO
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string LocationMapping { get; set; }
        
    }
}
