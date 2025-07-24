using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Approvals.Models
{
    public class ApprovalViewModel
    {
        public List<ApprovalModel> ToBeApproved { get; set; }
        public List<ApprovalModel> Approved { get; set; }
        public List<ApprovalModel> Rejected { get; set; }
    }
    public class ApprovalModel
    {
        public int ApprovalID { get; set; }
        public string Name { get; set; }
        public int TransID { get; set; }
        public string TransNo { get; set; }
        public int ApprovalFlowID { get; set; }
        public bool IsApproved { get; set; }
        public string Status { get; set; }
        public int LastActionUserID { get; set; }
        public int NextActionUserID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Date { get; set; }
        public int ApprovalTypeID { get; set; }
        public string UserName { get; set; }
        public string Requirement { get; set; }
        public int LoggedInUserID { get; set; }
        public List<ApprovalProcessModel> Process { get; set; }
        public List<ApprovalProcessModel> History { get; set; }
        public SelectList UsersList { get; set; }
    }
    public class ApprovalProcessModel
    {
        public int ApprovalTransID { get; set; }
        public int ApprovalID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveForUser { get; set; }
        public DateTime DateTime { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string Requirement { get; set; }
        public bool IsApproved { get; set; }
        public decimal? SortOrder { get; set; }
        public int NextActionUserID { get; set; }
        private string iconClass;
        private string iconText;
        private string statusText;
        public string IconClass
        {
            get
            {
                switch (Status)
                {
                    case "Approved":
                        iconClass = "timeline_icon_success";
                        break;
                    case "Initiated":
                        iconClass = "timeline_icon_primary";
                        break;
                    case "Re-initiated":
                        iconClass = "timeline_icon_primary";
                        break;
                    case "Requested Clarification":
                        iconClass = "timeline_icon_warning";
                        break;
                    case "Rejected":
                        iconClass = "timeline_icon_danger";
                        break;
                    default:
                        iconClass = "";
                        break;
                }

                return IsActive ? iconClass : "";
            }

            set
            {
                iconClass = value;
            }
        }

        public string IconText
        {
            get
            {
                switch (Status)
                {
                    case "Approved":
                        iconText = "check";
                        break;
                    case "Initiated":
                        iconText = "create";
                        break;
                    case "Re-initiated":
                        iconText = "create";
                        break;
                    case "Requested Clarification":
                        iconText = "help_outline";
                        break;
                    case "Rejected":
                        iconText = "close";
                        break;
                    default:
                        if (IsActive)
                        {
                            iconText = "query_builder";
                        }
                        else
                        {
                            iconText = "cached";
                        }

                        break;
                }
                return iconText;
            }

            set
            {
                IconText = value;
            }
        }

        public string StatusText
        {
            get
            {
                if (Status.Equals(""))
                {
                    if (IsActive)
                    {
                        if (NextActionUserID == UserID)
                        {
                            if (Requirement == "Approval")
                            {
                                return "Pending approval from " + UserName;
                            }
                            else
                            {
                                return "Pending clarification from " + UserName;
                            }

                        }
                        else
                        {
                            if (Requirement == "Approval")
                            {
                                return "Needs approval of " + UserName;
                            }
                            else
                            {
                                return "Needs clarification from " + UserName;
                            }

                        }

                    }
                    else
                    {
                        return "Approval skipped from " + UserName;
                    }
                }
                else
                {
                    if (Requirement == "Approval")
                    {
                        return Status + " By " + UserName;
                    }
                    else
                    {
                        return "Clarified by " + UserName;
                    }
                }
            }
            set
            {
                statusText = value;
            }
        }
    }
}