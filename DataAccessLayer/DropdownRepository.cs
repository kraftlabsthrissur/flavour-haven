using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.DBContext;
using PresentationContractLayer;


namespace DataAccessLayer
{
    public class DropDownRepository : IDropdownContract
    {

        #region Purchase Requisition

        public List<ItemCategoryBO> GetItemCategoryList()
        {
            List<ItemCategoryBO> itm = new List<ItemCategoryBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetItemCategoryForPurchaseRequisition().Select(a => new ItemCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ItemCategoryBO> GetItemCategoryForService()
        {
            List<ItemCategoryBO> itm = new List<ItemCategoryBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    itm = dbEntity.SpGetItemCategoryForServices().Select(a => new ItemCategoryBO
                    {
                        ID = a.ID,
                        Name = a.Name
                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemBO> GetItemList(string hint, int ItemCategoryID = 0, int PurchaseCategoryID = 0)
        {
            List<ItemBO> itm = new List<ItemBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    var itemList = dbEntity.SpItemByItemANDPurchaseCategories(ItemCategoryID, PurchaseCategoryID, hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).ToList();
                    itm = itemList.Select(a => new ItemBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Code = a.Code,
                        RetailMRP = a.RetailMRP.HasValue ? a.RetailMRP.Value : 0,
                        RetailLooseRate = a.RetailLooseRate.HasValue ? a.RetailLooseRate.Value : 0,
                        VATPercentage = a.VATPercentage.HasValue ? a.VATPercentage.Value : 0,
                        PartsNumber = a.PartsNumber,
                        DeliveryTerm = a.Remark,
                        Model = a.Model,
                        Stock = a.Stock,
                        QtyUnderQC = a.QtyUnderQC,
                        Unit = a.PrimaryUnit,
                        UnitID = a.PrimaryUnitID,
                        PurchaseUnit = a.PurchaseUnit,
                        PurchaseUnitID = (int)a.PurchaseUnitID,
                        QtyOrdered = a.QtyOrdered,
                        GSTPercentage = a.GSTPercentage
                    }).ToList();
                    return itm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region ServicePurchaseRequisition

        public List<InterCompanyBO> GetInterCompanyList()
        {
            List<InterCompanyBO> itm = new List<InterCompanyBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    itm = dbEntity.SpGetInterCompany().Select(InterCompany => new InterCompanyBO
                    {
                        ID = InterCompany.ID,
                        Code = InterCompany.Code,
                        Name = InterCompany.Name,
                        Place = InterCompany.Place
                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ProjectBO> GetProjectList()
        {
            List<ProjectBO> itm = new List<ProjectBO>();
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    itm = dbEntity.SpGetProject().Select(ProjectList => new ProjectBO
                    {
                        ID = ProjectList.ID,
                        Code = ProjectList.Code,
                        Name = ProjectList.Name,
                        Place = ProjectList.Place,
                        Description = ProjectList.Description

                    }).ToList();
                }
                return itm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Purchase Order
        //Purchase order  



        public List<PurchaseOrderItemBO> GetPurchaseOrderItems(string hint, int SupplierID, int ItemCategoryID = 0, int PurchaseCategoryID = 0, int BusinessCategoryID = 0)
        {
            List<PurchaseOrderItemBO> obj = new List<PurchaseOrderItemBO>();
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpItemByItemANDPurchaseCategoriesForPurchaseOrder(ItemCategoryID, PurchaseCategoryID, BusinessCategoryID, SupplierID, hint, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new PurchaseOrderItemBO
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        UnitID = a.PrimaryUnitID,
                        Unit = a.PrimaryUnit,
                        LastPR = a.LastPR,
                        LowestPR = a.LowestPR,
                        PendingOrderQty = a.PendingOrderQty,
                        QtyUnderQC = a.QtyUnderQC,
                        QtyAvailable = a.QtyAvailable,
                        RequestedQty = a.RequestedQty,
                        OrderedQty = a.OrderedQty,
                        GSTCategoryID = a.GSTCategoryID,
                        GSTPercentage = (decimal)a.GSTPercentage,
                        FGCategoryID = a.FGCategoryID,
                        PurchaseUnit = a.PurchaseUnit,
                        PurchaseUnitID = (int)a.PurchaseUnitID

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ItemCategoryBO> GetTransactionTypeList()
        {
            List<ItemCategoryBO> itm = new List<ItemCategoryBO>();
            try
            {
                using (ReportsEntities RdbEntity = new ReportsEntities())
                {
                    itm = RdbEntity.SpGetTransactionType().Select(a => new ItemCategoryBO
                    {
                        Name = a.Name

                    }).ToList();
                }
                return itm;
            }
            catch (Exception)
            {

                throw;
            }
        }




        #endregion

        #region Treasury Details
        /// <summary>
        /// Get Bank details
        /// </summary>
        /// <returns></returns>
        public List<TreasuryDetailBO> GetBankDetails()
        {
            return GetTreasuryDetails("Bank");
        }
        /// <summary>
        /// Get TreasyryDetail by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<TreasuryDetailBO> GetTreasuryDetails(string type)
        {
            List<TreasuryDetailBO> treasuryDetailList = new List<TreasuryDetailBO>();
            using (AccountsEntities dbEntity = new AccountsEntities())
            {
                var treasuryDetails = dbEntity.SpGetTreasuryDetails(type);
                if (treasuryDetails != null)
                {
                    treasuryDetailList = (from td in treasuryDetails
                                          select new TreasuryDetailBO()
                                          {
                                              ID = td.ID,
                                              Type = td.Type,
                                              BankCode = td.BankCode,
                                              BankName = td.BankName,
                                              BranchName = td.BranchName,
                                              AccountNo = td.AccountNo,
                                              IFSCCode = td.IFSCCode,
                                              LocationMapping = td.LocationMapping
                                          }).ToList();
                }

            }
            return treasuryDetailList;
        }


        public List<PurchaseOrderBO> GetUnProcessedPurchaseOrderForGrn(int SupplierID)
        {
            try
            {
                using (PurchaseEntities dbEntity = new PurchaseEntities())
                {
                    return dbEntity.SpGetUnProcessedPurchaseOrder(SupplierID, GeneralBO.CreatedUserID, GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(m => new PurchaseOrderBO()
                    {
                        ID = m.ID,
                        PurchaseOrderNo = m.PurchaseOrderNo,
                        PurchaseOrderDate = m.PurchaseOrderDate,
                        PaymentWithin = m.PaymentWithInDays
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region TDS

        public List<TDSBO> GetTDS()
        {
            List<TDSBO> tdsBOList = new List<TDSBO>();
            using (MasterEntities dbEntity = new MasterEntities())
            {
                var tdsList = dbEntity.SpGetTDS().ToList();

                tdsBOList = tdsList.Select(x => new TDSBO()
                {
                    ID = x.ID,
                    Code = x.Code,
                    Description = string.Concat(x.Code, "-", x.Name, '-', /*x.CompanyType,*/ '-', x.Remarks),
                    Name = x.Name,
                    Rate = string.Concat(x.ID, "#", x.TDSRate),
                    Remarks = x.Remarks

                }).ToList();
            }
            return tdsBOList;
        }





        #endregion



    }
}
