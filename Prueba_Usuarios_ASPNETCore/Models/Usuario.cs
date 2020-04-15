﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Prueba_Usuarios_ASPNETCore.Models
{
    public class Usuario
    {
        private string password;

        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debes ingresar un nombre!")]
        [StringLength(50, MinimumLength = 3)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes ingresar un apellido!")]
        [StringLength(50, MinimumLength = 3)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debes ingresar un Email!")]
        [EmailAddress(ErrorMessage = "Debes ingresar un Email Válido!")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debes ingresar una contraseña!")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password {
            get
            {
                return password;
            }
            set
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var data = Encoding.UTF8.GetBytes(value);
                var sha1data = sha1.ComputeHash(data);
                password = Convert.ToBase64String(sha1data);
            }
        }

        [Required(ErrorMessage = "Debes ingresar la fecha de registro!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }
}
