using DataAccessLayer;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace BusinessLayer
{
    public class ItemBL : ItemContract
    {
        ItemDAL itemDAL;
        public ItemBL()
        {
            itemDAL = new ItemDAL();
        }

        public List<ItemBO> GetPackingItemsForAutoComplete(int PackingSequence, string Hint)
        {
            return itemDAL.GetPackingItemsForAutoComplete(PackingSequence, Hint);
        }
        public List<ItemBO> GetReceiptItemsForAutoComplete(int ProductGroupID, string Hint)
        {
            return itemDAL.GetReceiptItemsForAutoComplete(ProductGroupID, Hint);
        }

        public List<ItemBO> GetSaleableItemsForAutoComplete(string Hint, int ItemCategoryID = 0, int SalesCategoryID = 0, int PriceListID = 0)
        {
            return itemDAL.GetSaleableItemsForAutoComplete(Hint, ItemCategoryID, SalesCategoryID, PriceListID);
        }

        public DatatableResultBO GetSaleableItemsList(int ItemCategoryID, int SalesCategoryID, int BusinessCategoryID, int PriceListID, int StoreID, bool CheckStock, int BatchTypeID, string FullOrLoose, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string PartsNoHint, string ModelHint, string RemarkHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetSaleableItemsList(ItemCategoryID, SalesCategoryID, BusinessCategoryID, PriceListID, StoreID, CheckStock, BatchTypeID, FullOrLoose, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, PartsNoHint, ModelHint, RemarkHint, SortField, SortOrder, Offset, Limit);
        }

        // Items for purchase of service
        public List<ItemBO> GetServiceItems(string Hint, int ItemCategoryID = 0, int PurchaseCategoryID = 0)
        {
            return itemDAL.GetServiceItems(Hint, ItemCategoryID, PurchaseCategoryID);
        }

        public DatatableResultBO GetItemsListForReport(string StockType, int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForReport(StockType, ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetStockableItemsList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string PartsNumberHHit, string ModelHHit, string MakeHHit, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetStockableItemsList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, PartsNumberHHit, ModelHHit, MakeHHit, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetProductionDefinitionMaterialList(string Type, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetProductionDefinitionMaterialList(Type, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public List<ItemBO> GetItemsListByPurchaseRequisitionIDS(int[] PurchaseRequisitionIDS)
        {
            List<ItemBO> items = new List<ItemBO>();
            if (PurchaseRequisitionIDS != null)
            {
                foreach (var PurchaseRequisitionID in PurchaseRequisitionIDS)
                {
                    items.AddRange(itemDAL.GetItemsListByPurchaseRequisition(PurchaseRequisitionID));
                }

            }
            return items;
        }
        public List<ItemBO> GetItemsListByPurchaseRequisition(int PurchaseRequisitionID)
        {
            return itemDAL.GetItemsListByPurchaseRequisition(PurchaseRequisitionID);

        }
        public DatatableResultBO GetItemsListForPurchase(string Type, int ItemCategoryID, int PurchaseCategoryID, int BusinessCategoryID, int SupplierID, string CodeHint, string NameHint, string PartsNumberHit ,string ModelHint,string RemarksHint ,string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForPurchase(Type, ItemCategoryID, PurchaseCategoryID, BusinessCategoryID, SupplierID, CodeHint, NameHint, PartsNumberHit, ModelHint, RemarksHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetItemsListForMaterialPurification(int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForMaterialPurification(ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetReturnItemList(int ProductionGroupID, int IssueItemID, int ReceiptItemID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetReturnItemList(ProductionGroupID, IssueItemID, ReceiptItemID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetAvailableStockItemsList(int ItemCategoryID, int WarehouseID, int BatchTypeID, string CodeHint, string NameHint, string ItemCategoryHint, string PartsNoHint, string MakeHint, string ModelHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetAvailableStockItemsList(ItemCategoryID, WarehouseID, BatchTypeID, CodeHint, NameHint, ItemCategoryHint, PartsNoHint, MakeHint, ModelHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetDebitAndCreditItemsList(string NoteType, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetDebitAndCreditItemsList(NoteType, CodeHint, NameHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ItemBO> GetStockableItemsForAutoComplete(string Hint, int ItemCategoryID = 0)
        {
            return itemDAL.GetStockableItemsForAutoComplete(Hint, ItemCategoryID);
        }

        public List<ItemBO> GetPackingReturnForAutoComplete(string Hint, int ProductGroupID = 0, int ReceiptItemID = 0, int IssueItemID = 0)
        {
            return itemDAL.GetPackingReturnForAutoComplete(Hint, ProductGroupID, ReceiptItemID, IssueItemID);
        }
        public List<ItemBO> GetAvailableStocktemsForAutoComplete(string Hint, int ItemCategoryID = 0, int WareHouseID = 0)
        {
            return itemDAL.GetAvailableStocktemsForAutoComplete(Hint, ItemCategoryID, WareHouseID);
        }
        public List<ItemBO> GetAllItemsForAutoComplete(string Hint, int ItemCategoryID = 0, int SalesCategoryID = 0)
        {
            return itemDAL.GetAllItemsForAutoComplete(Hint, ItemCategoryID, SalesCategoryID);
        }

        public List<ItemBO> GetReciptItemsItemList(int ProductionGroupID)
        {
            return itemDAL.GetReciptItemsItemList(ProductionGroupID);
        }

        public DatatableResultBO GetGRNWiseItemsList(int SupplierID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetGRNWiseItemsList(SupplierID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ItemBO> GetGRNWiseItemsForAutoComplete(string Hint, int SupplierID)
        {
            return itemDAL.GetGRNWiseItemsForAutoComplete(Hint, SupplierID);
        }

        public DatatableResultBO GetItemsForSchemeItem(int CategoryID)
        {
            return itemDAL.GetItemsForSchemeItem(CategoryID);
        }
        public DatatableResultBO GetProductionGroupList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetProductionGroupList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetProductionGroupItemAutoCompleteForReport(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetProductionGroupItemAutoCompleteForReport(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetItemsListForSaleableServiceItem(int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForSaleableServiceItem(ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit);
        }


        public DatatableResultBO GetItemsListForSaleableServiceAndStockItem(int SalesCategoryID, int SalesIncentiveCategoryID, int BusinessCategoryID, string CodeHint, string NameHint, string SalesCategoryHint, string SalesIncentiveCategoryHint, string BusinessCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForSaleableServiceAndStockItem(SalesCategoryID, SalesIncentiveCategoryID, BusinessCategoryID, CodeHint, NameHint, SalesCategoryHint, SalesIncentiveCategoryHint, BusinessCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public DatatableResultBO GetItemsListForStockAdjustmentForAlopathy(DateTime FromDate, DateTime ToDate, string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForStockAdjustmentForAlopathy(FromDate, ToDate, CodeHint, NameHint, SortField, SortOrder, Offset, Limit);

        }


        public DatatableResultBO GetRawMaterialList(int WarehouseID, string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetRawMaterialList(WarehouseID, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetPackingMaterialList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetPackingMaterialList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetProductionIssueMaterialReturnList(int ProductionID, int ProductionSequence, string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetProductionIssueMaterialReturnList(ProductionID, ProductionSequence, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetPackingItemList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetPackingItemList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ItemBO> GetProductionGroupItemsForAutoComplete(string Hint)
        {
            return itemDAL.GetProductionGroupItemsForAutoComplete(Hint);
        }

        public DatatableResultBO GetPreProcessIssueItemsList(string CodeHint, string NameHint, string ItemCategoryHint, string ActivityHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetPreProcessIssueItemsList(CodeHint, NameHint, ItemCategoryHint, ActivityHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ItemBO> GetItemTypeList()
        {
            return itemDAL.GetItemTypeList();
        }
        public List<ItemBO> GetBrandList()
        {
            return itemDAL.GetBrandList();
        }
        
        public List<ItemBO> GetDrugScheduleTypeList()
        {
            return itemDAL.GetDrugScheduleTypeList();
        }
        public List<ItemBO> GetMasterFormulaList()
        {
            return itemDAL.GetMasterFormulaList();
        }
        public int CreateItem(ItemBO itemBO, List<ItemBO> ItemLocationList)
        {
            if (itemBO.ID == 0)
            {
                return itemDAL.CreateItem(itemBO, ItemLocationList);
            }
            else
            {
                return itemDAL.UpdateItem(itemBO, ItemLocationList);
            }
        }
        public int CreateItemV3(ItemBO itemBO, List<ItemBO> ItemLocationList)
        {
            if (itemBO.ID == 0)
            {
                return itemDAL.CreateItemV3(itemBO, ItemLocationList);
            }
            else
            {
                return itemDAL.UpdateItemV3(itemBO, ItemLocationList);
            }
        }
        public DatatableResultBO GetAllItemList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetAllItemList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetItemList(string Code, string Description, string Category, string PartsNo, string PartsClass, string partsGroup, string remark, string model, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemList(Code, Description, Category, PartsNo, PartsClass, partsGroup, remark, model, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetAllItemListV3(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetAllItemListV3(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetAlternativeItemList(string Code, string Name, string ItemCategory, string ItemUnit, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetAlternativeItemList(Code, Name, ItemCategory, ItemUnit, SortField, SortOrder, Offset, Limit);
        }
        public ItemBO GetItemDetails(int ItemID)
        {
            return itemDAL.GetItemDetails(ItemID);
        }
        public ItemBO GetItemDetail(int ItemID)
        {
            return itemDAL.GetItemDetail(ItemID);
        }
        public ItemBO GetItemDetailsV3(int ItemID)
        {
            return itemDAL.GetItemDetailsV3(ItemID);
        }

        public List<ItemBO> GetItemsByPackingCodes(string[] PackingCodes)
        {
            string CommaSeparatedPackingCodes = string.Join(",", PackingCodes);
            return itemDAL.GetItemsByPackingCodes(CommaSeparatedPackingCodes);
        }

        public List<ItemBO> GetItemsByItemCodes(string[] ItemCodes)
        {
            string CommaSeparatedItemCodes = string.Join(",", ItemCodes);
            return itemDAL.GetItemsByItemCodes(CommaSeparatedItemCodes);
        }

        public List<ItemRateBO> GetRateOfItems(int[] ItemIDs, int PriceListID)
        {
            string CommaSeparatedItemIDs = string.Join(",", ItemIDs);
            return itemDAL.GetRateOfItems(CommaSeparatedItemIDs, PriceListID);
        }

        public decimal GetAverageSales(int ItemID)
        {
            return itemDAL.GetAverageSales(ItemID);
        }

        public decimal GetAvailableStock(int ItemID, int? BatchID, int? BatchTypeID, int WarehouseID, int LocationID)
        {
            return itemDAL.GetAvailableStock(ItemID, BatchID, BatchTypeID, WarehouseID, LocationID);
        }


        public string IsExist(string Name, string HSNCode, int ID)
        {
            return itemDAL.IsExist(Name, HSNCode, ID);
        }

        public string getItemCode(string QRCode)
        {
            return itemDAL.GetItemCode(QRCode);
        }

        public DatatableResultBO GetItemsForOfficialAdvance(string CodeHint, string NameHint, string Type, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsForOfficialAdvance(CodeHint, NameHint, Type, SortField, SortOrder, Offset, Limit);
        }

        public List<ItemBO> GetItemForChequeStatus()
        {
            return itemDAL.GetItemForChequeStatus();
        }

        public List<ItemBO> GetItemTransactionDetails(int ItemID, int BatchTypeID)
        {
            return itemDAL.GetItemTransactionDetails(ItemID, BatchTypeID);
        }
        public DatatableResultBO GetItemsList(string Type, int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsList(Type, ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
        }
        public DatatableResultBO GetItemsListForStockAdjustment(int ItemCategoryID, int SalesCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemsListForStockAdjustment(ItemCategoryID, SalesCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit);

        }

        public List<ItemBO> GetPreProcessReceiptItemForAutoComplete(string Hint)
        {
            return itemDAL.GetPreProcessReceiptItemForAutoComplete(Hint);
        }

        public DatatableResultBO GetItemListForTreatment(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetItemListForTreatment(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
        }
        public ItemBO GetAllCategoryID()
        {
            return itemDAL.GetAllCategoryID();
        }
        public ItemBO GetServiceItemCategory()
        {
            return itemDAL.GetServiceItemCategory();
        }
        public DatatableResultBO GetServiceItemList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            return itemDAL.GetServiceItemList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit);
        }

        public List<ItemDescriptionBO> GetDescription(int ItemID, string Type)
        {
            return itemDAL.GetDescription(ItemID, Type);
        }

        public List<ItemBO> GetItemListForAPI(int Offset, int Limit)
        {
            return itemDAL.GetItemListForAPI(Offset, Limit);
        }

        public List<StockBO> GetStockListForAPI(int ItemID)
        {
            return itemDAL.GetStockListForAPI(ItemID);
        }

        public string GetItemCode(string QRCode)
        {
            return itemDAL.GetItemCode(QRCode);
        }
        public string GetBatchNo(string QRCode)
        {
            return itemDAL.GetBatchNo(QRCode);
        }
        public int DeleteItemWareHouse(int ID)
        {
            return itemDAL.DeleteItemWareHouse(ID);
        }
        public int DeleteItemParts(int ItemID, int ID)
        {
            return itemDAL.DeleteItemParts(ItemID, ID);
        }
        public int DeleteAlternativeItem(int ID)
        {
            return itemDAL.DeleteAlternativeItem(ID);
        }
        public int DeleteSecondaryUnitItem(int ID)
        {
            return itemDAL.DeleteSecondaryUnitItem(ID);
        }
        public int DeleteItemTax(int ID)
        {
            return itemDAL.DeleteItemTax(ID);
        }
    }
}
