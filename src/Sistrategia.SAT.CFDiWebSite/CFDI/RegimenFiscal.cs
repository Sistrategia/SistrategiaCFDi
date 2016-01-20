// Copyright (c) JEOCSI SA DE CV. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class RegimenFiscal
    {
        public RegimenFiscal() {
            this.Status = "A";
        }

        [Key]
        public int RegimenFiscalId { get; set; }
        public string Regimen { get; set; }

        public int Ordinal { get; set; }

        //[XmlIgnore]
        public string Status { get; set; }
    }
}