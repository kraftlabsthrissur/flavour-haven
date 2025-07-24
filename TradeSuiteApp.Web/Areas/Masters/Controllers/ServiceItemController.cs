using AutoMapper;
using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeSuiteApp.Web.Areas.Masters.Models;
using TradeSuiteApp.Web.Models;
using TradeSuiteApp.Web.Utils;

namespace TradeSuiteApp.Web.Areas.Masters.Controllers
{
    public class ServiceItemController : Controller
    {
        private IGSTCategoryContract GSTCategoryBL;
        private IGSTSubCategoryContract GSTSubCategoryBL;
        private ItemContract itemBL;
        private IUnitContract unitBL;
        private ICategoryContract categoryBL;
        private IGeneralContract generalBL;
        private ILocationContract locationBL;

        public ServiceItemController()
        {
            GSTCategoryBL = new GSTCategoryBL();
            GSTSubCategoryBL = new GSTSubCategoryBL();
            itemBL = new ItemBL();
            categoryBL = new CategoryBL();
            unitBL = new UnitBL();
            generalBL = new GeneralBL();
            locationBL = new LocationBL();
        }
        // GET: Masters/ServiceItem
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            ItemModel itemModel = new ItemModel();
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
            itemModel.UnitID = obj.UnitID;
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
            itemModel.DisContinuedDate = itemModel.SeasonEnds = itemModel.SeasonPurchaseEnds = General.FormatDate(DateTime.Now.AddYears(20));
            itemModel.BirthDate = General.FormatDate(DateTime.Now);
            itemModel.SeasonPurchaseStarts = General.FormatDate(DateTime.Now);
            itemModel.SeasonStarts = General.FormatDate(DateTime.Now);
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            var item = itemBL.GetServiceItemCategory();
            itemModel.CategoryID = item.CategoryID;
            itemModel.CostingCategoryID = item.CostingCategoryID;
            itemModel.InventoryUnitID = item.InventoryUnitID;
            itemModel.SecondaryUnitID = item.SecondaryUnitID;
            itemModel.PurchaseUnitID = item.PurchaseUnitID;
            itemModel.SalesUnitID = item.SalesUnitID;
            itemModel.IsSaleable = item.IsSaleable;
            itemModel.Code = generalBL.GetSerialNo("Service Item", "Code");
            return View(itemModel);
        }

        [HttpPost]
        public ActionResult Save(ItemModel model)
        {
            List<ItemBO> ItemLocationList = new List<ItemBO>();
            try
            {
                if (model.ID == 0)
                {
                    var j = generalBL.UpdateSerialNo("Service Item","Code");
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
                    SecondaryUnitID = model.SecondaryUnitID,
                    InventoryUnitID = model.InventoryUnitID,
                    UnitID = model.UnitID,
                    PackSize = 1,
                    ConversionFactorPtoI = model.ConversionFactorPtoI,
                    MinStockQTY = model.MinStockQTY,
                    MaxStockQTY = model.MaxStockQTY,
                    IsStockItem = model.IsStockItem,
                    IsStockValue = model.IsStockValue,
                    IsDemandPlanRequired = model.IsDemandPlanRequired,
                    IsMaterialPlanRequired = model.IsMaterialPlanRequired,
                    IsPhantomItem = model.IsPhantomItem,
                    SalesIncentiveCategoryID = model.SalesIncentiveCategoryID,
                    SalesCategoryID = model.SalesCategoryID,
                    DrugScheduleID = model.DrugScheduleID,
                    SalesUnitID = model.SalesUnitID,
                    ConversionFactorPtoS = (decimal)model.ConversionFactorPtoI,
                    MinSalesQtyFull = model.MinSalesQtyFull,
                    MinSalesQtyLoose = model.MinSalesQtyLoose,
                    MaxSalesQty = model.MaxSalesQty,
                    DiseaseCategoryID = model.DiseaseCategoryID,
                    IsSaleable = model.IsSaleable,
                    N2GActivity = model.N2GActivity,
                    IsMrp = model.IsMrp,
                    IsPriceListReference = model.IsPriceListReference,
                    BotanicalName = model.BotanicalName,
                    QCCategoryID = model.QCCategoryID,
                    PatentNo = model.PatentNo,
                    ConversionFactorPtoSecondary = (decimal)model.ConversionFactorPtoI,
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
                    ConversionFactorPurchaseToInventory = 1,
                    ConversionFactorSalesToInventory = 1

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
                itemBL.CreateItem(itemBO, ItemLocationList);
                return Json(new { Status = "success", data = model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetServiceItemList(DatatableModel Datatable)
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
                DatatableResultBO resultBO = itemBL.GetServiceItemList(ItemCategoryID, CodeHint, NameHint, ItemCategoryHint, SalesCategoryHint, AccountsCategoryHint, SortField, SortOrder, Offset, Limit);
                var result = new { draw = Datatable.Draw, recordsTotal = resultBO.recordsTotal, recordsFiltered = resultBO.recordsFiltered, data = resultBO.data };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = "failure", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

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

            return View(itemModel);
        }

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
            itemModel.GSTCategoryList = new SelectList(GSTCategoryBL.GetGSTCategoryList(), "ID", "Name");
            itemModel.LocationList = new SelectList(locationBL.GetLocationList(), "ID", "Name");
            return View(itemModel);
        }
    }
}