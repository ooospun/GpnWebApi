using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GpnWebApi.EF;
using GpnWebApi.Models;
using Microsoft.Extensions.Options;
using GpnWebApi.Share;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace GpnWebApi.Controllers
{
    /// <summary>
    /// Справочник пользователей системы
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GpnContext _context;
        private readonly IOptions<WebOptions> webOptions;

        public UsersController(GpnContext context, IOptions<WebOptions> webOptions)
        {
            _context = context;
            this.webOptions = webOptions;
        }

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        /// <returns></returns>
        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.UrR)
                .ToListAsync();
        }

        /// <summary>
        /// Получить пользователя по коду
        /// </summary>
        /// <param name="id">Код пользователя</param>
        /// <returns></returns>
        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.UrR)
                .Where(x => x.Uid == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Изменить пользователя
        /// </summary>
        /// <param name="id">Код пользователя</param>
        /// <param name="user">Информация о пользователе</param>
        /// <returns></returns>
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Uid)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        /// Создать пользователя
        /// </summary>
        /// <param name="user">Информация о пользователе</param>
        /// <returns></returns>
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Uid }, user);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="request">Данные для авторизации пользователя </param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]Login request)
        {
            var user = await AuthentificateUser(request);

            if (user != null)
            {
                var token = GenerateJWT(user);
                return Ok(new
                {
                    access_token = token
                });
            }
            return Unauthorized();
        }

        private async Task<User> AuthentificateUser (Login login )
        {
            return await _context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.UrR)
                .SingleOrDefaultAsync(x => x.Uemail == login.Email && x.Upassword == login.Password);
        }

        private string GenerateJWT(User user)
        {
            var webParams = webOptions.Value;
            var securityKey = webParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Uemail),
                new Claim(JwtRegisteredClaimNames.Sub, user.Uid.ToString())
            };

            foreach (var item in user.UserRoles)
            {
                claims.Add(new Claim("role", item.UrR.Rname));
            }

            var token = new JwtSecurityToken(webParams.Issuer, 
                webParams.Audience, 
                claims, 
                expires: DateTime.Now.AddSeconds(webParams.TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Uid == id);
        }

    }
}
