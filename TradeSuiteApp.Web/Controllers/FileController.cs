using BusinessLayer;
using BusinessObject;
using PresentationContractLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradeSuiteApp.Web.Controllers
{
    public class FileController : Controller
    {
        private IGeneralContract generalBL;
        private IFileContract fileBL;

        public FileController()
        {
            fileBL = new FileBL();
            generalBL = new GeneralBL();
        }
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            decimal MaxAllowedSize = Convert.ToDecimal(generalBL.GetConfig("MaxAllowedSize"));

            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "gif", "png", "csv" };
            string fileExt;
            string UniqueName;
            string FileName;
            string Name;
            // Checking no of files injected in Request object
            if (Request.Files.Count > 0)
            {
                try
                {
                    List<FileBO> Files = new List<FileBO>();
                    //  Get all files from Request object
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                        //string filename = Path.GetFileName(Request.Files[i].FileName);

                        HttpPostedFileBase file = files[i];


                        // Checking for Internet Explorer
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }
                        Name = FileName;
                        fileExt = System.IO.Path.GetExtension(FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExt.ToLower()))
                        {
                            Files.Add(new FileBO { ID = 0, Name = Name, Path = FileName, URL = "", Description = "File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT/Image File" });
                        }
                        else if (file.ContentLength / 1024 > MaxAllowedSize)
                        {
                            Files.Add(new FileBO { ID = 0, Name = Name, Path = FileName, URL = "", Description = "File size should be upto " + (MaxAllowedSize / 1024) + " MB" });
                        }
                        else
                        {
                            try
                            {
                                UniqueName = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + FileName;
                                // Get the complete folder path and store the file inside it.
                                FileName = Path.Combine(Server.MapPath("~/Uploads/"), UniqueName);
                                file.SaveAs(FileName);
                                //Saved file to DB
                                int Id = fileBL.SaveUploadedFilePath(UniqueName, FileName, GeneralBO.CreatedUserID);
                                Files.Add(new FileBO { ID = Id, Name = Name, Path = FileName, URL = "/Uploads/" + UniqueName, Description = "Uploaded Successfully" });
                            }
                            catch (Exception e)
                            {
                                Files.Add(new FileBO { ID = 0, Name = Name, Path = FileName, URL = "", Description = "Error occurred.Error details: " + e.Message });
                            }
                        }
                    }
                    var _out = new { Status = "Success", Data = Files };
                    return Json(_out, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    var _out = new { Status = "Failure", Message = "Error occurred.Error details: " + ex.Message };
                    // Returns message that successfully uploaded
                    return Json(_out, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var _out = new { Status = "Failure", Message = "Please Select some file to upload" };
                return Json(_out, JsonRequestBehavior.AllowGet);
            }
        }
    }
}