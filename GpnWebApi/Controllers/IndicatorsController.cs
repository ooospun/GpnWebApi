using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GpnWebApi.EF;
using GpnWebApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GpnWebApi.Controllers
{
    /// <summary>
    /// Справочник индикаторов силосных башен
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "User")]
    public class IndicatorsController : ControllerBase
    {
        private readonly GpnContext _context;

//        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public IndicatorsController(GpnContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список индикаторов силосных башен
        /// </summary>
        /// <returns></returns>
        // GET: api/Indicators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Indicator>>> GetIndicators()
        {
            return await _context.Indicators.OrderBy(x => x.Title).ToListAsync();
        }

        /// <summary>
        /// Получить индикатор силосной башни
        /// </summary>
        /// <param name="id">Код индикатора</param>
        /// <returns></returns>
        // GET: api/Indicators/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Indicator>> GetIndicator(Guid id)
        {
            var indicator = await _context.Indicators.FindAsync(id);

            if (indicator == null)
            {
                return NotFound();
            }

            return indicator;
        }

        /// <summary>
        /// Изменить индикатор силосной башни
        /// </summary>
        /// <param name="id">Код индикатора</param>
        /// <param name="indicator">Информация об индикаторе</param>
        /// <returns></returns>
        // PUT: api/Indicators/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndicator(Guid id, Indicator indicator)
        {
            if (id != indicator.Id)
            {
                return BadRequest();
            }

            _context.Entry(indicator).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicatorExists(id))
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
        /// Добавить новый индикатор силосной башни
        /// </summary>
        /// <param name="indicator">Информация об индикаторе</param>
        /// <returns></returns>
        // POST: api/Indicators
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Indicator>> PostIndicator(Indicator indicator)
        {
            _context.Indicators.Add(indicator);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndicator", new { id = indicator.Id }, indicator);
        }

        /// <summary>
        /// Удалить индикатор силосной башни
        /// </summary>
        /// <param name="id">Код индикатора</param>
        /// <returns></returns>
        // DELETE: api/Indicators/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndicator(Guid id)
        {
            var indicator = await _context.Indicators.FindAsync(id);
            if (indicator == null)
            {
                return NotFound();
            }

            _context.Indicators.Remove(indicator);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IndicatorExists(Guid id)
        {
            return _context.Indicators.Any(e => e.Id == id);
        }
    }
}
