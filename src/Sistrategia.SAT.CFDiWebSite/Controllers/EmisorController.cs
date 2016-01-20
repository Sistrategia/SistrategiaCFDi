using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.CFDiWebSite.CFDI;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class EmisorController : BaseController
    {
        public ActionResult Index() {
            //var model = new EmisorIndexViewModel {
            //    Emisores = this.DBContext.Emisores.Where(e=>e.Status == "A").ToList()
            //};
            //return View(model);
            return View(); 
        }

        [HttpPost]
        public JsonResult LoadEmisores(int page, int pageSize, string search = null, string sort = null, string sortDir = null) {
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            List<object> itemList = new List<object>();
            try {

                Func<Emisor, Object> orderByFunc = null;
                switch (sort) {
                    case "EmisorNombre":
                        orderByFunc = sl => sl.Nombre;
                        break;
                    case "RFC":
                        orderByFunc = sl => sl.RFC;
                        break;
                    default:
                        orderByFunc = sl => sl.Nombre;
                        break;
                }

                List<Emisor> Emisores = new List<Emisor>();
                if (search != null)
                    Emisores = sortDir == "ASC" ? DBContext.Emisores.Where(x => x.Status.Equals("A") && (x.Nombre.Contains(search)
                        || x.RFC.Contains(search))
                        ).OrderBy(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)
                        .Skip(((page - 1) * pageSize)).ToList()
                        :
                        DBContext.Emisores.Where(x => x.Status.Equals("A") && (x.Nombre.Contains(search)
                        || x.RFC.Contains(search))
                        ).OrderByDescending(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)
                        .Skip(((page - 1) * pageSize)).ToList();
                else
                    Emisores = sortDir == "ASC" ? DBContext.Emisores.Where(x => x.Status.Equals("A")).OrderBy(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList()
                        : DBContext.Emisores.Where(x => x.Status.Equals("A")).OrderByDescending(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList();

                if (Emisores.Count > 0) {
                    int EmisoresTotalRows = DBContext.Emisores.Where(x => x.Status.Equals("A") && (x.Nombre.Contains(search) || x.RFC.Contains(search))).Count();

                    foreach (Emisor emisor in Emisores) {
                        var dynamicItems = new {
                            error = false,
                            total_rows = EmisoresTotalRows,
                            returned_rows = Emisores.Count,
                            emisor_id = emisor.EmisorId,
                            public_key = emisor.PublicKey.ToString("N"),
                            emisor = emisor.Nombre,
                            rfc = emisor.RFC,
                            emisor_initial_letter = emisor.Nombre.Substring(0, 1),

                        };
                        itemList.Add(dynamicItems);
                    }
                }
            }
            catch (Exception ex) {
                var errorMessage = new {
                    error = true,
                    errorMsg = ex.ToString()
                };
                itemList.Add(errorMessage);
            }

            return Json(itemList);
        }

        public ActionResult Create() {
            var model = new EmisorCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EmisorCreateViewModel model) {
            //var model = new EmisorCreateViewModel();
            var emisor = new Emisor();

            emisor.Status = model.Status;
            emisor.RFC = model.RFC;

            if (!string.IsNullOrEmpty(model.Nombre))
                emisor.Nombre = model.Nombre;

            if (model.DomicilioFiscal != null) {
                if (!string.IsNullOrEmpty(model.DomicilioFiscal.Pais)) {
                    emisor.DomicilioFiscal = new UbicacionFiscal {
                        Pais = model.DomicilioFiscal.Pais,
                        Calle = string.IsNullOrEmpty(model.DomicilioFiscal.Calle) ? null : model.DomicilioFiscal.Calle,
                        NoExterior = string.IsNullOrEmpty(model.DomicilioFiscal.NoExterior) ? null : model.DomicilioFiscal.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.DomicilioFiscal.NoInterior) ? null : model.DomicilioFiscal.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.DomicilioFiscal.Colonia) ? null : model.DomicilioFiscal.Colonia,
                        Localidad = string.IsNullOrEmpty(model.DomicilioFiscal.Localidad) ? null : model.DomicilioFiscal.Localidad,
                        Municipio = string.IsNullOrEmpty(model.DomicilioFiscal.Municipio) ? null : model.DomicilioFiscal.Municipio,
                        Estado = string.IsNullOrEmpty(model.DomicilioFiscal.Estado) ? null : model.DomicilioFiscal.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.DomicilioFiscal.CodigoPostal) ? null : model.DomicilioFiscal.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.DomicilioFiscal.Referencia) ? null : model.DomicilioFiscal.Referencia,
                        Status = string.IsNullOrEmpty(model.DomicilioFiscal.Status) ? null : model.DomicilioFiscal.Status
                    };
                }
            }

            if (model.ExpedidoEn != null) {
                if (!string.IsNullOrEmpty(model.ExpedidoEn.Pais)) {
                    //emisor.ExpedidoEn = new Ubicacion {
                    emisor.ExpedidoEn = new List<Ubicacion>();
                    emisor.ExpedidoEn.Add(new Ubicacion {
                        Pais = model.ExpedidoEn.Pais,
                        Calle = string.IsNullOrEmpty(model.ExpedidoEn.Calle) ? null : model.ExpedidoEn.Calle,
                        NoExterior = string.IsNullOrEmpty(model.ExpedidoEn.NoExterior) ? null : model.ExpedidoEn.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.ExpedidoEn.NoInterior) ? null : model.ExpedidoEn.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.ExpedidoEn.Colonia) ? null : model.ExpedidoEn.Colonia,
                        Localidad = string.IsNullOrEmpty(model.ExpedidoEn.Localidad) ? null : model.ExpedidoEn.Localidad,
                        Municipio = string.IsNullOrEmpty(model.ExpedidoEn.Municipio) ? null : model.ExpedidoEn.Municipio,
                        Estado = string.IsNullOrEmpty(model.ExpedidoEn.Estado) ? null : model.ExpedidoEn.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.ExpedidoEn.CodigoPostal) ? null : model.ExpedidoEn.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.ExpedidoEn.Referencia) ? null : model.ExpedidoEn.Referencia
                    });
                }
            }            

            if (!string.IsNullOrEmpty(model.RegimenFiscal)) 
                emisor.RegimenFiscal = new List<RegimenFiscal> {
                    new RegimenFiscal {
                        Regimen = model.RegimenFiscal
                    }
                };

            emisor.Telefono = string.IsNullOrWhiteSpace(model.Telefono) ? null : model.Telefono;
            emisor.Correo = string.IsNullOrWhiteSpace(model.Correo) ? null : model.Correo;

            emisor.CifUrl = string.IsNullOrWhiteSpace(model.CifUrl) ? null : model.CifUrl;
            emisor.LogoUrl = string.IsNullOrWhiteSpace(model.LogoUrl) ? null : model.LogoUrl;

            //if (!string.IsNullOrEmpty(model.RegimenFiscal)) {
            //    if (emisor.RegimenFiscal == null)
            //        emisor.RegimenFiscal = new List<RegimenFiscal>();
            //    emisor.RegimenFiscal.Add(new RegimenFiscal {
            //        Regimen = model.RegimenFiscal
            //    });
            //}

            if (model.ViewTemplateId != null) {
                emisor.ViewTemplateId = model.ViewTemplateId;
                emisor.ViewTemplate = this.DBContext.ViewTemplates.Find(model.ViewTemplateId); // .Where(v => v.PublicKey == publicKey).SingleOrDefault();
            }

            this.DBContext.Emisores.Add(emisor);
            this.DBContext.SaveChanges();

            return RedirectToAction("Details", new { Id = emisor.PublicKey.ToString("N") }); // "Index", "Home");

            //return View(model);
        }

        public ActionResult Detail(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (emisor==null)
                return HttpNotFound();

            var model = new EmisorDetailViewModel(emisor);
            return View(model);
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey && e.Status == "A")
                .SingleOrDefault();

            if (emisor == null)
                return HttpNotFound();

            var model = new EmisorDetailViewModel(emisor);
            return View(model);
        }

        public ActionResult Edit(string id) {
            Guid publicKey = Guid.Parse(id);
            var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey && e.Status == "A")
                .SingleOrDefault();
            var model = new EmisorEditViewModel(emisor);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmisorEditViewModel model) {
            Guid publicKey = model.PublicKey; //  Guid.Parse(id);            
            if (ModelState.IsValid) {
                //var emisor = DBContext.Emisores.Where(e => e.PublicKey == model.PublicKey).SingleOrDefault();
                //if (emisor != null) {
                //    emisor.RFC = model.RFC;
                //    emisor.Nombre = model.Nombre;
                //    //emisor.RegimenFiscal
                //    emisor.Telefono = string.IsNullOrWhiteSpace(model.Telefono) ? null : model.Telefono;
                //    emisor.Correo = string.IsNullOrWhiteSpace(model.Correo) ? null : model.Correo;
                //    emisor.CifUrl = string.IsNullOrWhiteSpace(model.CifUrl) ? null : model.CifUrl;
                //    emisor.LogoUrl = string.IsNullOrWhiteSpace(model.LogoUrl) ? null : model.LogoUrl;
                //}
                //DBContext.SaveChanges();
                //return RedirectToAction("Index");
                var originalEmisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey && e.Status == "A")
                       .SingleOrDefault();
                var newEmisor = new Emisor();

                newEmisor.RFC = model.RFC;

                if (!string.IsNullOrEmpty(model.Nombre))
                    newEmisor.Nombre = model.Nombre;

                //if (model.DomicilioFiscal != null) {
                    if (!string.IsNullOrEmpty(model.Pais)) {
                        newEmisor.DomicilioFiscal = new UbicacionFiscal {
                            Pais = model.Pais,
                            Calle = string.IsNullOrEmpty(model.Calle) ? null : model.Calle,
                            NoExterior = string.IsNullOrEmpty(model.NoExterior) ? null : model.NoExterior,
                            NoInterior = string.IsNullOrEmpty(model.NoInterior) ? null : model.NoInterior,
                            Colonia = string.IsNullOrEmpty(model.Colonia) ? null : model.Colonia,
                            Localidad = string.IsNullOrEmpty(model.Localidad) ? null : model.Localidad,
                            Municipio = string.IsNullOrEmpty(model.Municipio) ? null : model.Municipio,
                            Estado = string.IsNullOrEmpty(model.Estado) ? null : model.Estado,
                            CodigoPostal = string.IsNullOrEmpty(model.CodigoPostal) ? null : model.CodigoPostal,
                            Referencia = string.IsNullOrEmpty(model.Referencia) ? null : model.Referencia
                        };
                    }
                //}

                //if (model.ExpedidoEn != null) {
                    if (!string.IsNullOrEmpty(model.ExpedidoEnPais)) {
                        //emisor.ExpedidoEn = new Ubicacion {
                        newEmisor.ExpedidoEn = new List<Ubicacion>();
                        newEmisor.ExpedidoEn.Add(new Ubicacion {
                            Pais = model.ExpedidoEnPais,
                            Calle = string.IsNullOrEmpty(model.ExpedidoEnCalle) ? null : model.ExpedidoEnCalle,
                            NoExterior = string.IsNullOrEmpty(model.ExpedidoEnNoExterior) ? null : model.ExpedidoEnNoExterior,
                            NoInterior = string.IsNullOrEmpty(model.ExpedidoEnNoInterior) ? null : model.ExpedidoEnNoInterior,
                            Colonia = string.IsNullOrEmpty(model.ExpedidoEnColonia) ? null : model.ExpedidoEnColonia,
                            Localidad = string.IsNullOrEmpty(model.ExpedidoEnLocalidad) ? null : model.ExpedidoEnLocalidad,
                            Municipio = string.IsNullOrEmpty(model.ExpedidoEnMunicipio) ? null : model.ExpedidoEnMunicipio,
                            Estado = string.IsNullOrEmpty(model.ExpedidoEnEstado) ? null : model.ExpedidoEnEstado,
                            CodigoPostal = string.IsNullOrEmpty(model.ExpedidoEnCodigoPostal) ? null : model.ExpedidoEnCodigoPostal,
                            Referencia = string.IsNullOrEmpty(model.ExpedidoEnReferencia) ? null : model.ExpedidoEnReferencia
                        });
                    }
                //}

                if (!string.IsNullOrEmpty(model.RegimenFiscal))
                    newEmisor.RegimenFiscal = new List<RegimenFiscal> {
                    new RegimenFiscal {
                        Regimen = model.RegimenFiscal
                    }
                };

                newEmisor.Telefono = string.IsNullOrWhiteSpace(model.Telefono) ? null : model.Telefono;
                newEmisor.Correo = string.IsNullOrWhiteSpace(model.Correo) ? null : model.Correo;

                newEmisor.CifUrl = string.IsNullOrWhiteSpace(model.CifUrl) ? null : model.CifUrl;
                newEmisor.LogoUrl = string.IsNullOrWhiteSpace(model.LogoUrl) ? null : model.LogoUrl;

                if (model.ViewTemplateId != null) {
                    newEmisor.ViewTemplateId = model.ViewTemplateId;
                    newEmisor.ViewTemplate = this.DBContext.ViewTemplates.Find(model.ViewTemplateId); // .Where(v => v.PublicKey == publicKey).SingleOrDefault();
                }

                originalEmisor.Status = "I";
                this.DBContext.Emisores.Add(newEmisor);
                this.DBContext.SaveChanges();

                return RedirectToAction("Details", new { Id = newEmisor.PublicKey.ToString("N") }); // "Index", "Home");

            }

            return View(model);
        }
    }
}