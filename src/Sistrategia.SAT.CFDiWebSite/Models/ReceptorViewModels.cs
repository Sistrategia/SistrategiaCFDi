using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;
using Sistrategia.SAT.Resources;
using Sistrategia.SAT.CFDiWebSite.CFDI;

namespace Sistrategia.SAT.CFDiWebSite.Models
{
    public class ReceptorIndexViewModel
    {
        public ReceptorIndexViewModel() {
            this.Receptores = new List<Receptor>();
        }

        public List<Receptor> Receptores { get; set; }
    }

    public class ReceptorCreateViewModel
    {
        public ReceptorCreateViewModel() {

        }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        //public string RegimenFiscal { get; set; }

        //public UbicacionFiscalViewModel DomicilioFiscal { get; set; }
        public UbicacionViewModel Domicilio { get; set; }

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
}