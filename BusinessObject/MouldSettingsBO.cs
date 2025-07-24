using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
   public class MouldSettingsBO
    {
        public int ID { get; set; }
        public int MouldID { get; set; }
        public string MouldName { get; set; }
        public string MachineName { get; set; }
        public string MachineCode { get; set; }
        public string SettingTime { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }
    }
    public class MouldSettingsHistoryBO
    {
        public DateTime Date { get; set; }
        public string Mould { get; set; }
        public string Reason { get; set; }
        public string AddorRemove { get; set; }
        public string SettingTime { get; set; }
    }
}
