using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Certificado
    {
        public Certificado()
            : base() {
            this.PublicKey = Guid.NewGuid();
        }

        [Key]
        public int CertificadoId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }        

        public string NumSerie { get; set; }
        public string RFC { get; set; }
        public DateTime Inicia { get; set; }
        public DateTime Finaliza { get; set; }

        public string CertificadoBase64 { get; set; }
        public byte[] PFXArchivo { get; set; }

        public string PFXContrasena { get; set; }
        public string Estado { get; set; }

        public string GetSello(string cadenaOriginal) {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(this.PFXArchivo,
                 this.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
            System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoIPT = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            byte[] binData = encoder.GetBytes(cadenaOriginal);
            byte[] binSignature = rsaCryptoIPT.SignData(binData, sha1);
            string sello = Convert.ToBase64String(binSignature);
            return sello;
        }
    }
}