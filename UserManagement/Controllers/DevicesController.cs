using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Devices")]
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public IEnumerable<Device> GetDevice()
        {
            return _context.Device;
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _context.Device.SingleOrDefaultAsync(m => m.DeviceId == id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Devices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice([FromRoute] int id, [FromBody] Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.DeviceId)
            {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        [HttpPost]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Device.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = device.DeviceId }, device);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _context.Device.SingleOrDefaultAsync(m => m.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Device.Remove(device);
            await _context.SaveChangesAsync();

            return Ok(device);
        }

        private bool DeviceExists(int id)
        {
            return _context.Device.Any(e => e.DeviceId == id);
        }
    }
}