﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_Usuarios_ASPNETCore.Models
{
    public class UsuarioRol
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}
