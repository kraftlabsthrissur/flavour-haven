using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class MailBO
    {
        public string SenderMailID { get; set; }
        public string SenderMailPassword { get; set; }
        public string ReceiverMailID { get; set; }
        public bool IsMailSent { get; set; }
    }
}
