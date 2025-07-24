using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class JSONOutputBO
    {
        private string _Status = "success";
        private string _Message = "Saved Successfully";
        public OutputDataBO Data { get; set; }

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }
    public class OutputDataBO
    {
        public int ID { get; set; }
        public bool IsDraft { get; set; }
        public string TransNo { get; set; }
    }

}
