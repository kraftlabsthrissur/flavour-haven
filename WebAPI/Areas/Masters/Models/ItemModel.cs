using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Utils;

namespace WebAPI.Areas.Masters.Models
{
    public class ItemModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public int ItemCategoryID { get; set; }
        public int UnitID { get; set; }
        public int ProductGroupID { get; set; }
        public int ProductID { get; set; }
        public decimal? Stock { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal Rate { get; set; }
        public String ItemCategory { get; set; }
        public int ID { get; set; }
        public decimal BatchSize { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int InventoryUnitID { get; set; }
        public string InventoryUnit { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public int StorageCategoryID { get; set; }
        public string StorageCategoryName { get; set; }
        public int ItemTypeID { get; set; }
        public int SecondaryUnitID { get; set; }
        public decimal? ConversionFactorPtoI { get; set; }
        public decimal MinStockQTY { get; set; }
        public decimal MaxStockQTY { get; set; }
        public bool IsDemandPlanRequired { get; set; }
        public bool IsMaterialPlanRequired { get; set; }
        public bool IsStockItem { get; set; }
        public bool IsStockValue { get; set; }
        public bool IsPhantomItem { get; set; }
        public string BirthDate { get; set; }
        public string DisContinuedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string HSNCode { get; set; }
        public int BusinessCategoryID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public string OldItemCode { get; set; }
        public int PackSize { get; set; }
        public int SalesCategoryID { get; set; }
        public bool IsSaleable { get; set; }
        public int SalesUnitID { get; set; }
        public decimal ConversionFactorPtoS { get; set; }
        public decimal MinSalesQtyLoose { get; set; }
        public decimal MinSalesQtyFull { get; set; }
        public decimal MaxSalesQty { get; set; }
        // public DateTime SeasonStarts { get; set; }
        public string SeasonEnds { get; set; }
        public string SeasonPurchaseStarts { get; set; }
        public string SeasonPurchaseEnds { get; set; }
        public string SeasonStarts { get; set; }

        public bool N2GActivity { get; set; }
        public bool IsMrp { get; set; }
        public decimal ConversionFactorPtoSecondary { get; set; }
        public string BotanicalName { get; set; }
        public bool IsQCRequired { get; set; }
        public bool IsQCRequiredForProduction { get; set; }
        public int QCCategoryID { get; set; }
        public int CostingCategoryID { get; set; }
        public bool IsProprietary { get; set; }
        public bool IsPurchaseItem { get; set; }
        public string PatentNo { get; set; }
        public int PurchaseCategoryID { get; set; }
        public int PurchaseUnitID { get; set; }
        public decimal ReOrderLevel { get; set; }
        public decimal MinPurchaseQTY { get; set; }
        public decimal MaxPurchaseQTY { get; set; }
        public bool IsSeasonalPurchase { get; set; }
        public bool IsPORequired { get; set; }
        public int PurchaseLeadTime { get; set; }
        public int AssetCategoryID { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public decimal ReOrderQty { get; set; }
        public int GSTCategoryID { get; set; }
        public int AccountsCategoryID { get; set; }
        public int DiseaseCategoryID { get; set; }
        public int GSTSubCategoryID { get; set; }
        public decimal NormalLossQty { get; set; }
        public decimal NormalLossPercent { get; set; }
        public int ProductLeadDays { get; set; }
        public bool IsReProcessAllowed { get; set; }
        public bool IsDraft { get; set; }
        public bool IsPriceListReference { get; set; }
        public bool IsLocation { get; set; }
        public bool IsInterCompany { get; set; }
        public bool IsDepartment { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsProject { get; set; }
        public bool IsAsset { get; set; }
        public int ProductionCategoryID { get; set; }
        public int MasterFormulaRefNo { get; set; }
        public bool IsMasterFormula { get; set; }
        public bool IsBatch { get; set; }
        public string BarCode { get; set; }
        public string QRCode { get; set; }
        public string Description { get; set; }
        public int BatchSizeQTY { get; set; }
        public bool IsDisContinued { get; set; }
        public string OldItemCode2 { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal? LoosePrice { get; set; }
        public SelectList StorageCategoryList { get; set; }
        public List<CategoryModel> ItemCategoryList { get; set; }
        public SelectList ItemTypeList { get; set; }
        public SelectList UOMList { get; set; }
        public SelectList BusinessCategoryList { get; set; }
        public SelectList SalesIncentiveCategoryList { get; set; }
        public SelectList SalesCategoryList { get; set; }
        public SelectList DrugScheduleTypeList { get; set; }
        public SelectList QCCategoryList { get; set; }
        public SelectList CostCategoryList { get; set; }
        public SelectList PurchaseCategoryList { get; set; }
        public SelectList AssetCategoryList { get; set; }
        public SelectList GSTCategoryList { get; set; }
        public SelectList AccountCategoryList { get; set; }
        public SelectList DiseaseCategoryList { get; set; }
        public SelectList GSTSubCategoryList { get; set; }
        public SelectList productionCategoryList { get; set; }
        public SelectList ProductionGroupList { get; set; }
        public SelectList GetMasterFormulaList { get; set; }
        public List<UnitModel> UnitList { get; set; }
        public SelectList LocationList { get; set; }
        public List<LocationList> ItemLocationList { get; set; }
        public int QCLeadTime { get; set; }
        public int DiscountID { get; set; }
        public int CostingGroupID { get; set; }
        public int CostComponentID { get; set; }
        public string CategoryName { get; set; }
        public string PurchaseCategoryName { get; set; }
        public string QCCategoryName { get; set; }
        public string CostingCategoryName { get; set; }
        public string SecondaryUnitName { get; set; }
        public string InventoryUnitName { get; set; }
        public string SalesUnitName { get; set; }
        public string PurchaseUnitName { get; set; }
        public string SalesCategoryName { get; set; }
        public string DiseaseCategoryName { get; set; }
        public string GSTCategoryName { get; set; }
        public string AccountsCategoryName { get; set; }
        public string DrugScheduleName { get; set; }
        public string AssetCategoryName { get; set; }
        public string ItemTypeName { get; set; }
        public string BusinessCategoryName { get; set; }
        public string SalesIncentiveCategoryName { get; set; }
        public string GSTSubCategoryName { get; set; }
        public string ProductionCategoryName { get; set; }
        public string MasterFormulaName { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal PendingOrderStock { get; set; }
        public string UnitOMID { get; set; }
        public int DrugScheduleID { get; set; }
        public string MalayalamName { get; set; }
        public string HindiName { get; set; }
        public string SanskritName { get; set; }
        public string OldName { get; set; }
        public string SalesUnit { get; set; }
        public int PrimaryUnitID { get; set; }
        public string PrimaryUnit { get; set; }
        public bool Isactive { get; set; }
        public string Status { get; set; }
        public string ProductionGroup { get; set; }
        public int ShelfLifeMonths { get; set; }
        public decimal ConversionFactorSalesToInventory { get; set; }
        public decimal ConversionFactorPurchaseToInventory { get; set; }
        public bool IsKalkan { get; set; }
        public decimal LastPR { get; set; }
        public decimal LowestPR { get; set; }
        public decimal? QtyUnderQC { get; set; }
        public decimal PendingPOQty { get; set; }
        public int LocationID { get; set; }
        public string SalesCategory { get; set; }
        public int FullUnitID { get; set; }
        public string FullUnit { get; set; }
        public int LooseUnitID { get; set; }
        public string LooseUnit { get; set; }
        public string PurchaseUnit { get; set; }
        public bool IsEnabled { get; set; }
        public decimal GSTPercentage { get; set; }
        public int ManufacturerID { get; set; }
        public string Manufacturer { get; set; }
    }

    public class ItemTransDetailsModel
    {
        public int ItemID { get; set; }
        public decimal? Stock { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal LastPR { get; set; }
        public decimal LowestPR { get; set; }
        public decimal? QtyUnderQC { get; set; }
    }

    public class ItemDescriptionModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class StockModel
    {
        public int id { get; set; }
        public int itemid { get; set; }
        public int batchid { get; set; }
        public int warehouseid { get; set; }
        public int SalesUnitID { get; set; }
        public string transactiontype { get; set; }
        public string BatchNo { get; set; }
        public string ExpiryDate { get; set; }
        public decimal MRP { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal issue { get; set; }
        public decimal Receipt { get; set; }
        public decimal value { get; set; }
    }
}