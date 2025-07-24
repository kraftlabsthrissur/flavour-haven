using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ActionBO
    {
        public int ID { get; set; }
        public int ActionID { get; set; }
        public int TabID { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string ControllerNameFormatted { get; set; }
        public string Action { get; set; }
        public string Key { get; set; }
        public string TableName { get; set; }
        public string ReturnType { get; set; }
        public string Attributes { get; set; }
        public int SortOrder { get; set; }
        public string Checked { get; set; }
        public string Type { get; set; }

    }
}
