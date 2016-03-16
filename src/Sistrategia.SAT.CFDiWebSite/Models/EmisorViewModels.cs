using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;
using Sistrategia.SAT.Resources;
using Sistrategia.SAT.CFDiWebSite.CFDI;

namespace Sistrategia.SAT.CFDiWebSite.Models
{
    public class EmisorIndexViewModel
    {
        public EmisorIndexViewModel() {
            this.Emisores = new List<Emisor>();
        }

        public List<Emisor> Emisores { get; set; }

        public string GetQrCode(string qrValue, int size) {
            var barcodeWriter = new ZXing.BarcodeWriter {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = size, // height,
                    Width = size, // width,
                    Margin = 0, // margin
                }
            };
            using (var bitmap = barcodeWriter.Write(qrValue))
            using (var stream = new System.IO.MemoryStream()) {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
                return String.Format("data:image/gif;base64,{0}", 
                    Convert.ToBase64String(stream.ToArray()));
            }            
        }
    }

    public class UbicacionViewModel
    {
        public UbicacionViewModel() {
        }

        public UbicacionViewModel(Ubicacion ubicacion) {
            if (ubicacion != null) {
                this.UbicacionId = ubicacion.UbicacionId;
                this.PublicKey = ubicacion.PublicKey;
                this.Calle = ubicacion.Calle;
                this.NoExterior = ubicacion.NoExterior;
                this.NoInterior = ubicacion.NoInterior;
                this.Colonia = ubicacion.Colonia;
                this.Localidad = ubicacion.Localidad;
                this.Municipio = ubicacion.Municipio;
                this.Estado = ubicacion.Estado;
                this.Pais = ubicacion.Pais;
                this.CodigoPostal = ubicacion.CodigoPostal;
                this.Referencia = ubicacion.Referencia;
                this.Ordinal = ubicacion.Ordinal;
                //this.Status = ubicacion.Status;
            }
        }

        public int? UbicacionId { get; set; }
        public Guid? PublicKey { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string Calle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string NoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string NoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string Colonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string Municipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string Pais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string Referencia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "OrdinalField")]
        public int Ordinal { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "StatusField")]
        //public string Status { get; set; }

        public string ToHtml() {
            string result = "";

            if(!string.IsNullOrEmpty(this.Calle))
                result += this.Calle.Trim();

            if (!string.IsNullOrEmpty(this.NoExterior))
                result += " " + this.NoExterior.Trim();

            if (!string.IsNullOrEmpty(this.NoInterior))
                result += " " + this.NoInterior.Trim();

            if (!string.IsNullOrEmpty(this.Colonia))
                result += "<br />" + this.Colonia.Trim();

            if (!string.IsNullOrEmpty(this.Municipio))
                result += "<br />" + this.Municipio.Trim();

            if (!string.IsNullOrEmpty(this.Localidad))
                if (!this.Localidad.Equals(this.Municipio))
                result += ", " + this.Localidad.Trim();

            if (!string.IsNullOrEmpty(this.Estado))
                result += ", " + this.Estado.Trim();

            if (!string.IsNullOrEmpty(this.CodigoPostal))
                result += "<br />" + this.CodigoPostal.Trim(); // C.P.:

            if (!string.IsNullOrEmpty(this.Pais))
                result += ", " + this.Pais.Trim();

            return result.Trim();
        }
    }

    public class UbicacionFiscalViewModel
    {
        public UbicacionFiscalViewModel() {
        }

        public UbicacionFiscalViewModel(UbicacionFiscal ubicacion) {
            if (ubicacion != null) {
                this.UbicacionId = ubicacion.UbicacionId;
                this.PublicKey = ubicacion.PublicKey;
                this.Calle = ubicacion.Calle;
                this.NoExterior = ubicacion.NoExterior;
                this.NoInterior = ubicacion.NoInterior;
                this.Colonia = ubicacion.Colonia;
                this.Localidad = ubicacion.Localidad;
                this.Municipio = ubicacion.Municipio;
                this.Estado = ubicacion.Estado;
                this.Pais = ubicacion.Pais;
                this.CodigoPostal = ubicacion.CodigoPostal;
                this.Referencia = ubicacion.Referencia;
                this.Ordinal = ubicacion.Ordinal;
                //this.Status = ubicacion.Status;
            }
        }

        public int? UbicacionId { get; set; }
        public Guid? PublicKey { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string Calle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string NoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string NoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string Colonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string Municipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string Pais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string Referencia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "OrdinalField")]
        public int Ordinal { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "StatusField")]
        //public string Status { get; set; }
    }

    public class EmisorCreateViewModel
    {
        public EmisorCreateViewModel() {
            this.Status = "A";
            this.ViewTemplateId = 2;
        }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        public string RegimenFiscal { get; set; }

        public UbicacionFiscalViewModel DomicilioFiscal { get; set; }
        public UbicacionViewModel ExpedidoEn { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PhoneField")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "CifUrl")]
        public string CifUrl { get; set; }

        [Required]
        [Display(Name = "LogoUrl")]
        public string LogoUrl { get; set; }

        public int? ViewTemplateId { get; set; }

        [Display(Name = "LogoUrl")]
        public string Status { get; set; }

        //[Required]
        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        //public string Calle { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        //public string NoExterior { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        //public string NoInterior { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        //public string Colonia { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        //public string Localidad { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        //public string Municipio { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        //public string Estado { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        //public string Pais { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        //public string CodigoPostal { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        //public string Referencia { get; set; }


        


        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        //public string ExpedidoEnCalle { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        //public string ExpedidoEnNoExterior { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        //public string ExpedidoEnNoInterior { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        //public string ExpedidoEnColonia { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        //public string ExpedidoEnLocalidad { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        //public string ExpedidoEnMunicipio { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        //public string ExpedidoEnEstado { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        //public string ExpedidoEnPais { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        //public string ExpedidoEnCodigoPostal { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        //public string ExpedidoEnReferencia { get; set; }

        

        // public Emisor Emisor { get; set; }
    }

    public class EmisorDetailViewModel
    {
        public EmisorDetailViewModel() {

        }

        public EmisorDetailViewModel(Emisor emisor) {
        //public EmisorDetailViewModel(ComprobanteEmisor emisor) {
            if (emisor == null)
                throw new ArgumentNullException("emisor");

            this.PublicKey = emisor.PublicKey;

            this.RFC = emisor.RFC;
            this.Nombre = emisor.Nombre;
            //if (emisor.RegimenFiscal != null && emisor.RegimenFiscal.Count > 0)
            //    this.RegimenFiscal = emisor.RegimenFiscal[0].Regimen;
            if (emisor.RegimenFiscal != null && emisor.RegimenFiscal.Count > 0)
                foreach (var regimen in emisor.RegimenFiscal.OrderByDescending(x => x.RegimenFiscalId)) {
                    if ("A".Equals(regimen.Status, StringComparison.InvariantCultureIgnoreCase)) {
                        this.RegimenFiscal = regimen.Regimen;
                        break;
                    }
                }

            if (emisor.DomicilioFiscal != null) {
                this.Calle = emisor.DomicilioFiscal.Calle;
                this.NoExterior = emisor.DomicilioFiscal.NoExterior;
                this.NoInterior = emisor.DomicilioFiscal.NoInterior;
                this.Colonia = emisor.DomicilioFiscal.Colonia;
                this.Localidad = emisor.DomicilioFiscal.Localidad;
                this.Municipio = emisor.DomicilioFiscal.Municipio;
                this.Estado = emisor.DomicilioFiscal.Estado;
                this.Pais = emisor.DomicilioFiscal.Pais;
                this.CodigoPostal = emisor.DomicilioFiscal.CodigoPostal;
                this.Referencia = emisor.DomicilioFiscal.Referencia;
            }
            //this.RegimenFiscal = emisor.RegimenFiscal;

            //  this.EmisorLogoUrl = comprobante.Emisor.LogoUrl;
            //this.EmisorTelefono = comprobante.Emisor.Telefono;
            //this.EmisorCorreo = comprobante.Emisor.Correo;
            //this.EmisorCifUrl = comprobante.Emisor.CifUrl;

            this.Correo = emisor.Correo;
            this.Telefono = emisor.Telefono;
            this.CifUrl = emisor.CifUrl;
            this.LogoUrl = emisor.LogoUrl;

            this.ViewTemplateId = emisor.ViewTemplateId;
        }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")] //[Display(Name = "Correo")]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PhoneField")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "CifUrl")]
        public string CifUrl { get; set; }

        [Required]
        [Display(Name = "LogoUrl")]
        public string LogoUrl { get; set; }

        //[Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string Calle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string NoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string NoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string Colonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string Municipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string Pais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string Referencia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string ExpedidoEnCalle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string ExpedidoEnNoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string ExpedidoEnNoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string ExpedidoEnColonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string ExpedidoEnLocalidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string ExpedidoEnMunicipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string ExpedidoEnEstado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string ExpedidoEnPais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string ExpedidoEnCodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string ExpedidoEnReferencia { get; set; }


        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        public string RegimenFiscal { get; set; }

        public Guid PublicKey { get; set; }

        public int? ViewTemplateId { get; set; }
    }

    public class EmisorEditViewModel
    {
        public EmisorEditViewModel() {

        }

        public EmisorEditViewModel(Emisor emisor) {
            if (emisor == null)
                throw new ArgumentNullException("emisor");

            this.PublicKey = emisor.PublicKey;

            this.RFC = emisor.RFC;
            this.Nombre = emisor.Nombre;
            if (emisor.RegimenFiscal != null && emisor.RegimenFiscal.Count > 0)
                this.RegimenFiscal = emisor.RegimenFiscal[0].Regimen;

            if (emisor.DomicilioFiscal != null) {
                this.Calle = emisor.DomicilioFiscal.Calle;
                this.NoExterior = emisor.DomicilioFiscal.NoExterior;
                this.NoInterior = emisor.DomicilioFiscal.NoInterior;
                this.Colonia = emisor.DomicilioFiscal.Colonia;
                this.Localidad = emisor.DomicilioFiscal.Localidad;
                this.Municipio = emisor.DomicilioFiscal.Municipio;
                this.Estado = emisor.DomicilioFiscal.Estado;
                this.Pais = emisor.DomicilioFiscal.Pais;
                this.CodigoPostal = emisor.DomicilioFiscal.CodigoPostal;
                this.Referencia = emisor.DomicilioFiscal.Referencia;
            }
            //this.RegimenFiscal = emisor.RegimenFiscal;

            this.Correo = emisor.Correo;
            this.Telefono = emisor.Telefono;
            this.CifUrl = emisor.CifUrl;
            this.LogoUrl = emisor.LogoUrl;
            this.ViewTemplateId = emisor.ViewTemplateId;
        }

        public Guid PublicKey { get; set; }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Correo")]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        //[Required]
        [Display(Name = "CifUrl")]
        public string CifUrl { get; set; }

        //[Required]
        [Display(Name = "LogoUrl")]
        public string LogoUrl { get; set; }

        //[Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string Calle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string NoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string NoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string Colonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string Municipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string Pais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string Referencia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string ExpedidoEnCalle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string ExpedidoEnNoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string ExpedidoEnNoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string ExpedidoEnColonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string ExpedidoEnLocalidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string ExpedidoEnMunicipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string ExpedidoEnEstado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string ExpedidoEnPais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string ExpedidoEnCodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string ExpedidoEnReferencia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        public string RegimenFiscal { get; set; }

        public int? ViewTemplateId { get; set; }
    }
}