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
    public class Complemento
    {
        [Key]
        public int ComplementoId { get; set; }

        [XmlIgnore]
        public int? Ordinal { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        //private System.Xml.XmlElement[] any;

        //[XmlAnyElement]
        //public virtual List<System.Xml.XmlElement> Any { get; set; }
        ////public System.Xml.XmlElement[] Any {
        ////    get { return this.any; }
        ////    set { this.any = value; }
        ////}

        //[ForeignKey("TimbreFiscalDigital")]
        //public int? TimbreFiscalDigitalId { get; set; }

        //public virtual TimbreFiscalDigital TimbreFiscalDigital { get; set; }

        ///// <summary>
        ///// Nodo opcional para capturar los impuestos retenidos aplicables
        ///// </summary>
        //[XmlArrayItem("Retencion", IsNullable = false)]
        //public virtual List<Retencion> Retenciones { get; set; }
        ////public Retencion[] Retenciones {
        ////    get { return this.retenciones; }
        ////    set { this.retenciones = value; }
        ////}
    }

    //public class Complemento // : System.Xml.XmlDocument
    //{
    //    public int ComplementoId { get; set; }

    //    public virtual string Complemento { get; set; }

    //    //private System.Xml.XmlElement[] any;

    //    //[XmlAnyElement]
    //    //public virtual List<System.Xml.XmlElement> Any { get; set; }
    //    //public System.Xml.XmlElement[] Any {
    //    //    get { return this.any; }
    //    //    set { this.any = value; }
    //    //}
    //}
}