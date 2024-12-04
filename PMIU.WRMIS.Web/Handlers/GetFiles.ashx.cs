//using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIU.WRMIS.Common;
using System.IO;

namespace PMIU.WRMIS.Web.Handlers
{
    /// <summary>
    /// Summary description for GetFiles
    /// </summary>
    public class GetFiles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string moduleName = string.Empty;
            string filename = string.Empty;
            string pathToImage = string.Empty;
            try
            {
                filename = context.Request.Params[0].ToString();
                moduleName = context.Request.Params[1].ToString();
                pathToImage = Utility.GetImagePath(moduleName) + "/" + filename;

                if (!File.Exists(pathToImage))
                {
                    filename = "noimage.jpg";
                    pathToImage = "/Design/img/" + filename;
                }
            }
            catch (Exception)
            {
                filename = "noimage.jpg";
                pathToImage = "/Design/img/" + filename;
            }
            finally
            {
                context.Response.ContentType = "image/png";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(pathToImage);
                context.Response.Flush();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}