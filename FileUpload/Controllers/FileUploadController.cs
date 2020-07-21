using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;

namespace FileUpload.Controllers
{
    public class FileUploadController : ApiController
    {
        [HttpPost()]
        public HttpResponseMessage UploadFiles()
        {
            HttpResponseMessage result;
            int iUploadedCnt = 0;         //Count of the Number of Uploaded Files
            var httpRequest = HttpContext.Current.Request;
            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                    {
                        System.Web.HttpPostedFile hpf = hfc[iCnt];

                        if (hpf !=null && hpf.ContentLength > 0)      //checks if a valid file has been uploaded or not
                        {
                            string filePath = httpRequest.Form["Main Path"] + httpRequest.Form["Sub Path"];   // This is used when we ant to save the file to a specific path
                            //var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);      // This is used when we ant to save the file to the current project directory

                            bool exists = System.IO.Directory.Exists(filePath);          //Checks to see if the Directory exists
                            if (!exists)                                                 //If not it creates a separate new Directory
                                System.IO.Directory.CreateDirectory(filePath);
                            filePath += hpf.FileName;
                            hpf.SaveAs(filePath);
                            docfiles.Add(filePath);
                            iUploadedCnt = iUploadedCnt + 1;         //Increment the Count per Save
                        }
                    }
                    string uploadedFiles = string.Join(",", docfiles);
                    result = Request.CreateResponse(HttpStatusCode.Created, "The following " + iUploadedCnt + " Files have been uploaded: " + uploadedFiles);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "File Not Uploaded!!");
                }
            }
            catch(Exception ex)
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest, "File Not Uploaded!!");
            }
            return result;
        }
    }
}
