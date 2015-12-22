using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sistrategia.SAT.CFDiWebSite.Messaging;
using Sistrategia.SAT.CFDiWebSite.Data;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
//using MessagingToolkit.Barcode.QRCode;

namespace Sistrategia.SAT.CFDiWebSite
{
    public class SATManager
    {
        internal static TimeSpan GetCFDIServiceTimeSpan() {
            return new TimeSpan(0, 0, -5, 0);
        }

        internal static string GetFormaDePagoDefault() {
            return "PAGO EN UNA SOLA EXHIBICION";
        }

        internal static int GetDecimalPlacesDefault() {
            //return 6;
            return 2;
        }

        //internal static IFormatProvider GetDecimalFormatDefault() {
        //    return "0.000000";
        //}
        internal static string GetDecimalFormatDefault() {
            //return "0.000000";
            return "0.00";
        }

        public static string GetQrCode(string info) {
            //info = "?re=JOE110617QB7&rr=RSC940221A48&tt=34800.00&id=3EF99271-8206-414E-BA07-2DCAE3C722FE";
            string cbb = QrCodeModel.GenerateQrCode(info);
            return cbb;
        }

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


    internal class QrCodeModel
    {
        //Especificaciones de Código QR SAT
        //URL: https://google-developers.appspot.com/chart/infographics/docs/qr_codes
        //QR codes are squares, with an equal number of rows and columns. There are a fixed set of QR code sizes: from 21 to 177 rows/columns, increasing in steps of four. Each configuration is called a version. The more rows/columns, the more data the code can store. Here is a summary of the versions:

        //•	Version 1 has 21 rows and 21 columns, and can encode up to 25 alphanumeric characters
        //•	Version 2 has 25 rows and 25 columns, and can encode up to 47 alphanumeric characters
        //•	Version 3 has 29 rows and 29 columns, and can encode up to 77 alphanumeric characters
        //•	...
        //•	Version 40 has 177 rows and 177 columns, and can encode up to 4,296 alphanumeric characters

        //Como se genera el CBB del CFDi
        //URL: http://www.validacfd.com/phpbb3/viewtopic.php?f=24&t=3236#p22259

        //Decodificador de códigos QR, permite cargar una imagen y comprobarla para verificar el contenido del código QR, útil para validar si las salidas del generador de código que se creó,  pueden ser decodificadas por otros lectores externos.
        //URL: http://zxing.org/w/decode.jspx

        //QR Code Standardization
        //URL: http://www.qrcode.com/en/about/standards.html

        //Infromacion sobre las diferentes versiones de QR Code 
        //URL: http://www.qrcode.com/en/about/version.html

        //Librería usada para la generación del QR Code
        //URL: http://platform.twit88.com/projects/show/mt-barcode
        //URL: http://platform.twit88.com/

        public static string GenerateQrCode(string info) {
            try {
                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                encoder.QRCodeScale = 3;
                encoder.QRCodeVersion = 8;
                encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

                Bitmap img = encoder.Encode(info);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();

                //path = "C:\\Code\\QRCodeCreator\\QRCodeCreator\\QRCodeCreator\\img" + DateTime.Now.ToString("d_MM_yy_HH_mm_ss") + ".jpg";
                //img.Save(path, ImageFormat.Jpeg);

                string QR_Code = Convert.ToBase64String(byteImage);
                return QR_Code;
            }
            catch (Exception e) {
                return e.Message.ToString();
            }
        }
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