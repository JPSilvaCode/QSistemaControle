using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using QSistemaControle.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace QSistemaControle.Controllers.API
{
    [RoutePrefix("API/Grupo")]
    public class GrupoController : ApiController
    {
        private ControleContext db = new ControleContext();

        //CAPTURAR ESTUDANTES
        [HttpGet]
        [Route("GetEstudantes/{grupoId}")]
        public IHttpActionResult GetEstudantes(int grupoId)
        {
            var estudantes = db.GrupoDetalhes.Where(gd => gd.GrupoId == grupoId).ToList();
            var meuEstudantes = new List<object>();
            foreach (var estudante in estudantes)
            {
                var meuEstudante = db.Usuarios.Find(estudante.UserId);
                meuEstudantes.Add(new
                {
                    GrupoDetalhesId = estudante.GrupoDetalhesId,
                    GrupoId = estudante.GrupoId,
                    Estudante = meuEstudante
                });
            }

            return Ok(meuEstudantes);

        }

        //CAPTURAR NOTAS
        [HttpGet]
        [Route("GetNotas/{grupoId}/{userId}")]
        public IHttpActionResult GetNotas(int grupoId, int userId)
        {
            var notaDef = 0.0;
            var notas = db.GrupoDetalhes.Where(gd => gd.GrupoId == grupoId && gd.UserId == userId).ToList();
            foreach (var nota in notas)
            {
                foreach (var nota2 in nota.Notas)
                {
                    notaDef += nota2.Percentual + nota2.Nota;
                }
            }


            return Ok<object>(new { Notas = notaDef });

        }

        //METODO PARA NOTAS
        [HttpPost]
        [Route("SalvarNotas")]
        public IHttpActionResult SalvarNotas(JObject form)
        {
            var meuEstudanteResponse = JsonConvert.DeserializeObject<MeuEstudanteResponse>(form.ToString());
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var estudante in meuEstudanteResponse.Estudante)
                    {
                        var nota = new Notas
                        {
                            GrupoDetalhesId = estudante.GrupoDetalhesId,
                            Percentual = (float)meuEstudanteResponse.Porcentagem,
                            Nota = (float)estudante.Nota
                        };

                        db.Notas.Add(nota);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }

        [Route("GetGrupo/{userId}")]
        //GET Personalizado
        public IHttpActionResult GetGrupo(int userId)
        {
            var grupos = db.Grupoes.Where(g => g.UserId == userId).ToList();
            var objetos = db.GrupoDetalhes.Where(gd => gd.UserId == userId).ToList();
            var materias = new List<object>();

            foreach (var objeto in objetos)
            {
                var professor = db.Usuarios.Find(objeto.Grupo.UserId);

                materias.Add(new
                {
                    GrupoId = objeto.GrupoId,
                    Descricao = objeto.Grupo.Descricao,
                    professor = professor
                });
            }

            var resposta = new
            {
                MateriasProf = grupos,
                MatriculadoEm = materias
            };

            return Ok(resposta);
        }

        // GET: api/Grupo
        public IQueryable<Grupo> GetGrupoes()
        {
            return db.Grupoes;
        }

        // GET: api/Grupo/5
        //[ResponseType(typeof(Grupo))]
        //public IHttpActionResult GetGrupo(int id)
        //{
        //    Grupo grupo = db.Grupoes.Find(id);
        //    if (grupo == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(grupo);
        //}

        // PUT: api/Grupo/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrupo(int id, Grupo grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grupo.GrupoId)
            {
                return BadRequest();
            }

            db.Entry(grupo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Grupo
        [ResponseType(typeof(Grupo))]
        public IHttpActionResult PostGrupo(Grupo grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Grupoes.Add(grupo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = grupo.GrupoId }, grupo);
        }

        // DELETE: api/Grupo/5
        [ResponseType(typeof(Grupo))]
        public IHttpActionResult DeleteGrupo(int id)
        {
            Grupo grupo = db.Grupoes.Find(id);
            if (grupo == null)
            {
                return NotFound();
            }

            db.Grupoes.Remove(grupo);
            db.SaveChanges();

            return Ok(grupo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrupoExists(int id)
        {
            return db.Grupoes.Count(e => e.GrupoId == id) > 0;
        }
    }
}