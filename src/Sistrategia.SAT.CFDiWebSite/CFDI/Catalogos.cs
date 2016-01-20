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
    public class TipoTipoDeComprobante
    {
        [Key]
        public int? TipoTipoDeComprobanteId { get; set; }
        public string TipoTipoDeComprobanteValue { get; set; }
    }

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

    public class TipoFormaDePago
    {
        [Key]
        public int? TipoFormaDePagoId { get; set; }
        public string TipoFormaDePagoValue { get; set; }
    }

    public class TipoMoneda
    {
        [Key]
        public int? TipoMonedaId { get; set; }
        public string TipoMonedaValue { get; set; }
    }

    public class Banco
    {
        [Key]
        public int? BancoId { get; set; }
        [Required]
        public Guid PublicKey { get; set; }
        [Required]
        public string Clave { get; set; }
        [Required]
        public string NombreCorto { get; set; }
        [Required]
        public string RazonSocial { get; set; }        
        [Required]
        public string Status { get; set; }
    }
}


