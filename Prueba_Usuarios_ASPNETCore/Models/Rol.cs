using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Usuarios_ASPNETCore.Models
{
    public class Rol
    {
        public int RolId { get; set; }

        public string Nombre { get; set; }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }
}
