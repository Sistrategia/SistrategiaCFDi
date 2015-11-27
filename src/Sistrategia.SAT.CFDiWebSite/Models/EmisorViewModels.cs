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
    public class EmisorIndexViewModel
    {
        public EmisorIndexViewModel() {
            this.Emisores = new List<Emisor>();
        }

        public List<Emisor> Emisores { get; set; }
    }

    public class EmisorCreateViewModel
    {
        public EmisorCreateViewModel() {

        }

        public string RFC { get; set; }

        public string Nombre { get; set; }

        public string Calle { get; set; }

        public string NoExterior { get; set; }

        public string NoInterior { get; set; }

        public string Colonia { get; set; }

        public string Localidad { get; set; }

        public string Municipio { get; set; }

        public string Estado { get; set; }

        public string Pais { get; set; }

        public string CodigoPostal { get; set; }

        public string Referencias { get; set; }

        public string RegimenFiscal { get; set; }

        // public Emisor Emisor { get; set; }
    }
}