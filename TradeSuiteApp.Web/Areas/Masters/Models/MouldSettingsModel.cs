using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Areas.Masters.Models
{
    public class MouldSettingsModel
    {
        public int ID { get; set; }
        public int MouldID { get; set; }
        public string MouldName { get; set; }
        public string MachineName { get; set; }
        public string MachineCode { get; set; }
        public string SettingTime { get; set; }
        public string Reason { get; set; }
        public string Date { get; set; }
        public string Code { get; set; }
        public List<MouldSettingsHistoryModel> MouldSettingList { get; set; }
    }
    public class MouldSettingsHistoryModel
    {
        public string Date { get; set; }
        public string Mould { get; set; }
        public string Reason { get; set; }
        public string AddorRemove { get; set; }
        public string SettingTime { get; set; }
    }
}