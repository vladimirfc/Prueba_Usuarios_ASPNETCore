using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prueba_Usuarios_ASPNETCore.Data;
using Prueba_Usuarios_ASPNETCore.Models;

namespace Prueba_Usuarios_ASPNETCore.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuariosDbContext _context;

        public UsuariosController(UsuariosDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string buscar)
        {
            ViewData["buscar"] = buscar;

            var usuarios = await _context.Usuarios
                            .Include(u => u.UsuarioRoles)
                                .ThenInclude(ur => ur.Rol)
                            .AsNoTracking()
                            .ToListAsync();

            if (!String.IsNullOrEmpty(buscar))
            {
                usuarios = usuarios.Where(u => u.NombreCompleto.ToLower().Contains(buscar.ToLower())
                                       || u.Email.ToLower().Contains(buscar)
                                       || string.Join(' ',
                                            u.UsuarioRoles
                                                .Select(ur => ur.Rol.Nombre.ToLower())
                                                .ToArray()).Contains(buscar)).ToList();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.roles = await _context.Roles.ToListAsync();
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Apellido,Email,Password,FechaRegistro")] Usuario usuario, int[] rol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                int id = usuario.UsuarioId;
                foreach (int r in rol)
                {
                    var usuarioRol = new UsuarioRol { RolId = r, UsuarioId = id };
                    _context.Add(usuarioRol);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.roles = await _context.Roles.ToListAsync();
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.roles = await _context.Roles.ToListAsync();
            //var usuarioUpdate = new UsuarioUpdate
            //{
            //    Nombre = usuario.Nombre,
            //    Apellido = usuario.Apellido,
            //    Email = usuario.Email,
            //    FechaRegistro = usuario.FechaRegistro,
            //    UsuarioRoles = usuario.UsuarioRoles,
            //};
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Apellido,Email,FechaRegistro")] UsuarioUpdate usuarioUpdate, string password, int[] rol)
        {
            if (id != usuarioUpdate.UsuarioId)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            usuario.Nombre = usuarioUpdate.Nombre;
            usuario.Apellido = usuarioUpdate.Apellido;
            usuario.Email = usuarioUpdate.Email;
            usuario.FechaRegistro = usuarioUpdate.FechaRegistro;
            if (!String.IsNullOrEmpty(password))
                usuario.Password = password;

            usuarioUpdate.UsuarioRoles = new List<UsuarioRol>();
            foreach (int r in rol)
            {
                var usuarioRol = new UsuarioRol { RolId = r, UsuarioId = id };
                usuarioUpdate.UsuarioRoles.Add(usuarioRol);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRoles = _context.UsuarioRoles.Where(ur => ur.UsuarioId == id);
                    foreach (var ur in usuarioRoles)
                    {
                        _context.Remove(ur);
                    }
                    await _context.SaveChangesAsync();

                    foreach (int r in rol)
                    {
                        var usuarioRol = new UsuarioRol { RolId = r, UsuarioId = id };
                        _context.Add(usuarioRol);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.roles = await _context.Roles.ToListAsync();
            return View(usuarioUpdate);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
