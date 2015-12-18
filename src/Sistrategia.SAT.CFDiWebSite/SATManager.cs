using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sistrategia.SAT.CFDiWebSite.Messaging;
using Sistrategia.SAT.CFDiWebSite.Data;

namespace Sistrategia.SAT.CFDiWebSite
{
    public class SATManager
    {

        #region Utilities
        internal static string NormalizeWhiteSpace(string S) {
            if (string.IsNullOrEmpty(S))
                return S;

            string s = S.Trim();
            bool iswhite = false;
            // int iwhite;
            int sLength = s.Length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder(sLength);
            foreach (char c in s.ToCharArray()) {
                if (Char.IsWhiteSpace(c)) {
                    if (iswhite) {
                        //Continuing whitespace ignore it.
                        continue;
                    }
                    else {
                        //New WhiteSpace

                        //Replace whitespace with a single space.
                        sb.Append(" ");
                        //Set iswhite to True and any following whitespace will be ignored
                        iswhite = true;
                    }
                }
                else {
                    sb.Append(c.ToString());
                    //reset iswhitespace to false
                    iswhite = false;
                }
            }
            return sb.ToString();
        }

        #endregion

        


    }
}

#region Code To Review


//public class HomeController : Controller
//    {
//        // GET: Home
//        public ActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Index(FormCollection post, HttpPostedFileBase xml_file)
//        {
//            try
//            {
//                string invoicesPath = Server.MapPath(Url.Content("~/FileInvoices/"));
//                string invoiceFileName = DateTime.Now.ToString("yyyyMMddHmmss_comprobante");

//                // Guardar el comprobante con cadena y sello
//                string xmlFullPath = invoicesPath + invoiceFileName + "_send.xml";
//                xml_file.SaveAs(xmlFullPath);

//                // Comprimir y enviar al servicio web
//                Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
//                string saveToFilePath = invoicesPath + invoiceFileName + "_send.zip";
//                zip.AddFile(xmlFullPath, "");
//                zip.Save(saveToFilePath);

//                string filePath = saveToFilePath;
//                string responsePath = invoicesPath + invoiceFileName + "_response.zip";

//                //byte[] response = new byte[0]; 
                
//                byte[] response = GetCFDI(
//                        ConfigurationManager.AppSettings["EDICOM_user"],
//                        ConfigurationManager.AppSettings["EDICOM_password"],
//                        filePath, responsePath);                   
                
              
//                Ionic.Zip.ZipFile zipR = Ionic.Zip.ZipFile.Read(invoicesPath + invoiceFileName + "_response.zip");
//                zipR.ExtractAll(invoicesPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
//                zipR.Dispose();

//                TempData["success"] = "¡XML timbrado!";

//            }
//            catch (Exception ex)
//            {
//                TempData["error"] = ex.Message.ToString();
//            }

//            return View();
//        }

//        public static ICFDIService CreateCFDIService() {
//            return CreateCFDIService("cfdiServiceTest");
//        }

//        public static ICFDIService CreateCFDIService(string serviceName) {
//            string serviceClass = System.Configuration.ConfigurationManager.AppSettings[serviceName].ToString();

//            switch (serviceClass) {
//                case "Sistrategia.Server.SAT.EDICOM.CFDI.CFDiService":
//                    return new EDICOM.CFDI.CFDiService();                   
//                case "Sistrategia.Server.SAT.EDICOM.TestCFDI.CFDiService":
//                default:
//                    return new EDICOM.TestCFDI.CFDiService();                   
//            }
//        }

//        public static byte[] GetCFDI(string user, string password, byte[] file) {
//            ICFDIService service = CreateCFDIService(); // new EDICOM.TestCFDI.CFDiService();
//            return service.GetCFDI(user, password, file);
//        }

//        public static byte[] GetCFDI(string user, string password, string requestFilePath, string responseFilePath) {
//            ICFDIService service = new EDICOM.TestCFDI.CFDiService();

//            byte[] file = null;
//            byte[] response = null;

//            System.IO.FileStream fileStream = new System.IO.FileStream(requestFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
//            try {
//                int lenght = (int)fileStream.Length;
//                file = new byte[lenght];
//                int count;
//                int sum = 0;

//                while ((count = fileStream.Read(file, sum, lenght - sum)) > 0)
//                    sum += count;
//            }
//            finally {
//                fileStream.Close();
//            }

//            try {
//                response = GetCFDI(user, password, file);
//                // response = GetCFDI("USUARIO", "PASSWORD", file);
//                // Response.Write("Respuesta OK" + response.ToString());
//            }

