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
    public class ComprobanteIndexViewModel
    {
        public ComprobanteIndexViewModel() {
            this.Comprobantes = new List<Comprobante>();
        }

        public List<Comprobante> Comprobantes { get; set; }
    }
}