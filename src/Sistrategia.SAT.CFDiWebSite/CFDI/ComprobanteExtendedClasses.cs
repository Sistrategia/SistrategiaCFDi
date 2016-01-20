using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using System.Text;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class ViewTemplate
    {
        [Key]
        public int ViewTemplateId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public string CodeName { get; set; }
    }

    public class ReceptorCorreoEntrega
    {
        [Key]
        public int ReceptorCorreoEntregaId { get; set; }
        public string Correo { get; set; }
        //public string Nombre { get; set; }
    }
}