//            catch (Exception ex) {
//                // Response.Write(ex.Message.ToString());
//                ex.Message.ToString();
//                throw;
//            }

//            if (response != null) {
//                System.IO.FileStream fileStream2 = System.IO.File.Create(responseFilePath);
//                fileStream2.Write(response, 0, response.Length);
//                fileStream2.Close();
//            }

//            return response;

//            //return service.GetCFDI(user, password, file);


//        }



//        //public static ICFDIService CreateCFDIService()
//        //{

//        //    return CreateCFDIService("cfdiServiceTest");
//        //    // return CreateCFDIService("Sistrategia.Server.SAT.EDICOM.TestCFDI.CFDiService");
//        //}

//        //public static ICFDIService CreateCFDIService(string serviceName)
//        //{
//        //    string serviceClass = System.Configuration.ConfigurationManager.AppSettings[serviceName].ToString();

//        //    switch (serviceClass)
//        //    {
//        //        case "Sistrategia.Server.SAT.EDICOM.CFDI.CFDiService":
//        //            return new Sistrategia.Server.SAT.EDICOM.CFDI.CFDiService();
//        //            break;
//        //        case "Sistrategia.Server.SAT.EDICOM.TestCFDI.CFDiService":
//        //        default:
//        //            return new Sistrategia.Server.SAT.EDICOM.TestCFDI.CFDiService();
//        //            break;
//        //    }
//        //}

//        //public static byte[] GetCFDI(string user, string password, string requestFilePath, string responseFilePath)
//        //{
//        //    Sistrategia.Server.SAT.CFDI.ICFDIService service = Sistrategia.Server.SAT.CFDI.CFDIServiceFactory.CreateCFDIService();

//        //    byte[] file = null;
//        //    byte[] response = null;

//        //    System.IO.FileStream fileStream = new System.IO.FileStream(requestFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
//        //    try
//        //    {
//        //        int lenght = (int)fileStream.Length;
//        //        file = new byte[lenght];
//        //        int count;
//        //        int sum = 0;

//        //        while ((count = fileStream.Read(file, sum, lenght - sum)) > 0)
//        //            sum += count;
//        //    }
//        //    finally
//        //    {
//        //        fileStream.Close();
//        //    }

//        //    try
//        //    {
//        //        response = Sistrategia.Server.SAT.SATManager.GetCFDI("USUARIO", "PASSWORD", file);
//        //        // Response.Write("Respuesta OK" + response.ToString());
//        //    }

//        //    catch (Exception ex)
//        //    {
//        //        // Response.Write(ex.Message.ToString());
//        //        ex.Message.ToString();
//        //        throw;
//        //    }

//        //    if (response != null)
//        //    {
//        //        System.IO.FileStream fileStream2 = System.IO.File.Create(responseFilePath);
//        //        fileStream2.Write(response, 0, response.Length);
//        //        fileStream2.Close();
//        //    }

//        //    return service.GetCFDI(user, password, file);


//        //}

//        //public static byte[] GetCFDI(string user, string password, byte[] file)
//        //{
//        //    Sistrategia.Server.SAT.CFDI.ICFDIService service = Sistrategia.Server.SAT.CFDI.CFDIServiceFactory.CreateCFDIService();
//        //    return service.GetCFDI(user, password, file);
//        //}
//    }
//}






//@{
//    ViewBag.Title = "Index";
//}


//    <div class="col-md-8 col-md-offset-2">
//        <div class="panel panel-default">
//            <div class="panel-heading"><h3 class="panel-title text-center">Nuevo timbrado</h3></div>
//            <div class="panel-body">
                
//                @if (TempData["error"] != null)
//                {
//                    @Html.Raw("<div class='alert alert-danger text-center'><span class='glyphicon glyphicon-warning-sign'></span> " + TempData["error"] + "</div>")
//                }
//                else if (TempData["success"] != null)
//                {
//                    @Html.Raw("<div class='alert alert-success text-center'>" + TempData["success"] + "</div>")
//                }
//                <form method="post" class="form-horizontal" role="form" enctype="multipart/form-data">                  

//                    <div class="form-group">
//                        <label for="xml_file" class="col-sm-3 control-label">Archivo XML *</label>
//                        <div class="col-sm-9">
//                            <input id="xml_file" type="file" name="xml_file" required />
//                        </div>
//                    </div>

//                    <div class="form-group">
//                        <div class="col-sm-offset-4 col-sm-10">                            
//                            <button type="submit" class="btn btn-primary">Timbrar</button>
//                        </div>
//                    </div>

//                </form>

//            </div>
//        </div>
//    </div>


#endregion