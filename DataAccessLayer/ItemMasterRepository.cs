using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class ItemMasterRepository
    {

        public ItemMasterRepository()
        {

        }

        public int InsertItemMaster(ItemMasterBO itemBO)
        {
            try
            {

                //ItemMaster itemMaster = new ItemMaster()
                //{
                //    AyurwareCode = itemBO.AyurCode,
                //    BarCode = itemBO.BarCode,
                //    CategoryID = itemBO.CategoryID,
                //    CFormTaxID = itemBO.CFormTaxID,
                //    CFormTaxPer = itemBO.CFormPer,
                //    CommodityCode = string.Empty,
                //    CSTPer = itemBO.CSTPer,
                //    CSTTaxId = itemBO.CSTTaxID,
                //    Description = itemBO.Description,
                //    Discount = 0,
                //    DistributorID = 1,
                //    FullPurchasePrice = 90,
                //    FullRetailPrice = (decimal)itemBO.FullPurPrice,
                //    GroupID = null,
                //    ItemID = itemBO.ItemID,
                //    ItemName = itemBO.ItemName,
                //    LooseCF = (decimal)itemBO.LooseCF,
                //    LoosePurchasePrice = 12,
                //    LooseRetailPrice = 90,
                //    LooseUnitId = itemBO.LooseUnit,
                //    ManufacturerID = 123,
                //    MaxLevel = itemBO.MaxLevel,
                //    MinLevel = itemBO.MinLevel,
                //    MinLooseQty = (decimal)itemBO.MinLooseQty,
                //    MRP = (decimal)itemBO.MRP,
                //    RackNo = itemBO.RackNo,
                //    ReOrderLevel = itemBO.ReOrderLevel,
                //    TaxId = (int)itemBO.TaxId,
                //    TaxPer = (decimal)itemBO.TaxPer,
                //    UnitId = null,
                //    WholeSalePrice = (decimal)itemBO.WholeSalePrice,



                //};


                //using (TradeEntities entities = new TradeEntities())
                //{


                //    entities.ItemMasters.Add(itemMaster);
                //    entities.SaveChanges();
                //}



                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ItemMasterBO> GetItemMasterDetails()
        {
            try
            {
                return null;
                //using (TradeEntities entities = new TradeEntities())
                //{
                //    return entities.ItemMasters.Select(itemBO => new ItemMasterBO()
                //    {
                //        AyurCode = itemBO.AyurwareCode,
                //        BarCode = itemBO.BarCode,
                //        CategoryID = (int)itemBO.CategoryID,
                //        CFormTaxID = itemBO.CFormTaxID,
                //        CFormPer = itemBO.CFormTaxPer,
                //        CSTPer = itemBO.CSTPer,
                //        CSTTaxID = itemBO.CSTTaxId,
                //        Description = itemBO.Description,
                //        Discount = 0,
                //        DistributorID = 1,
                //        FullPurPrice = (double)itemBO.FullPurchasePrice,
                //        FullSellingPrice = (double)itemBO.FullRetailPrice,
                //        GroupID = (long)itemBO.GroupID,
                //        ItemID = itemBO.ItemID,
                //        ItemName = itemBO.ItemName,
                //        LooseCF = (double)itemBO.LooseCF,
                //        LPurPrice = 12,
                //        LooseSellingPrice = 90,
                //        LooseUnit = (short)itemBO.LooseUnitId,
                //        ManufacturerID = 123,
                //        MaxLevel = (long)itemBO.MaxLevel,
                //        MinLevel = (long)itemBO.MinLevel,
                //        MinLooseQty = (double)itemBO.MinLooseQty,
                //        MRP = (double)itemBO.MRP,
                //        RackNo = itemBO.RackNo,
                //        ReOrderLevel = (long)itemBO.ReOrderLevel,
                //        TaxId = (int)itemBO.TaxId,
                //        TaxPer = (double)itemBO.TaxPer,
                //        UnitID = (long)itemBO.UnitId,
                //        WholeSalePrice = (double)itemBO.WholeSalePrice,
                //    }).ToList();
                //};
            }
            catch (Exception)
            {

                throw;
            }

        }


        public int Getsalesdetails()
        {


            return 0;

        }

    }
}
