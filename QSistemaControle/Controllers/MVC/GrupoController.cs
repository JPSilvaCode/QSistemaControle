using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QSistemaControle.Models;

namespace QSistemaControle.Controllers.MVC
{
    [Authorize(Roles = "Professor")]
    public class GrupoController : Controller
    {
        private ControleContext db = new ControleContext();

        //Add Estudantes
        public ActionResult AddEstudante(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }

            var view = new GrupoDetalhes
            {
                GrupoId = id.Value
            };

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Estudante).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");

            return View(view);
        }

        // GET: Grupo
        public ActionResult Index()
        {
            var grupoes = db.Grupoes.Include(g => g.Professor);
            return View(grupoes.ToList());
        }

        // GET: Grupo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // GET: Grupo/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View();
        }

        // POST: Grupo/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GrupoId,Descricao,UserId")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                db.Grupoes.Add(grupo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View(grupo);
        }

        // GET: Grupo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", grupo.UserId);
            return View(grupo);
        }

        // POST: Grupo/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrupoId,Descricao,UserId")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", grupo.UserId);
            return View(grupo);
        }

        // GET: Grupo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // POST: Grupo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grupo grupo = db.Grupoes.Find(id);
            db.Grupoes.Remove(grupo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
