using ismat_36_proj.Data;
using ismat_36_proj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BookingService.Models;

namespace BookingService.Controllers
{
    [Authorize]
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [Route("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                Roles = (from userRole in _context.UserRoles
                         join role in _context.Roles on userRole.RoleId equals role.Id
                         where userRole.UserId == u.Id
                         select role.Name).ToList()
            }).ToList();

            return Ok(users);
        }


        // GET: api/Bookings/GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.Booking.ToListAsync();
            return Ok(bookings);
        }

        // GET: api/Bookings/GetById/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound(new { Message = $"Booking with id {id} is not found." });
            }
            return Ok(booking);
        }

        // POST: api/Bookings/Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = booking.BookingID }, booking);
        }

        // PUT: api/Bookings/Put/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Booking booking)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.Booking.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new { Message = $"Booking with id {id} is not found." });
            }

            // Update properties
            entity.From = booking.From;
            entity.Destination = booking.Destination;
            entity.StartDate = booking.StartDate;
            entity.ReturnDate = booking.ReturnDate;
            entity.Adults = booking.Adults;
            entity.Child = booking.Child;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log exception details here
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "A concurrency error occurred." });
            }

            return Ok(entity);
        }

        // DELETE: api/Bookings/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Booking.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new { Message = $"Booking with id {id} is not found." });
            }

            _context.Booking.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(entity);
        }
    }
}
