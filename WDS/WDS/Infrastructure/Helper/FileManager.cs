using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WDS.Infrastructure.Helper
{
    public class FileManager
    {
        public string UploadImage(HttpPostedFileBase file)
        {

            try
            {
                const string initialPath = @"~/Content/Uploads";
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var todayTime = DateTime.Now;
                        var time = new DateTime(todayTime.Year, todayTime.Month, todayTime.Day, todayTime.Hour,
                            todayTime.Minute, todayTime.Second, todayTime.Millisecond);
                        var timeFormat = string.Format("{0:yyyy-MM-dd_hh-mm-ss-fff}", time);
                        fileName = timeFormat + "-" + fileName;
                        var path = Path.Combine(HttpContext.Current.Server.MapPath(initialPath), fileName);
                        file.SaveAs(path);
                        //string finalPath = @"C:\uploads" + @"\" + userDirectory + @"\" + moduleDirectory;
                        //if (Directory.Exists(finalPath))
                        //{
                        //    File.Copy(path, finalPath + "\\" + fileName, true);
                        //}
                        //else
                        //{
                        //    Directory.CreateDirectory(finalPath);
                        //    File.Copy(path, finalPath + "\\" + fileName, true);
                        //}
                        return path;
                    }
                }
                return "no file found or null file uploaded";
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return msg;
            }

        }

        public string GetFile(string path)
        {
            const string webServerFilePath = @"../Content/Uploads";
            var fileName = Path.GetFileName(path);
            try
            {
                if (path != null)
                {
                    string webServerFileName = Path.Combine(HttpContext.Current.Server.MapPath(webServerFilePath), fileName);
                    if (!File.Exists(webServerFileName))
                    {
                        File.Copy(path, webServerFileName, true);
                    }
                }
                return webServerFilePath + "/" + fileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool DeleteFile(string path)
        {
            var webServerFilePath = @"../Content/Uploads";
#if DEBUG
             webServerFilePath = @"~/Content/Uploads";

#endif
            

            var fileName = Path.GetFileName(path);
            try
            {
                if (path == null) return false;
                string webServerFileName = Path.Combine(HttpContext.Current.Server.MapPath(webServerFilePath), fileName);
                if (!File.Exists(webServerFileName))
                    return true;

                File.Delete(webServerFileName);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}