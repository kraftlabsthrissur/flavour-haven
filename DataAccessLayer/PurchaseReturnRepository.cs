using BusinessObject;
using DataAccessLayer.DBContext;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class PurchaseReturnRepository : IPurchaseReturn
    {
        private readonly PurchaseEntities _entity;
        #region Constructor

        public PurchaseReturnRepository()
        {
            _entity = new PurchaseEntities();
        }

        #endregion

        /// <summary>
        /// Save purchase invoice details
        /// </summary>
        /// <param name="PurchaseReturnBO"></param>
        /// <returns></returns>
        public int SavePurchaseReturn(PurchaseReturnBO PurchaseReturnBO, int createdUserID, int finYear, int locationID, int applicationID)
        {

            int PurchaseReturnID = 0;
            if (PurchaseReturnBO != null && PurchaseReturnBO.Id <= 0)         //Create
                PurchaseReturnID = CreatePurchaseReturn(PurchaseReturnBO, createdUserID, finYear, locationID, applicationID);
            //else                                                                //Edit
            //    PurchaseReturnID = UpdatePurchaseReturn(PurchaseReturnBO, createdUserID, finYear, locationID, applicationID);

            return PurchaseReturnID;
        }

        /// <summary>
        /// Insert new Purchase Invoice and Purchase Invoice details
        /// </summary>
        /// <param name="PurchaseReturnBO"></param>
        /// <returns></returns>
        private int CreatePurchaseReturn(PurchaseReturnBO purchaseReturnBO, int createdUserID, int finYear, int locationID, int applicationID)
        {
            int purchaseReturnID = 0;
            if (purchaseReturnBO != null)
            {
                using (var transaction = _entity.Database.BeginTransaction())
                {
                    try
                    {
                        System.Data.Entity.Core.Objects.ObjectParameter purchaseReturnIDOut =
                            new System.Data.Entity.Core.Objects.ObjectParameter("PurchaseReturnID", typeof(int));
                        ObjectParameter RetValue = new ObjectParameter("RetValue", typeof(bool));

                        bool isDraft = purchaseReturnBO.IsDraft;
                        var i = _entity.SpCreatePurchaseReturn(purchaseReturnBO.ReturnNo,
                            purchaseReturnBO.ReturnDate,
                            purchaseReturnBO.SupplierID,
                            purchaseReturnBO.SGSTAmount,
                            purchaseReturnBO.CGSTAmount,
                            purchaseReturnBO.IGSTAmount,
                            0,
                            0,
                            0,
                            purchaseReturnBO.NetAmount,
                            purchaseReturnBO.IsDraft,
                            false,
                            purchaseReturnBO.ReturnDate,
                            GeneralBO.CreatedUserID,
                             purchaseReturnBO.ReturnDate,
                             GeneralBO.FinYear,
                             GeneralBO.LocationID,
                             GeneralBO.ApplicationID,
                             purchaseReturnID: purchaseReturnIDOut);


                        _entity.SaveChanges();

                        if (purchaseReturnIDOut.Value != null && Convert.ToInt32(purchaseReturnIDOut.Value) > 0)
                        {
                            purchaseReturnID = Convert.ToInt32(purchaseReturnIDOut.Value);

                            if (purchaseReturnBO.PurchaseReturnTrnasItemBOList != null)
                                foreach (var transItemBO in purchaseReturnBO.PurchaseReturnTrnasItemBOList)
                                {
                                    _entity.SpCreatePurchaseReturnTrans(purchaseReturnID,
                                        1,
                                        transItemBO.GRNID,
                                        transItemBO.ItemID,
                                        transItemBO.Quantity,
                                        0,
                                        transItemBO.Rate,
                                        transItemBO.SGSTPercent,
                                        transItemBO.CGSTPercent,
                                        transItemBO.IGSTPercent,
                                        transItemBO.SGSTAmount,
                                        transItemBO.CGSTAmount,
                                        transItemBO.IGSTAmount,
                                        transItemBO.Amount,
                                        0,
                                        transItemBO.SecondaryUnitSize,
                                        transItemBO.SecondaryUnit,
                                        transItemBO.SecondaryReturnQty,
                                        transItemBO.SecondaryRate,
                                        transItemBO.Remarks,
                                        finYear,
                                        locationID,
                                        applicationID,
                                        1,
                                    transItemBO.BatchTypeID,
                                    transItemBO.UnitID, 
                                    transItemBO.VATPercentage,
                                     transItemBO.VATAmount,
                                    1,RetValue);
                                }
                        };
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        purchaseReturnID = 0;
                        throw ex;
                    }
                }
            }
            return purchaseReturnID;
        }

    }
}
