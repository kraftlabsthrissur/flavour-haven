using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using PresentationContractLayer;
using DataAccessLayer;


namespace BusinessLayer
{
    public class FileBL : IFileContract
    {
        FileDAL fileDAL;

        public FileBL() {
            fileDAL = new FileDAL();
        } 
        public List<FileBO> GetAttachments(string ID)
        {
            return fileDAL.GetAttachments(ID);
        }
        public int SaveUploadedFilePath(string FileName, string FilePath, int? UserId)
        {
            return fileDAL.SaveUploadedFilePath(FileName, FilePath, UserId);
        }
    }
}
