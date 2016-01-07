using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Cancelacion
    {
        [Key]
        public int CancelacionId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        //[XmlElement(ElementName = "ack", IsNullable = true)]
        public string Ack { get; set; }

        //[XmlElement(ElementName = "text", IsNullable = true)]
        public string Text { get; set; }

        public virtual List<CancelacionUUIDComprobante> UUIDComprobantes { get; set; }

        //public List<CancelacionComprobante> CancelacionComprobantes { get; set; }

        //[XmlIgnore]
        public string CancelacionXmlResponseUrl { get; set; }
    }

    public class CancelacionUUIDComprobante
    {
        [Key]
        public int? CancelacionUUIDComprobanteId { get; set; }

        [ForeignKey("Cancelacion")]
        public int? CancelacionId { get; set; }

        public virtual Cancelacion Cancelacion { get; set; }
        
        public string UUID { get; set; }

        [ForeignKey("Comprobante")]
        public int? ComprobanteId { get; set; }

        public virtual Comprobante Comprobante { get; set; }
    }
}

 //Guid ComprobanteId { get; set; }
 //       string Ack { get; set; }
 //       string Text { get; set; }
 //       string[] UUIDs { get; set; }
 //       string XmlResponse { get; set; }