using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;
using Sistrategia.SAT.Resources;
using Sistrategia.SAT.CFDiWebSite.CFDI;
using System.Web;

namespace Sistrategia.SAT.CFDiWebSite.Models
{
    public class CertificadoIndexViewModel
    {
        public CertificadoIndexViewModel() {
            this.Certificados = new List<Certificado>();
        }

        public List<Certificado> Certificados { get; set; }
    }

    public class CertificadoCreateViewModel
    {
        public CertificadoCreateViewModel() {
        }

        //[Required]
        //[Display(Name = "Número de serie")]
        //public string NumSerie { get; set; }

        //[Required]
        //[Display(Name = "R.F.C.")]
        //public string RFC { get; set; }

        //[Required]
        //[Display(Name = "Inicia")]
        //public DateTime Inicia { get; set; }

        //[Required]
        //[Display(Name = "Finaliza")]
        //public DateTime Finaliza { get; set; }

        //[Display(Name = "Certificado")]
        [Required, DataType(DataType.Upload), Display(Name = "Certificado")]
        public HttpPostedFileBase CertificadoArchivo { get; set; }
        //public string CertificadoBase64 { get; set; }

        //[Display(Name = "Archivo PFX")]
        //public byte[] PFXArchivo { get; set; }
        [Required, DataType(DataType.Upload), Display(Name = "Archivo PFX")]
        public HttpPostedFileBase PFXArchivo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña del PFX")] // [Display(Name = "Contraseña del Cert")]
        public string PFXContrasena { get; set; }

        //[Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        

        //[Required]
        //[Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        //public string Nombre { get; set; }
    }

    public class CertificadoDetailsViewModel
    {
        public CertificadoDetailsViewModel(Certificado certificado) {
            if (certificado == null)
                throw new ArgumentNullException("receptor");

            this.NumSerie = certificado.NumSerie;
            this.RFC = certificado.RFC;
            this.Inicia = certificado.Inicia;
            this.Finaliza = certificado.Finaliza;
            this.CertificadoBase64 = certificado.CertificadoBase64;
            // this.PFXArchivo = certificado.PFXArchivo;

            this.PFXContrasena = certificado.PFXContrasena;
            this.Estado = certificado.Estado;
            
            //if (receptor.RegimenFiscal != null && receptor.RegimenFiscal.Count > 0)
            //    this.RegimenFiscal = receptor.RegimenFiscal[0].Regimen;
        }

        [Required]
        [Display(Name = "Número de serie")]
        public string NumSerie { get; set; }

        [Required]
        [Display(Name = "R.F.C.")]
        public string RFC { get; set; }

        [Required]
        [Display(Name = "Inicia")]
        public DateTime Inicia { get; set; }

        [Required]
        [Display(Name = "Finaliza")]
        public DateTime Finaliza { get; set; }

        [Display(Name = "Certificado")]
        public string CertificadoBase64 { get; set; }

        ////[Display(Name = "Archivo PFX")]
        ////public byte[] PFXArchivo { get; set; }
        //[Required, DataType(DataType.Upload), Display(Name = "Archivo PFX")]
        //public HttpPostedFileBase PFXArchivo { get; set; }

        [Required]
        [Display(Name = "Contraseña del PFX")]
        public string PFXContrasena { get; set; }

        //[Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name="Issuer")]
        public string Issuer { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }
    }
}