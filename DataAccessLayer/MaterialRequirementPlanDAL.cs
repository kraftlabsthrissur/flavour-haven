using BusinessObject;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class MaterialRequirementPlanDAL
    {
        
        public List<MaterialRequirementPlanItemBO> GetMaterialRequirmentPlanList(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    return dbEntity.SpGetMaterialRequirmentPlanList(fromDate, toDate, GeneralBO.FinYear,GeneralBO.LocationID,GeneralBO.ApplicationID).Select(a => new MaterialRequirementPlanItemBO()
                    {
                        ItemID = (int)a.ItemID,
                        RequiredQty =(decimal) a.RequiredQty,
                        ItemName = a.ItemName,
                        AvailableStock = (decimal)a.AvailableStock,
                        QtyInQC = (decimal)a.QtyInQC,
                        OrderedQty = (decimal)a.OrderedQty,
                        RequestedQty = (decimal)a.RequestedQty,
                        UnitID = a.UnitID,
                        RequiredDate =(DateTime) a.RequiredDate
                    }
                    ).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Save(MaterialRequirementPlanBO materialRequirementPlanBO, List<MaterialRequirementPlanItemBO> Items)
        {
            try
            {
                using (StockEntities dbEntity = new StockEntities())
                {
                    ObjectParameter SerialNo = new ObjectParameter("SerialNo", typeof(string));
                    ObjectParameter ReturnValue = new ObjectParameter("PurchaseRequisitionID", typeof(int));

                    var j = dbEntity.SpUpdateSerialNo("PurchaseRequisition", "Code", GeneralBO.FinYear, GeneralBO.LocationID, GeneralBO.ApplicationID, SerialNo);
                    dbEntity.SpCreateMaterialRequirementPlan(
                        SerialNo.Value.ToString(),
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID,
                        ReturnValue
                        );
                    foreach (var item in Items)
                    {
                        dbEntity.SpCreateMaterialRequirementPlanTrans(
                        Convert.ToInt32(ReturnValue.Value),
                        item.ItemID,
                        item.UnitID,
                        item.RequiredQty,
                        item.AvailableStock,
                        item.QtyInQC,
                        item.OrderedQty,
                        item.RequestedQty, 
                        item.RequiredDate,  
                        GeneralBO.CreatedUserID,
                        GeneralBO.FinYear,
                        GeneralBO.LocationID,
                        GeneralBO.ApplicationID
                            );
                    }
                    return Convert.ToInt32(ReturnValue.Value);
                };

                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
