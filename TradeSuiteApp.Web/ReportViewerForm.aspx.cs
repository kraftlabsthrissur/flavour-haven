using BusinessLayer;
using BusinessObject;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeSuiteApp.Web
{
    public partial class ReportViewerForm : System.Web.UI.Page
    {


        protected void Page_LoadComplete(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    string Key = Request.QueryString["Key"];
                    ReportViewer SessionReportViewer = (Microsoft.Reporting.WebForms.ReportViewer)Session[Key];

                    ReportViewer1.ProcessingMode = SessionReportViewer.ProcessingMode;
                    ReportViewer1.SizeToReportContent = SessionReportViewer.SizeToReportContent;
                    ReportViewer1.AsyncRendering = SessionReportViewer.AsyncRendering;
                    ReportViewer1.ZoomMode = SessionReportViewer.ZoomMode;
                    ReportViewer1.BackColor = SessionReportViewer.BackColor;
                    ReportViewer1.LocalReport.EnableExternalImages = SessionReportViewer.LocalReport.EnableExternalImages = true;
                    ReportViewer1.KeepSessionAlive = SessionReportViewer.KeepSessionAlive;

                    ReportViewer1.LocalReport.ReportPath = SessionReportViewer.LocalReport.ReportPath;
                    foreach (var item in SessionReportViewer.LocalReport.DataSources)
                    {
                        ReportViewer1.LocalReport.DataSources.Add(item);
                    }

                    List<ReportParameter> ReportParameters = new List<ReportParameter>();
                    foreach (var item in SessionReportViewer.LocalReport.GetParameters())
                    {
                        ReportParameters.Add(new ReportParameter(item.Name, item.Values[0]));
                    }
                    ReportViewer1.LocalReport.SetParameters(ReportParameters);
                    Session[Key] = null;


                }
                catch (Exception ex)
                {
                    GeneralBL generalBL = new GeneralBL();
                    generalBL.LogError("Reports", "Reorts", "ReportViewer", 0, ex);
                    Message1.Text = "Something went wrong";
                }

            }
        }
    }
}