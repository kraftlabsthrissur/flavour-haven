using AutoMapper;
using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ItemDAL
    {
        public List<ItemBO> GetServiceItems(string Hint, int ItemCategoryID = 0, int PurchaseCategoryID = 0)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpItemByServiceCategory(ItemCategoryID, PurchaseCategoryID, Hint)
                        .Select(a => new ItemBO
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Code = a.Code,
                            Unit = a.Unit,
                            UnitID = a.UnitID,
                            GSTPercentage = a.GSTPercentage,
                            TravelCategoryID = a.TravelCategoryID

                        })
                        .ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetProductionGroupItemsForAutoComplete(string Hint)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpPProductionGroupItemAutoComplete(Hint, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = (int)a.ItemID,
                        Name = a.ItemName,
                        Code = a.Code,
                        Unit = a.Unit,
                        IsKalkan = (bool)a.IsKalkan
                        //UnitID = a.UnitID,

                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetPackingItemsForAutoComplete(int PackingSequence, string Hint)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpPGetPackingItemAutoComplete(PackingSequence, Hint).Select(a => new ItemBO
                    {
                        ID = (int)a.ItemID,
                        Name = a.ItemName,
                        Code = a.Code,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        ProductGroupID = (int)a.ProductGroupID,
                        ProductID = (int)a.ProductID,
                    }).ToList();
                    return item;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetReceiptItemsForAutoComplete(int ProductGroupID, string Hint)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetReceiptItemAutoComplete(ProductGroupID, Hint).Select(a => new ItemBO
                    {
                        Name = a.ItemName,
                        ItemID = (int)a.ItemID,
                        BatchSize = (decimal)a.BatchSize,
                        Code = a.ProductCode,
                        ConversionFactorPtoS = (decimal)a.ConversionFactorPtoS
                    }).ToList();
                    return item;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<ItemBO> GetSaleableItemsForAutoComplete(string Hint, int ItemCategoryID = 0, int SalesCategoryID = 0, int PriceListID = 0)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpItemByItemANDSalesCategories(ItemCategoryID, SalesCategoryID, PriceListID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        Stock = a.Stock,
                        CGSTPercentage = (decimal)a.CGSTPercent,
                        IGSTPercentage = (decimal)a.IGSTPercent,
                        SGSTPercentage = (decimal)a.SGSTPercent,
                        Rate = (decimal)a.Rate
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetStockableItemsForAutoComplete(string Hint, int ItemCategoryID = 0)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetStockableItemAutoComplete(ItemCategoryID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name.TrimStart(),
                        Stock = a.Stock,
                        Unit = a.PrimaryUnit,
                        UnitID = a.PrimaryUnitID,
                        ItemCategory = a.Category,
                        InventoryUnit = a.InventoryUnit,
                        InventoryUnitID = (int)a.InventoryUnitID,
                        Code = a.Code,
                        SalesCategoryName = a.SalesCategory
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetPackingReturnForAutoComplete(string Hint, int ProductGroupID = 0, int ReceiptItemID = 0, int IssueItemID = 0)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetRepackingReturnItemsAutoComplete(ProductGroupID, IssueItemID, ReceiptItemID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Stock = a.Stock,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        ItemCategory = a.Category
                    }).ToList();
                    return item;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetAvailableStocktemsForAutoComplete(string Hint, int ItemCategoryID = 0, int WarehouseID = 0)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetAvailableStockItemAutoComplete(ItemCategoryID, WarehouseID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Stock = a.Stock,
                        Unit = a.Unit,
                        UnitID = a.UnitID,
                        ItemCategory = a.Category
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetAllItemsForAutoComplete(string Hint, int ItemCategoryID = 0, int SalesCategoryID = 0)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetAllItemAutoComplete(ItemCategoryID, SalesCategoryID, Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        Unit = a.Unit,
                        Type = a.Type,
                        SalesCategoryID = (int)a.SalesCategoryID,
                        SalesCategoryName = a.SalesCategory,
                        CategoryName = a.Category
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ItemBO> GetReciptItemsItemList(int ProductionGroupID)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetReceiptItemList(ProductionGroupID).Select(a => new ItemBO
                    {
                        Name = a.ItemName,
                        ItemID = (int)a.ItemID,
                        BatchSize = (decimal)a.BatchSize,
                        Code = a.ProductCode,
                        ConversionFactorPtoS = (decimal)a.ConversionFactorPtoS
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetSaleableItemsList(int ItemCategoryID, int SalesCategoryID, int BusinessCategoryID, int PriceListID, int StoreID, bool CheckStock, int BatchTypeID, string FullOrLoose, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string PartsNoHint, string ModelHint, string RemarkHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    ItemCategoryID = 0;
                    var result = dbEntity.SpGetSaleableItemList(ItemCategoryID, SalesCategoryID, PriceListID, StoreID, BatchTypeID, FullOrLoose, CheckStock,
                        CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, PartsNoHint, ModelHint, RemarkHint,
                        SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count(); i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                item.ID,
                                item.Code,
                                item.Name,
                                PartsNumber = string.IsNullOrEmpty(item.PartsNumber) ? "" : item.PartsNumber,
                                Remarks = item.Remark,
                                item.Model,
                                item.Make,
                                Unit = item.PrimaryUnit,
                                UnitID = item.PrimaryUnitID,
                                item.SecondaryUnits,
                                ItemCategory = item.Category,
                                item.SalesCategory,
                                Stock = ((decimal)item.Stock).ToString("N2"),
                                PendingOrderQty = item.PendingOrderQty,
                                CGSTPercentage = item.CGSTPercent,
                                IGSTPercentage = item.IGSTPercent,
                                SGSTPercentage = item.SGSTPercent,
                                VATPercentage = item.VATPercentage,
                                item.TaxTypeID,
                                item.TaxType,
                                item.CessPercentage,
                                Rate = (decimal)item.Rate,
                                SalesUnit = item.Unit,
                                SalesUnitID = item.UnitID,
                                item.LooseRate,
                                item.SalesCategoryID,
                                item.MaxSalesQtyFull,
                                item.MinSalesQtyFull,
                                item.MinSalesQtyLoose,
                                item.MaxSalesQtyLoose,
                                ItemStoreLocations = !string.IsNullOrEmpty(item.ItemStoreLocations) ? item.ItemStoreLocations.Trim(';') : ""
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetItemsListForReport(string StockType, int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemListForReport(StockType, ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                UnitID = item.UnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                Stock = item.Stock
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetStockableItemsList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string PartsNumberHHit, string ModelHHit, string MakeHHit, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetStockableItemList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, PartsNumberHHit, ModelHHit, MakeHHit, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                PrimaryUnit = item.PrimaryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                SecondaryUnits = item.SecondaryUnits,
                                PartsNumber = item.PartsNumber,
                                Model = item.Model,
                                Make = item.Make,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = item.CGSTPercent.HasValue ? item.CGSTPercent.Value : 0,
                                IGSTPercentage = item.IGSTPercent.HasValue ? item.IGSTPercent.Value : 0,
                                SGSTPercentage = item.SGSTPercent.HasValue ? item.SGSTPercent.Value : 0,
                                stock = item.Stock,
                                InventoryUnit = item.InventoryUnit,
                                InventoryUnitID = item.InventoryUnitID,

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetProductionDefinitionMaterialList(string Type, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetProductionDefinitionMaterialList(Type, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                PrimaryUnit = item.PrimaryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                stock = item.Stock,
                                InventoryUnit = item.InventoryUnit,
                                InventoryUnitID = item.InventoryUnitID,

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public List<ItemBO> GetItemsListByPurchaseRequisition(int PurchaseRequisitionID)
        {
            List<ItemBO> itemList = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var data = dbEntity.SpGetItemListForPurchaseRequisition(PurchaseRequisitionID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    itemList = data.Select(item => new ItemBO
                    {

                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        PartsNumber = string.IsNullOrEmpty(item.PartsNumber) ? "" : item.PartsNumber.Replace("NULL", ""),
                        Qty = item.Quantity.HasValue ? item.Quantity.Value : 0,
                        SecondaryUnits = item.SecondaryUnits,
                        Unit = item.PurchaseUnit,
                        UnitID = item.PurchaseUnitID.HasValue ? item.PurchaseUnitID.Value : 0,
                        Remark = item.Remark,
                        Model = item.Model,
                        IsVat = 1,
                        IsGST = 0,
                        IGSTPercent = 0,
                        CGSTPercent = 0,
                        SGSTPercent = 0,
                        VATPercentage = 0,
                        CessPercentage = 0,
                        GSTPercentage = 0,
                        GSTAmount = 0,
                        PurchaseRequisitionID = PurchaseRequisitionID,
                        PurchaseRequisitionTrasID = item.PurchaseRequisitionTrasID,
                        MRP = item.PurchaseMRP.HasValue ? item.PurchaseMRP.Value : 0,
                        Rate = item.PurchaseMRP.HasValue ? item.PurchaseMRP.Value : 0,
                        QtyUnderQC = item.QtyUnderQC,
                        QtyOrdered = item.QtyOrdered,
                        PendingOrderQty = item.QtyOrdered,//PendingOrderQty
                        QtyAvailable = item.QtyAvailable,
                        Stock = item.Stock,
                        ItemCategory = item.Category,
                        PurchaseCategory = item.PurchaseCategory,
                        LastPR = item.LastPR.HasValue ? item.LastPR.Value : 0,
                        LowestPR = item.LowestPR.HasValue ? item.LowestPR.Value : 0,
                        ExchangeRate = 1,
                        // Value: clean($("#txtValue").val()),
                        CountryID = item.CountryID.HasValue ? item.CountryID.Value : 0,
                        CurrencyID = item.CurrencyID.HasValue ? item.CurrencyID.Value : 0,
                        RetailMRP = item.RetailMRP.HasValue ? item.RetailMRP.Value : 0,
                        RetailLooseRate = item.RetailLooseRate.HasValue ? item.RetailLooseRate.Value : 0,
                        ItemCategoryID = item.CategoryID,
                        TravelCategoryID = item.TravelCategoryID,
                        FGCategoryID = item.FGCategoryID,
                        PurchaseCategoryID = item.PurchaseCategoryID
                    }).ToList();
                    return itemList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DatatableResultBO GetItemsListForPurchase(string Type, int ItemCategoryID, int PurchaseCategoryID, int BusinessCategoryID, int SupplierID, string CodeHint, string NameHint, string PartsNumberHit, string PurchaseCategoryHint, string ItemCategoryHint,string UnitHint , string ModelHint, string RemarksHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemListForPurchase(Type, ItemCategoryID, PurchaseCategoryID, BusinessCategoryID, SupplierID, CodeHint, NameHint, PartsNumberHit, ModelHint, RemarksHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                BatchNo = item.BatchNo,
                                ExpiryDate = item.ExpiryDate,
                                PartsNumber = string.IsNullOrEmpty(item.PartsNumber) ? "" : item.PartsNumber.Replace("NULL", ""),
                               // PartsNumber = item.PartsNumber,
                               
                                SecondaryUnits = item.SecondaryUnits,
                                Remark = item.Remark,
                                Model = item.Model,
                                PrimaryUnit = item.PrimaryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                QtyUnderQC = item.QtyUnderQC,
                                QtyOrdered = item.QtyOrdered,
                                Stock = item.Stock,
                                ItemCategory = item.Category,
                                PurchaseCategory = item.PurchaseCategory,
                                LastPr = item.LastPR,
                                LowestPr = item.LowestPR,
                                PurchaseMRP = item.PurchaseMRP.HasValue ? item.PurchaseMRP : 0,
                                PurchaseLooseRate = item.PurchaseLooseRate.HasValue ? item.PurchaseLooseRate : 0,
                                PendingOrderQty = item.PendingOrderQty,
                                QtyAvailable = item.QtyAvailable,
                                GstPercentage = item.GSTPercentage.HasValue ? item.GSTPercentage.Value : 0,
                                VATPercentage = item.VATPercentage.HasValue ? item.VATPercentage.Value : 0,
                                RetailMRP = item.RetailMRP.HasValue ? item.RetailMRP : 0,
                                RetailLooseRate = item.RetailLooseRate.HasValue ? item.RetailLooseRate : 0,
                                ItemCategoryID = item.CategoryID,
                                TravelCategoryID = item.TravelCategoryID,
                                FinishedGoodsCategoryID = item.FGCategoryID,
                                PurchaseUnit = item.PurchaseUnit,
                                PurchaseUnitID = item.PurchaseUnitID,
                                PurchaseCategoryID = item.PurchaseCategoryID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetItemsListForMaterialPurification(int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemListForMaterialPurification(ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                PrimaryUnit = item.PrimaryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                QtyUnderQC = item.QtyUnderQC,
                                QtyOrdered = item.QtyOrdered,
                                Stock = item.Stock,
                                ItemCategory = item.Category,
                                PurchaseCategory = item.PurchaseCategory,
                                LastPr = item.LastPR,
                                LowestPr = item.LowestPR,
                                PendingOrderQty = item.PendingOrderQty,
                                QtyAvailable = item.QtyAvailable,
                                GstPercentage = (decimal)item.GSTPercentage,
                                QtyWithQc = item.QtyUnderQC,
                                ItemCategoryID = item.CategoryID,
                                TravelCategoryID = item.TravelCategoryID,
                                FinishedGoodsCategoryID = item.FGCategoryID,
                                PurchaseUnit = item.PurchaseUnit,
                                PurchaseUnitID = item.PurchaseUnitID,
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetItemsListForSaleableServiceItem(int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemsListForSaleableServiceItem(ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                PrimaryUnit = item.PrimaryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                GstPercentage = (decimal)item.GSTPercentage,
                                ItemCategoryID = item.CategoryID,
                                SalesUnit = item.SalesUnit,
                                SalesUnitID = item.SalesUnitID,
                                GSTPercentage = item.GSTPercentage,
                                CessPercentage = item.CessPercentage,
                                MRP = item.MRP,
                                InventoryUnitID = item.InventoryUnitID,
                                InventoryUnit = item.InventoryUnit
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetItemsListForSaleableServiceAndStockItem(int SalesCategoryID, int SalesIncentiveCategoryID, int BusinessCategoryID, string CodeHint, string NameHint, string SalesCategoryHint, string SalesIncentiveCategoryHint, string BusinessCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemsListForSaleableServiceAndStockItem(SalesCategoryID, SalesIncentiveCategoryID, BusinessCategoryID, CodeHint, NameHint, SalesCategoryHint, SalesIncentiveCategoryHint, BusinessCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                SalesCategory = item.SalesCategory,
                                SalesCategoryID = (int)item.SalesCategoryID,
                                BusinessCategoryID = (int)item.BusinessCategoryID,
                                BusinessCategory = item.BusinessCategory,
                                SalesIncentiveCategoryID = (int)item.SalesIncentiveCategoryID,
                                SalesIncentiveCategory = item.SalesIncentiveCategory
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public DatatableResultBO GetItemsListForStockAdjustmentForAlopathy(DateTime FromDate, DateTime ToDate, string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemListForStockAdjustmentAllopathy(FromDate, ToDate, CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetReturnItemList(int ProductionGroupID, int IssueItemID, int ReceiptItemID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetRepackingReturnItems(ProductionGroupID, IssueItemID, ReceiptItemID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                UnitID = item.UnitID,
                                ItemCategory = item.Category,
                                stock = item.Stock
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetAvailableStockItemsList(int ItemCategoryID, int WarehouseID, int BatchTypeID, string CodeHint, string NameHint, string ItemCategoryHint, string PartsNoHint, string MakeHint, string ModelHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAvailableStockItemList(ItemCategoryID, WarehouseID, BatchTypeID, CodeHint, NameHint, ItemCategoryHint, PartsNoHint, MakeHint, ModelHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                PartsNo = item.PartsNo,
                                Make = item.Make,
                                Model = item.Model,
                                PrimaryUnit = item.PrimaryUnit,
                                SecondaryUnits = item.SecondaryUnits,
                                PrimaryUnitID = item.PrimaryUnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                Stock = item.Stock,
                                Rate = item.Rate,
                                InventoryUnit = item.InventoryUnit,
                                InventoryUnitID = (int)item.InventoryUnitID,
                                BatchTypeName = item.BatchTypeName.ToLower(),
                                BatchTypeID = item.BatchTypeID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetDebitAndCreditItemsList(string NoteType, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string PurchaseCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetDebitAndCreditNoteItemList(NoteType, CodeHint, NameHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                PurchaseCategory = item.PurchaseCategory,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                Type = item.Type

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetGRNWiseItemsList(int SupplierID, string CodeHint, string NameHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetGRNWiseItemsList(SupplierID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                UnitID = item.UnitID,
                                ItemCategory = item.Category,
                                Stock = item.Stock
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<ItemBO> GetGRNWiseItemsForAutoComplete(string Hint, int SupplierID)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetGRNWiseItemsForAutoComplete(Hint, SupplierID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ItemID = a.ID,
                        Name = a.Name,
                        Stock = (decimal)a.Stock,
                        UnitName = a.Unit,
                        UnitID = (int)a.UnitID,
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DatatableResultBO GetItemsForSchemeItem(int CategoryID)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemsForSchemeItem(CategoryID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                CategoryID = item.CategoryID,
                                Category = item.Category

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetPackingItemList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpPGetPackingItemList(1, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                ProductID = item.ProductID,
                                ProductionGroupID = item.ProductGroupID,
                                Unit = item.Unit,
                                Category = item.Category,
                                BatchSize = item.BatchSize,
                                UnitID = item.UnitID,
                                ConversionFactorPtoS = item.ConversionFactorPtoS
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetProductionGroupList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetProductionGroupList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                Category = item.Category,
                                ItemID = item.ItemID,
                                StdBatchSize = item.StandardBatchSize,
                                IsKalkan = item.IsKalkan
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetProductionGroupItemAutoCompleteForReport(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetProductionGroupListForReport(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                Category = item.Category,
                                ItemID = item.ItemID,
                                StdBatchSize = item.StandardBatchSize,
                                IsKalkan = item.IsKalkan
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetRawMaterialList(int WarehouseID, string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetRawMaterialList(WarehouseID, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Code = item.Code,
                                Unit = item.Unit,
                                Category = item.Category,
                                Stock = item.Stock,
                                UnitID = item.UnitID,
                                BatchTypeName = item.BatchType.ToLower(),
                                BatchTypeID = item.BatchTypeID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetPackingMaterialList(string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetPackingMaterialList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Code = item.Code,
                                Unit = item.Unit,
                                ItemCategory = item.Category,
                                Stock = item.Stock,
                                UnitID = item.UnitID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetProductionIssueMaterialReturnList(int ProductionID, int ProductionSequence, string CodeHint, string NameHint, string CategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetProductionIssueMaterialList(ProductionID, ProductionSequence, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ItemID,
                                Name = item.ItemName,
                                Code = item.Code,
                                Unit = item.Unit,
                                Category = item.Category,
                                Stock = item.Stock,
                                UnitID = item.UnitID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetPreProcessIssueItemsList(string CodeHint, string NameHint, string ItemCategoryHint, string ActivityHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetPreProcessIssueItemsList(CodeHint, NameHint, ItemCategoryHint, ActivityHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Code = item.Code,
                                Unit = item.Unit,
                                ItemCategory = item.Category,
                                Stock = item.Stock,
                                Activity = item.Activity,
                                ProcessID = item.ProcessID,
                                UnitID = item.UnitID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public int CreateItem(ItemBO itemBO, List<ItemBO> ItemLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    int ID;
                    //ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    //var j = dbEntity.SpUpdateSerialNo(itemBO.ItemTypeName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ItemBO, Item>();
                    });
                    IMapper iMapper = config.CreateMapper();
                    var destination = iMapper.Map<ItemBO, Item>(itemBO);
                    destination.CreatedDate = DateTime.Now;
                    destination.CreatedUserID = GeneralBO.CreatedUserID;
                    destination.BirthDate = DateTime.Now;
                    destination.IsSaleable = destination.IsStockitem;
                    destination.IsDisContinued = !destination.IsActive;
                    destination.GSTSubCategoryID = 1;
                    if (destination.IsDisContinued.HasValue && destination.IsDisContinued.Value)
                        destination.DisContinuedDate = DateTime.Now;
                    else
                        destination.DisContinuedDate = DateTime.Now.AddYears(50);
                    destination.MaxSalesQty = 10;
                    destination.MinSalesQtyLoose = 1;
                    destination.MinSalesQtyFull = 1;
                    destination.ConversionFactorPtoI = itemBO.ConversionFactorPurchaseToInventory;
                    destination.ConversionFactorPtoS = itemBO.ConversionFactorPurchaseToLoose;
                    if (itemBO.ItemPartsNumberBOList != null && itemBO.ItemPartsNumberBOList.Count > 0)
                        destination.Description = string.Join(", ", itemBO.ItemPartsNumberBOList.Select(x => x.PartsNumber)).Trim(' ').Trim(',');
                    dbEntity.Items.Add(destination);
                    dbEntity.SaveChanges();
                    dbEntity.SpCreateStockOrServiceType(itemBO.CategoryID, itemBO.IsStockValue);
                    //dbEntity.SpCreateBatch()
                    //dbEntity.SpUpdateItemBatch(itemBO.ItemID, itemBO.CostPrice, itemBO.PurchasePrice, itemBO.SalePrice, itemBO.LoosePrice, itemBO.LandedCost);
                    if (destination.ID != 0)
                    {
                        foreach (var items in ItemLocationList)
                        {
                            dbEntity.SpCreateItemLocation(destination.ID, itemBO.Name, items.LocationID);
                        }
                        if (itemBO.ItemTaxBOList != null)
                        {
                            foreach (var itemTax in itemBO.ItemTaxBOList)
                            {
                                dbEntity.SpCreateItemTax(destination.ID, itemTax.LocationID, itemTax.TaxTypeID, itemTax.GSTCategoryID, GeneralBO.CreatedUserID);
                            }
                        }
                        if (itemBO.ItemWareHouseBOList != null)
                        {
                            foreach (var itemWarehouse in itemBO.ItemWareHouseBOList)
                            {
                                int Default = itemWarehouse.IsDefault ? 1 : 0;
                                dbEntity.SpCreateItemWareHouse(destination.ID, itemWarehouse.WareHouseID, itemWarehouse.BinID, itemWarehouse.LotID, Default, GeneralBO.CreatedUserID);
                            }
                        }
                        if (itemBO.AlternativeItemBOList != null)
                        {
                            foreach (var alternativeItem in itemBO.AlternativeItemBOList)
                            {
                                dbEntity.SpCreateAlternativeItem(destination.ID, alternativeItem.AlternativeItemID, GeneralBO.CreatedUserID);
                            }
                        }
                        if (itemBO.ItemPartsNumberBOList != null)
                        {
                            foreach (var PartsNumber in itemBO.ItemPartsNumberBOList)
                            {
                                dbEntity.SpCreateItemPartsNumber(destination.ID, PartsNumber.PartsNumber, GeneralBO.CreatedUserID, PartsNumber.IsDefault);
                            }
                        }
                        if (itemBO.ItemSecondaryUnitList != null)
                        {
                            foreach (var itemSecondaryUnit in itemBO.ItemSecondaryUnitList)
                            {
                                dbEntity.SpCreateItemSecondaryUnit(destination.ID, itemSecondaryUnit.SecondaryUnitID, GeneralBO.CreatedUserID);
                            }
                        }
                        if (itemBO.SalesInquiryItemID > 0 || itemBO.PurchaseRequisitionTrasID > 0)
                        {
                            dbEntity.SpSetItemIdUsingPopUP(destination.ID, itemBO.SalesInquiryItemID, itemBO.PurchaseRequisitionTrasID, GeneralBO.CreatedUserID);
                        }
                        if (itemBO.ItemSalesPriceBOList != null)
                        {
                            foreach (var itemSalesPrice in itemBO.ItemSalesPriceBOList)
                            {
                                dbEntity.SpCreateUpdateItemPriceDetails(destination.ID, itemSalesPrice.LocationID, GeneralBO.ApplicationID, itemSalesPrice.SalesPrice, itemSalesPrice.LoosePrice);
                                dbEntity.SpCreateUpdateItemBatch(destination.ID, destination.Code, itemBO.CostPrice, itemSalesPrice.SalesPrice, itemSalesPrice.SalesPrice, itemSalesPrice.SalesPrice, itemSalesPrice.LoosePrice, itemBO.LandedCost, itemSalesPrice.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID);
                            }
                        }
                    }
                    return destination.ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public int CreateItemV3(ItemBO itemBO, List<ItemBO> ItemLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    int ID;
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    var j = dbEntity.SpUpdateSerialNo(itemBO.ItemTypeName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ItemBO, Item>();
                    });
                    IMapper iMapper = config.CreateMapper();
                    var destination = iMapper.Map<ItemBO, Item>(itemBO);
                    dbEntity.Items.Add(destination);
                    dbEntity.SaveChanges();
                    dbEntity.SpCreateStockOrServiceType(itemBO.CategoryID, itemBO.IsStockValue);
                    if (destination.ID != 0)
                    {
                        foreach (var items in ItemLocationList)
                        {
                            dbEntity.SpCreateItemLocation(destination.ID, itemBO.Name, items.LocationID);
                        }

                        //dbEntity.SpCreateProductionGroupForItems(destination.ID, itemBO.Name, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    }

                    return destination.ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public int UpdateItem(ItemBO itemBO, List<ItemBO> ItemLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    var ItemPartsNumberExistingList = dbEntity.SPGetItemPartsNumberByItemID(itemBO.ID).Select(x => x.PartsNumber).ToList();
                    var j = dbEntity.SpUpdateSerialNo(itemBO.ItemTypeName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    var config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<ItemBO, Item>()
                            .ForMember(d => d.ID, o => o.Ignore())
                            .ForMember(d => d.Code, o => o.Ignore())
                            .ForMember(d => d.CreatedDate, o => o.Ignore())
                            .ForMember(d => d.CreatedUserID, o => o.Ignore())
                            .ForMember(d => d.BirthDate, o => o.Ignore());
                        });
                    IMapper iMapper = config.CreateMapper();
                    Item item = dbEntity.Items.Find(itemBO.ID);
                    string ExistingItemImagePath1 = item.ItemImagePath1;
                    string ExistingItemImagePath2 = item.ItemImagePath2;
                    string ExistingItemImagePath3 = item.ItemImagePath3;
                    string ExistingItemImagePath4 = item.ItemImagePath4;
                    string ExistingItemImagePath5 = item.ItemImagePath5;
                    string ExistingItemImagePath6 = item.ItemImagePath6;
                    string ExistingItemImagePath7 = item.ItemImagePath7;
                    string ExistingItemImagePath8 = item.ItemImagePath8;
                    iMapper.Map(itemBO, item);
                    item.IsSaleable = item.IsStockitem;
                    item.IsDisContinued = !item.IsActive;
                    if (item.IsDisContinued.HasValue && item.IsDisContinued.Value)
                        item.DisContinuedDate = DateTime.Now;
                    else
                        item.DisContinuedDate = DateTime.Now.AddYears(50);
                    if (!item.BirthDate.HasValue)
                        item.BirthDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(itemBO.Code))
                    {
                        item.Code = itemBO.Code;
                    }
                    if (itemBO.ItemPartsNumberBOList != null && itemBO.ItemPartsNumberBOList.Count > 0)
                    {
                        ItemPartsNumberExistingList.AddRange(itemBO.ItemPartsNumberBOList.Select(x => x.PartsNumber).ToList());
                    }
                    if (ItemPartsNumberExistingList != null)
                        item.Description = string.Join(", ", ItemPartsNumberExistingList).Trim(' ').Trim(',');
                    item.ConversionFactorPtoI = itemBO.ConversionFactorPurchaseToInventory;
                    item.ConversionFactorPtoS = itemBO.ConversionFactorPurchaseToLoose;
                    if (string.IsNullOrEmpty(item.ItemImagePath1))
                    {
                        item.ItemImagePath1 = ExistingItemImagePath1;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath2))
                    {
                        item.ItemImagePath2 = ExistingItemImagePath2;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath3))
                    {
                        item.ItemImagePath3 = ExistingItemImagePath3;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath4))
                    {
                        item.ItemImagePath4 = ExistingItemImagePath4;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath5))
                    {
                        item.ItemImagePath5 = ExistingItemImagePath5;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath6))
                    {
                        item.ItemImagePath6 = ExistingItemImagePath6;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath7))
                    {
                        item.ItemImagePath7 = ExistingItemImagePath7;
                    }
                    if (string.IsNullOrEmpty(item.ItemImagePath8))
                    {
                        item.ItemImagePath8 = ExistingItemImagePath8;
                    }
                    item.ModifiedDate = DateTime.Now;
                    dbEntity.SpLogChange("item", "ID", itemBO.ID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    dbEntity.SaveChanges();
                    dbEntity.SpCreateStockOrServiceType(itemBO.CategoryID, itemBO.IsStockValue);
                    dbEntity.SpDeleteItemLocation(itemBO.ID);

                    foreach (var items in ItemLocationList)
                    {
                        dbEntity.SpCreateItemLocation(itemBO.ID, itemBO.Name, items.LocationID);
                    }
                    if (itemBO.ItemTaxBOList != null)
                    {
                        foreach (var itemTax in itemBO.ItemTaxBOList)
                        {
                            dbEntity.SpCreateItemTax(itemBO.ID, itemTax.LocationID, itemTax.TaxTypeID, itemTax.GSTCategoryID, GeneralBO.CreatedUserID);
                        }
                    }
                    if (itemBO.ItemWareHouseBOList != null)
                    {
                        foreach (var itemWarehouse in itemBO.ItemWareHouseBOList)
                        {
                            int Default = itemWarehouse.IsDefault ? 1 : 0;
                            dbEntity.SpCreateItemWareHouse(itemBO.ID, itemWarehouse.WareHouseID, itemWarehouse.BinID, itemWarehouse.LotID, Default, GeneralBO.CreatedUserID);
                        }
                    }
                    if (itemBO.AlternativeItemBOList != null)
                    {
                        foreach (var alternativeItem in itemBO.AlternativeItemBOList)
                        {
                            dbEntity.SpCreateAlternativeItem(itemBO.ID, alternativeItem.ItemID, GeneralBO.CreatedUserID);
                        }
                    }
                    if (itemBO.UpdateItemPartsNumberBOList != null)
                    {
                        var UpdateItemPartsNumber = itemBO.UpdateItemPartsNumberBOList.Where(x => x.IsDefault).FirstOrDefault();
                        if (UpdateItemPartsNumber != null)
                        {
                            dbEntity.SpUpdateItemPartsNumber(UpdateItemPartsNumber.ID, itemBO.ID, GeneralBO.CreatedUserID);
                        }
                        else
                        {
                            dbEntity.SpUpdateItemPartsNumber(0, itemBO.ID, GeneralBO.CreatedUserID);
                        }

                    }
                    if (itemBO.ItemPartsNumberBOList != null)
                    {
                        foreach (var PartsNumber in itemBO.ItemPartsNumberBOList)
                        {
                            dbEntity.SpCreateItemPartsNumber(itemBO.ID, PartsNumber.PartsNumber, GeneralBO.CreatedUserID, PartsNumber.IsDefault);
                        }
                    }
                    if (itemBO.ItemSecondaryUnitList != null)
                    {
                        foreach (var itemSecondaryUnit in itemBO.ItemSecondaryUnitList)
                        {
                            dbEntity.SpCreateItemSecondaryUnit(itemBO.ID, itemSecondaryUnit.SecondaryUnitID, GeneralBO.CreatedUserID);
                        }
                    }
                    if (itemBO.ItemSalesPriceBOList != null)
                    {
                        foreach (var itemSalesPrice in itemBO.ItemSalesPriceBOList)
                        {
                            dbEntity.SpCreateUpdateItemPriceDetails(itemBO.ID, itemSalesPrice.LocationID, GeneralBO.ApplicationID, itemSalesPrice.SalesPrice, itemSalesPrice.LoosePrice);
                            dbEntity.SpCreateUpdateItemBatch(itemBO.ID, itemBO.Code, itemBO.CostPrice, itemBO.PurchasePrice, itemBO.PurchaseLoosePrice, itemSalesPrice.SalesPrice, itemSalesPrice.LoosePrice, itemBO.LandedCost, itemSalesPrice.LocationID, GeneralBO.ApplicationID, GeneralBO.CreatedUserID);
                        }
                    }
                    return itemBO.ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public int UpdateItemV3(ItemBO itemBO, List<ItemBO> ItemLocationList)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    var j = dbEntity.SpUpdateSerialNo(itemBO.ItemTypeName, "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);

                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ItemBO, Item>();
                    });
                    IMapper iMapper = config.CreateMapper();
                    Item item = dbEntity.Items.Find(itemBO.ID);
                    iMapper.Map(itemBO, item);
                    dbEntity.SpLogChange("item", "ID", itemBO.ID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                    dbEntity.SaveChanges();
                    dbEntity.SpCreateStockOrServiceType(itemBO.CategoryID, itemBO.IsStockValue);
                    dbEntity.SpDeleteItemLocation(itemBO.ID);
                    foreach (var items in ItemLocationList)
                    {
                        dbEntity.SpCreateItemLocation(itemBO.ID, itemBO.Name, items.LocationID);
                    }
                    return itemBO.ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public ItemBO GetItemDetails(int id)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SpGetItemDetails_Result, ItemBO>().ForMember(d => d.ID, o => o.Ignore());
                    });

                    IMapper iMapper = config.CreateMapper();
                    var result = dbEntity.SpGetItemDetails(id).FirstOrDefault();
                    ItemBO itemBO = iMapper.Map<SpGetItemDetails_Result, ItemBO>(result);
                    return itemBO;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ItemBO GetItemDetail(int id)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SpGetItemDetail_Result, ItemBO>()
                            .ForMember(dest => dest.ReOrderLevel, opt => opt.MapFrom(src => src.ReOrderLevelQty));
                    });
                    IMapper iMapper = config.CreateMapper();
                    var result = dbEntity.SpGetItemDetail(id).FirstOrDefault();
                    ItemBO itemBO = iMapper.Map<SpGetItemDetail_Result, ItemBO>(result);

                    itemBO.ItemTaxBOList = dbEntity.SPGetItemTaxByItemID(id).Select(a => new ItemTaxBO
                    {
                        ID = a.ID,
                        LocationID = a.LocationID,
                        Location = a.Location,
                        ItemID = a.ItemID,
                        TaxTypeID = a.TaxTypeID,
                        TaxType = a.TaxType,
                        GSTCategoryID = a.GSTCategoryID,
                        GSTCategory = a.GSTCategory
                    }).ToList();

                    itemBO.ItemWareHouseBOList = dbEntity.SPGetItemWareHouseByItemID(id).Select(a => new ItemWareHouseBO
                    {
                        ID = a.ID,
                        WareHouseID = a.WareHouseID,
                        ItemID = a.ItemID,
                        WareHouse = a.WareHouse,
                        LotID = a.LotID,
                        Lot = a.Lot,
                        BinID = a.BinID,
                        Bin = a.Bin,
                        IsDefault = a.IsDefault.HasValue ? a.IsDefault.Value : false,
                        Default = a.Default,
                    }).ToList();

                    itemBO.AlternativeItemBOList = dbEntity.SPGetAlternativeItemByItemID(id).Select(a => new AlternativeItemBO
                    {
                        ID = a.ID,
                        ItemID = a.ItemID,
                        AlternativeItemID = a.AlternativeItemID,
                        Code = a.Code,
                        Name = a.Name,
                        Category = a.CategoryName
                    }).ToList();
                    itemBO.ItemPartsNumberBOList = dbEntity.SPGetItemPartsNumberByItemID(id).Select(a => new ItemPartsNumberBO
                    {
                        ID = a.ID,
                        ItemID = a.ItemID,
                        PartsNumber = a.PartsNumber,
                        IsDefault = a.IsDefault
                    }).ToList();
                    itemBO.ItemSecondaryUnitList = dbEntity.SPGetItemSecondaryUnitByItemID(id).Select(a => new ItemSecondaryUnitBO
                    {
                        ID = a.ID,
                        ItemID = a.ItemID,
                        SecondaryUnit = a.SecondaryUnit,
                        SecondaryUnitID = a.SecondaryUnitID,
                    }).ToList();
                    itemBO.ItemSalesPriceBOList = dbEntity.SpGetItemPriceDetails(id).Select(a => new ItemSalesPriceBO
                    {
                        ID = a.ID,
                        ItemID = a.ItemID,
                        Location = a.Location,
                        LocationID = a.LocationID.HasValue ? a.LocationID.Value : 0,
                        SalesPrice = a.SalesPrice,
                        LoosePrice = a.LoosePrice.HasValue ? a.LoosePrice.Value : 0
                    }).ToList();

                    return itemBO;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ItemBO GetItemDetailsV3(int id)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SpGetItemDetailsV3_Result, ItemBO>().ForMember(d => d.ID, o => o.Ignore());
                    });

                    IMapper iMapper = config.CreateMapper();
                    var result = dbEntity.SpGetItemDetailsV3(id).FirstOrDefault();
                    ItemBO itemBO = iMapper.Map<SpGetItemDetailsV3_Result, ItemBO>(result);
                    return itemBO;
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;
                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public DatatableResultBO GetItemList(string Code, string Description, string Category, string PartsNo, string partclass, string partsGroup, string remark, string model, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetItemList(Code, Description, Category, PartsNo, partclass, partsGroup, remark, model, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Description = item.Description,
                                CategoryName = item.CategoryName,
                                PartsNo = item.PartsNo,
                                PartsClass = item.PartsClass,
                                PartsGroup = item.PartsGroup,
                                Stock = item.Stock,
                                Remark = item.Remark,
                                Model = item.Model,
                                Status = item.Isactive.HasValue && item.Isactive.Value ? "" : "processed"
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public DatatableResultBO GetAllItemList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAllItemList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                UnitID = item.UnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                stock = item.Stock,
                                Status = (bool)item.Isactive ? "" : "processed",
                                AccountsCategory = item.AccountsCategory,

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetAllItemListV3(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAllItemListV3(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                UnitID = item.UnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                stock = item.Stock,
                                Status = (bool)item.Isactive ? "" : "processed",
                                AccountsCategory = item.AccountsCategory,

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public DatatableResultBO GetAlternativeItemList(string Code, string Name, string ItemCategory, string ItemUnit, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAlternativeItemList(Code, Name, ItemCategory, ItemUnit, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        for (int i = 0; i < result.Count; i++)
                        {
                            var item = result.Skip(i).Take(1).FirstOrDefault();
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                Category = item.Category,
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public List<ItemBO> GetItemTypeList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetItemType().Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<ItemBO> GetBrandList()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetBrand().Select(a => new ItemBO
                    {
                        ID = a.BrandID,
                        Name = $"{a.BrandName ?? ""} ({a.Code ?? ""})"
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<ItemBO> GetMasterFormulaList()
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetMasterFormulaList().Select(a => new ItemBO
                    {
                        MasterFormulaRefNo = a.ID,
                        MasterFormulaName = a.Name,
                    }).ToList();
                    return item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetDrugScheduleTypeList()
        {

            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.DrugScheduleTypes.Select(a => new ItemBO
                    {
                        DrugID = a.ID,
                        DrugName = a.Name
                    }
                    ).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<ItemBO> GetItemsByPackingCodes(string CommaSeparatedPackingCodes)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetItemsByPackingCodes(CommaSeparatedPackingCodes, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ItemID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        OldItemCode2 = a.OldItemCode2,
                        IGSTPercentage = (decimal)a.IGSTPercent,
                        CGSTPercentage = (decimal)a.CGSTPercent,
                        SGSTPercentage = (decimal)a.SGSTPercent,
                        ItemCategoryID = a.CategoryID,
                        UnitID = (int)a.UnitID,
                        CessPercentage = a.CessPercentage

                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemBO> GetItemsByItemCodes(string CommaSeparatedItemCodes)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetItemsByItemCodes(CommaSeparatedItemCodes, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ID = a.ID,
                        ItemID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        OldItemCode2 = a.OldItemCode2,
                        IGSTPercentage = (decimal)a.IGSTPercent,
                        CGSTPercentage = (decimal)a.CGSTPercent,
                        SGSTPercentage = (decimal)a.SGSTPercent,
                        ItemCategoryID = a.CategoryID,
                        Unit = a.Unit,
                        UnitID = (int)a.UnitID,
                        SalesCategoryID = (int)a.SalesCategoryID,
                        SalesCategoryName = a.SalesCategory,
                        PrimaryUnitID = a.PrimaryUnitID,
                        PrimaryUnit = a.PrimaryUnit
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ItemRateBO> GetRateOfItems(string CommaSeperatedItemIds, int PriceListID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    return dbEntity.SpGetRateOfItems(CommaSeperatedItemIds, PriceListID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemRateBO
                    {
                        ItemID = a.ItemID,
                        Rate = a.Rate,
                        BatchTypeID = a.BatchTypeID
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal GetAvailableStock(int ItemID, int? BatchID, int? BatchTypeID, int WarehouseID, int LocationID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    ObjectParameter Stock = new ObjectParameter("Stock", typeof(int));
                    dbEntity.SpGetAvailableStock(ItemID, BatchID, BatchTypeID, WarehouseID, GeneralBO.FinYear, LocationID, GeneralBO.ApplicationID, Stock);
                    return Convert.ToDecimal(Stock.Value.ToString());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public decimal GetAverageSales(int ItemID)
        {
            using (SalesEntities dbEntity = new SalesEntities())
            {
                try
                {
                    ObjectParameter AverageSales = new ObjectParameter("AverageSales", typeof(int));
                    dbEntity.SpGetAverageSales(ItemID, GeneralBO.LocationID, GeneralBO.ApplicationID, AverageSales);
                    return Convert.ToDecimal(AverageSales.Value.ToString());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        //public List<ItemBO> GetItemsForPersonalAdvanceAutoComplete(string Hint)
        //{
        //    List<ItemBO> item = new List<ItemBO>();
        //    try
        //    {
        //        using (MasterEntities dbEntity = new MasterEntities())
        //        {
        //            item = dbEntity.SpGetItemsForPersonalAdvanceAutoComplete(Hint).Select(a => new ItemBO
        //            {
        //                ID = a.ID,
        //                Name = a.Name,
        //                Code = a.Code
        //            }).ToList();
        //            return item;
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public List<ItemBO> GetItemsForOfficalAdvanceAutoComplete(string Hint)
        //{
        //    List<ItemBO> item = new List<ItemBO>();
        //    try
        //    {
        //        using (MasterEntities dbEntity = new MasterEntities())
        //        {

        //            //needs to confirm
        //            //item = dbEntity.SpGetItemsForOfficialAdvanceAutoComplete(Hint).Select(a => new ItemBO
        //            //{
        //            //    ID = a.ID,
        //            //    Name = a.Name,
        //            //    Code = a.Code
        //            //}).ToList();
        //            return item;
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public string IsExist(string Name, string HSNCode, int ID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    ObjectParameter ReturnValue = new ObjectParameter("ReturnValue", typeof(string));
                    dbEntity.SpIsItemExist(
                            Name,
                            HSNCode,
                            ID,
                            ReturnValue
                        );

                    return ReturnValue.Value.ToString();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public DatatableResultBO GetItemsForOfficialAdvance(string CodeHint, string NameHint, string Type, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetItemsForOfficialAdvance(Type, CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<ItemBO> GetItemForChequeStatus()
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    var item = dbEntity.SpGetItemForChequeStatus().Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        IGSTPercentage = (decimal)a.IGSTPercent,
                        CGSTPercentage = (decimal)a.CGSTPercent,
                        SGSTPercentage = (decimal)a.SGSTPercent
                    }).ToList();
                    return item;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }
        public List<ItemBO> GetItemTransactionDetails(int ItemID, int BatchTypeID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    var item = dbEntity.SPGetItemTransactionStatus(ItemID, BatchTypeID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        Stock = a.Stock,
                        LastPR = (decimal)a.LastPR,
                        LowestPR = (decimal)a.LowestPR,
                        QtyUnderQC = a.QtyUnderQC,
                        PendingPOQty = (decimal)a.PendingOrderQty,
                        ID = (int)a.ItemID
                    }).ToList();
                    return item;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }
        public DatatableResultBO GetItemsList(string Type, int ItemCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetItemsList(Type, ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ItemID = item.ItemID,
                                ItemCode = item.ItemCode,
                                ItemName = item.ItemName,
                                ItemCategoryID = item.ItemCategoryID,
                                SalesCategoryID = item.SalesCategoryID,
                                ItemCategory = item.ItemCategory,
                                SalesCategory = item.SalesCategory,
                                IGSTPercentage = item.IGSTPercent,
                                CGSTPercentage = item.CGSTPercent,
                                SGSTPercentage = item.SGSTPercent,
                                UnitID = item.UnitID,
                                UnitName = item.UnitName
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public DatatableResultBO GetItemsListForStockAdjustment(int ItemCategoryID, int SalesCategoryID, string CodeHint, string NameHint, string UnitHint, string ItemCategoryHint, string SalesCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemListForStockAdjustment(ItemCategoryID, SalesCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                PrimaryUnit = item.PrimaryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                QtyUnderQC = item.QtyUnderQC,
                                QtyOrdered = item.QtyOrdered,
                                Stock = item.Stock,
                                ItemCategory = item.Category,
                                PurchaseCategory = item.PurchaseCategory,
                                LastPr = item.LastPR,
                                LowestPr = item.LowestPR,
                                PendingOrderQty = item.PendingOrderQty,
                                QtyAvailable = item.QtyAvailable,
                                GstPercentage = (decimal)item.GSTPercentage,
                                QtyWithQc = item.QtyUnderQC,
                                ItemCategoryID = item.CategoryID,
                                TravelCategoryID = item.TravelCategoryID,
                                FinishedGoodsCategoryID = item.FGCategoryID,
                                PurchaseUnit = item.PurchaseUnit,
                                PurchaseUnitID = item.PurchaseUnitID,
                                PurchaseCategoryID = item.PurchaseCategoryID,
                                SalesCategory = item.SalesCategory,
                                SalesCategoryID = item.SalesCategoryID,
                                Rate = item.Rate,
                                LooseRate = item.LooseRate,
                                InventoryUnit = item.InventoryUnit,
                                InventoryUnitID = item.InventoryUnitID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public List<ItemBO> GetPreProcessReceiptItemForAutoComplete(string Hint)
        {
            List<ItemBO> item = new List<ItemBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    item = dbEntity.SpGetPreProcessReceiptItemAutoComplete(Hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        Name = a.Name,
                        ItemID = (int)a.ID,
                    }).ToList();
                    return item;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DatatableResultBO GetItemListForTreatment(string CodeHint, string NameHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {

                    var result = dbEntity.SpGetItemListForTreatment(CodeHint, NameHint, SortField, SortOrder, Offset, Limit, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Name = item.ProductionGroupName,
                                PrimaryUnit = item.PriamryUnit,
                                PrimaryUnitID = item.PrimaryUnitID,
                                CategoryID = item.CategoryID
                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }

        public ItemBO GetAllCategoryID()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetAllCategory().Select(a => new ItemBO()
                    {
                        PurchaseCategoryID = (int)a.PurchaseCategoryID,
                        QCCategoryID = (int)a.QCCategoryID,
                        GSTSubCategoryID = (int)a.GSTSubCategoryID,
                        SalesCategoryID = (int)a.SalesCategoryID,
                        SalesIncentiveCategoryID = (int)a.SalesIncentiveCategoryID,
                        StorageCategoryID = (int)a.StorageCategoryID,
                        ItemTypeID = (int)a.ItemTypeID,
                        AccountsCategoryID = (int)a.AccountsCategoryID,
                        BusinessCategoryID = (int)a.BusinessCategoryID,
                        UnitID = (int)a.UnitID
                    }
                    ).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ItemBO GetServiceItemCategory()
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SpGetServiceItemCategory().Select(a => new ItemBO()
                    {
                        CategoryID = (int)a.CategoryID,
                        CostingCategoryID = (int)a.CostingCategoryID,
                        PurchaseUnitID = (int)a.PurchaseUnitID,
                        SalesUnitID = (int)a.SalesUnitID,
                        InventoryUnitID = (int)a.InventoryUnitID,
                        SecondaryUnitID = (int)a.SecondaryUnitID,
                        IsSaleable = (bool)a.IsSaleable
                    }
                    ).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DatatableResultBO GetServiceItemList(int ItemCategoryID, string CodeHint, string NameHint, string ItemCategoryHint, string SalesCategoryHint, string AccountsCategoryHint, string SortField, string SortOrder, int Offset, int Limit)
        {
            DatatableResultBO DatatableResult = new DatatableResultBO();
            DatatableResult.data = new List<object>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    var result = dbEntity.SpGetAllServiceItems(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    if (result != null && result.Count > 0)
                    {
                        DatatableResult.recordsFiltered = (int)result[0].recordsFiltered;
                        DatatableResult.recordsTotal = (int)result[0].totalRecords;
                        var obj = new object();
                        foreach (var item in result)
                        {
                            obj = new
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                Unit = item.Unit,
                                UnitID = item.UnitID,
                                ItemCategory = item.Category,
                                SalesCategory = item.SalesCategory,
                                CGSTPercentage = (decimal)item.CGSTPercent,
                                IGSTPercentage = (decimal)item.IGSTPercent,
                                SGSTPercentage = (decimal)item.SGSTPercent,
                                stock = item.Stock,
                                Status = (bool)item.Isactive ? "" : "processed",
                                AccountsCategory = item.AccountsCategory,

                            };
                            DatatableResult.data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return DatatableResult;
        }
        public List<ItemDescriptionBO> GetDescription(int ItemID, string Type)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {

                    return dbEntity.SpGetDescription(Type, ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemDescriptionBO
                    {
                        Name = a.Name,
                        Key = a.Keys,
                        Value = a.Value

                    }).ToList();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<ItemBO> GetItemListForAPI(int Offset, int Limit)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    var item = dbEntity.SpGetItemListForAPI(Offset, Limit, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ItemBO
                    {
                        ItemID = a.ItemID,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        SalesCategoryID = (int)a.SalesCategoryID,
                        SalesCategory = a.SalesCategory,
                        PackSize = (int)a.PackSize,
                        FullUnitID = a.FullUnitID,
                        LooseUnitID = (int)a.LooseUnitID,
                        FullUnit = a.FullUnit,
                        LooseUnit = a.LooseUnit,
                        PurchaseUnitID = (int)a.PurchaseUnitID,
                        PurchaseUnit = a.PurchaseUnit,
                        IsEnabled = (bool)a.IsEnabled,
                        GSTPercentage = a.GSTPercentage,
                        ManufacturerID = (int)a.ManufacturerID,
                        Manufacturer = a.Manufacturer,
                    }).ToList();
                    return item;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


        }

        public List<StockBO> GetStockListForAPI(int ItemID)
        {
            using (MasterEntities dbEntity = new MasterEntities())
            {
                try
                {
                    var item = dbEntity.SpGetStockListForAPI(ItemID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new StockBO
                    {
                        id = a.ID,
                        itemid = (int)a.ItemID,
                        batchid = (int)a.BatchID,
                        MRP = (decimal)a.MRP,
                        SalesUnitID = (int)a.SalesUnitID,
                        GSTPercentage = (decimal)a.GSTPercentage,
                        BatchNo = a.BatchNo,
                        ExpiryDate = (DateTime)a.ExpiryDate,
                        warehouseid = (int)a.WarehouseID,
                        transactiontype = a.TransactionType,
                        issue = (decimal)a.Issue,
                        Receipt = (decimal)a.Receipt,
                        value = (decimal)a.Value,
                    }).ToList();
                    return item;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public string GetItemCode(string QRCode)
        {

            try
            {
                ObjectParameter ItemCode = new ObjectParameter("ItemCode", typeof(string));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpGetItemCode(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, QRCode, ItemCode);
                    return ItemCode.Value.ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string GetBatchNo(string QRCode)
        {

            try
            {
                ObjectParameter Batch = new ObjectParameter("Batch", typeof(string));
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    dbEntity.SpGetBatchNo(GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, QRCode, Batch);
                    return Batch.Value.ToString();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int DeleteItemWareHouse(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPDeleteItemWareHouse(ID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int DeleteItemParts(int ItemID, int ID)
        {
            try
            {
                int Executed = 0;
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    Executed = dbEntity.SPDeleteItemPartsNumber(ID);
                    var ItemPartsNumberExistingList = dbEntity.SPGetItemPartsNumberByItemID(ItemID).Select(x => x.PartsNumber).ToList();
                    Item item = dbEntity.Items.Find(ItemID);
                    if (ItemPartsNumberExistingList != null)
                    {
                        item.Description = string.Join(", ", ItemPartsNumberExistingList).Trim(' ').Trim(',');
                        dbEntity.SpLogChange("item", "ID", ItemID, GeneralBO.CreatedUserID, GeneralBO.LocationID, GeneralBO.ApplicationID);
                        dbEntity.SaveChanges();
                    }
                }
                return Executed;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public int DeleteSecondaryUnitItem(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPDeleteSecondaryUnitItems(ID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int DeleteAlternativeItem(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPDeleteAlternativeItems(ID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public int DeleteItemTax(int ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    return dbEntity.SPDeleteItemTax(ID);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
