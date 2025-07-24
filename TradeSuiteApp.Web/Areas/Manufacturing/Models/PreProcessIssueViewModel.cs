using BusinessObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class PreProcessIssueViewModel
    {

        public int ID { get; set; }
        public string IssueNo { get; set; }

        public DateTime? IssueDate
        {
            get
            {
                return IssueDateStr.ToDateTime();
            }
            set { IssueDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public bool IsDraft { get; set; }
        public bool IsCancelled { get; set; }
        public string Date { get; set; }
        public string IssueDateStr { get; set; }
        public int CreatedUserID { get; set; }
        public string CreatedUser { get; set; }
        public string Status { get; set; }
        public string ItemName  { get; set; }
        public List<PreProcessIssueItemModel> Items { get; set; }
    }


    public class PreProcessIssueItemModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public decimal Quantity { get;  set; }
        public decimal Stock { get; set; }
        public int BatchID { get; set; }
        public string BatchNo { get; set; }

    }

    public static partial class Mapper
    {

        /// <summary>
        /// Map PreprocessItemBO to PreProcessIssueItemModel
        /// </summary>
        /// <param name="itemBOList"></param>
        /// <returns></returns>
        public static List<PreProcessIssueItemModel> MapToModelList(this List<PreprocessIssueItemBO> boList)
        {
            if (boList != null && boList.Count() > 0)
            {
                return (from x in boList
                        select new PreProcessIssueItemModel()
                        {
                            ID = x.ID,
                            ItemID = x.ItemID,
                            UnitID = x.UnitID,
                            ItemName = x.ItemName,
                            Unit = x.Unit,
                            ProcessID = x.ProcessID,
                            ProcessName = x.ProcessName,
                            Quantity = x.Quantity,
                            Stock =x.Stock,
                            BatchID=x.BatchID,
                            BatchNo=x.BatchNo
                        }).ToList();
            }
            else
            {
                return new List<PreProcessIssueItemModel>();
            }
        }

        /// <summary>
        /// Map PreProcessIssueBO to PreProcessIssueViewModel
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static PreProcessIssueViewModel MapToModel(this PreprocessIssueBO bo)
        {
            if (bo != null)
                return new PreProcessIssueViewModel()
                {
                    ID = bo.PreProcessIssueID,
                    IssueNo = bo.IssueNo,
                    IssueDate = bo.IssueDate,
                    IsDraft = bo.IsDraft,
                    Items = bo.PreprocessIssueItemBOList.MapToModelList(),
                    CreatedUserID = bo.CreatedUserID,
                    CreatedUser = bo.CreatedUser,
                    IsCancelled=bo.IsCancelled
                };
            else
            {
                return new PreProcessIssueViewModel();
            }
        }

        public static List<PreProcessIssueViewModel> MapToModelList(this List<PreprocessIssueBO> boList)
        {
            if (boList != null && boList.Count > 0)
                return (boList.Select(x => x.MapToModel())).ToList();
            else return new List<PreProcessIssueViewModel>();
        }




        /// <summary>
        /// Map PreProcessIssueViewModel to PreprocessIssueBO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PreprocessIssueBO MapToBo(this PreProcessIssueViewModel model)
        {
            if (model != null)
                return new PreprocessIssueBO()
                {
                    PreProcessIssueID = model.ID,
                    IssueNo = model.IssueNo,
                    IssueDate = (DateTime)model.IssueDate,
                    IsDraft = model.IsDraft,
                    PreprocessIssueItemBOList = model.Items.MapToBOList()
                };
            else
            {
                return new PreprocessIssueBO();
            }
        }

        /// <summary>
        ///  Map PreProcessIssueItemModel list to  PreprocessIssueItemBO list
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public static List<PreprocessIssueItemBO> MapToBOList(this List<PreProcessIssueItemModel> modelList)
        {
            if (modelList != null)
                return (from m in modelList
                        select new PreprocessIssueItemBO
                        {
                            ID = m.ID,
                            ItemID = m.ItemID,
                            UnitID = m.UnitID,
                            ItemName = m.ItemName,
                            Unit = m.Unit,
                            ProcessID = m.ProcessID,
                            ProcessName = m.ProcessName,
                            Quantity = m.Quantity,
                            BatchID=m.BatchID,
                            BatchNo=m.BatchNo

                        }).ToList();
            else
            {
                return new List<PreprocessIssueItemBO>();
            }
        }
    }

}
