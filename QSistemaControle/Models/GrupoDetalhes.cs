using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QSistemaControle.Models
{
    public class GrupoDetalhes
    {
        [Key]
        public int GrupoDetalhesId { get; set; }

        public int GrupoId { get; set; }

        public int UserId { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual Usuario Estudante { get; set; }

        public string GrupoEstudante { get { return string.Format("{0} / {1}", Grupo.Descricao, Estudante.NomeCompleto); } }

        public virtual ICollection<Notas> Notas { get; set; }

    }
}