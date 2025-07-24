using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface ItemContract
    {
        List<ItemBO> GetServiceItems(string Hint, int ItemCategoryID = 0, int PurchaseCategoryID = 0);

        List<ItemBO> GetPackingItemsForAutoComplete(int PackingSequence, string Hint);

        List<ItemBO> GetProductionGroupItemsForAutoComplete(string Hint);

        List<ItemBO> GetSaleableItemsForAutoComplete(string Hint, int ItemCategoryID = 0, int SalesCategoryID = 0, int PriceListID = 0);

        List<ItemBO> GetStockableItemsForAutoComplete(string Hint, int ItemCategoryID = 0);

        List<ItemBO> GetGRNWiseItemsForAutoComplete(string Hint, int SupplierID);

        DatatableResultBO GetItemsForSchemeItem(int CategoryID);

        List<ItemBO> GetAllItemsForAutoComplete(string Hint, int ItemCategoryID = 0, int SalesCategoryID = 0);

        List<ItemBO> GetReceiptItemsForAutoComplete(int ProductGroupID, string Hint);

        List<ItemBO> GetPackingReturnForAutoComplete(string Hint, int ProductGroupID = 0, int ReceiptItemID = 0, int IssueItemID = 0);

        List<ItemBO> GetReciptItemsItemList(int ProductGroupID);

        DatatableResultBO GetSaleableItemsList(int ItemCategoryID, int SalesCategoryID, int BusinessCategoryID, int PriceListID, int StoreID, bool CheckStock, int BatchTypeID, string FullOrLoose, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string PartsNoHint, string ModelHint, string RemarkHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetItemsListForReport(string StockType, int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetStockableItemsList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string PartsNumberHHit, string ModelHHit, string MakeHHit, string SortField, string SortOrder, int Offset, int Limit);
        List<ItemBO> GetItemsListByPurchaseRequisitionIDS(int[] PurchaseRequisitionIDS);
        List<ItemBO> GetItemsListByPurchaseRequisition(int PurchaseRequisitionID);

        DatatableResultBO GetItemsListForPurchase(string Type, int ItemCategoryID, int PurchaseCategoryID, int BusinessCategoryID, int SupplierID, string CodeHint, string NameHint, string UnitHint, string PurchaseCategoryHint, string ItemCategoryHint, string PartsNumberHit, string ModelHint, string RemarksHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetReturnItemList(int ProductionGroupID, int IssueItemID, int ReceiptItemID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetGRNWiseItemsList(int SupplierID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetProductionGroupList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetProductionGroupItemAutoCompleteForReport(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetRawMaterialList(int WarehouseID, string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetPackingMaterialList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetPackingItemList(string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetPreProcessIssueItemsList(string CodeHint, string NameHint, string ItemCategoryHint, string ActivityHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetAvailableStockItemsList(int ItemCategoryID, int WarehouseID, int BatchTypeID, string CodeHint, string NameHint, string ItemCategoryHint, string PartsNoHint, string MakeHint, string ModelHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetDebitAndCreditItemsList(string Type, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetProductionIssueMaterialReturnList(int ProductionID, int ProductionSequence, string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetProductionDefinitionMaterialList(string Type, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetItemsListForMaterialPurification(int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetItemsListForSaleableServiceItem(int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit);

        DatatableResultBO GetItemsListForSaleableServiceAndStockItem(int SalesCategoryID, int SalesIncentiveCategoryID, int BusinessCategoryID, string CodeHint, string NameHint, string SalesCategoryHint, string SalesIncentiveCategoryHint, string BusinessCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetItemsListForStockAdjustmentForAlopathy(DateTime FromDate, DateTime ToDate, string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);

        List<ItemBO> GetAvailableStocktemsForAutoComplete(string Hint, int ItemCategoryID = 0, int WarehouseID = 0);

        List<ItemBO> GetItemTypeList();
        List<ItemBO> GetBrandList();
        
        List<ItemBO> GetDrugScheduleTypeList();

        List<ItemBO> GetMasterFormulaList();

        int CreateItem(ItemBO itemBO, List<ItemBO> ItemLocationList);
        int CreateItemV3(ItemBO itemBO, List<ItemBO> ItemLocationList);
        DatatableResultBO GetAllItemList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetItemList(string Code, string Description, string Category, string PartsNo, string PartsClass, string partsGroup, string remark, string model, string SortField, string SortOrder, int Offset, int Limit);
        ItemBO GetItemDetails(int ItemID);
        ItemBO GetItemDetail(int ItemID);
        ItemBO GetItemDetailsV3(int ItemID);
        List<ItemBO> GetItemsByPackingCodes(string[] PackingCodes);

        List<ItemBO> GetItemsByItemCodes(string[] ItemCodes);

        decimal GetAverageSales(int ItemID);

        decimal GetAvailableStock(int ItemID, int? BatchID, int? BatchTypeID, int WarehouseID, int LocationID);

        string IsExist(string Name, string HSNCode, int ID);
        string GetItemCode(string QRCode);
        string GetBatchNo(string QRCode);

        DatatableResultBO GetItemsForOfficialAdvance(string CodeHint, string NameHint, string Type, string SortField, string SortOrder, int Offset, int Limit);

        List<ItemBO> GetItemForChequeStatus();
        List<ItemBO> GetItemTransactionDetails(int ItemID, int BatchTypeID);
        DatatableResultBO GetItemsList(string Type, int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetItemsListForStockAdjustment(int ItemCategoryID, int SalesCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        List<ItemBO> GetPreProcessReceiptItemForAutoComplete(string Hint);

        DatatableResultBO GetItemListForTreatment(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit);

        ItemBO GetAllCategoryID();
        ItemBO GetServiceItemCategory();
        List<ItemDescriptionBO> GetDescription(int ItemID, string Type);
        DatatableResultBO GetServiceItemList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        DatatableResultBO GetAllItemListV3(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit);
        List<ItemBO> GetItemListForAPI(int Offset, int Limit);
        List<StockBO> GetStockListForAPI(int ItemID);

        DatatableResultBO GetAlternativeItemList(string Code, string Name, string ItemCategory, string ItemUnit, string SortField, string SortOrder, int Offset, int Limit);
        int DeleteItemWareHouse(int ID);
        int DeleteItemParts(int ItemID, int ID);
        int DeleteAlternativeItem(int ID);
        int DeleteSecondaryUnitItem(int ID);
        int DeleteItemTax(int ID);
    }
}