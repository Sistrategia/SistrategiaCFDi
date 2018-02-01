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
    /// <summary>
    /// Nodo opcional para capturar los impuestos aplicables al presente concepto. Cuando un concepto 
    /// no registra un impuesto, implica que no es objeto del mismo.
    /// </summary>
    public class ConceptoImpuestos
    {
        #region
        //private decimal? totalImpuestosRetenidos;        
        //private decimal? totalImpuestosTrasladados;
        #endregion
        [Key]
        public int ImpuestosId { get; set; }

        /// <summary>
        /// Nodo opcional para asentar los impuestos trasladados aplicables al presente concepto.
        /// </summary>
        [XmlArrayItem("Traslado", IsNullable = false)]
        public virtual List<ConceptoImpuestosTraslado> Traslados { get; set; }

        /// <summary>
        /// Nodo condicional para capturar los impuestos retenidos aplicables. Es requerido cuando 
        /// en los conceptos se registre algún impuesto retenido.
        /// </summary>
        [XmlArrayItem("Retencion", IsNullable = false)]
        public virtual List<ConceptoImpuestosRetencion> Retenciones { get; set; }        
    }
}