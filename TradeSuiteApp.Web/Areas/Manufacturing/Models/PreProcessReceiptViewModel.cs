using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TradeSuiteApp.Web.Areas.Manufacturing.Models
{
    public class PreProcessReceiptViewModel
    {

        public int ID { get; set; }
        public string Activity { get; set; }

        public string TransNo { get; set; }
        public string TransDateStr { get; set; }
        public DateTime? TransDate
        {
            get
            {
                return TransDateStr.ToDateTime();
            }
            set { TransDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public List<PreProcessReceiptPurificationItemModel> PreProcessReceiptPurificationItemModelList { get; set; }
        public bool IsDraft { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get;  set; }
    }
    /// <summary>
    /// Specifically for Index page
    /// </summary>
    public class PreProcessReceiptDisplayViewModel
    {
        public int ID { get; set; }
        public string IssuedItem { get; set; }
        public string IssuedItemUnit { get; set; }
        public decimal IssuedQuantity { get; set; }
        public string ReceiptItem { get; set; }
        public string ReceiptItemUnit { get; set; }
        public decimal ReceiptQuantity { get; set; }
        public string Activity { get; set; }
        public decimal QuantityLoss { get; set; }
        public bool IsDraft { get; set; }
        public string Status { get; set; }
        public string ReceiptCode { get; set; }
    }

    public class PreProcessReceiptItemModel
    {
        public int ID { get; set; }
        public int PurificationIssueID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int PurifiedItemID { get; set; }
        public string PurifiedItemName { get; set; }
        public string PurifiedItemUnit { get; set; }
        public int UnitID { get; set; }
        public decimal Quantity { get; set; }
        public decimal QtyMet { get; set; }
        public decimal BalanceQty { get; set; }
        public int ProcessID { get; set; }
        public int PurifiedItemUnitID { get; set; }
        public string TransDateStr { get; private set; }
        public DateTime? TransDate
        {
            get
            {
                return TransDateStr.ToDateTime();
            }
            set { TransDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public DateTime TransTime { get; set; }
        public string ProcessName { get; set; }
        public string TransNo { get; set; }
    }

    public class PreProcessReceiptPurificationItemModel
    {
        public int ID { get; set; }
        public int ReceiptItemID { get; set; }
        public string ReceiptItem { get; set; }
        public decimal _receiptQuantity { get; set; }
        public decimal ReceiptQuantity { get { return Math.Round(_receiptQuantity, 4); } set { _receiptQuantity = value; } }
        public string ReceiptDateStr { get; set; }
        public DateTime? ReceiptDate
        {
            get
            {
                return ReceiptDateStr.ToDateTime();
            }
            set { ReceiptDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }

        public string ReceiptItemUnit { get; set; }
        public int ReceiptItemUnitID { get; set; }
        public int IssuedItemID { get; set; }
        public string IssuedItemName { get; set; }
        public string IssuedItemUnit { get; set; }
        public decimal _issuedQuantity { get; set; }
        public decimal IssuedQuantity { get { return Math.Round(_issuedQuantity, 2); } set { _issuedQuantity = value; } }
        public string IssuedDateStr { get; set; }
        public DateTime? IssuedDate
        {
            get
            {
                return IssuedDateStr.ToDateTime();
            }
            set { IssuedDateStr = value == null ? "" : ((DateTime)value).ToDateStr(); }
        }
        public string ProcessName { get; set; }
        public decimal _transQuantity { get; set; }
        public decimal TransQuantity { get { return Math.Round(_transQuantity, 2); } set { _transQuantity = value; } }
        //public string TransDateStr { get; set; }
        //public DateTime TransDate
        //{
        //    get
        //    {
        //        return TransDateStr.ToDateTime();
        //    }
        //    set { TransDateStr = value.ToDateStr(); }
        //}

        public int MaterialPurificationIssueTransID { get; set; }
        public bool IsCompleted { get; set; }
        public decimal QtyMet { get; set; }
        public decimal BalanceQty { get; set; }
    }

    public static partial class Mapper
    {
        public static List<PreProcessReceiptItemModel> MapToModelList(this List<PreprocessReceiptItemBO> boList)
        {
            if (boList != null && boList.Count() > 0)
            {
                return (from x in boList
                        select new PreProcessReceiptItemModel()
                        {
                            ID = x.ID,
                            PurificationIssueID = x.PurificationIssueID,
                            ItemID = x.ItemID,
                            ItemName = x.ItemName,
                            Unit = x.Unit,
                            PurifiedItemID = x.PurifiedItemID,
                            PurifiedItemName = x.PurifiedItemName,
                            PurifiedItemUnit = x.PurifiedItemUnit,
                            PurifiedItemUnitID = x.PurifiedItemUnitID,
                            Quantity = x.Quantity,
                            QtyMet = x.QtyMet,
                            BalanceQty = x.BalanceQty,
                            ProcessID = x.ProcessID,
                            TransDate = x.TransDate,
                            TransTime = x.TransTime,
                            ProcessName = x.ProcessName,
                            TransNo=x.TransNo,
                           
                        }).ToList();
            }
            else
                return null;
        }

        public static List<PreProcessReceiptPurificationItemModel> MapToModelList(this List<PreProcessReceiptPurificationItemBO> boList)
        {
            if (boList != null && boList.Count() > 0)
            {
                return (from x in boList
                        select new PreProcessReceiptPurificationItemModel()
                        {
                            ReceiptItemID = x.ReceiptItemID,
                            ReceiptItem = x.ReceiptItem,
                            ReceiptItemUnit = x.ReceiptItemUnit,
                            ReceiptItemUnitID = x.ReceiptItemUnitID,
                            IssuedItemID = x.IssuedItemID,
                            IssuedItemName = x.IssuedItemName,
                            IssuedItemUnit = x.IssuedItemUnit,
                            IssuedQuantity = x.IssuedQuantity,
                            IssuedDate = x.IssuedDate,
                            ProcessName = x.ProcessName,
                            ReceiptDate = x.ReceiptDate,
                            ReceiptQuantity = x.ReceiptQuantity,
                            MaterialPurificationIssueTransID = x.MaterialPurificationIssueTransID,
                            BalanceQty = x.BalanceQty,
                          
                        }).ToList();
            }
            else
                return null;
        }

        public static List<PreProcessReceiptPurificationItemBO> MapToBoList(this List<PreProcessReceiptPurificationItemModel> modelList)
        {
            if (modelList != null)
                return (from x in modelList
                        select new PreProcessReceiptPurificationItemBO()
                        {
                            ReceiptItemID = x.ReceiptItemID,
                            ReceiptItem = x.ReceiptItem,
                            ReceiptItemUnit = x.ReceiptItemUnit,
                            ReceiptItemUnitID = x.ReceiptItemUnitID,
                            IssuedItemID = x.IssuedItemID,
                            IssuedItemName = x.IssuedItemName,
                            IssuedItemUnit = x.IssuedItemUnit,
                            IssuedQuantity = x.IssuedQuantity,
                            IssuedDate = (DateTime)x.IssuedDate,
                            ProcessName = x.ProcessName,
                            ReceiptDate = (DateTime)x.ReceiptDate,
                            ReceiptQuantity = x.ReceiptQuantity,
                            MaterialPurificationIssueTransID = x.MaterialPurificationIssueTransID,
                            IsCompleted =x.IsCompleted,
                            BalanceQty = x.BalanceQty
                        }).ToList();
            else return new List<PreProcessReceiptPurificationItemBO>();
        }

        public static PreprocessReceiptBO MapToBo(this PreProcessReceiptViewModel model)
        {
            if (model != null)
                return new PreprocessReceiptBO()
                {
                    ID = model.ID,
                    TransNo = model.TransNo,
                    TransDate = (DateTime)model.TransDate,
                    IsDraft = model.IsDraft,
                    PreProcessReceiptPurificationItemBOList = model.PreProcessReceiptPurificationItemModelList.MapToBoList()
                };
            else return new PreprocessReceiptBO();
        }

        public static PreProcessReceiptViewModel MapToModel(this PreprocessReceiptBO bo)
        {
            if (bo != null)
                return new PreProcessReceiptViewModel()
                {
                    ID = bo.ID,
                    IsDraft = bo.IsDraft,
                    TransDate = bo.TransDate,
                    TransNo = bo.TransNo,
                    IsCancelled = bo.IsCancelled,
                    PreProcessReceiptPurificationItemModelList = bo.PreProcessReceiptPurificationItemBOList.MapToModelList()
                };
            else return null;
        }

        public static List<PreProcessReceiptDisplayViewModel> MapToModelList(this List<PreProcessReceiptDisplayBO> boList)
        {
            if (boList != null)
                return (from x in boList
                        select new PreProcessReceiptDisplayViewModel()
                        {
                            ID = x.ID,
                            IssuedItem = x.IssuedItem,
                            IssuedItemUnit = x.IssuedItemUnit,
                            IssuedQuantity = x.IssuedQuantity,
                            Activity = x.Activity,
                            ReceiptItem = x.ReceiptItem,
                            ReceiptItemUnit = x.ReceiptItemUnit,
                            ReceiptQuantity = x.ReceiptQuantity,
                            QuantityLoss = x.IssuedQuantity - x.ReceiptQuantity,
                            IsDraft = x.IsDraft,
                            ReceiptCode=x.ReceiptCode,
                            Status = x.IsCancelled ? "cancelled" : x.IsDraft ? "draft" : ""
                        }).ToList();
            else return new List<PreProcessReceiptDisplayViewModel>();
        }
    }
}