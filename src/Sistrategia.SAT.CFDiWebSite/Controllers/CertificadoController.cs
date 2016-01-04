using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.CFDiWebSite.CFDI;
using System.IO;
using System.Text;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
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
               if (model.CertificadoArchivo == null || model.CertificadoArchivo.ContentLength == 0) {
                    return View();
                }
                try {
                    // var user = UserManager.FindById(this.GetUserId());
                    Guid publicKey = Guid.NewGuid();
                    Certificado certificado = new Certificado();
                    //certificado.NumSerie = model.NumSerie;
                    //certificado.RFC = model.RFC;
                    //certificado.Inicia = model.Inicia; // DateTime.Parse(post["inicia"].ToString(), new System.Globalization.CultureInfo("es-MX"));
                    //certificado.Finaliza = model.Finaliza;
                    ////certificado.CertificadoBase64 = model.CertificadoArchivo;
                    ////certificado.PFXArchivo = model.PFXArchivo;

                    certificado.PFXContrasena = model.PFXContrasena;
                    certificado.Estado = model.Estado;

                    if (model.CertificadoArchivo != null) {
                        MemoryStream target = new MemoryStream();
                        model.CertificadoArchivo.InputStream.CopyTo(target);
                        Byte[] data = target.ToArray();
                        certificado.CertificadoDER = data;
                        //certificado.PFXArchivo = data;
                        certificado.CertificadoBase64 = Convert.ToBase64String(data);

                        System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                        //System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(data, certificado.PFXContrasena);
                        System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(data);
                        // cert.FriendlyName.ToString();

                        certificado.NumSerie = Certificado.GetSerialNumberString(cert);
                        
                        //certificado.RFC = cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);
                        string[] subject = cert.Subject.Split(',');
                        foreach (string strVal in subject) {
                            string value = strVal.Trim();
                            if (value.StartsWith("OID.2.5.4.45=")) {
                                string value2 = value.Replace("OID.2.5.4.45=", "");
                                certificado.RFC = value2.Substring(0, value2.IndexOf('/') >= 0 ? value2.IndexOf('/') : value2.Length).Trim();
                            }
                        }

                        certificado.Inicia = DateTime.Parse(cert.GetEffectiveDateString());
                        certificado.Finaliza = DateTime.Parse(cert.GetExpirationDateString());
                        //certificado.CertificadoBase64 = model.CertificadoArchivo;
                        //certificado.PFXArchivo = model.PFXArchivo;

                    }

                    if (model.PFXArchivo != null) {
                        MemoryStream target2 = new MemoryStream();
                        model.PFXArchivo.InputStream.CopyTo(target2);
                        Byte[] dataPFX = target2.ToArray();
                        certificado.PFXArchivo = dataPFX;



                    //    MemoryStream target3 = new MemoryStream();
                    //    model.CertificadoArchivo.InputStream.Position = 0;
                    //    model.CertificadoArchivo.InputStream.CopyTo(target3);
                    //    Byte[] data3 = target3.ToArray();
                    //    //string certificadoBase64 = Convert.ToBase64String(data);

                        
                    //    //System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificado.PFXArchivo,
                    //    //     certificado.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
                    //    //System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoIPT = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;




                    }
                    //else {
                    //    MemoryStream target3 = new MemoryStream();
                    //    model.CertificadoArchivo.InputStream.CopyTo(target3);
                    //    Byte[] data3 = target3.ToArray();
                    //    //string certificadoBase64 = Convert.ToBase64String(data);

                    //    System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                    //    System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(data3, certificado.PFXContrasena);
                    //    cert.FriendlyName.ToString();
                    //    //System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificado.PFXArchivo,
                    //    //     certificado.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
                    //    System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoIPT = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;


                    //}

                    if (model.PrivateKeyDER != null) {
                        MemoryStream ms = new MemoryStream();
                        model.PrivateKeyDER.InputStream.CopyTo(ms);
                        Byte[] dataDER = ms.ToArray();
                        certificado.PrivateKeyDER = dataDER;
                    }
                    certificado.PrivateKeyContrasena = model.PrivateKeyContrasena;

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
            //System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificado.PFXArchivo,
            //     certificado.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
            //// System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoIPT = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;
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