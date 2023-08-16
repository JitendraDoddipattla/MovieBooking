using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Data;
using MovieBooking.Models;

namespace MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsBookingController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;
        public TicketsBookingController(MovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Booking bk)
        {
            bk.BookingTime = DateTime.Now;
            _dbContext.Tickets.Add(bk);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult GetBookings()
        {
            var bookings =  from booking in _dbContext.Tickets
                            join customer in _dbContext.Users on booking.UserId equals customer.Id
                            join movie in _dbContext.Movies on booking.MovieId equals movie.Id
                            select new
                            {
                                Id = booking.Id,
                                BookingTime = booking.BookingTime,
                                CustomerName = customer.Id,
                                MovieName = movie.Title,
                                TheatreName = booking.TheatreName,
                            };
            return Ok(bookings);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetBookingDetails(int id)
        {
            var bookings = (from booking in _dbContext.Tickets
                           join customer in _dbContext.Users on booking.UserId equals customer.Id
                           join movie in _dbContext.Movies on booking.MovieId equals movie.Id
                           where booking.Id == id
                           select new
                           {
                               Id = booking.Id,
                               Theatrename = booking.TheatreName,
                               SeatNumber = booking.SeatNumber,
                               BookingTime = booking.BookingTime,
                               CustomerName = customer.Id,
                               MovieName = movie.Title,
                               NoOfTikects = booking.NoOfTickets,
                               Price = booking.Price,
                               Phone = booking.Phone,
                               PlayingDate = movie.PlayingDate,
                               PlayingTime = movie.PlayingTime,
                           }).FirstOrDefault();
            return Ok(bookings);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("[action]")]
        public IActionResult TicketStatus()
        {
              var rem_tickets = from booking in _dbContext.Tickets
                                join customer in _dbContext.Users on booking.UserId equals customer.Id
                                join movie in _dbContext.Movies on booking.MovieId equals movie.Id
                                select new
                                {
                                    Id = booking.Id,
                                   
                                    CustomerName = customer.Id,
                                    MovieName = movie.Title,
                                    TheatreName = booking.TheatreName,
                                    Rem_tickets = movie.TicketsAllotted - booking.NoOfTickets, 
                                };
            return Ok(rem_tickets);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var booking = _dbContext.Tickets.Find(id);
            if (booking == null)
            {
                return NotFound("The movie with this id is not found");
            }
            else
            {
                _dbContext.Tickets.Remove(booking);
                _dbContext.SaveChanges(true);
                return Ok("Booking deleted successfully");
            }
        }
    }
}
