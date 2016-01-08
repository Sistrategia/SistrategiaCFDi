using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sistrategia.SAT.CFDiWebSite.Security;
using Sistrategia.SAT.CFDiWebSite.CFDI;


namespace Sistrategia.SAT.CFDiWebSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<SecurityUser, SecurityRole
        , int, SecurityUserLogin, SecurityUserRole, SecurityUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultDatabase") {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Emisor> Emisores { get; set; }
        public virtual DbSet<Ubicacion> Ubicaciones { get; set; }
        //public virtual DbSet<UbicacionFiscal> UbicacionesFiscales { get; set; }

        public virtual DbSet<Certificado> Certificados { get; set; }

        public virtual DbSet<Receptor> Receptores { get; set; }

        public virtual DbSet<Comprobante> Comprobantes { get; set; }

        public virtual DbSet<Cancelacion> Cancelaciones { get; set; }


        public virtual DbSet<TipoMetodoDePago> TiposMetodoDePago { get; set; }
        public virtual DbSet<TipoImpuestoTraslado> TiposImpuestoTraslado { get; set; }
        public virtual DbSet<TipoImpuestoRetencion> TiposImpuestoRetencion { get; set; }
        public virtual DbSet<TipoFormaDePago> TiposFormaDePago { get; set; }

        public virtual DbSet<Banco> Bancos { get; set; }

        public virtual DbSet<ViewTemplate> ViewTemplates { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
            var user = modelBuilder.Entity<SecurityUser>()
                .ToTable("security_user");
            user.Property(u => u.Id).HasColumnName("user_id")
                .HasColumnOrder(1);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnOrder(2)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_name_index") { IsUnique = true }));
            user.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_public_key_index") { IsUnique = true }));
            user.Property(p => p.FullName)
                .HasColumnName("full_name")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_full_name_index") { IsUnique = false }));
            user.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(256);
            user.Property(u => u.EmailConfirmed)
                .HasColumnName("email_confirmed");
            user.Property(u => u.PasswordHash)
                .HasColumnName("password_hash");
            user.Property(u => u.SecurityStamp)
                .HasColumnName("security_stamp");
            user.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number");
            user.Property(u => u.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed");
            user.Property(u => u.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled");
            user.Property(u => u.LockoutEndDateUtc)
                .HasColumnName("lockout_end_date_utc");
            user.Property(u => u.LockoutEnabled)
                .HasColumnName("lockout_enabled");
            user.Property(u => u.AccessFailedCount)
                .HasColumnName("access_failed_count");

            var role = modelBuilder.Entity<SecurityRole>()
               .ToTable("security_roles");
            role.Property(r => r.Id).HasColumnName("role_id");
            role.Property(r => r.Name)
                .HasColumnName("role_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_role_name_index") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);

            var userRole = modelBuilder.Entity<SecurityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("security_user_roles");
            userRole.Property(pr1 => pr1.RoleId).HasColumnName("role_id");
            userRole.Property(pr2 => pr2.UserId).HasColumnName("user_id");

            var userLogin = modelBuilder.Entity<SecurityUserLogin>()
                 .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                 .ToTable("security_user_logins");
            userLogin.Property(pr1 => pr1.LoginProvider).HasColumnName("login_provider");
            userLogin.Property(pr2 => pr2.ProviderKey).HasColumnName("provider_key");
            userLogin.Property(pr3 => pr3.UserId).HasColumnName("user_id");

            var userClaim = modelBuilder.Entity<SecurityUserClaim>()
                .ToTable("security_user_claims");
            userClaim.Property(pr1 => pr1.Id).HasColumnName("claim_id");
            userClaim.Property(pr2 => pr2.UserId).HasColumnName("user_id");
            userClaim.Property(pr3 => pr3.ClaimType).HasColumnName("claim_type");
            userClaim.Property(pr4 => pr4.ClaimValue).HasColumnName("claim_value");

        //      public virtual DbSet<TipoMetodoDePago> TiposMetodoDePago { get; set; }
        //public virtual DbSet<TipoImpuestoTraslado> TiposImpuestoTraslado { get; set; }
        //public virtual DbSet<TipoImpuestoRetencion> TiposImpuestoRetencion { get; set; }

        var tipoMetodoDePago = modelBuilder.Entity<TipoMetodoDePago>()
            .ToTable("sat_tipo_metodo_de_pago");
            tipoMetodoDePago.Property(p => p.TipoMetodoDePagoId)
                .HasColumnName("tipo_metodo_de_pago_id");
            tipoMetodoDePago.Property(p => p.TipoMetodoDePagoValue)
            .HasColumnName("tipo_metodo_de_pago_value");

        var tipoImpuestoRetencion = modelBuilder.Entity<TipoImpuestoRetencion>()
            .ToTable("sat_tipo_impuesto_retencion");
            tipoImpuestoRetencion.Property(p => p.TipoImpuestoRetencionId)
                .HasColumnName("tipo_impuesto_retencion_id");
            tipoImpuestoRetencion.Property(p => p.TipoImpuestoRetencionValue)
            .HasColumnName("tipo_impuesto_retencion_value");

        var tipoImpuestoTraslado = modelBuilder.Entity<TipoImpuestoTraslado>()
            .ToTable("sat_tipo_impuesto_traslado");
            tipoImpuestoTraslado.Property(p => p.TipoImpuestoTrasladoId)
                .HasColumnName("tipo_impuesto_traslado_id");
            tipoImpuestoTraslado.Property(p => p.TipoImpuestoTrasladoValue)
            .HasColumnName("tipo_impuesto_traslado_value");

            var tipoFormaDePago = modelBuilder.Entity<TipoFormaDePago>()
                .ToTable("sat_tipo_forma_de_pago");
            tipoFormaDePago.Property(p => p.TipoFormaDePagoId)
                .HasColumnName("tipo_forma_de_pago_id");
            tipoFormaDePago.Property(p => p.TipoFormaDePagoValue)
                .HasColumnName("tipo_forma_de_pago_value");

        var banco = modelBuilder.Entity<Banco>()
            .ToTable("sat_banco");
        banco.Property(p => p.BancoId)
            .HasColumnName("banco_id");
        banco.Property(p => p.PublicKey)
            .HasColumnName("public_key");
        banco.Property(p => p.Clave)
            .HasColumnName("clave");
        banco.Property(p => p.NombreCorto)
            .HasColumnName("nombre_corto");
        banco.Property(p => p.RazonSocial)
            .HasColumnName("razon_social");
        banco.Property(p => p.Status)
            .HasColumnName("status");


    //        public class Bank
    //{
    //    [Key]
    //    public int? BankId { get; set; }
    //    [Required]
    //    public Guid PublicKey { get; set; }
    //    [Required]
    //    public string Clave { get; set; }
    //    [Required]
    //    public string NombreCorto { get; set; }
    //    [Required]
    //    public string RazonSocial { get; set; }        
    //    [Required]
    //    public string Status { get; set; }
    //}

            var emisor = modelBuilder.Entity<Emisor>()
                .ToTable("sat_emisor");
            emisor.Property(p => p.EmisorId)
                .HasColumnName("emisor_id");
            emisor.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            emisor.Property(p => p.RFC)
                .HasColumnName("rfc");
            emisor.Property(p => p.Nombre)
                .HasColumnName("nombre");

            emisor.Property(p => p.DomicilioFiscalId)
                .HasColumnName("domicilio_fiscal_id");
            emisor.Property(p => p.ExpedidoEnId)
                .HasColumnName("expedido_en_id");

            emisor.Property(p => p.Telefono)
                .HasColumnName("telefono");
            emisor.Property(p => p.Correo)
                .HasColumnName("correo");
            emisor.Property(p => p.LogoUrl)
                .HasColumnName("logo_url");
            emisor.Property(p => p.CifUrl)
                .HasColumnName("cif_url");

            emisor.Property(p => p.Status)
                .HasColumnName("status")
                .HasMaxLength(50);

            var ubicacion = modelBuilder.Entity<Ubicacion>()
                // .ToTable("sat_ubicacion");
                ;
            ubicacion.Property(p => p.UbicacionId)
                .HasColumnName("ubicacion_id");
            ubicacion.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            ubicacion.Property(p => p.Calle)
                .HasColumnName("calle")
                .IsOptional();
            ubicacion.Property(p => p.NoExterior)
                .HasColumnName("no_exterior")
                .IsOptional();
            ubicacion.Property(p => p.NoInterior)
                .HasColumnName("no_interior")
                .IsOptional();
            ubicacion.Property(p => p.Colonia)
                .HasColumnName("colonia")
                .IsOptional();
            ubicacion.Property(p => p.Localidad)
                .HasColumnName("localidad")
                .IsOptional();
            ubicacion.Property(p => p.Referencia)
                .HasColumnName("referencia")
                .IsOptional();
            ubicacion.Property(p => p.Municipio)
                .HasColumnName("municipio")
                .IsOptional();
            ubicacion.Property(p => p.Estado)
                .HasColumnName("estado")
                .IsOptional();
            ubicacion.Property(p => p.Pais)
                .HasColumnName("pais")
                .IsRequired();
            ubicacion.Property(p => p.CodigoPostal)
                .HasColumnName("codigo_postal")
                .IsOptional();

            ubicacion.Property(p => p.LugarExpedicion)
                .HasColumnName("lugar_expedicion")
                .IsOptional() // .IsRequired() // Requerido
                .HasMaxLength(2048);


            modelBuilder.Entity<Ubicacion>()
                .Map<Ubicacion>(m => m.Requires("ubicacion_type").HasValue("Ubicacion"))
                .Map<UbicacionFiscal>(m => m.Requires("ubicacion_type").HasValue("UbicacionFiscal"))
                .ToTable("sat_ubicacion");

            //modelBuilder.Entity<UbicacionFiscal>().Map(m =>
            //{
            //    m.MapInheritedProperties();
            //    m.ToTable("UbicacionFiscal");
            //});

            //var ubicacionFiscal = modelBuilder.Entity<UbicacionFiscal>()
            //    .ToTable("sat_ubicacion_fiscal");
            //ubicacionFiscal.Property(p => p.UbicacionId)
            //    .HasColumnName("ubicacion_fiscal_id");
            //ubicacionFiscal.Property(p => p.PublicKey)
            //    .HasColumnName("public_key")
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            //ubicacionFiscal.Property(p => p.Calle)
            //    .HasColumnName("calle");
            //ubicacionFiscal.Property(p => p.NoExterior)
            //    .HasColumnName("no_exterior");
            //ubicacionFiscal.Property(p => p.NoInterior)
            //    .HasColumnName("no_interior");
            //ubicacionFiscal.Property(p => p.Colonia)
            //    .HasColumnName("colonia");
            //ubicacionFiscal.Property(p => p.Localidad)
            //    .HasColumnName("localidad");
            //ubicacionFiscal.Property(p => p.Referencia)
            //    .HasColumnName("referencia");
            //ubicacionFiscal.Property(p => p.Municipio)
            //    .HasColumnName("municipio");
            //ubicacionFiscal.Property(p => p.Estado)
            //    .HasColumnName("estado");
            //ubicacionFiscal.Property(p => p.Pais)
            //    .HasColumnName("pais");
            //ubicacionFiscal.Property(p => p.CodigoPostal)
            //    .HasColumnName("codigo_postal");

            var certificado = modelBuilder.Entity<Certificado>()
                .ToTable("sat_certificado");
            certificado.Property(p => p.CertificadoId)
                .HasColumnName("certificado_id");
            certificado.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            certificado.Property(p => p.NumSerie)
                .HasColumnName("num_serie")
                .IsRequired()
                .HasMaxLength(20);
            certificado.Property(p => p.RFC)
                .HasColumnName("rfc")
                .IsRequired()
                .HasMaxLength(13);
            certificado.Property(p => p.Inicia)
                .HasColumnName("inicia")
                .IsRequired();
            certificado.Property(p => p.Finaliza)
                .HasColumnName("finaliza")
                .IsRequired();
            certificado.Property(p => p.CertificadoBase64)
                .HasColumnName("certificado")
                //.HasMaxLength(20)
                .IsRequired();
            certificado.Property(p => p.PFXArchivo)
                .HasColumnName("pfx_archivo");
            certificado.Property(p => p.PFXContrasena)
                .HasColumnName("pfx_contrasena")
                .HasMaxLength(256);

            certificado.Property(p => p.CertificadoDER)
                .HasColumnName("certificado_der");

            certificado.Property(p => p.PrivateKeyDER)
                .HasColumnName("private_key_der");

            certificado.Property(p => p.PrivateKeyContrasena)
                .HasColumnName("private_key_contrasena")
                .HasMaxLength(256);

            certificado.Property(p => p.Estado)
                .HasColumnName("estado")
                .IsRequired();
            certificado.Property(p => p.Ordinal)
               .HasColumnName("ordinal")          
               //.IsRequired();
               ;
            //certificado.Property(p => p.Status)
            //   .HasColumnName("status")
            //   .HasMaxLength(50);

            var regimenFiscal = modelBuilder.Entity<RegimenFiscal>()
                .ToTable("sat_regimen_fiscal");
            regimenFiscal.Property(p => p.RegimenFiscalId)
                .HasColumnName("regimen_fiscal_id");
            regimenFiscal.Property(p => p.Regimen)
                .HasColumnName("regimen");
            regimenFiscal.Property(p => p.Ordinal)
               .HasColumnName("ordinal");

            emisor.HasMany<Certificado>(p => p.Certificados)
                .WithOptional()
                .Map(pe => pe.MapKey("emisor_id"));

            emisor.HasMany<RegimenFiscal>(p => p.RegimenFiscal)
                .WithOptional()
                .Map(pe => pe.MapKey("emisor_id"));

            var receptor = modelBuilder.Entity<Receptor>()
                .ToTable("sat_receptor");
            receptor.Property(p => p.ReceptorId)
                .HasColumnName("receptor_id");
            receptor.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            receptor.Property(p => p.RFC)
                .HasColumnName("rfc");
            receptor.Property(p => p.Nombre)
                .HasColumnName("nombre");

            receptor.Property(p => p.DomicilioId)
                .HasColumnName("domicilio_id");

            receptor.Property(p => p.Status)
               .HasColumnName("status")
               .HasMaxLength(50);

            var comprobante = modelBuilder.Entity<Comprobante>()
                .ToTable("sat_comprobante");
            comprobante.Property(p => p.ComprobanteId)
                .HasColumnName("comprobante_id");
            comprobante.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            comprobante.Property(p => p.Version)
                .HasColumnName("version")
                .IsRequired()
                .HasMaxLength(20);
            comprobante.Property(p => p.Serie)
                .HasColumnName("serie")
                .IsOptional()
                .HasMaxLength(25);
            comprobante.Property(p => p.Folio)
                .HasColumnName("folio")
                .IsOptional()
                .HasMaxLength(20);
            comprobante.Property(p => p.Fecha)
                .HasColumnName("fecha")
                .IsRequired();
            comprobante.Property(p => p.Sello)
                .HasColumnName("sello")
                .IsOptional() // porque sino no se puede guardar el comprobante antes de sellarse.
                .HasMaxLength(2048);
            comprobante.Property(p => p.NoAprobacion)
                .HasColumnName("no_aprobacion")
                .IsOptional();
            comprobante.Property(p => p.AnoAprobacion)
                .HasColumnName("ano_aprobacion")
                .IsOptional();
            comprobante.Property(p => p.FormaDePago)
                .HasColumnName("forma_de_pago")
                .IsRequired() // DEFAULT 'PAGO EN UNA SOLA EXHIBICION'
                .HasMaxLength(256);
            //comprobante.Property(p => p.NoCertificado)
            //    .HasColumnName("no_certificado")
            //    .IsOptional()
            //    .HasMaxLength(20);
            //comprobante.Property(p => p.Certificado)
            //    .HasColumnName("certificado")
            //    .IsOptional()
            //    .HasMaxLength(2048);
            comprobante.Ignore(p => p.NoCertificado);
            comprobante.Ignore(p => p.CertificadoBase64);
            comprobante.Property(p => p.CertificadoId)
                .HasColumnName("certificado_id");
            comprobante.Property(p => p.HasNoCertificado)
                .HasColumnName("has_no_certificado");
                //.HasMaxLength(20)
                //.IsOptional()
            comprobante.Property(p => p.HasCertificado)
                .HasColumnName("has_certificado");
                //.IsOptional()
                //.HasMaxLength(2048);
            comprobante.Property(p => p.CondicionesDePago)
                .HasColumnName("condiciones_de_pago")
                .IsOptional()
                .HasMaxLength(2048);
            comprobante.Property(p => p.SubTotal)
                .HasColumnName("sub_total")
                .HasPrecision(18, 6)
                .IsRequired();
            comprobante.Property(p => p.Descuento)
                .HasColumnName("descuento")
                .HasPrecision(18, 6)
                .IsOptional();
            //comprobante.Ignore(p => p.DescuentoSpecified);
            comprobante.Property(p => p.MotivoDescuento)
                .HasColumnName("motivo_descuento")
                .IsOptional()
                .HasMaxLength(2048);
            comprobante.Property(p => p.TipoCambio)
                .HasColumnName("tipo_cambio")
                .IsOptional()
                .HasMaxLength(50);
            comprobante.Property(p => p.Moneda)
                .HasColumnName("moneda")
                .IsOptional()
                .HasMaxLength(50);
            comprobante.Property(p => p.Total)
                .HasColumnName("total")
                .HasPrecision(18, 6)
                .IsRequired();
            comprobante.Property(p => p.TipoDeComprobante)
                .HasColumnName("tipo_de_comprobante")
                .HasMaxLength(50)
                .IsRequired();
            comprobante.Property(p => p.MetodoDePago)
                .HasColumnName("metodo_de_pago")
                .IsOptional() // .IsRequired() // Requerido
                .HasMaxLength(256);
            comprobante.Property(p => p.LugarExpedicion)
                .HasColumnName("lugar_expedicion")
                .IsOptional() // .IsRequired() // Requerido
                .HasMaxLength(2048);
            comprobante.Property(p => p.NumCtaPago)
                .HasColumnName("num_cta_pago")
                .IsOptional()
                .HasMaxLength(256);
            comprobante.Property(p => p.FolioFiscalOrig)
                .HasColumnName("folio_fiscal_orig")
                .IsOptional()
                .HasMaxLength(256);
            comprobante.Property(p => p.SerieFolioFiscalOrig)
                .HasColumnName("serie_folio_fiscal_orig")
                .IsOptional()
                .HasMaxLength(256);
            comprobante.Property(p => p.FechaFolioFiscalOrig)
                .HasColumnName("fecha_folio_fiscal_orig")
                .IsOptional();
            //comprobante.Ignore(p => p.FechaFolioFiscalOrigSpecified);
            comprobante.Property(p => p.MontoFolioFiscalOrig)
                .HasColumnName("monto_folio_fiscal_orig")
                .HasPrecision(18, 6)
                .IsOptional();
            //comprobante.Ignore(p => p.MontoFolioFiscalOrigSpecified);
            comprobante.Property(p => p.EmisorId)
                .HasColumnName("emisor_id");
            comprobante.Property(p => p.ReceptorId)
                .HasColumnName("receptor_id");

            comprobante.Ignore(p => p.DecimalFormat);

            comprobante.Property(p => p.ExtendedIntValue1)
                .HasColumnName("extended_int_value_1");
            comprobante.Property(p => p.ExtendedIntValue2)
                .HasColumnName("extended_int_value_2");
            comprobante.Property(p => p.ExtendedIntValue3)
                .HasColumnName("extended_int_value_3");

            comprobante.Property(p => p.ExtendedStringValue1)
                .HasColumnName("extended_string_value_1");
            comprobante.Property(p => p.ExtendedStringValue2)
                .HasColumnName("extended_string_value_2");
            comprobante.Property(p => p.ExtendedStringValue3)
                .HasColumnName("extended_string_value_3");

            comprobante.Property(p => p.GeneratedCadenaOriginal)
                .HasColumnName("generated_cadena_original")
                //.HasMaxLength(2048)
                ;

            comprobante.Property(p => p.GeneratedXmlUrl)
                .HasColumnName("generated_xml_url")
                .HasMaxLength(1024);
            comprobante.Property(p => p.GeneratedPDFUrl)
                .HasColumnName("generated_pdf_url")
                .HasMaxLength(1024);

            comprobante.Property(p => p.Status)
                .HasColumnName("status")
                .HasMaxLength(50);

            var concepto = modelBuilder.Entity<Concepto>()
                .ToTable("sat_concepto");
            concepto.Property(p => p.ConceptoId)
                .HasColumnName("concepto_id");
            concepto.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            concepto.Property(p => p.Cantidad)
                .HasColumnName("cantidad")
                .HasPrecision(18, 6)
                .IsRequired();
            concepto.Property(p => p.Unidad)
                .HasColumnName("unidad")
                .HasMaxLength(50)
                .IsOptional();
            concepto.Property(p => p.NoIdentificacion)
                .HasColumnName("no_identificacion")
                .HasMaxLength(256)
                .IsOptional();
            concepto.Property(p => p.Descripcion)
                .HasColumnName("descripcion")
                //.HasMaxLength(2048)
                .IsOptional();
            concepto.Property(p => p.ValorUnitario)
                .HasColumnName("valor_unitario")
                .HasPrecision(18, 6)
                .IsRequired();
            concepto.Property(p => p.Importe)
                .HasColumnName("importe")
                .HasPrecision(18, 6)
                .IsRequired();
            concepto.Property(p => p.Ordinal)
                .HasColumnName("ordinal");

            comprobante.HasMany<Concepto>(p => p.Conceptos)
                .WithOptional()
                .Map(pe => pe.MapKey("comprobante_id"));

            //comprobante.HasMany<Concepto>(p => p.Conceptos)
            //    .WithOptional()
            //    .Map(pe => pe.MapKey("comprobante_id"));

            //var impuestos = modelBuilder.Entity<Impuestos>()
            //    .HasRequired(i => i.Comprobante)
            //    .WithOptional(c => c.Impuestos)

            var impuestos = modelBuilder.Entity<Impuestos>()
                .ToTable("sat_impuestos");

            impuestos.Property(p => p.ImpuestosId)
                .HasColumnName("impuestos_id");

            comprobante.Property(p => p.ImpuestosId)
                .HasColumnName("impuestos_id");

            comprobante.HasOptional(c => c.Impuestos);
            //<Impuestos>(p => p.Impuestos)
            //.WithRequired()
            //.Map(pe => pe.MapKey("impuestos_id"));

            var retencion = modelBuilder.Entity<Retencion>()
                .ToTable("sat_retencion");

            retencion.Property(p => p.RetencionId)
                .HasColumnName("retencion_id")
                ;
            retencion.Property(p => p.Impuesto)
                .HasColumnName("impuesto");
            retencion.Property(p => p.Importe)
                .HasColumnName("importe");

            retencion.Property(p => p.Ordinal)
                .HasColumnName("ordinal");

            impuestos.HasMany<Retencion>(p => p.Retenciones)
                .WithOptional()
                .Map(pe => pe.MapKey("impuesto_id"));

            var traslado = modelBuilder.Entity<Traslado>()
                .ToTable("sat_traslado");
            traslado.Property(p => p.TrasladoId)
                .HasColumnName("traslado_id")
                ;
            traslado.Property(p => p.Importe)
                .HasColumnName("importe");
            traslado.Property(p => p.Impuesto)
                .HasColumnName("impuesto");
            traslado.Property(p => p.Tasa)
                .HasColumnName("tasa");

            traslado.Property(p => p.Ordinal)
                .HasColumnName("ordinal");

            impuestos.HasMany<Traslado>(p => p.Traslados)
                .WithOptional()
                .Map(pe => pe.MapKey("impuesto_id"));

            var complemento = modelBuilder.Entity<Complemento>()
                .ToTable("sat_complemento");
            complemento.Property(p => p.ComplementoId)
                .HasColumnName("complemento_id");
            complemento.Property(p => p.Ordinal)
                .HasColumnName("ordinal");

            comprobante.HasMany<Complemento>(p => p.Complementos)
                .WithOptional()
                .Map(pe => pe.MapKey("comprobante_id"));


            var timbre = modelBuilder.Entity<TimbreFiscalDigital>()
                .ToTable("sat_timbre_fiscal_digital");
            //timbre.Property(p => p.TimbreFiscalDigitalId)
            //    .HasColumnName("timbre_fiscal_digital_id");
            //PublicKey
            timbre.Property(p => p.Version)
                .HasColumnName("version");
            timbre.Property(p => p.UUID)
                .HasColumnName("uuid");
            timbre.Property(p => p.FechaTimbrado)
                .HasColumnName("fecha_timbrado");
            timbre.Property(p => p.SelloCFD)
                .HasColumnName("sello_cfd");
            timbre.Property(p => p.NoCertificadoSAT)
                .HasColumnName("no_certificado_sat");
            timbre.Property(p => p.SelloSAT)
                .HasColumnName("sello_sat");


            var cancelacion = modelBuilder.Entity<Cancelacion>()
                .ToTable("sat_cancelacion");
            cancelacion.Property(p => p.CancelacionId)
                .HasColumnName("cancelacion_id");  
            cancelacion.Property(p => p.Ack)
                .HasColumnName("ack");  
            cancelacion.Property(p => p.Text)
                .HasColumnName("text");
            cancelacion.Property(p => p.CancelacionXmlResponseUrl)
                .HasColumnName("cancelacion_xml_response_url");

            var cancelacionUUIDComprobante = modelBuilder.Entity<CancelacionUUIDComprobante>()
                .ToTable("sat_cancelacion_uuid_comprobantes");
            cancelacionUUIDComprobante.Property(p => p.CancelacionUUIDComprobanteId)
                .HasColumnName("cancelacion_uuid_comprobantes_id");
            cancelacionUUIDComprobante.Property(p => p.UUID)
                .HasColumnName("uuid");
            cancelacionUUIDComprobante.Property(p => p.CancelacionId)
                .HasColumnName("cancelacion_id");
            
            cancelacionUUIDComprobante.Property(p => p.ComprobanteId)
                .HasColumnName("comprobante_id");

            //cancelacion.HasMany<CancelacionUUIDComprobante>(p => p.UUIDComprobantes)
            //    .WithOptional()
            //    .Map(pe => pe.MapKey("cancelacion_id"));
            //    ;

            cancelacionUUIDComprobante.HasOptional<Comprobante>(p => p.Comprobante)
                // .WithOptionalPrincipal
                ;

            var viewTemplate = modelBuilder.Entity<ViewTemplate>()
                .ToTable("ui_view_template");
            viewTemplate.Property(p => p.ViewTemplateId)
                .HasColumnName("view_template_id");
            viewTemplate.Property(p => p.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(256);
            viewTemplate.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(2048);
            viewTemplate.Property(p => p.CodeName)
                .HasColumnName("code_name")
                .HasMaxLength(128);

            comprobante.Property(p => p.ViewTemplateId)
                .HasColumnName("view_template_id");

            emisor.Property(p => p.ViewTemplateId)
                .HasColumnName("view_template_id");

            var receptorCorreoEntrega = modelBuilder.Entity<ReceptorCorreoEntrega>()
                .ToTable("sat_receptor_correo_entrega");

            receptorCorreoEntrega.Property(p => p.ReceptorCorreoEntregaId)
                .HasColumnName("receptor_correo_entrega_id");
            receptorCorreoEntrega.Property(p => p.Correo)
                .HasColumnName("correo")
                .HasMaxLength(256);

            comprobante.HasMany<ReceptorCorreoEntrega>(p => p.CorreosEntrega)
                .WithOptional()
                .Map(pe => pe.MapKey("comprobante_id"));
        }
                
    }
}