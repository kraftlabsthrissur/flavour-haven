using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//test
namespace BusinessObject
{
    public class ItemBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; } // Same As ID 
        public string Code { get; set; }
        public string Name { get; set; }
        public int SalesInquiryItemID { get; set; }
        public string PartsNumbers { get; set; }
        public string Unit { get; set; }
        public string SecondaryUnits { get; set; }
        public int UnitID { get; set; }
        public decimal? Stock { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal SGSTPercentage { get; set; }
        public decimal CessPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public int PurchaseRequisitionTrasID { get; set; }
        public decimal Rate { get; set; }
        public decimal LooseRate { get; set; }
        public decimal Qty { get; set; }
        
        public string InventoryUnit { get; set; }
        public decimal? QtyUnderQC { get; set; }
        public Nullable<decimal> ReqQty { get; set; }
        public Nullable<decimal> QtyOrdered { get; set; }
        public string PurchaseUnit { get; set; }
        public string Remarks { get; set; }
        public string Remark { get; set; }
        public string DeliveryTerm { get; set; }
        public string PartsClass { get; set; }
        public string PartsGroup { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int ItemTypeID { get; set; }
        public int ItemCategoryID { get; set; }
        public int applicationID { get; set; }
        public int LocationID { get; set; }
        public int FinYear { get; set; }
        public string RequiredStatus { get; set; }
        public string BatchType { get; set; }
        public int BatchTypeID { get; set; }
        public int TravelCategoryID { get; set; }
        public int FGCategoryID { get; set; }
        
        public String ItemCategory { get; set; }
        public string PurchaseCategory { get; set; }
        public decimal MRP { get; set; }
        public decimal? PendingOrderQty { get; set; }
        public decimal? QtyAvailable { get; set; }
        
        public string Type { get; set; }
        public int WareHouseID { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal? LoosePrice { get; set; }
        //GoodsReceiptNoteTrans
        public string UOMGroup { get; set; }
        public string InventoryUOM { get; set; }
        public string PurchaseUOM { get; set; }
        public string SalesUOM { get; set; }
        public string LooseUnit { get; set; }
        public string LengthUOM { get; set; }
        public string WidthUOM { get; set; }
        public string HightUOM { get; set; }

        public string NetWeightUOM { get; set; }

        public string PurchaseOrderNo { get; set; }
        public decimal PendingPOQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal AcceptedQty { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? GSTPercentage { get; set; }


        //Production Issue

        public string ProductName { get; set; }
        public string UOM { get; set; }
        public int QOM { get; set; }
        public string UnitName { get; set; }
        public decimal BatchSize { get; set; }
        public decimal StandardOutput { get; set; }
        public string ProcessStage { get; set; }
        public int ProductionGroupId { get; set; }
        public int ProductionSequence { get; set; }

        public int ProductID { get; set; }
        public int ProductGroupID { get; set; }


        public string ItemTypeName { get; set; }
        public int DrugID { get; set; }
        public string DrugName { get; set; }


        public string MalayalamName { get; set; }
        public string HindiName { get; set; }
        public string SanskritName { get; set; }
        public int StorageCategoryID { get; set; }
        public string StorageCategoryName { get; set; }
        public int SecondaryUnitID { get; set; }
        public int InventoryUnitID { get; set; }
        public decimal? ConversionFactorPtoI { get; set; }
        public decimal MinStockQTY { get; set; }
        public decimal MaxStockQTY { get; set; }
        public bool IsDemandPlanRequired { get; set; }
        public bool IsMaterialPlanRequired { get; set; }
        public bool IsStockItem { get; set; }
        public bool IsStockValue { get; set; }
        public bool IsPhantomItem { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DisContinuedDate { get; set; }
        public string HSNCode { get; set; }
        public int? BuyerID { get; set; }
        public int? SupplierID { get; set; }
        public string BuyerName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPartCode { get; set; }
        public string OEMCode { get; set; }
        public int? OEMCountryID { get; set; }
        public string OEMCountryName { get; set; }
        public int? ABCCodeID { get; set; }
        public string ABCCode { get; set; }
        public string EANCode { get; set; }
        public int BusinessCategoryID { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public string OldItemCode { get; set; }
        public int PackSize { get; set; }
        public int SalesCategoryID { get; set; }
        public int PrimaryUnitID { get; set; }
        public string PrimaryUnit { get; set; }
        public bool IsSaleable { get; set; }
        public int DrugScheduleID { get; set; }
        public int SalesUnitID { get; set; }
        public decimal ConversionFactorPtoS { get; set; }
        public decimal MinSalesQtyLoose { get; set; }
        public decimal MinSalesQtyFull { get; set; }
        public decimal MaxSalesQty { get; set; }
        public DateTime? SeasonStarts { get; set; }
        public DateTime? SeasonEnds { get; set; }
        public DateTime? SeasonPurchaseStarts { get; set; }
        public DateTime? SeasonPurchaseEnds { get; set; }
        public bool N2GActivity { get; set; }
        public bool IsMrp { get; set; }
        public decimal ConversionFactorPtoSecondary { get; set; }

        public string BotanicalName { get; set; }
        public bool IsQCRequired { get; set; }
        public bool IsQCRequiredForProduction { get; set; }
        public string PatentNo { get; set; }
        public int QCCategoryID { get; set; }
        public int CostingCategoryID { get; set; }
        public bool IsProprietary { get; set; }
        public bool IsPurchaseItem { get; set; }
        public int PurchaseCategoryID { get; set; }
        public int PurchaseUnitID { get; set; }
        public string ReOrderLevelName { get; set; }
        public decimal ReOrderLevel { get; set; }
        public decimal MinPurchaseQTY { get; set; }
        public decimal MaxPurchaseQTY { get; set; }
        public bool IsSeasonalPurchase { get; set; }
        public bool IsPORequired { get; set; }
        public int PurchaseLeadTime { get; set; }
        public int AssetCategoryID { get; set; }
        public decimal QtyTolerancePercent { get; set; }
        public decimal ReOrderQty { get; set; }
        public decimal QuantityOnHand { get; set; }
        public decimal ItemLength { get; set; }
        public int? LengthUOMID { get; set; }
        public decimal ItemWidth { get; set; }
        public int? WidthUOMID { get; set; }
        public decimal ItemHight { get; set; }
        public int? HightUOMID { get; set; }
        public decimal NetWeight { get; set; }
        public int? NetWeightUOMID { get; set; }
        public decimal InnerDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public int GSTCategoryID { get; set; }
        public int AccountsCategoryID { get; set; }
        public int DiseaseCategoryID { get; set; }
        public int GSTSubCategoryID { get; set; }
        public string GSTSubCategoryName { get; set; }
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
        public decimal? BudgetQuantity { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int IsVat { get; set; }
        public decimal IGSTPercent { get; set; }
        public decimal CGSTPercent { get; set; }
        public decimal SGSTPercent { get; set; }
        public int IsGST { get; set; }
        public string QRCode { get; set; }
        public string Description { get; set; }
        public int UnitGroupID { get; set; }
        public int BatchSizeQTY { get; set; }
        public bool IsDisContinued { get; set; }
        public string MasterFormulaName { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string OldItemCode2 { get; set; }
        public int CategoryID { get; set; }

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
        public string BusinessCategoryName { get; set; }
        public string SalesIncentiveCategoryName { get; set; }
        public string ProductionCategoryName { get; set; }
        public int CounterSalesTransUnitID { get; set; }
        public bool Isactive { get; set; }
        public string Status { get; set; }
        public string ProductionGroup { get; set; }
        public int ShelfLifeMonths { get; set; }
        public string OldName { get; set; }
        public decimal ConversionFactorSalesToInventory { get; set; }
        public decimal ConversionFactorPurchaseToInventory { get; set; }
        public decimal ConversionFactorPurchaseToLoose { get; set; }
        public decimal ConversionFactorLooseToSales { get; set; }

        public bool IsKalkan { get; set; }
        public decimal LastPR { get; set; }
        public decimal LowestPR { get; set; }
        public decimal ExchangeRate { get; set; }
        public int CountryID { get; set; }
        public int CurrencyID { get; set; }
        
        public List<LocationList> ItemLocationList { get; set; }
        public int ManufacturerID { get; set; }
        public string Manufacturer { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string SalesCategory { get; set; }
        public int FullUnitID { get; set; }
        public string FullUnit { get; set; }
        public int LooseUnitID { get; set; }
        public bool IsEnabled { get; set; }
        public decimal CostPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal PurchaseLoosePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal LandedCost { get; set; }
        public int CostingMethodID { get; set; }
        public string CostingMethod { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<ItemTaxBO> ItemTaxBOList { get; set; }
        public List<ItemWareHouseBO> ItemWareHouseBOList { get; set; }
        public List<AlternativeItemBO> AlternativeItemBOList { get; set; }
        public List<ItemSalesPriceBO> ItemSalesPriceBOList { get; set; }
        public List<ItemPartsNumberBO> ItemPartsNumberBOList { get; set; }
        public List<ItemPartsNumberBO> UpdateItemPartsNumberBOList { get; set; }
        public List<ItemSecondaryUnitBO> ItemSecondaryUnitList { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal GstPercentage { get; set; }
        public string PartsNumber { get; set; }
        public decimal RetailMRP { get; set; }
        public decimal RetailLooseRate { get; set; }
        public string ItemImagePath1 { get; set; }
        public string ItemImagePath2 { get; set; }
        public string ItemImagePath3 { get; set; }
        public string ItemImagePath4 { get; set; }
        public string ItemImagePath5 { get; set; }
        public string ItemImagePath6 { get; set; }
        public string ItemImagePath7 { get; set; }
        public string ItemImagePath8 { get; set; }
        public Nullable<decimal> DisplayPercentage { get; set; }
        public Nullable<int> BrandID { get; set; }
        public string cross_reference { get; set; }

    }

    public class LocationList
    {
        public int LocationID { get; set; }
        public int ItemLocationID { get; set; }
        public string LocationName { get; set; }
    }

    public class ItemDescriptionBO
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class StockBO
    {
        public int id { get; set; }
        public int itemid { get; set; }
        public int batchid { get; set; }
        public int warehouseid { get; set; }
        public int SalesUnitID { get; set; }
        public string transactiontype { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal MRP { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal issue { get; set; }
        public decimal Receipt { get; set; }
        public decimal value { get; set; }
    }
    public class ItemTaxBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public int TaxTypeID { get; set; }
        public string TaxType { get; set; }
        public int GSTCategoryID { get; set; }
        public string GSTCategory { get; set; }
    }
    public class ItemWareHouseBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int WareHouseID { get; set; }
        public int BinID { get; set; }
        public int LotID { get; set; }
        public bool IsDefault { get; set; }
        public string WareHouse { get; set; }
        public string Bin { get; set; }
        public string Lot { get; set; }
        public string Default { get; set; }
    }
    public class AlternativeItemBO
    {
        public int ID { get; set; }
        public int AlternativeItemID { get; set; }
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
    }
    public class ItemSalesPriceBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal LoosePrice { get; set; }
    }

    public class ItemPartsNumberBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string PartsNumber { get; set; }
        public bool IsDefault { get; set; }
    }
    public class ItemSecondaryUnitBO
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int SecondaryUnitID { get; set; }
        public string SecondaryUnit { get; set; }
    }
}
