using DatosPrueba.Model;
using DatosPrueba.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using practica.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practica.Controllers
{
    [Authorize]
    public class CuentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CuentasController(ApplicationDbContext context) 
        {
            _context = context;
        }
        private void Combox()
        {
            ViewData["CodigoSocio"] = new SelectList(_context.Socios.Select(x => new SocioCuenta
            {
                CedulaSocio = x.Cedula,
                NombreSocio = $"{x.Nombre}  {x.Apellido}",
                Estado = x.Estado

            }
               ).Where(D => D.Estado == 1).ToList(), "CedulaSocio", "NombreSocio"); ; ;

        }
        [Authorize(Roles = "Rey,Peon")]
        public ActionResult Index()
        {
            List<SocioCuenta> ltscuentas = new List<SocioCuenta>();

            ltscuentas = _context.Cuenta.Select(x => new SocioCuenta
            {
               NumeroCuenta=x.Numero,
               Saldo=x.SaldoTotal,
                NombreSocio = $"{x.CodigoSocioNavigation.Nombre}  {x.CodigoSocioNavigation.Apellido}",
                Estado = x.Estado
            }  ).ToList();

            return View(ltscuentas);
        }
        [Authorize(Roles = "Rey,Peon")]
        // GET: CuentasController/Details/5
        public ActionResult Details(String id)
        {
            Cuenta cuenta = _context.Cuenta.Where(D => D.Numero == id).FirstOrDefault();
            return View(cuenta);
        }
        [Authorize(Roles = "Rey")]
        // GET: CuentasController/Create
        public ActionResult Create()
        {
            Combox();
               return View();
        }
        [Authorize(Roles = "Rey")]
        // POST: CuentasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cuenta cuenta)
        {
            try
            {
                cuenta.Estado = 1;
                _context.Add(cuenta);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Rey")]
        // GET: CuentasController/Edit/5
        public ActionResult Edit(string id)
        {
            Cuenta cuenta = _context.Cuenta.Where(D => D.Numero == id).FirstOrDefault();
            Combox(); 
            return View(cuenta);
        }
        [Authorize(Roles = "Rey")]
        // POST: CuentasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Cuenta cuenta)
        {
            if (id==cuenta.CodigoSocio)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _context.Update(cuenta);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                Combox();
                return View();
            }
        }
        [Authorize(Roles = "Rey")]
        // GET: CuentasController/Delete/5
        public ActionResult Activar(string id)
        {
            Cuenta cuenta = _context.Cuenta.Where(D => D.Numero == id).FirstOrDefault();
            cuenta.Estado = 1;
            _context.Update(cuenta);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Rey")]
        public ActionResult Desactivar(string id)
        {
            Cuenta cuenta = _context.Cuenta.Where(D => D.Numero == id).FirstOrDefault();
            cuenta.Estado = 0;
            _context.Update(cuenta);
            _context.SaveChanges();
            return RedirectToAction("Index");
            
        }

    }
}
