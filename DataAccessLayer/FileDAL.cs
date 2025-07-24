using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DBContext;

namespace DataAccessLayer
{
    public class FileDAL
    {
        public List<FileBO> GetAttachments(string ID)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                        return dbEntity.SpGetAttachments(ID).Select(a => new FileBO
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Path = a.Path,
                        URL = "/Uploads/" + a.Name
                    }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int SaveUploadedFilePath(string FileName, string FilePath, int? UserId)
        {
            try
            {
                using (MasterEntities dbEntity = new MasterEntities())
                {
                    System.Data.Entity.Core.Objects.ObjectParameter UploadID =
                       new System.Data.Entity.Core.Objects.ObjectParameter("UploadID", typeof(int));

                    var result = dbEntity.SpQuotationUpload(FileName, FilePath, UserId, UploadID);
                    return (int)UploadID.Value;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
