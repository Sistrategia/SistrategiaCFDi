using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.CFDiWebSite.CFDI;
using System.IO;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    public class CertificadoController : BaseController
    {
        // GET: Certificado
        public ActionResult Index()
        {
            var model = new CertificadoIndexViewModel {
                Certificados = this.DBContext.Certificados.ToList()
            };
            return View(model);
        }

        public ActionResult Create() {
            var model = new CertificadoCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CertificadoCreateViewModel model) {
            //var model = new CertificadoCreateViewModel();
           if (ModelState.IsValid) {
                if (model.PFXArchivo == null || model.PFXArchivo.ContentLength == 0) {
                    return View();
                }
                try {
                    // var user = UserManager.FindById(this.GetUserId());
                    Guid publicKey = Guid.NewGuid();
                    Certificado certificado = new Certificado();
                    certificado.NumSerie = model.NumSerie;
                    certificado.RFC = model.RFC;
                    certificado.Inicia = model.Inicia; // DateTime.Parse(post["inicia"].ToString(), new System.Globalization.CultureInfo("es-MX"));
                    certificado.Finaliza = model.Finaliza;
                    //certificado.CertificadoBase64 = model.CertificadoArchivo;
                    //certificado.PFXArchivo = model.PFXArchivo;

                    certificado.PFXContrasena = model.PFXContrasena;
                    certificado.Estado = model.Estado;

                    if (model.CertificadoArchivo != null) {
                        MemoryStream target = new MemoryStream();
                        model.CertificadoArchivo.InputStream.CopyTo(target);
                        Byte[] data = target.ToArray();
                        certificado.CertificadoBase64 = Convert.ToBase64String(data);
                    }

                    if (model.PFXArchivo != null) {
                        MemoryStream target2 = new MemoryStream();
                        model.PFXArchivo.InputStream.CopyTo(target2);
                        Byte[] dataPFX = target2.ToArray();
                        certificado.PFXArchivo = dataPFX;
                    }

                    this.DBContext.Certificados.Add(certificado);
                    this.DBContext.SaveChanges();
                }
                catch (Exception ex) {
                    //log.Error(ex, "Error upload photo blob to storage");
                    ex.ToString();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var certificado = DBContext.Certificados.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (certificado == null)
                return HttpNotFound();


            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificado.PFXArchivo,
                 certificado.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
            System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoIPT = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;

             

            //System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            //byte[] binData = encoder.GetBytes(cadenaOriginal);
            //byte[] binSignature = rsaCryptoIPT.SignData(binData, sha1);
            //string sello = Convert.ToBase64String(binSignature);
            //return sello;


            var model = new CertificadoDetailsViewModel(certificado);
            //model.Issuer = cert.Issuer;
            model.Issuer = cert.IssuerName.Name;
            model.Subject = cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);

            model.Issuer = cert.GetEffectiveDateString();
            model.Issuer = cert.GetExpirationDateString();
            model.Issuer = cert.Subject;
            model.Issuer = certificado.GetNumeroSerie(); //  cert.SerialNumber; // cert.GetSerialNumberString();
            model.Issuer = cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, true);
            //model.Issuer = cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);
            return View(model);
        }            
    }
}