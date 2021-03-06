﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace QSistemaControle.Models
{
    public class ControleContext : DbContext
    {
        public ControleContext() : base("DefaultConnection")
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);    
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<QSistemaControle.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<QSistemaControle.Models.Grupo> Grupoes { get; set; }
    }
}