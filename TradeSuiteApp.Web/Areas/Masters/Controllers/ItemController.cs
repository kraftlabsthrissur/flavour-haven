using AutoMapper;
using BusinessLayer;
using BusinessObject;
using DataAccessLayer;
using DataAccessLayer.DBContext;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Newtonsoft.Json;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;
using WebGrease.ImageAssemble;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class ItemController : Controller
    {

        private IGSTCategoryContract GSTCategoryBL;
        private IGSTSubCategoryContract GSTSubCategoryBL;
        private ItemContract itemBL;
        private IUnitContract unitBL;
        private ICategoryContract categoryBL;
        private IGeneralContract generalBL;
        private ILocationContract locationBL;
        private IBatchContract batchBL;
        private IWareHouseContract warehouseBL;
        private IDiscountCategoryContract discountCategoryBL;
        private IUnitGroupContract unitGroupBL;
        private ILotContract lotBL;
        private IBinContract binBL;
        private IBuyerContract buyerBL;
        private IABCCodeContract aBCCodeBL;
        private ICountryContract countryBL;
        private ISupplierContract supplierBL;
        private IWareHouseContract wareHouseBL;
        private ITaxTypeContract taxTypeBL;
        private ICostingMethodContract costingMethodBL;
        public ItemController()
        {
            GSTCategoryBL = new GSTCategoryBL();
            GSTSubCategoryBL = new GSTSubCategoryBL();
            itemBL = new ItemBL();
            categoryBL = new CategoryBL();
            unitBL = new UnitBL();
            generalBL = new GeneralBL();
            locationBL = new LocationBL();
            warehouseBL = new WarehouseBL();
            batchBL = new BatchBL();
            discountCategoryBL = new DiscountCategoryBL();
            unitGroupBL = new UnitGroupBL();
            lotBL = new LotBL();
            binBL = new BinBL();
            buyerBL = new BuyerBL();
            aBCCodeBL = new ABCCodeBL();
            countryBL = new CountryBL();
            supplierBL = new SupplierBL();
            wareHouseBL = new WarehouseBL();
            taxTypeBL = new TaxTypeBL();
            costingMethodBL = new CostingMethodBL();
        }

        // GET: Masters/Item
        public ActionResult Index()
        {
            return View();
        }
        // GET: Masters/Item --For Doctor's Clinic
        public ActionResult IndexV3()
        {
            return View();
        }

        public ActionResult IndexV4()
        {
            return View();
        }

        // GET: Masters/Item/Details/5 -- For Doctor's Clinic
        public ActionResult Details(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItemBO, ItemModel>().ForMember(d => d.ID, o => o.Ignore());
            });

            IMapper iMapper = config.CreateMapper();
            ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBL.GetItemDetails(id));
            ItemBO itemBO = itemBL.GetItemDetails(id);
            itemModel.SeasonStarts = itemBO.SeasonStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonStarts);
            itemModel.SeasonEnds = itemBO.SeasonEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonEnds);
            itemModel.SeasonPurchaseStarts = itemBO.SeasonPurchaseStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseStarts);
            itemModel.SeasonPurchaseEnds = itemBO.SeasonPurchaseEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseEnds);
            itemModel.BirthDate = itemBO.BirthDate == null ? "" : General.FormatDate((DateTime)itemBO.BirthDate);
            itemModel.DisContinuedDate = itemBO.DisContinuedDate == null ? "" : General.FormatDate((DateTime)itemBO.DisContinuedDate);
            itemModel.AvailableStock = 0;
            itemModel.PendingOrderStock = 0;
            itemModel.ID = id;
            itemModel.SalesUnit = itemBO.UnitName;
            itemModel.LocationList = new SelectList(locationBL.GetItemLocationByItemID(itemModel.ID), "LocationID", "LocationName");
            itemModel.InvoiceDetails = batchBL.GetBatchTrans((int)id, "Item").Select(a => new PreviousBatchModel()
            {

                ID = a.ID,
                InvoiceNo = a.InvoiceNo,
                InvoiceDate = General.FormatDate(a.InvoiceDate, "view"),
                SupplierName = a.SupplierName,
                Unit = a.Unit,
                Quantity = a.Quantity,
                OfferQty = a.OfferQty,
                PurchasePrice = a.PurchasePrice,
                PurchaseLooseRate = a.PurchaseLooseRate,
                SalesRate = a.SalesRate,
                LooseSalesRate = a.LooseSalesRate,
                LooseQty = a.LooseQty,
                GSTPercentage = a.GSTPercentage,
                GSTAmount = a.GSTAmount,
                ProfitRatio = a.ProfitRatio,
                DiscountID = a.DiscountID,
                RetailMRP = a.RetailMRP,
                CessPercentage = a.CessPercentage,
                InvoiceRate = a.InvoiceRate,
                CGSTAmt = a.CGSTAmt,
                SGSTAmt = a.SGSTAmt,
                Discount = a.Discount,
                DiscountPercent = a.DiscountPercent,


            }).ToList();

            return View(itemModel);
        }

        // GET: Masters/Item/Details/5
        public ActionResult DetailsV3(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItemBO, ItemModel>().ForMember(d => d.ID, o => o.Ignore());
            });

            IMapper iMapper = config.CreateMapper();
            ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBL.GetItemDetailsV3(id));
            ItemBO itemBO = itemBL.GetItemDetails(id);
            itemModel.SeasonStarts = itemBO.SeasonStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonStarts);
            itemModel.SeasonEnds = itemBO.SeasonEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonEnds);
            itemModel.SeasonPurchaseStarts = itemBO.SeasonPurchaseStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseStarts);
            itemModel.SeasonPurchaseEnds = itemBO.SeasonPurchaseEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseEnds);
            itemModel.BirthDate = itemBO.BirthDate == null ? "" : General.FormatDate((DateTime)itemBO.BirthDate);
            itemModel.DisContinuedDate = itemBO.DisContinuedDate == null ? "" : General.FormatDate((DateTime)itemBO.DisContinuedDate);
            itemModel.AvailableStock = 0;
            itemModel.PendingOrderStock = 0;
            itemModel.ID = id;
            itemModel.SalesUnit = itemBO.UnitName;
            itemModel.LocationList = new SelectList(locationBL.GetItemLocationByItemID(itemModel.ID), "LocationID", "LocationName");
            itemModel.InvoiceDetails = batchBL.GetBatchTrans((int)id, "Item").Select(a => new PreviousBatchModel()
            {

                ID = a.ID,
                InvoiceNo = a.InvoiceNo,
                InvoiceDate = General.FormatDate(a.InvoiceDate, "view"),
                SupplierName = a.SupplierName,
                Unit = a.Unit,
                Quantity = a.Quantity,
                OfferQty = a.OfferQty,
                PurchasePrice = a.PurchasePrice,
                PurchaseLooseRate = a.PurchaseLooseRate,
                SalesRate = a.SalesRate,
                LooseSalesRate = a.LooseSalesRate,
                LooseQty = a.LooseQty,
                GSTPercentage = a.GSTPercentage,
                GSTAmount = a.GSTAmount,
                ProfitRatio = a.ProfitRatio,
                DiscountID = a.DiscountID,
                RetailMRP = a.RetailMRP,
                CessPercentage = a.CessPercentage,
                InvoiceRate = a.InvoiceRate,
                CGSTAmt = a.CGSTAmt,
                SGSTAmt = a.SGSTAmt,
                Discount = a.Discount,
                DiscountPercent = a.DiscountPercent,


            }).ToList();

            return View(itemModel);
        }
        public ActionResult Summary(int id)
        {
            return View();
        }

        // GET: Masters/Item/Create
        public ActionResult Create()
        {
            ItemModel itemModel = new ItemModel();
            itemModel.ItemCategoryList = categoryBL.GetCategoryListByCategoryGroupID(1).Select(a => new CategoryModel()
            {
                ID = a.ID,
                Type = a.Type,
                Name = a.Name,
                Code = a.Code
            }).ToList();
            //new  SelectList(categoryBL.GetCategoryListByCategoryGroupID(1), "ID", "Name");
            itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
            itemModel.StorageCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(3), "ID", "Name");
            itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");
            itemModel.SalesCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(5), "ID", "Name");
            itemModel.SalesIncentiveCategoryList = new SelectList(categoryBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            itemModel.QCCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(9), "ID", "Name");
            itemModel.productionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
            itemModel.AccountCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name");
            itemModel.CostCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(12), "ID", "Name");
            itemModel.AssetCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(13), "ID", "Name");
            itemModel.DiseaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(14), "ID", "Name");
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
            itemModel.GSTSubCategoryList = new SelectList(GSTSubCategoryBL.GetGSTSubCategoryList(), "ID", "Name");
            itemModel.ManufacturerList = new SelectList(categoryBL.ManufacturerList(), "ID", "Name");
            itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
            itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM
            }).ToList();
            itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "Value", "UOM");
            itemModel.GetMasterFormulaList = new SelectList(itemBL.GetMasterFormulaList(), "MasterFormulaRefNo", "MasterFormulaName");
            itemModel.DrugScheduleTypeList = new SelectList(itemBL.GetDrugScheduleTypeList(), "DrugID", "DrugName");
            itemModel.DisContinuedDate = itemModel.SeasonEnds = itemModel.SeasonPurchaseEnds = General.FormatDate(DateTime.Now.AddYears(20));
            itemModel.BirthDate = General.FormatDate(DateTime.Now);
            itemModel.SeasonPurchaseStarts = General.FormatDate(DateTime.Now);
            itemModel.SeasonStarts = General.FormatDate(DateTime.Now);
            var obj = itemBL.GetAllCategoryID();
            itemModel.PurchaseCategoryID = obj.PurchaseCategoryID;
            itemModel.QCCategoryID = obj.QCCategoryID;
            itemModel.GSTSubCategoryID = obj.GSTSubCategoryID;
            itemModel.SalesCategoryID = obj.SalesCategoryID;
            itemModel.SalesIncentiveCategoryID = obj.SalesIncentiveCategoryID;
            itemModel.StorageCategoryID = obj.StorageCategoryID;
            itemModel.ItemTypeID = obj.ItemTypeID;
            itemModel.AccountsCategoryID = obj.AccountsCategoryID;
            itemModel.BusinessCategoryID = obj.BusinessCategoryID;
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

            return View(itemModel);
        }

        // GET: Masters/Item/CreateV3 -- For Doctor's Clinic
        public ActionResult CreateV3()
        {
            ItemModel itemModel = new ItemModel();
            itemModel.ItemCategoryList = categoryBL.GetCategoryListByCategoryGroupID(1).Select(a => new CategoryModel()
            {
                ID = a.ID,
                Type = a.Type,
                Name = a.Name,
                Code = a.Code
            }).ToList();
            //new  SelectList(categoryBL.GetCategoryListByCategoryGroupID(1), "ID", "Name");
            itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
            itemModel.StorageCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(3), "ID", "Name");
            itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");
            itemModel.SalesCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(5), "ID", "Name");
            itemModel.SalesIncentiveCategoryList = new SelectList(categoryBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            itemModel.QCCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(9), "ID", "Name");
            itemModel.productionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
            itemModel.AccountCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name");
            itemModel.CostCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(12), "ID", "Name");
            itemModel.AssetCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(13), "ID", "Name");
            itemModel.DiseaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(14), "ID", "Name");
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
            itemModel.GSTSubCategoryList = new SelectList(GSTSubCategoryBL.GetGSTSubCategoryList(), "ID", "Name");
            itemModel.ManufacturerList = new SelectList(categoryBL.ManufacturerList(), "ID", "Name");
            itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
            itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                PackSize = a.PackSize
            }).ToList();
            itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "Value", "UOM");
            itemModel.GetMasterFormulaList = new SelectList(itemBL.GetMasterFormulaList(), "MasterFormulaRefNo", "MasterFormulaName");
            itemModel.DrugScheduleTypeList = new SelectList(itemBL.GetDrugScheduleTypeList(), "DrugID", "DrugName");
            itemModel.DisContinuedDate = itemModel.SeasonEnds = itemModel.SeasonPurchaseEnds = General.FormatDate(DateTime.Now.AddYears(20));
            itemModel.BirthDate = General.FormatDate(DateTime.Now);
            itemModel.SeasonPurchaseStarts = General.FormatDate(DateTime.Now);
            itemModel.SeasonStarts = General.FormatDate(DateTime.Now);
            var obj = itemBL.GetAllCategoryID();
            itemModel.PurchaseCategoryID = obj.PurchaseCategoryID;
            itemModel.QCCategoryID = obj.QCCategoryID;
            itemModel.GSTSubCategoryID = obj.GSTSubCategoryID;
            itemModel.SalesCategoryID = obj.SalesCategoryID;
            itemModel.SalesIncentiveCategoryID = obj.SalesIncentiveCategoryID;
            itemModel.StorageCategoryID = obj.StorageCategoryID;
            itemModel.ItemTypeID = obj.ItemTypeID;
            itemModel.AccountsCategoryID = obj.AccountsCategoryID;
            itemModel.BusinessCategoryID = obj.BusinessCategoryID;
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(itemModel);
        }

        // GET: Masters/Item/CreateV4 -- For All clients
        public ActionResult CreateV4()
        {

            ItemModel itemModel = new ItemModel();
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            itemModel.TaxTypeList = new SelectList(new List<SelectListItem>());// new SelectList(taxTypeBL.GetTaxTypeList(), "ID", "Name");
            itemModel.GSTCategoryList = new SelectList(new List<SelectListItem>());// new SelectList(GSTCategoryBL.GetTaxCategoryList(), "ID", "Name");
            itemModel.CategorySelectList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(1), "ID", "Name");
            itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
            itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");
            var PurchaseCategory = categoryBL.GetCategoryListByCategoryGroupID(2).FirstOrDefault();
            if (PurchaseCategory != null)
            {
                itemModel.PurchaseCategoryID = PurchaseCategory.ID;
            }
            itemModel.CostingMethodList = new SelectList(costingMethodBL.GetCostingMethodList(), "ID", "Name");
            var CostingMethod = costingMethodBL.GetCostingMethodList().FirstOrDefault();
            if (CostingMethod != null)
            {
                itemModel.CostingMethodID = CostingMethod.ID;
            }
            itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
            itemModel.BrandList = new SelectList(itemBL.GetBrandList(), "ID", "Name");
            itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                PackSize = a.PackSize
            }).ToList();
            //  itemModel.SecondaryUnitList = new SelectList(new List<SelectListItem>());// new SelectList(unitBL.GetSecondaryUnitList(), "ID", "Name");
            itemModel.SecondaryUnitList =  new SelectList(unitBL.GetSecondaryUnitList(), "ID", "Name");
            itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "ID", "UOM");
            itemModel.UnitGroupList = new SelectList(unitGroupBL.GetUnitGroupList(), "ID", "Name");
            var Group = unitGroupBL.GetUnitGroupList().OrderBy(x => x.ID).FirstOrDefault();
            if (Group != null)
            {
                itemModel.UnitGroupID = Group.ID;
            }
            itemModel.BuyerList = new SelectList(buyerBL.GetBuyerList(), "ID", "Name");
            itemModel.LotList = new SelectList(new List<SelectListItem>());// new SelectList(lotBL.GetLotList(), "ID", "LotNumber");
            itemModel.BinList = new SelectList(new List<SelectListItem>());// new SelectList(binBL.GetBinList(), "ID", "BinCode");
            itemModel.ABCCodeList = new SelectList(aBCCodeBL.GetABCCodeList(), "ID", "Code");
            itemModel.OEMCountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            itemModel.SupplierList = new SelectList(supplierBL.GetAllSupplierList(), "ID", "Name");
            itemModel.WareHouseList = new SelectList(wareHouseBL.GetWareHouseList(), "ID", "Name");
            itemModel.IsStockValue = true;
            itemModel.ConversionFactorPurchaseToInventory = 1;
            itemModel.ConversionFactorPurchaseToLoose = 1;
            itemModel.ConversionFactorSalesToInventory = 1;
            itemModel.ConversionFactorLooseToSales = 1;
            return View(itemModel);
        }

        public ActionResult DetailsV4(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItemBO, ItemModel>();//.ForMember(d => d.ID, o => o.Ignore())
            });

            IMapper iMapper = config.CreateMapper();
            var itemBO = itemBL.GetItemDetail(id);
            ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBO);
            itemModel.ItemImages = new string[] {
                itemBO.ItemImagePath1,itemBO.ItemImagePath2,
                itemBO.ItemImagePath3,itemBO.ItemImagePath4,
                itemBO.ItemImagePath5, itemBO.ItemImagePath6,
                itemBO.ItemImagePath7, itemBO.ItemImagePath8
              };
            if (itemModel.ItemImages != null)
            {
                for (int i = 0; i < itemModel.ItemImages.Length; i++)
                {
                    var imagePath = itemModel.ItemImages[i];
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        try
                        {
                            var serverPath = Server.MapPath(imagePath);
                            byte[] imageBytes = System.IO.File.ReadAllBytes(serverPath);
                            string base64String = Convert.ToBase64String(imageBytes);
                            itemModel.ItemImages[i] = "data:image/png;base64," + base64String; // Prefix with Base64 image identifier
                            if (string.IsNullOrEmpty(itemModel.ItemImagePath))
                            {
                                itemModel.ItemImagePath = Url.Content(imagePath); ;
                            }

                        }
                        catch
                        {
                            itemModel.ItemImages[i] = "";
                        }
                    }
                    else
                    {
                        itemModel.ItemImages[i] = "";
                    }
                }
            }

            itemModel.ItemTaxList = itemBO.ItemTaxBOList.Select(a => new ItemTax
            {
                ID = a.ID,
                ItemID = a.ItemID,
                LocationID = a.LocationID,
                Location = a.Location,
                TaxTypeID = a.TaxTypeID,
                TaxType = a.TaxType,
                GSTCategoryID = a.GSTCategoryID,
                GSTCategory = a.GSTCategory
            }).ToList();

            itemModel.ItemWareHouseList = itemBO.ItemWareHouseBOList.Select(a => new ItemWareHouse
            {
                ID = a.ID,
                ItemID = a.ItemID,
                WareHouseID = a.WareHouseID,
                WareHouse = a.WareHouse,
                LotID = a.LotID,
                Lot = a.Lot,
                BinID = a.BinID,
                Bin = a.Bin,
                IsDefault = a.IsDefault,
                Default = a.Default,
            }).ToList();

            itemModel.AlternativeItemList = itemBO.AlternativeItemBOList.Select(a => new AlternativeItem
            {
                ID = a.ID,
                AlternativeItemID = a.AlternativeItemID,
                ItemID = a.ItemID,
                Code = a.Code,
                Name = a.Name,
                Category = a.Category
            }).ToList();

            itemModel.ItemSalesPriceList = itemBO.ItemSalesPriceBOList.Select(a => new ItemSalesPrice
            {
                ID = a.ID,
                ItemID = a.ItemID,
                LocationID = a.LocationID,
                Location = a.Location,
                SalesPrice = a.SalesPrice,
                LoosePrice = a.LoosePrice
            }).ToList();
            itemModel.ItemPartsNumberList = itemBO.ItemPartsNumberBOList.Select(a => new ItemPartsNumber
            {
                ID = a.ID,
                ItemID = a.ItemID,
                PartsNumber = a.PartsNumber,
                IsDefault = a.IsDefault,
            }).ToList();
            itemModel.BrandList = new SelectList(itemBL.GetBrandList(), "ID", "Name");
            return View(itemModel);
        }

        public ActionResult EditV4(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItemBO, ItemModel>();
            });

            IMapper iMapper = config.CreateMapper();
            var itemBO = itemBL.GetItemDetail(id);
            ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBO);

            itemModel.ItemImages = new string[] {
                itemBO.ItemImagePath1,itemBO.ItemImagePath2,
                itemBO.ItemImagePath3,itemBO.ItemImagePath4,
                itemBO.ItemImagePath5, itemBO.ItemImagePath6,
                itemBO.ItemImagePath7, itemBO.ItemImagePath8
              };
            if (itemModel.ItemImages != null)
            {
                for (int i = 0; i < itemModel.ItemImages.Length; i++)
                {
                    var imagePath = itemModel.ItemImages[i];
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        try
                        {
                            var serverPath = Server.MapPath(imagePath);
                            byte[] imageBytes = System.IO.File.ReadAllBytes(serverPath);
                            string base64String = Convert.ToBase64String(imageBytes);
                            itemModel.ItemImages[i] = "data:image/png;base64," + base64String; // Prefix with Base64 image identifier
                            if (string.IsNullOrEmpty(itemModel.ItemImagePath))
                            {
                                itemModel.ItemImagePath = Url.Content(imagePath); ;
                            }

                        }
                        catch
                        {
                            itemModel.ItemImages[i] = "";
                        }
                    }
                    else
                    {
                        itemModel.ItemImages[i] = "";
                    }
                }
            }

            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            itemModel.TaxTypeList = new SelectList(taxTypeBL.GetTaxTypeListByLocation(itemModel.LocationID), "ID", "Name");// new SelectList(taxTypeBL.GetTaxTypeList(), "ID", "Name");
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetTaxCategoryListByTaxType(itemModel.TaxTypeID), "ID", "Name");// new SelectList(GSTCategoryBL.GetTaxCategoryList(), "ID", "Name");
            itemModel.CategorySelectList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(1), "ID", "Name");
            itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
            itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");

            itemModel.CostingMethodList = new SelectList(costingMethodBL.GetCostingMethodList(), "ID", "Name");
            itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
            itemModel.BrandList = new SelectList(itemBL.GetBrandList(), "ID", "Name");
            itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                PackSize = a.PackSize
            }).ToList();
            itemModel.SecondaryUnitList = new SelectList(unitBL.GetSecondarytUnitListByUnitID(itemModel.InventoryUnitID), "ID", "Name");
            itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "ID", "UOM");
            itemModel.UnitGroupList = new SelectList(unitGroupBL.GetUnitGroupList(), "ID", "Name");
            var Group = unitGroupBL.GetUnitGroupList().OrderBy(x => x.ID).FirstOrDefault();
            if (Group != null)
            {
                itemModel.UnitGroupID = Group.ID;
            }
            itemModel.BuyerList = new SelectList(buyerBL.GetBuyerList(), "ID", "Name");
            itemModel.LotList = new SelectList(lotBL.GetLotListByBin(itemModel.BinID), "ID", "LotNumber");// new SelectList(lotBL.GetLotList(), "ID", "LotNumber");
            itemModel.BinList = new SelectList(binBL.GetBinListByWareHouse(itemModel.WareHouseID), "ID", "BinCode");// new SelectList(binBL.GetBinList(), "ID", "BinCode");
            itemModel.ABCCodeList = new SelectList(aBCCodeBL.GetABCCodeList(), "ID", "Code");
            itemModel.OEMCountryList = new SelectList(countryBL.GetCountryList(), "ID", "Name");
            itemModel.SupplierList = new SelectList(supplierBL.GetAllSupplierList(), "ID", "Name");
            itemModel.WareHouseList = new SelectList(wareHouseBL.GetWareHouseList(), "ID", "Name");
            itemModel.ItemTaxList = itemBO.ItemTaxBOList.Select(a => new ItemTax
            {
                ID = a.ID,
                LocationID = a.LocationID,
                ItemID = a.ItemID,
                Location = a.Location,
                TaxTypeID = a.TaxTypeID,
                TaxType = a.TaxType,
                GSTCategoryID = a.GSTCategoryID,
                GSTCategory = a.GSTCategory
            }).ToList();

            itemModel.ItemWareHouseList = itemBO.ItemWareHouseBOList.Select(a => new ItemWareHouse
            {
                ID = a.ID,
                WareHouseID = a.WareHouseID,
                ItemID = a.ItemID,
                WareHouse = a.WareHouse,
                LotID = a.LotID,
                Lot = a.Lot,
                BinID = a.BinID,
                Bin = a.Bin,
                IsDefault = a.IsDefault,
                Default = a.Default,
            }).ToList();

            itemModel.AlternativeItemList = itemBO.AlternativeItemBOList.Select(a => new AlternativeItem
            {
                ID = a.ID,
                AlternativeItemID = a.AlternativeItemID,
                ItemID = a.ItemID,
                Code = a.Code,
                Name = a.Name,
                Category = a.Category
            }).ToList();

            itemModel.ItemSalesPriceList = itemBO.ItemSalesPriceBOList.Select(a => new ItemSalesPrice
            {
                ID = a.ID,
                ItemID = a.ItemID,
                LocationID = a.LocationID,
                Location = a.Location,
                SalesPrice = a.SalesPrice,
                LoosePrice = a.LoosePrice
            }).ToList();
            itemModel.ItemPartsNumberList = itemBO.ItemPartsNumberBOList.Select(a => new ItemPartsNumber
            {
                ID = a.ID,
                ItemID = a.ItemID,
                PartsNumber = a.PartsNumber,
                IsDefault = a.IsDefault
            }).ToList();
            itemModel.ItemSecondaryUnitList = itemBO.ItemSecondaryUnitList.Select(a => new ItemSecondaryUnit
            {
                ID = a.ID,
                ItemID = a.ItemID,
                SecondaryUnitID = a.SecondaryUnitID,
                SecondaryUnit = a.SecondaryUnit
            }).ToList();
            itemModel.ConversionFactorPurchaseToInventory = 1;
            itemModel.ConversionFactorPurchaseToLoose = 1;
            itemModel.ConversionFactorSalesToInventory = 1;
            itemModel.ConversionFactorLooseToSales = 1;
            return View(itemModel);
        }

        public ActionResult EditV3(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItemBO, ItemModel>().ForMember(d => d.ID, o => o.Ignore());
            });

            IMapper iMapper = config.CreateMapper();
            ItemBO itemBO = itemBL.GetItemDetails(id);
            ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBL.GetItemDetailsV3(id));

            itemModel.SeasonStarts = itemBO.SeasonStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonStarts);
            itemModel.SeasonEnds = itemBO.SeasonEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonEnds);
            itemModel.SeasonPurchaseStarts = itemBO.SeasonPurchaseStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseStarts);
            itemModel.SeasonPurchaseEnds = itemBO.SeasonPurchaseEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseEnds);
            itemModel.BirthDate = itemBO.BirthDate == null ? "" : General.FormatDate((DateTime)itemBO.BirthDate);
            itemModel.DisContinuedDate = itemBO.DisContinuedDate == null ? "" : General.FormatDate((DateTime)itemBO.DisContinuedDate);
            itemModel.AvailableStock = 0;
            itemModel.PendingOrderStock = 0;
            itemModel.ID = id;
            itemModel.ManufacturerList = new SelectList(categoryBL.ManufacturerList(), "ID", "Name");
            itemModel.ItemCategoryList = categoryBL.GetCategoryListByCategoryGroupID(1).Select(a => new CategoryModel()
            {
                ID = a.ID,
                Type = a.Type,
                Name = a.Name,
                Code = a.Code
            }).ToList();
            itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
            itemModel.StorageCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(3), "ID", "Name");
            itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");
            itemModel.SalesCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(5), "ID", "Name");
            itemModel.SalesIncentiveCategoryList = new SelectList(categoryBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            itemModel.QCCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(9), "ID", "Name");
            itemModel.productionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
            itemModel.AccountCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name");
            itemModel.CostCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(12), "ID", "Name");
            itemModel.AssetCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(13), "ID", "Name");
            itemModel.DiseaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(14), "ID", "Name");
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
            itemModel.GSTSubCategoryList = new SelectList(GSTSubCategoryBL.GetGSTSubCategoryList(), "ID", "Name");
            itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
            itemModel.ManufacturerList = new SelectList(categoryBL.ManufacturerList(), "ID", "Name");
            itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM
            }).ToList();
            itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "ID", "UOM");
            itemModel.GetMasterFormulaList = new SelectList(itemBL.GetMasterFormulaList(), "MasterFormulaRefNo", "MasterFormulaName");
            itemModel.DrugScheduleTypeList = new SelectList(itemBL.GetDrugScheduleTypeList(), "DrugID", "DrugName");
            itemModel.UnitOMID = itemModel.UnitID + "#" + itemModel.PackSize;
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(itemModel);
        }


        // POST: Masters/Item/Create
        [HttpPost]
        public ActionResult Save(ItemModel model)
        {
            decimal SalePrice = 0;
            string PartsNumbers = "", ItemImagePath = "",
                ItemImagePath1 = "", ItemImagePath2 = "",
                ItemImagePath3 = "", ItemImagePath4 = "",
                ItemImagePath5 = "", ItemImagePath6 = "",
                ItemImagePath7 = "", ItemImagePath8 = "";

            List<ItemBO> ItemLocationList = new List<ItemBO>();
            if (model.ItemImages != null)
            {
                for (int i = 1; i <= model.ItemImages.Length; i++)
                {
                    var ItemImage = model.ItemImages[i - 1];
                    if (!string.IsNullOrEmpty(ItemImage))
                    {
                        try
                        {
                            var base64Data = ItemImage.Substring(ItemImage.IndexOf(",") + 1);
                            byte[] imageBytes = Convert.FromBase64String(base64Data);
                            string fileName = "item_" + Guid.NewGuid().ToString() + "_" + DateTime.Now.Ticks + ".png"; // Generate a unique file name
                            string folderPath = "~/Outputs/Item/"; // Save in the Images folder in your project
                            ItemImagePath = Path.Combine(folderPath, fileName);
                            var ItemImageServerPath = Server.MapPath(ItemImagePath);
                            if (!Directory.Exists(Server.MapPath(folderPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(folderPath));
                            }
                            using (var fileStream = new FileStream(ItemImageServerPath, FileMode.Create, FileAccess.Write))
                            {
                                fileStream.Write(imageBytes, 0, imageBytes.Length);
                            }
                        }
                        catch
                        {
                            ItemImagePath = "";
                        }
                        if (i == 1)
                            ItemImagePath1 = ItemImagePath;
                        if (i == 2)
                            ItemImagePath2 = ItemImagePath;
                        if (i == 3)
                            ItemImagePath3 = ItemImagePath;
                        if (i == 4)
                            ItemImagePath4 = ItemImagePath;
                        if (i == 5)
                            ItemImagePath5 = ItemImagePath;
                        if (i == 6)
                            ItemImagePath6 = ItemImagePath;
                        if (i == 7)
                            ItemImagePath7 = ItemImagePath;
                        if (i == 8)
                            ItemImagePath8 = ItemImagePath;
                    }

                }
            }
            try
            {
                if (model.ID == 0)
                {
                    if (string.IsNullOrEmpty(model.Code))
                        model.Code = generalBL.UpdateSerialNo("ItemMaster", model.CategoryName);//??
                }

                ItemBO itemBO = new ItemBO()
                {
                    //general
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    SalesInquiryItemID = model.SalesInquiryItemID,
                    PurchaseRequisitionTrasID = model.PurchaseRequisitionTrasID,
                    CategoryID = model.CategoryID,
                    CategoryName = model.CategoryName,
                    SanskritName = model.SanskritName,//Remarks 
                    MalayalamName = model.MalayalamName,//Regional Language 
                    PurchaseCategoryID = model.PurchaseCategoryID,//PartsClass- 
                    BusinessCategoryID = model.BusinessCategoryID,
                    Description = model.Description,//PartsNo
                    UnitGroupID = model.UnitGroupID,
                    SalesUnitID = model.InventoryUnitID,
                    PurchaseUnitID = model.InventoryUnitID,
                    InventoryUnitID = model.InventoryUnitID,
                    LooseUnitID = model.InventoryUnitID,
                    ConversionFactorPurchaseToInventory = 1,
                    ConversionFactorPurchaseToLoose = 1,
                    ConversionFactorSalesToInventory = 1,
                    ConversionFactorLooseToSales = 1,
                    Isactive = model.Isactive,
                    IsStockItem = model.IsStockItem,
                    IsStockValue = model.IsStockValue,
                    //ReOrderQty = model.ReOrderQty,
                    //Cost
                    CostPrice = model.CostPrice,
                    PurchasePrice = model.PurchasePrice,
                    SalePrice = model.SalePrice,
                    LandedCost = model.LandedCost,
                    CostingMethodID = model.CostingMethodID,
                    //quantity
                    ReOrderLevelName = model.ReOrderLevelName,
                    ReOrderLevel = model.ReOrderLevel,
                    ReOrderQty = model.ReOrderQty,
                    //QuantityOnHand = model.QuantityOnHand,
                    //unit
                    ItemLength = model.ItemLength,
                    LengthUOMID = model.LengthUOMID == 0 ? null : (int?)model.LengthUOMID,
                    ItemWidth = model.ItemWidth,
                    WidthUOMID = model.WidthUOMID == 0 ? null : (int?)model.WidthUOMID,
                    ItemHight = model.ItemHight,
                    HightUOMID = model.HightUOMID == 0 ? null : (int?)model.HightUOMID,
                    NetWeight = model.NetWeight,
                    NetWeightUOMID = model.NetWeightUOMID == 0 ? null : (int?)model.NetWeightUOMID,
                    InnerDiameter = model.InnerDiameter,
                    OuterDiameter = model.OuterDiameter,
                    //mise
                    BuyerID = model.BuyerID == 0 ? null : (int?)model.BuyerID,
                    SupplierPartCode = model.SupplierPartCode,
                    SupplierID = model.SupplierID == 0 ? null : (int?)model.SupplierID,
                    OEMCode = model.OEMCode,
                    OEMCountryID = model.OEMCountryID == 0 ? null : (int?)model.OEMCountryID,
                    ABCCodeID = model.ABCCodeID == 0 ? null : (int?)model.ABCCodeID,
                    ABCCode = model.ABCCode,
                    HSNCode = model.HSNCode,
                    EANCode = model.EANCode,
                    BarCode = model.BarCode,
                    BudgetQuantity = model.BudgetQuantity,
                    Make = model.Make,
                    Model = model.Model,
                    ItemImagePath1 = ItemImagePath1,
                    ItemImagePath2 = ItemImagePath2,
                    ItemImagePath3 = ItemImagePath3,
                    ItemImagePath4 = ItemImagePath4,
                    ItemImagePath5 = ItemImagePath5,
                    ItemImagePath6 = ItemImagePath6,
                    ItemImagePath7 = ItemImagePath7,
                    ItemImagePath8 = ItemImagePath8,
                    DisplayPercentage = model.DisplayPercentage,
                    BrandID = model.BrandID,
                    cross_reference = model.cross_reference,
                    DiscountPercentage = model.DiscountPercentage,
                };

                if (model.ItemLocationList != null)
                {
                    ItemBO ItemLocation;
                    for (int i = 0; i < model.ItemLocationList.Count; i++)
                    {
                        var location = model.ItemLocationList.Skip(i).FirstOrDefault();
                        ItemLocation = new ItemBO()
                        {
                            LocationID = location.LocationID
                        };
                        ItemLocationList.Add(ItemLocation);
                    }
                }

                if (model.ItemTaxList != null && model.ItemTaxList.Count > 0)
                {
                    itemBO.ItemTaxBOList = new List<ItemTaxBO>();
                    for (int i = 0; i < model.ItemTaxList.Count; i++)
                    {
                        var tax = model.ItemTaxList.Skip(i).Take(1).FirstOrDefault();
                        var iemTaxBO = new ItemTaxBO()
                        {
                            ID = tax.ID,
                            LocationID = tax.LocationID,
                            TaxTypeID = tax.TaxTypeID,
                            GSTCategoryID = tax.GSTCategoryID,
                        };
                        if (iemTaxBO.ID == 0)
                            itemBO.ItemTaxBOList.Add(iemTaxBO);
                    }
                }
                else
                {
                    itemBO.ItemTaxBOList = new List<ItemTaxBO>();
                    int LocationID = GeneralBO.LocationID;
                    int TaxTypeID = taxTypeBL.GetTaxTypeListByLocation(LocationID).FirstOrDefault().ID;
                    int GSTCategoryID = GSTCategoryBL.GetTaxCategoryListByTaxType(TaxTypeID).FirstOrDefault(x => x.VATPercent == 0).ID;
                    var iemTaxBO = new ItemTaxBO()
                    {
                        ID = 0,
                        LocationID = LocationID,
                        TaxTypeID = TaxTypeID,
                        GSTCategoryID = GSTCategoryID,
                    };
                    itemBO.ItemTaxBOList.Add(iemTaxBO);


                }
                if (model.ItemWareHouseList != null)
                {
                    itemBO.ItemWareHouseBOList = new List<ItemWareHouseBO>();
                    for (int i = 0; i < model.ItemWareHouseList.Count; i++)
                    {
                        var warehouse = model.ItemWareHouseList.Skip(i).Take(1).FirstOrDefault();
                        var wareHouseBOItem = new ItemWareHouseBO()
                        {
                            ID = warehouse.ID,
                            WareHouseID = warehouse.WareHouseID,
                            BinID = warehouse.BinID,
                            LotID = warehouse.LotID,
                            IsDefault = warehouse.IsDefault,
                        };
                        if (wareHouseBOItem.ID == 0)
                            itemBO.ItemWareHouseBOList.Add(wareHouseBOItem);
                    }
                }
                if (model.AlternativeItemList != null)
                {
                    itemBO.AlternativeItemBOList = new List<AlternativeItemBO>();
                    for (int i = 0; i < model.AlternativeItemList.Count; i++)
                    {
                        var alternativeitem = model.AlternativeItemList.Skip(i).Take(1).FirstOrDefault();
                        var alternativeBOItem = new AlternativeItemBO()
                        {
                            ID = alternativeitem.ID,
                            AlternativeItemID = alternativeitem.AlternativeItemID,
                            Name = alternativeitem.Name,
                            Category = alternativeitem.Category,
                        };
                        if (alternativeBOItem.ID == 0)
                            itemBO.AlternativeItemBOList.Add(alternativeBOItem);
                    }
                }
                if (model.ItemSalesPriceList != null && model.ItemSalesPriceList.Count > 0)
                {
                    itemBO.ItemSalesPriceBOList = new List<ItemSalesPriceBO>();
                    for (int i = 0; i < model.ItemSalesPriceList.Count; i++)
                    {
                        var itemSalesPrice = model.ItemSalesPriceList.Skip(i).Take(1).FirstOrDefault();
                        if (itemSalesPrice.LocationID == GeneralBO.LocationID)
                        {
                            SalePrice = itemSalesPrice.SalesPrice;
                        }
                        var itemSalesPriceBO = new ItemSalesPriceBO()
                        {
                            ID = itemSalesPrice.ID,
                            LocationID = itemSalesPrice.LocationID,
                            Location = itemSalesPrice.Location,
                            SalesPrice = itemSalesPrice.SalesPrice,
                            LoosePrice = itemSalesPrice.LoosePrice,
                        };
                        itemBO.ItemSalesPriceBOList.Add(itemSalesPriceBO);//if (itemSalesPrice.ID == 0 || itemSalesPrice.Edited == 1)
                    }
                }

                if (model.ItemPartsNumberList != null)
                {
                    itemBO.ItemPartsNumberBOList = new List<ItemPartsNumberBO>();
                    itemBO.UpdateItemPartsNumberBOList = new List<ItemPartsNumberBO>();
                    for (int i = 0; i < model.ItemPartsNumberList.Count; i++)
                    {
                        var itemPartsNumber = model.ItemPartsNumberList.Skip(i).Take(1).FirstOrDefault();
                        var itemPartsNumberItem = new ItemPartsNumberBO()
                        {
                            ID = itemPartsNumber.ID,
                            PartsNumber = itemPartsNumber.PartsNumber,
                            IsDefault = itemPartsNumber.IsDefault
                        };
                        if (itemPartsNumber.ID == 0)
                            itemBO.ItemPartsNumberBOList.Add(itemPartsNumberItem);
                        else
                            itemBO.UpdateItemPartsNumberBOList.Add(itemPartsNumberItem);
                    }
                    PartsNumbers = string.Join(",", model.ItemPartsNumberList.Select(x => x.PartsNumber).ToList());
                }
                if (model.ItemSecondaryUnitList != null)
                {
                    itemBO.ItemSecondaryUnitList = new List<ItemSecondaryUnitBO>();
                    for (int i = 0; i < model.ItemSecondaryUnitList.Count; i++)
                    {
                        var itemSecondaryUnit = model.ItemSecondaryUnitList.Skip(i).Take(1).FirstOrDefault();
                        var itemSecondaryUnitItem = new ItemSecondaryUnitBO()
                        {
                            ID = itemSecondaryUnit.ID,
                            SecondaryUnitID = itemSecondaryUnit.SecondaryUnitID,
                        };
                        if (itemSecondaryUnit.ID == 0)
                            itemBO.ItemSecondaryUnitList.Add(itemSecondaryUnitItem);
                    }
                }
                var ItemID = itemBL.CreateItem(itemBO, ItemLocationList);
                model.PartsNumbers = PartsNumbers;
                model.ItemID = ItemID;
                model.SalePrice = SalePrice;
                var returnData = new { model.ItemID, model.PurchaseUnitID, model.PartsNumbers, model.Code, model.Name, model.SalePrice };
                return Json(new { Status = "success", data = returnData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Masters/Item/Edit/5
        public ActionResult Edit(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ItemBO, ItemModel>().ForMember(d => d.ID, o => o.Ignore());
            });

            IMapper iMapper = config.CreateMapper();
            ItemBO itemBO = itemBL.GetItemDetails(id);
            ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBL.GetItemDetails(id));

            itemModel.SeasonStarts = itemBO.SeasonStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonStarts);
            itemModel.SeasonEnds = itemBO.SeasonEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonEnds);
            itemModel.SeasonPurchaseStarts = itemBO.SeasonPurchaseStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseStarts);
            itemModel.SeasonPurchaseEnds = itemBO.SeasonPurchaseEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseEnds);
            itemModel.BirthDate = itemBO.BirthDate == null ? "" : General.FormatDate((DateTime)itemBO.BirthDate);
            itemModel.DisContinuedDate = itemBO.DisContinuedDate == null ? "" : General.FormatDate((DateTime)itemBO.DisContinuedDate);
            itemModel.AvailableStock = 0;
            itemModel.PendingOrderStock = 0;
            itemModel.ID = id;
            itemModel.ManufacturerList = new SelectList(categoryBL.ManufacturerList(), "ID", "Name");

            itemModel.ItemCategoryList = categoryBL.GetCategoryListByCategoryGroupID(1).Select(a => new CategoryModel()
            {
                ID = a.ID,
                Type = a.Type,
                Name = a.Name,
                Code = a.Code
            }).ToList();
            itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
            itemModel.StorageCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(3), "ID", "Name");
            itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");
            itemModel.SalesCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(5), "ID", "Name");
            itemModel.SalesIncentiveCategoryList = new SelectList(categoryBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
            itemModel.QCCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(9), "ID", "Name");
            itemModel.productionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
            itemModel.AccountCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name");
            itemModel.CostCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(12), "ID", "Name");
            itemModel.AssetCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(13), "ID", "Name");
            itemModel.DiseaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(14), "ID", "Name");
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
            itemModel.GSTSubCategoryList = new SelectList(GSTSubCategoryBL.GetGSTSubCategoryList(), "ID", "Name");
            itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
            itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
            {
                UOM = a.UOM,
                ID = a.ID,
                QOM = a.QOM
            }).ToList();
            itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "ID", "UOM");
            itemModel.GetMasterFormulaList = new SelectList(itemBL.GetMasterFormulaList(), "MasterFormulaRefNo", "MasterFormulaName");
            itemModel.DrugScheduleTypeList = new SelectList(itemBL.GetDrugScheduleTypeList(), "DrugID", "DrugName");
            itemModel.UnitOMID = itemModel.UnitID + "#" + itemModel.PackSize;
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(itemModel);
        }

        //GET: Masters/Item/Edit/5 --For Doctor's Clinic
        //public ActionResult EditV3(int id)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<ItemBO, ItemModel>().ForMember(d => d.ID, o => o.Ignore());
        //    });

        //    IMapper iMapper = config.CreateMapper();
        //    ItemBO itemBO = itemBL.GetItemDetails(id);
        //    ItemModel itemModel = iMapper.Map<ItemBO, ItemModel>(itemBL.GetItemDetails(id));

        //    itemModel.SeasonStarts = itemBO.SeasonStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonStarts);
        //    itemModel.SeasonEnds = itemBO.SeasonEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonEnds);
        //    itemModel.SeasonPurchaseStarts = itemBO.SeasonPurchaseStarts == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseStarts);
        //    itemModel.SeasonPurchaseEnds = itemBO.SeasonPurchaseEnds == null ? "" : General.FormatDate((DateTime)itemBO.SeasonPurchaseEnds);
        //    itemModel.BirthDate = itemBO.BirthDate == null ? "" : General.FormatDate((DateTime)itemBO.BirthDate);
        //    itemModel.DisContinuedDate = itemBO.DisContinuedDate == null ? "" : General.FormatDate((DateTime)itemBO.DisContinuedDate);
        //    itemModel.AvailableStock = 0;
        //    itemModel.PendingOrderStock = 0;
        //    itemModel.ID = id;
        //    itemModel.ManufacturerList = new SelectList(categoryBL.ManufacturerList(), "ID", "Name");
        //    itemModel.ItemCategoryList = categoryBL.GetCategoryListByCategoryGroupID(1).Select(a => new CategoryModel()
        //    {
        //        ID = a.ID,
        //        Type = a.Type,
        //        Name = a.Name,
        //        Code = a.Code
        //    }).ToList();
        //    itemModel.PurchaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(2), "ID", "Name");
        //    itemModel.StorageCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(3), "ID", "Name");
        //    itemModel.BusinessCategoryList = new SelectList(categoryBL.GetBusinessCategoryList(0), "ID", "Name");
        //    itemModel.SalesCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(5), "ID", "Name");
        //    itemModel.SalesIncentiveCategoryList = new SelectList(categoryBL.GetSalesIncentiveCategoryList(0), "ID", "Name");
        //    itemModel.QCCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(9), "ID", "Name");
        //    itemModel.productionCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(10), "ID", "Name");
        //    itemModel.AccountCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(11), "ID", "Name");
        //    itemModel.CostCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(12), "ID", "Name");
        //    itemModel.AssetCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(13), "ID", "Name");
        //    itemModel.DiseaseCategoryList = new SelectList(categoryBL.GetCategoryListByCategoryGroupID(14), "ID", "Name");
        //    itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
        //    itemModel.GSTSubCategoryList = new SelectList(GSTSubCategoryBL.GetGSTSubCategoryList(), "ID", "Name");
        //    itemModel.ItemTypeList = new SelectList(itemBL.GetItemTypeList(), "ID", "Name");
        //    itemModel.UnitList = unitBL.GetUnitList().Select(a => new UnitModel()
        //    {
        //        UOM = a.UOM,
        //        ID = a.ID,
        //        PackSize = a.PackSize
        //    }).ToList();
        //    itemModel.UOMList = new SelectList(unitBL.GetUnitList(), "ID", "UOM");
        //    itemModel.GetMasterFormulaList = new SelectList(itemBL.GetMasterFormulaList(), "MasterFormulaRefNo", "MasterFormulaName");
        //    itemModel.DrugScheduleTypeList = new SelectList(itemBL.GetDrugScheduleTypeList(), "DrugID", "DrugName");
        //    itemModel.UnitOMID = itemModel.UnitID + "#" + itemModel.PackSize;
        //    itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");

        //    return View(itemModel);
        //}
        public ActionResult GetItemCodeByItemType(string Form)
        {
            var SerialNo = generalBL.GetSerialNo(Form, "Code");
            return Json(SerialNo, JsonRequestBehavior.AllowGet);
        }

        //POST: Masters/Item/Create
        [HttpPost]
        public ActionResult SaveV3(ItemModel model)
        {
            List<ItemBO> ItemLocationList = new List<ItemBO>();
            //if (ModelState.IsValid)
            //{
            try
            {
                if (model.ID == 0)
                {
                    model.Code = generalBL.UpdateSerialNo("ItemMaster", model.CategoryName);
                }
                ItemBO itemBO = new ItemBO()
                {
                    ID = model.ID,
                    Code = model.Code,
                    Name = model.Name,
                    MalayalamName = model.MalayalamName,
                    HindiName = model.HindiName,
                    SanskritName = model.SanskritName,
                    StorageCategoryID = model.StorageCategoryID,
                    ItemTypeID = model.ItemTypeID,
                    OldItemCode = model.OldItemCode,
                    OldItemCode2 = model.OldItemCode2,
                    HSNCode = model.HSNCode,
                    BarCode = model.BarCode,
                    QRCode = model.QRCode,
                    Description = model.Description,
                    CategoryID = model.CategoryID,
                    BusinessCategoryID = model.BusinessCategoryID,
                    SecondaryUnitID = model.InventoryUnitID,
                    InventoryUnitID = model.InventoryUnitID,
                    UnitID = model.InventoryUnitID,
                    PackSize = model.PackSize,
                    ConversionFactorPtoI = 1,
                    MinStockQTY = model.MinStockQTY,
                    MaxStockQTY = model.MaxStockQTY,
                    //    BirthDate = model.BirthDate==null ?"": General.ToDateTime(model.BirthDate),
                    //    DisContinuedDate = model.DisContinuedDate == null ? "" : General.ToDateTime(model.DisContinuedDate),
                    IsStockItem = model.IsStockItem,
                    IsStockValue = model.IsStockValue,
                    IsDemandPlanRequired = model.IsDemandPlanRequired,
                    IsMaterialPlanRequired = model.IsMaterialPlanRequired,
                    IsPhantomItem = model.IsPhantomItem,
                    SalesIncentiveCategoryID = model.SalesIncentiveCategoryID,
                    SalesCategoryID = model.SalesCategoryID,
                    DrugScheduleID = model.DrugScheduleID,
                    SalesUnitID = model.InventoryUnitID,
                    ConversionFactorPtoS = 1,
                    MinSalesQtyFull = model.MinSalesQtyFull,
                    MinSalesQtyLoose = model.MinSalesQtyLoose,
                    MaxSalesQty = model.MaxSalesQty,
                    DiseaseCategoryID = model.DiseaseCategoryID,
                    // SeasonStarts = model.SeasonStarts == null ? "" : General.ToDateTime( model.SeasonStarts),
                    //  SeasonEnds = model.SeasonEnds == null ? "" : General.ToDateTime(model.SeasonEnds),
                    IsSaleable = model.IsSaleable,
                    N2GActivity = model.N2GActivity,
                    IsMrp = model.IsMrp,
                    IsPriceListReference = model.IsPriceListReference,
                    BotanicalName = model.BotanicalName,
                    QCCategoryID = model.QCCategoryID,
                    PatentNo = model.PatentNo,
                    ConversionFactorPtoSecondary = 1,
                    IsQCRequired = model.IsQCRequired,
                    IsQCRequiredForProduction = model.IsQCRequiredForProduction,
                    IsProprietary = model.IsProprietary,
                    CostingCategoryID = model.CostingCategoryID,
                    PurchaseCategoryID = model.PurchaseCategoryID,
                    PurchaseUnitID = model.PurchaseUnitID,
                    ReOrderLevel = model.ReOrderLevel,
                    MinPurchaseQTY = model.MinPurchaseQTY,
                    MaxPurchaseQTY = model.MaxPurchaseQTY,
                    QtyTolerancePercent = model.QtyTolerancePercent,
                    ReOrderQty = model.ReOrderQty,
                    // SeasonPurchaseStarts = model.SeasonPurchaseStarts == null ? "" : General.ToDateTime(model.SeasonPurchaseStarts),
                    //   SeasonPurchaseEnds = model.SeasonPurchaseEnds == null ? "" : General.ToDateTime(model.SeasonPurchaseEnds),
                    PurchaseLeadTime = model.PurchaseLeadTime,
                    IsPurchaseItem = model.IsPurchaseItem,
                    IsSeasonalPurchase = model.IsSeasonalPurchase,
                    IsPORequired = model.IsPORequired,
                    AssetCategoryID = model.AssetCategoryID,
                    GSTCategoryID = model.GSTCategoryID,
                    AccountsCategoryID = model.AccountsCategoryID,
                    GSTSubCategoryID = model.GSTSubCategoryID,
                    IsLocation = model.IsLocation,
                    IsInterCompany = model.IsInterCompany,
                    IsDepartment = model.IsDepartment,
                    IsEmployee = model.IsEmployee,
                    IsProject = model.IsProject,
                    IsAsset = model.IsAsset,
                    ProductionCategoryID = model.ProductionCategoryID,
                    MasterFormulaRefNo = model.MasterFormulaRefNo,
                    NormalLossQty = model.NormalLossQty,
                    NormalLossPercent = model.NormalLossPercent,
                    ProductLeadDays = model.ProductLeadDays,
                    BatchSizeQTY = model.BatchSizeQTY,
                    IsMasterFormula = model.IsMasterFormula,
                    IsReProcessAllowed = model.IsReProcessAllowed,
                    IsBatch = model.IsBatch,
                    IsDraft = model.IsDraft,
                    IsDisContinued = model.IsDisContinued,
                    Isactive = model.Isactive,
                    CreatedDate = DateTime.Now,
                    DiscountID = 0,
                    QCLeadTime = 0,
                    CostingGroupID = 0,
                    CostComponentID = 0,
                    ItemTypeName = model.ItemTypeName,
                    OldName = model.OldName,
                    ProductionGroup = model.ProductionGroup,
                    ShelfLifeMonths = model.ShelfLifeMonths,
                    ConversionFactorPurchaseToInventory = model.ConversionFactorPurchaseToInventory,
                    ConversionFactorSalesToInventory = 1,
                    ManufacturerID = model.ManufacturerID

                };
                if (model.SeasonStarts != null)
                {
                    itemBO.SeasonStarts = General.ToDateTime(model.SeasonStarts);
                }
                if (model.SeasonEnds != null)
                {
                    itemBO.SeasonEnds = General.ToDateTime(model.SeasonEnds);
                }
                if (model.SeasonPurchaseStarts != null)
                {
                    itemBO.SeasonPurchaseStarts = General.ToDateTime(model.SeasonPurchaseStarts);
                }
                if (model.SeasonPurchaseEnds != null)
                {
                    itemBO.SeasonPurchaseEnds = General.ToDateTime(model.SeasonPurchaseEnds);
                }
                if (model.BirthDate != null)
                {
                    itemBO.BirthDate = General.ToDateTime(model.BirthDate);
                }
                if (model.DisContinuedDate != null)
                {
                    itemBO.DisContinuedDate = General.ToDateTime(model.DisContinuedDate);
                }
                if (model.ItemLocationList != null)
                {

                    ItemBO ItemLocation;
                    foreach (var item in model.ItemLocationList)
                    {
                        ItemLocation = new ItemBO()
                        {
                            LocationID = item.LocationID
                        };
                        ItemLocationList.Add(ItemLocation);
                    }
                }
                itemBL.CreateItemV3(itemBO, ItemLocationList);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", data = e.Message }, JsonRequestBehavior.AllowGet);
            }
            //}
            //else
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    return Json(new { Status = "failure", data = errors }, JsonRequestBehavior.AllowGet);
            //}
        }
        public JsonResult GetPreProcessIssueItemsList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string ActivityHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = itemBL.GetPreProcessIssueItemsList(CodeHint, NameHint, ItemCategoryHint, ActivityHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();

                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSaleableItemsList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SalesCategoryHint = Datatable.Columns[5].Search.Value;
                string PartsNoHint = Datatable.Columns[6].Search.Value;
                string ModelHint = Datatable.Columns[7].Search.Value;
                string RemarkHint = Datatable.Columns[9].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int SalesCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("SalesCategoryID", Datatable.Params));
                int PriceListID = Convert.ToInt32(Datatable.GetValueFromKey("PriceListID", Datatable.Params));
                int StoreID = Convert.ToInt32(Datatable.GetValueFromKey("StoreID", Datatable.Params));
                bool CheckStock = Convert.ToBoolean(Datatable.GetValueFromKey("CheckStock", Datatable.Params));
                int BatchTypeID = Convert.ToInt32(Datatable.GetValueFromKey("BatchTypeID", Datatable.Params));
                int BusinessCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("BusinessCategoryID", Datatable.Params));
                string FullOrLoose = Datatable.GetValueFromKey("FullOrLoose", Datatable.Params);
                DatatableResultBO resultBO = itemBL.GetSaleableItemsList(ItemCategoryID, SalesCategoryID, BusinessCategoryID, PriceListID, StoreID, CheckStock, BatchTypeID, FullOrLoose, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, PartsNoHint, ModelHint, RemarkHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPurchaseItemsList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SalesCategoryHint = Datatable.Columns[5].Search.Value;
                string PartsNoHint = Datatable.Columns[6].Search.Value;
                string ModelHint = Datatable.Columns[7].Search.Value;
                string RemarkHint = Datatable.Columns[8].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int SalesCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("SalesCategoryID", Datatable.Params));
                int PriceListID = 0;/* Convert.ToInt32(Datatable.GetValueFromKey("PriceListID", Datatable.Params));*/
                int StoreID = Convert.ToInt32(Datatable.GetValueFromKey("StoreID", Datatable.Params));
                bool CheckStock = Convert.ToBoolean(Datatable.GetValueFromKey("CheckStock", Datatable.Params));
                int BatchTypeID = Convert.ToInt32(Datatable.GetValueFromKey("BatchTypeID", Datatable.Params));
                int BusinessCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("BusinessCategoryID", Datatable.Params));
                string FullOrLoose = "F";// Datatable.GetValueFromKey("FullOrLoose", Datatable.Params);
                DatatableResultBO resultBO = itemBL.GetSaleableItemsList(ItemCategoryID, SalesCategoryID, BusinessCategoryID, PriceListID, StoreID, CheckStock, BatchTypeID, FullOrLoose, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, PartsNoHint, ModelHint, RemarkHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockableItemsList(DatatableModel Datatable)

        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string PartsNumberHHit = Datatable.Columns[5].Search.Value;
                string MakeHHit = Datatable.Columns[6].Search.Value;
                string ModelHHit = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetStockableItemsList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, PartsNumberHHit, ModelHHit, MakeHHit, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductionDefinitionMaterialList(DatatableModel Datatable)

        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                string Type = Datatable.GetValueFromKey("Type", Datatable.Params);

                DatatableResultBO resultBO = itemBL.GetProductionDefinitionMaterialList(Type, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsListForReport(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                string Type = Convert.ToString(Datatable.GetValueFromKey("Type", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetItemsListForReport(Type, ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsListForPurchase(DatatableModel Datatable)
      {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string UnitHint = Datatable.Columns[7].Search.Value;
                string ItemCategoryHint = Datatable.Columns[8].Search.Value;
                string PurchaseCategoryHint = Datatable.Columns[9].Search.Value;
                string PartsNumberHit = Datatable.Columns[4].Search.Value;
                string ModelHint = Datatable.Columns[6].Search.Value;
                string RemarksHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;


                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int PurchaseCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("PurchaseCategoryID", Datatable.Params));
                int BusinessCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("BusinessCategoryID", Datatable.Params));
                int SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));
                string Type = Convert.ToString(Datatable.GetValueFromKey("Type", Datatable.Params));
                //Type = Type == null ? "stock" : Type;
                DatatableResultBO resultBO = itemBL.GetItemsListForPurchase(Type, ItemCategoryID, PurchaseCategoryID, BusinessCategoryID, SupplierID, CodeHint, NameHint, PartsNumberHit, ModelHint, RemarksHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemsListByPurchaseRequisitionIDS(int[] PurchaseRequisitionIDS)
        {
            try
            {
                List<ItemBO> resultBO = itemBL.GetItemsListByPurchaseRequisitionIDS(PurchaseRequisitionIDS);
                return Json(new { Status = "success", items = resultBO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemsListByPurchaseRequisition(int PurchaseRequisitionID)
        {
            try
            {
                List<ItemBO> resultBO = itemBL.GetItemsListByPurchaseRequisition(PurchaseRequisitionID);
                return Json(new { Status = "success", items = resultBO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemsListForStockAdjustment(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string UnitHint = Datatable.Columns[4].Search.Value;
                string ItemCategoryHint = Datatable.Columns[5].Search.Value;
                string SalesCategoryHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int SalesCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("SalesCategoryID", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetItemsListForStockAdjustment(ItemCategoryID, SalesCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemsListForStockAdjustmentForAlopathy(DatatableModel Datatable)
        {
            try
            {
                int StockAjustmentPremise = Convert.ToInt32(generalBL.GetConfig("DefaultStockAdjustmentStore", GeneralBO.CreatedUserID));
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DateTime FromDate = General.ToDateTime(Datatable.GetValueFromKey("FromDate", Datatable.Params));
                DateTime ToDate = General.ToDateTime(Datatable.GetValueFromKey("ToDate", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetItemsListForStockAdjustmentForAlopathy(FromDate, ToDate, CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDebitAndCreditItemList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string UnitHint = Datatable.Columns[4].Search.Value;
                string ItemCategoryHint = Datatable.Columns[5].Search.Value;
                string PurchaseCategoryHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = Convert.ToString(Datatable.GetValueFromKey("Type", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetDebitAndCreditItemsList(Type, CodeHint, NameHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, "", SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReturnItemList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ProductGroupID = Convert.ToInt32(Datatable.GetValueFromKey("ProductGroupID", Datatable.Params));
                int IssueItemID = Convert.ToInt32(Datatable.GetValueFromKey("IssueItemID", Datatable.Params));
                int ReceiptItemID = Convert.ToInt32(Datatable.GetValueFromKey("ReceiptItemID", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetReturnItemList(ProductGroupID, IssueItemID, ReceiptItemID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPackingItemList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string CategoryHint = Datatable.Columns[4].Search.Value;

                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = itemBL.GetPackingItemList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAvailableStockItemsList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string PartsNoHint = Datatable.Columns[5].Search.Value;
                string MakeHint = Datatable.Columns[6].Search.Value;
                string ModelHint = Datatable.Columns[7].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                int WarehouseID = Convert.ToInt32(Datatable.GetValueFromKey("WarehouseID", Datatable.Params));
                int BatchTypeID = Convert.ToInt32(Datatable.GetValueFromKey("BatchTypeID", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetAvailableStockItemsList(ItemCategoryID, WarehouseID, BatchTypeID, CodeHint, NameHint, ItemCategoryHint, PartsNoHint, MakeHint, ModelHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsListForMaterialPurification(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string UnitHint = Datatable.Columns[4].Search.Value;
                string ItemCategoryHint = Datatable.Columns[5].Search.Value;
                string PurchaseCategoryHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetItemsListForMaterialPurification(ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, PurchaseCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductionGroupItemAutoComplete(string NameHint)
        {
            try
            {
                DatatableResultBO resultBO = itemBL.GetProductionGroupList("", NameHint, "", "Name", "ASC", 0, 20);
                var result = new { Status = "success", data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetProductionGroupItemAutoCompleteForReport(string NameHint)
        {
            try
            {
                DatatableResultBO resultBO = itemBL.GetProductionGroupItemAutoCompleteForReport("", NameHint, "", "Name", "ASC", 0, 20);
                var result = new { Status = "success", data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ItemByServiceCategoryAndSupplierID(string Area, string NameHint, int ItemCategoryID = 0, int PurchaseCategoryID = 0, int SupplierID = 0, int BusinessCategoryID = 0)
        {
            try
            {
                DatatableResultBO resultBO = itemBL.GetItemsListForPurchase(Area, ItemCategoryID, PurchaseCategoryID, BusinessCategoryID, SupplierID, "", NameHint, "", "", "", "", "", "", "Name", "ASC", 0, 20);
                var result = new { Status = "success", data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductionGroupItemList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string CategoryHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = itemBL.GetProductionGroupList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRawMaterialList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string CategoryHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int WarehouseID = Convert.ToInt32(Datatable.GetValueFromKey("WarehouseID", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetRawMaterialList(WarehouseID, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPackingMaterialList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string CategoryHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                DatatableResultBO resultBO = itemBL.GetPackingMaterialList(CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductionIssueMaterialReturnList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string CategoryHint = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ProductionID = Convert.ToInt32(Datatable.GetValueFromKey("productionID", Datatable.Params));
                int ProductionSequence = Convert.ToInt32(Datatable.GetValueFromKey("productionSequence", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetProductionIssueMaterialReturnList(ProductionID, ProductionSequence, CodeHint, NameHint, CategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGRNWiseItemsList(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int SupplierID = Convert.ToInt32(Datatable.GetValueFromKey("SupplierID", Datatable.Params));

                DatatableResultBO resultBO = itemBL.GetGRNWiseItemsList(SupplierID, CodeHint, NameHint, ItemCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetRawMaterialForAutoComplete(int WarehouseID, string Hint = "")
        {
            DatatableResultBO resultBO = itemBL.GetRawMaterialList(WarehouseID, "", Hint, "", "Name", "ASC", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPackingMaterialForAutoComplete(string Hint = "")
        {
            DatatableResultBO resultBO = itemBL.GetPackingMaterialList("", Hint, "", "Name", "ASC", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPreProcessIssueItemsForAutoComplete(string Hint = "")
        {
            DatatableResultBO resultBO = itemBL.GetPreProcessIssueItemsList("", Hint, "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionGroupItemsForAutoComplete(string Hint = "")
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetProductionGroupItemsForAutoComplete(Hint).Select(a => new ItemModel()
            {
                ItemID = a.ID,
                ItemName = a.Name,
                ItemCode = a.Code,
                UnitName = a.Unit,
                IsKalkan = a.IsKalkan
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackingItemsForAutoComplete(string Hint = "")
        {
            DatatableResultBO resultBO = itemBL.GetPackingItemList("", Hint, "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSaleableItemsForAutoComplete(int StoreID, bool CheckStock, int BatchTypeID = 0, string FullOrLoose = "F", string Hint = "", int ItemCategoryID = 0, int SalesCategoryID = 0, int PriceListID = 0, int BusinessCategoryID = 0)
        {
            DatatableResultBO resultBO = itemBL.GetSaleableItemsList(ItemCategoryID, SalesCategoryID, BusinessCategoryID, 0, StoreID, CheckStock, BatchTypeID, FullOrLoose, null, Hint, null, null, null, null, null, "Name", "asc", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStockableItemsForAutoComplete(string Hint = "", int ItemCategoryID = 0)
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetStockableItemsForAutoComplete(Hint, ItemCategoryID).Select(a => new ItemModel()
            {
                ItemID = a.ID,
                ItemName = a.Name,
                ItemCode = a.Code,
                UnitName = a.Unit,
                UnitID = a.UnitID,
                CGSTPercentage = a.CGSTPercentage,
                IGSTPercentage = a.IGSTPercentage,
                SGSTPercentage = a.SGSTPercentage,
                Stock = a.Stock,
                ItemCategory = a.ItemCategory,
                InventoryUnit = a.InventoryUnit,
                InventoryUnitID = a.InventoryUnitID,
                Code = a.Code,
                SalesCategoryName = a.SalesCategoryName
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackingReturnForAutoComplete(string Hint = "", int ProductGroupID = 0, int ReceiptItemID = 0, int IssueItemID = 0)
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetPackingReturnForAutoComplete(Hint, ProductGroupID, ReceiptItemID, IssueItemID).Select(a => new ItemModel()
            {
                ItemID = a.ID,
                ItemName = a.Name,
                ItemCode = a.Code,
                UnitName = a.Unit,
                UnitID = a.UnitID,

            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableStockItemsForAutoComplete(string Hint = "", int ItemCategoryID = 0, int WarehouseID = 0, int BatchTypeID = 0)
        {
            DatatableResultBO resultBO = itemBL.GetAvailableStockItemsList(ItemCategoryID, WarehouseID, BatchTypeID, "", Hint, "", "", "", "", "Name", "ASC", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGRNWiseItemsForAutoComplete(string Hint = "", int SupplierID = 0)
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetGRNWiseItemsForAutoComplete(Hint, SupplierID).Select(a => new ItemModel()
            {
                ItemID = a.ItemID,
                ItemName = a.Name,
                Stock = (decimal)a.Stock,
                UnitName = a.UnitName,
                UnitID = a.UnitID,
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemsForSchemeItem(int CategoryID = 0)
        {
            DatatableResultBO resultBO = itemBL.GetItemsForSchemeItem(CategoryID);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult GetAllItemsForAutoComplete(string Hint = "", int ItemCategoryID = 0, int SalesCategoryID = 0)
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetAllItemsForAutoComplete(Hint, ItemCategoryID, SalesCategoryID).Select(a => new ItemModel()
            {
                ItemID = a.ID,
                ItemName = a.Name,
                ItemCode = a.Code,
                UnitName = a.Unit,
                UnitID = a.UnitID,
                Type = a.Type,
                SalesCategoryID = a.SalesCategoryID,
                SalesCategoryName = a.SalesCategoryName,
                CategoryName = a.CategoryName
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReciptItemsForAutoComplete(int ProductGroupID, string Hint = "")
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetReceiptItemsForAutoComplete(ProductGroupID, Hint).Select(a => new ItemModel()
            {
                ItemName = a.Name,
                ItemID = a.ItemID,
                BatchSize = a.BatchSize,
                Code = a.Code,
                ConversionFactorPtoS = a.ConversionFactorPtoS

            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDebitAndCrediItemsAutoComplete(string Hint = "", string Type = "")
        {
            DatatableResultBO resultBO = itemBL.GetDebitAndCreditItemsList(Type, "", Hint, "", "", "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetReciptItemsItemList(int ProductionGroupID)
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetReciptItemsItemList(ProductionGroupID).Select(a => new ItemModel()
            {
                ItemName = a.Name,
                ItemID = a.ItemID,
                BatchSize = a.BatchSize,
                Code = a.Code,
                ConversionFactorPtoS = a.ConversionFactorPtoS
            }).ToList();

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllItemList(DatatableModel Datatable)
        {
            try
            {
                string Code = Datatable.Columns[1].Search.Value;
                string Category = Datatable.Columns[2].Search.Value;
                string Description = Datatable.Columns[3].Search.Value;
                string PartsNo = Datatable.Columns[4].Search.Value;
                string PartsClass = Datatable.Columns[5].Search.Value;
                string PartsGroup = Datatable.Columns[6].Search.Value;
                string Remark = Datatable.Columns[7].Search.Value;
                string Model = Datatable.Columns[8].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = itemBL.GetItemList(Code, Description, Category, PartsNo, PartsClass, PartsGroup, Remark, Model, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //For Doctor's Clinic
        public JsonResult GetAllItemListV3(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string ItemCategoryHint = Datatable.Columns[4].Search.Value;
                string SalesCategoryHint = Datatable.Columns[5].Search.Value;
                string AccountsCategoryHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetAllItemListV3(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAlternativeItemList(DatatableModel Datatable)
        {
            try
            {
                string Code = Datatable.Columns[2].Search.Value;
                string Name = Datatable.Columns[3].Search.Value;
                string ItemCategory = Datatable.Columns[4].Search.Value;
                string ItemUnit = Datatable.Columns[5].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetAlternativeItemList(Code, Name, ItemCategory, ItemUnit, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsAutoComplete(string Hint = "", string Type = "", int ItemCategoryID = 0)
        {
            DatatableResultBO resultBO = itemBL.GetItemsList(Type, ItemCategoryID, "", Hint, "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAverageSalesAndStock(int ItemID, int? BatchTypeID, int WarehouseID, int LocationID)
        {
            try
            {
                decimal AverageSales = itemBL.GetAverageSales(ItemID);
                decimal Stock = itemBL.GetAvailableStock(ItemID, null, BatchTypeID, WarehouseID, LocationID);
                return Json(new { Status = "success", AverageSales = AverageSales, Stock = Stock }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "success", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsListForSaleableServiceItem(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string UnitHint = Datatable.Columns[4].Search.Value;
                string ItemCategoryHint = Datatable.Columns[5].Search.Value;
                string SalesCategoryHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int ItemCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("ItemCategoryID", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetItemsListForSaleableServiceItem(ItemCategoryID, CodeHint, NameHint, UnitHint, ItemCategoryHint, SalesCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsListForSaleableServiceItemForAutoComplete(string Hint = "", int ItemCategoryID = 0)
        {
            DatatableResultBO resultBO = itemBL.GetItemsListForSaleableServiceItem(ItemCategoryID, "", Hint, "", "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemsListForSaleableServiceAndStockItem(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string SalesCategoryHint = Datatable.Columns[4].Search.Value;
                string SalesIncentiveCategoryHint = Datatable.Columns[5].Search.Value;
                string BusinessCategoryHint = Datatable.Columns[6].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;

                int SalesCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("SalesCategoryID", Datatable.Params));
                int SalesIncentiveCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("SalesIncentiveCategoryID", Datatable.Params));
                int BusinessCategoryID = Convert.ToInt32(Datatable.GetValueFromKey("BusinessCategoryID", Datatable.Params));
                DatatableResultBO resultBO = itemBL.GetItemsListForSaleableServiceAndStockItem(SalesCategoryID, SalesIncentiveCategoryID, BusinessCategoryID, CodeHint, NameHint, SalesCategoryHint, SalesIncentiveCategoryHint, BusinessCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetItemsListForSaleableServiceAndStockItemForAutoComplete(string Hint = "", int SalesCategoryID = 0, int SalesIncentiveCategoryID = 0, int BusinessCategoryID = 0)
        {
            DatatableResultBO resultBO = itemBL.GetItemsListForSaleableServiceAndStockItem(SalesCategoryID, SalesIncentiveCategoryID, BusinessCategoryID, "", Hint, "", "", "", "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsItemExist(string Name, string HSNCode, int ID)
        {
            try
            {

                string message = itemBL.IsExist(Name, HSNCode, ID);
                //int BatchTypeID = customerBL.GetBatchTypeID(CustomerID);
                return
                     Json(new
                     {
                         Status = "success",
                         data = message,
                         message = ""
                     }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return
                        Json(new
                        {
                            Status = "",
                            data = "",
                            message = e.Message
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemsForOfficialAdvance(DatatableModel Datatable)

        {
            try
            {
                string CodeHint = Datatable.Columns[2].Search.Value;
                string NameHint = Datatable.Columns[3].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                string Type = (Datatable.GetValueFromKey("Type", Datatable.Params)).ToString();
                DatatableResultBO resultBO = itemBL.GetItemsForOfficialAdvance(CodeHint, NameHint, Type, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var res = new List<object>();
                return Json(new { Status = "failure", data = res, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getStockAdjustmentItemForAutoComplete(string FromDate, string ToDate, string term)
        {
            int StockAjustmentPremise = Convert.ToInt32(generalBL.GetConfig("DefaultStockAdjustmentStore", GeneralBO.CreatedUserID));
            DatatableResultBO resultBO = itemBL.GetItemsListForStockAdjustmentForAlopathy(General.ToDateTime(FromDate), General.ToDateTime(ToDate), "", term, "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getStockAdjustmentItemForAutoCompleteV3(int ItemCategoryID, int SalesCategoryID, string term)
        {
            DatatableResultBO resultBO = itemBL.GetItemsListForStockAdjustment(ItemCategoryID, SalesCategoryID, "", term, "", "", "", "Name", "ASC", 0, 20);

            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemsForOfficialAdvanceForAutoComplete(string Type, string Hint = "")
        {
            DatatableResultBO resultBO = itemBL.GetItemsForOfficialAdvance("", Hint, Type, "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemForChequeStatus()
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetItemForChequeStatus().Select(a => new ItemModel()
            {
                ID = a.ID,
                ItemName = a.Name,
                Code = a.Code,
                IGSTPercentage = a.IGSTPercentage,
                CGSTPercentage = a.CGSTPercentage,
                SGSTPercentage = a.SGSTPercentage
            }).ToList();
            return Json(new
            {
                Status = "success",
                data = itemList,
                message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTranasactionDetails(int ItemID, int BatchTypeID)
        {
            ItemTransDetailsModel list = new ItemTransDetailsModel();
            list = itemBL.GetItemTransactionDetails(ItemID, BatchTypeID).Select(a => new ItemTransDetailsModel()
            {
                Stock = a.Stock,
                PendingPOQty = a.PendingPOQty,
                LastPR = a.LastPR,
                LowestPR = a.LowestPR,
                QtyUnderQC = a.QtyUnderQC
            }).First();
            return Json(new
            {
                Status = "success",
                data = list,
                message = ""
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPreProcessReceiptItemForAutoComplete(string Hint = "")
        {
            List<ItemModel> itemList = new List<ItemModel>();
            itemList = itemBL.GetPreProcessReceiptItemForAutoComplete(Hint).Select(a => new ItemModel()
            {
                ItemID = a.ItemID,
                ItemName = a.Name,
            }).ToList();
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemListForTreatment(DatatableModel Datatable)
        {
            try
            {
                string CodeHint = Datatable.Columns[1].Search.Value;
                string NameHint = Datatable.Columns[2].Search.Value;
                string SortField = Datatable.Columns[Datatable.Order[0].Column].Data;
                string SortOrder = Datatable.Order[0].Dir;
                int Offset = Datatable.Start;
                int Limit = Datatable.Length;
                DatatableResultBO resultBO = itemBL.GetItemListForTreatment(CodeHint, NameHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTreatmentItemsAutoComplete(string Hint)
        {
            DatatableResultBO resultBO = itemBL.GetItemListForTreatment("", Hint, "Name", "ASC", 0, 20);
            return Json(resultBO.data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemLocationMapping(int ItemID)
        {
            List<ItemModel> ItemLocationList = locationBL.GetItemLocationByItemID(ItemID).Select(a => new ItemModel()
            {
                ItemID = a.ID,
                ItemName = a.LocationName,
                LocationID = a.LocationID
            }).ToList();
            return Json(ItemLocationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDescription(int ItemID, string Type = "")
        {
            List<ItemDescriptionModel> itemlist = new List<ItemDescriptionModel>();
            var Name = "";
            itemlist = itemBL.GetDescription(ItemID, Type).Select(a => new ItemDescriptionModel()
            {
                Name = a.Name,
                Key = a.Key,
                Value = a.Value
            }).ToList();
            Name = itemBL.GetDescription(ItemID, Type).FirstOrDefault().Name;
            return Json(new { data = itemlist, Name = Name }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getTaxTypeData(int LocationID)
        {
            var taxtypeData = taxTypeBL.GetTaxTypeListByLocation(LocationID);
            return Json(taxtypeData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTaxData(int TaxTypeID)
        {
            var taxData = GSTCategoryBL.GetTaxCategoryListByTaxType(TaxTypeID);
            return Json(taxData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getBinData(int WareHouseID)
        {
            var binData = binBL.GetBinListByWareHouse(WareHouseID);
            return Json(binData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getLotData(int BinID)
        {
            var lotData = lotBL.GetLotListByBin(BinID);
            return Json(lotData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSecondarytUnitList(int UnitID)
        {
            var SecondarytUnit = unitBL.GetSecondarytUnitListByUnitID(UnitID).Select(x => new { x.ID, x.Name, x.PackSize }).ToList();
            return Json(new { Status = "success", Data = SecondarytUnit }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteItemWareHouse(int ID)
        {
            itemBL.DeleteItemWareHouse(ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteItemParts(int ItemID, int ID)
        {
            itemBL.DeleteItemParts(ItemID, ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteAlternativeItem(int ID)
        {
            itemBL.DeleteAlternativeItem(ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteItemTax(int ID)
        {
            itemBL.DeleteItemTax(ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteSecondaryUnitItem(int ID)
        {
            itemBL.DeleteSecondaryUnitItem(ID);
            return Json(new { Status = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}
