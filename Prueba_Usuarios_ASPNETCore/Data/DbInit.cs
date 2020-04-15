using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prueba_Usuarios_ASPNETCore.Models;

namespace Prueba_Usuarios_ASPNETCore.Data
{
    public class DbInit
    {
        public static void Initialize(UsuariosDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.Add(new Rol { Nombre = "Administrador" });
            context.Roles.Add(new Rol { Nombre = "Trabajador" });
            context.Roles.Add(new Rol { Nombre = "Cliente" });

            context.SaveChanges();
        }
    }
}
