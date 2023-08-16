using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Data;
using MovieBooking.Models;

namespace MovieBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;
        public MoviesController(MovieDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        //[Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] Movie movieObj)
        {
            
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //[Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movie movieObj) 
        {
            var movie = _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("no record found against this id");
            }
            else
            {
                movie.Title = movieObj.Title;
                movie.Description = movieObj.Description;
                movie.TicketPrice = movieObj.TicketPrice;
                movie.Rating = movieObj.Rating;
                movie.PlayingDate = movieObj.PlayingDate;
                movie.Duration = movieObj.Duration;
                movie.Genre = movieObj.Genre;
                movie.TicketsAllotted = movieObj.TicketsAllotted;
                movie.TheatreName = movieObj.TheatreName;
            }
            _dbContext.SaveChanges();
            return Ok("Record updated successfully");
        }

        //[Authorize(Roles ="Admin")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deleteMovie = _dbContext.Movies.Find(id);
            if(deleteMovie == null)
            {
                return NotFound("The movie with this id is not found");
            }
            else
            {
                _dbContext.Movies.Remove(deleteMovie);
                _dbContext.SaveChanges(true);
                return Ok("Movie deleted successfully");
            }
        }

        [HttpGet("[action]")]
        public IActionResult AllMovies() 
        {
           var movies = from movie in _dbContext.Movies
                        select new
                        {
                            Id = movie,
                            Title = movie.Title,
                            Duration = movie.Duration,
                            Language = movie.Language,
                            Rating = movie.Rating,
                            Genre = movie.Genre,

                        };
            return Ok(movies);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult MovieDetails(int id) 
        {
            var movie = _dbContext.Movies.Find(id);
            if(movie == null)
            {
                return NotFound("Movie not found");
            }
            return Ok(movie);
        }


        [HttpGet("[action]/{movieName}")]
        public IActionResult FindMovies(string movieName)
        {
            var movies = from movie in _dbContext.Movies
                         where movie.Title.StartsWith(movieName)
                        select new
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Language = movie.Language,
                        };
            return Ok(movies);
        }
    }
}
