using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationContractLayer
{
    public interface IFileContract
    {
        List<FileBO> GetAttachments(string ID);
        int SaveUploadedFilePath(string FileName, string FilePath, int? UserId);
    }
}
