    using BusinessObject;
    using DataAccessLayer.DBContext;
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data.Entity.Core.Objects;

namespace DataAccessLayer
{
    public class ApprovalQueueDAL
    {
        public List<ApprovalQueueBO> GetApprovalQueues()    
        {   
            List<ApprovalQueueBO> item = new List<ApprovalQueueBO>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    item = dbEntity.SpGetApprovalQueue(GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ApprovalQueueBO
                    {
                        AppQueueID = k.AppQueueID,
                        QueueName = k.QueueName,
                        LocationID = (int)k.LocationID,
                        LocationName = k.LocationName

                    }).ToList();
                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApprovalQueueBO GetApprovalQueue(int ID)
        {
            ApprovalQueueBO item = new ApprovalQueueBO();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    item = dbEntity.SpGetApprovalQueueDetails(ID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(k => new ApprovalQueueBO()
                    {
                        AppQueueID=k.AppQueueID,
                        QueueName=k.QueueName,
                        LocationID=(int)k.LocationID,
                        LocationName=k.LocationName


                    }).FirstOrDefault();

                }
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApprovalQueueTransBO> GetApprovalQueueTrans(int ApprovalQueueID)
        {
            List<ApprovalQueueTransBO> items = new List<ApprovalQueueTransBO>();
            try
            {
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    items = dbEntity.SpGetApprovalQueueTrans(ApprovalQueueID, GeneralBO.LocationID, GeneralBO.ApplicationID).Select(a => new ApprovalQueueTransBO()
                    {
                        ID=a.ID,
                        ApprovalQueueID=(int)a.ApprovalQueueID,
                        UserID = a.UserID,
                        UserName = a.UserName,
                        SortOrder=(int)a.SortOrder


                    }).ToList();


                    return items;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }



      
        public int Save(ApprovalQueueBO ApprovalQueueItem, List<ApprovalQueueTransBO> ApprovalQueueItems)
        {
            try
            {

                ObjectParameter AQID = new ObjectParameter("ApprovalQueueID", typeof(int));
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    dbEntity.SpCreateApprovalQueue(
                            ApprovalQueueItem.QueueName,
                            Convert.ToDateTime(GeneralBO.FinStartDate),
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            AQID
                        );
                    foreach (var item in ApprovalQueueItems)
                    {
                        dbEntity.SpCreateApprovalQueueTrans(
                                Convert.ToInt16(AQID.Value.ToString()),
                                item.UserID,
                                item.SortOrder,
                                GeneralBO.CreatedUserID,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                            );
                    }

                };

                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update (ApprovalQueueBO ApprovalQueueItem, List<ApprovalQueueTransBO> ApprovalQueueItems)
        {
            try
            {

                //ObjectParameter AQID = new ObjectParameter("ApprovalQueueID", typeof(int));
                using (ApprovalEntities dbEntity = new ApprovalEntities())
                {
                    dbEntity.SpUpdateApprovalQueue(
                            ApprovalQueueItem.QueueName,
                            Convert.ToDateTime(GeneralBO.FinStartDate),
                            GeneralBO.CreatedUserID,
                            GeneralBO.LocationID,
                            GeneralBO.ApplicationID,
                            ApprovalQueueItem.AppQueueID
                        );
                    foreach (var item in ApprovalQueueItems)
                    {
                        dbEntity.SpCreateApprovalQueueTrans(
                                ApprovalQueueItem.AppQueueID,
                                item.UserID,
                                item.SortOrder,
                                GeneralBO.CreatedUserID,
                                GeneralBO.LocationID,
                                GeneralBO.ApplicationID
                            );
                    }

                };

                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
