﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QSistemaControle.Models
{
    public class MeuEstudante
    {
        public int GrupoDetalhesId { get; set; }
        public int GrupoId { get; set; }
        public Usuario Estudante { get; set; }
        public double Nota { get; set; }
    }
}