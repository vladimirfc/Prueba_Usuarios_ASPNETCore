using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prueba_Usuarios_ASPNETCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Prueba_Usuarios_ASPNETCore.Data
{
    public class UsuariosDbContext : DbContext
    {
        public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<UsuarioRol>().HasKey(sc => new { sc.UsuarioId, sc.RolId });
        }
    }
}
