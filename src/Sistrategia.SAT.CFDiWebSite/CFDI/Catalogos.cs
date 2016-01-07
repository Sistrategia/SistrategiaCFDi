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
    public class TipoMetodoDePago
    {
        [Key]
        public int? TipoMetodoDePagoId { get; set; }
        public string TipoMetodoDePagoValue { get; set; }
    }

    public class TipoImpuestoTraslado
    {
        [Key]
        public int? TipoImpuestoTrasladoId { get; set; }
        public string TipoImpuestoTrasladoValue { get; set; }
    }

    public class TipoImpuestoRetencion
    {
        [Key]
        public int? TipoImpuestoRetencionId { get; set; }
        public string TipoImpuestoRetencionValue { get; set; }
    }
}


