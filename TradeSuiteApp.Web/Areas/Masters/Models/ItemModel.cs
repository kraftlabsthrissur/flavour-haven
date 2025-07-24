using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Models
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
        public string InventoryUnit { get; set; }
        public String ItemCategory { get; set; }
        public int ID { get; set; }
        public int SalesInquiryItemID { get; set; }
        public int PurchaseRequisitionTrasID { get; set; }
        public decimal BatchSize { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public int InventoryUnitID { get; set; }
        public string InventoryUOM { get; set; }
        public int LooseUnitID { get; set; }
        public string LooseUnit { get; set; }
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
        public string PartsGroup { get; set; }
        public string PartsNumbers { get; set; }
        public string SecondaryUnit { get; set; }

        public string DeliveryTerm { get; set; }
        public int SalesIncentiveCategoryID { get; set; }
        public string OldItemCode { get; set; }
        public int PackSize { get; set; }
        public int SalesCategoryID { get; set; }
        public bool IsSaleable { get; set; }
        public string SalesUOM { get; set; }
        public int SalesUnitID { get; set; }
        public int UnitGroupID { get; set; }
        public string UOMGroup { get; set; }
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
        public string PartsClass { get; set; }
        public int PurchaseCategoryID { get; set; }
        public int PurchaseUnitID { get; set; }
        public string PurchaseUOM { get; set; }
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
        public int GSTCategoryID { get; set; } // TaxID
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
        public decimal? SalesPrice { get; set; }
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
        public SelectList CategorySelectList { get; set; }

        public SelectList AssetCategoryList { get; set; }
        public SelectList GSTCategoryList { get; set; }
        public SelectList AccountCategoryList { get; set; }
        public SelectList DiseaseCategoryList { get; set; }
        public SelectList GSTSubCategoryList { get; set; }
        public SelectList productionCategoryList { get; set; }
        public SelectList ProductionGroupList { get; set; }
        public SelectList GetMasterFormulaList { get; set; }
        public List<UnitModel> UnitList { get; set; }
        public SelectList UnitGroupList { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList BuyerList { get; set; }
        public SelectList OEMCountryList { get; set; }
        public SelectList OEMCodeList { get; set; }
        public SelectList ABCCodeList { get; set; }
        public SelectList TaxTypeList { get; set; }
        public SelectList SupplierList { get; set; }
        public SelectList WareHouseList { get; set; }
        public SelectList CostingMethodList { get; set; }
        public SelectList BrandList { get; set; }
        
        public SelectList BinList { get; set; }
        public SelectList SecondaryUnitList { get; set; }
        
        public SelectList LotList { get; set; }
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
        public string ItemImagePath { get; set; }
        public string[] ItemImages { get; set; }
        public string Status { get; set; }
        public string ProductionGroup { get; set; }
        public int ShelfLifeMonths { get; set; }
        public decimal ConversionFactorPurchaseToInventory { get; set; }
        public decimal ConversionFactorPurchaseToLoose { get; set; }
        public decimal ConversionFactorSalesToInventory { get; set; }

        public decimal ConversionFactorLooseToSales { get; set; }

        public bool IsKalkan { get; set; }
        public decimal LastPR { get; set; }
        public decimal LowestPR { get; set; }
        public decimal? QtyUnderQC { get; set; }
        public decimal PendingPOQty { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public int PriceLocationID { get; set; }
        public List<PreviousBatchModel> InvoiceDetails { get; set; }

        public int ManufacturerID { get; set; }
        public string Manufacturer { get; set; }
        public SelectList ManufacturerList { get; set; }

        public decimal CostPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal LandedCost { get; set; }
        public string CostingMethod { get; set; }
        public int CostingMethodID { get; set; }
        public decimal DiscountPercentage { get; set; }
        //public decimal QuantityOnHand { get; set; }

        public decimal ItemLength { get; set; }
        public string LengthUOM { get; set; }
        public int LengthUOMID { get; set; }
        public decimal ItemWidth { get; set; }
        public string WidthUOM { get; set; }
        public int WidthUOMID { get; set; }
        public decimal ItemHight { get; set; }
        public string HightUOM { get; set; }
        public string NetWeightUOM { get; set; }
        public int HightUOMID { get; set; }
        public decimal NetWeight { get; set; }
        public int NetWeightUOMID { get; set; }
        public decimal InnerDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public int BuyerID { get; set; }
        public string SupplierPartCode { get; set; }
        public string OEMCode { get; set; }
        public string OEMCountryName { get; set; }
        public int OEMCountryID { get; set; }
        public int ABCCodeID { get; set; }
        public string ABCCode { get; set; }
        public string EANCode { get; set; }
        public string BuyerName { get; set; }
        public string SupplierName { get; set; }
        public int SupplierID { get; set; }
        public decimal? BudgetQuantity { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string AlternativeItemName { get; set; }
        public int WareHouseID { get; set; }
        public int BinID { get; set; }
        public int ItemSecondaryUnitID { get; set; }
        public int LotID { get; set; }
        public bool IsDefault { get; set; }
        public int TaxTypeID { get; set; }
        public List<ItemTax> ItemTaxList { get; set; }
        public List<ItemPartsNumber> ItemPartsNumberList { get; set; }
        public List<ItemSecondaryUnit> ItemSecondaryUnitList { get; set; }
        public List<ItemWareHouse> ItemWareHouseList { get; set; }
        public List<AlternativeItem> AlternativeItemList { get; set; }
        public List<ItemSalesPrice> ItemSalesPriceList { get; set; }
        public decimal VATPercentage { get; set; }
        public decimal VATAmount { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyID { get; set; }
        public string TaxType { get; set; }
        public int IsGST { get; set; }
        public int IsVAT { get; set; }
        public decimal DisplayPercentage { get; set; }
        public int BrandID { get; set; }
        public string cross_reference { get; set; }

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
    public class ItemPartsNumber
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string PartsNumber { get; set; }
        public bool IsDefault { get; set; }
    }
    public class ItemSecondaryUnit
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int SecondaryUnitID { get; set; }
        public string SecondaryUnit { get; set; }
    }
    public class ItemTax
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int LocationID { get; set; }
        public int TaxTypeID { get; set; }
        public int GSTCategoryID { get; set; }
        public string Location { get; set; }
        public string TaxType { get; set; }
        public string GSTCategory { get; set; }
    }
    public class ItemWareHouse
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
    public class AlternativeItem
    {
        public int ID { get; set; }
        public int AlternativeItemID { get; set; }
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
    }
    public class ItemSalesPrice
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int LocationID { get; set; }
        public string Location { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal LoosePrice { get; set; }
        public int Edited { get; set; }
    }
}