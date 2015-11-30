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

        public UbicacionViewModel Domicilio { get; set; }
    }

    public class ReceptorDetailViewModel
    {
        public ReceptorDetailViewModel() {
            this.Domicilio = new UbicacionViewModel();
        }

        public ReceptorDetailViewModel(Receptor receptor) {
            if (receptor == null)
                throw new ArgumentNullException("receptor");

            this.RFC = receptor.RFC;
            this.Nombre = receptor.Nombre;
            //if (receptor.RegimenFiscal != null && receptor.RegimenFiscal.Count > 0)
            //    this.RegimenFiscal = receptor.RegimenFiscal[0].Regimen;

            if (receptor.Domicilio != null) {
                this.Domicilio = new UbicacionViewModel(receptor.Domicilio);
            }
        }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        //public string RegimenFiscal { get; set; }

        public UbicacionViewModel Domicilio { get; set; }
    }
}