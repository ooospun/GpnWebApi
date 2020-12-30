using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GpnWebApi.EF;
using GpnWebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GpnWebApi.Controllers
{
    /// <summary>
    /// Роли пользователя
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly GpnContext _context;

        public UserRolesController(GpnContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Список ролей пользователя
        /// </summary>
        /// <param name="userId">Код пользователя</param>
        /// <returns></returns>
        // GET: api/UserRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoles(Guid userId)
        {
            return await _context.UserRoles.Where(x => x.UrUid == userId).ToListAsync();
        }

        /// <summary>
        /// Получить запись по коду
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/UserRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(Guid id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
        }

        /// <summary>
        /// Изменить роль пользователя
        /// </summary>
        /// <param name="id">Код записи</param>
        /// <param name="userRole">Информация о роли</param>
        /// <returns></returns>
        // PUT: api/UserRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserRole(Guid id, UserRole userRole)
        {
            if (id != userRole.Urid)
            {
                return BadRequest();
            }

            _context.Entry(userRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Добавить новую запись
        /// </summary>
        /// <param name="userRole">Информация о роли</param>
        /// <returns></returns>
        // POST: api/UserRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserRole>> PostUserRole(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserRole", new { id = userRole.Urid }, userRole);
        }

        /// <summary>
        /// Удалить роль пользователя
        /// </summary>
        /// <param name="id">Код записи</param>
        /// <returns></returns>
        // DELETE: api/UserRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(Guid id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRoleExists(Guid id)
        {
            return _context.UserRoles.Any(e => e.Urid == id);
        }
    }
}
