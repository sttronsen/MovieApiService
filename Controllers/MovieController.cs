using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.Models;

namespace MovieApi.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class MovieController : ControllerBase {
        private static readonly List<Movie> Movies = new List<Movie> (10) {
            new Movie () { Name = "Citizen Kane", Genre = "Drama", Year = 1941 },
            new Movie () { Name = "The Wizard of Oz", Genre = "Fantasy", Year = 1939 },
            new Movie () { Name = "The Godfather", Genre = "Crime", Year = 1972 },
        };

        private readonly ILogger<MovieController> _logger;

        public MovieController (ILogger<MovieController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get () {

            if (Movies != null) return Ok (Movies);
            else return BadRequest ();
        }

        [HttpGet ("{name}", Name = "GetMovie")]
        public IActionResult GetMovieByName (string name) {
            if (name == null) return BadRequest ();
            Movie foundMovie = Movies.FirstOrDefault<Movie> (x => x.Name.Equals (name));
            if (foundMovie == null) return BadRequest ();
            else return Ok (foundMovie);

        }

        [HttpGet ("year/")]
        public IActionResult GetMovieByYear (int year) {
            Movie foundMovie = Movies.FirstOrDefault<Movie> (x => x.Year == year);
            if (foundMovie == null) return BadRequest ();
            else return Ok (foundMovie);
        }

        [HttpPost]
        public IActionResult CreateMovie (Movie movie) {
            try {
                Movies.Add (movie);
                return CreatedAtRoute ("GetMovie", new { name = movie.Name }, movie);
            } catch (Exception e) {
                return StatusCode (500, e.Message);
            }
        }

        [HttpPut ("{name}")]
        public IActionResult UpdateMovie (string name, Movie movieIn) {
            try {
                foreach (Movie m in Movies) {
                    if (m.Name.Equals (name)) {
                        m.Name = movieIn.Name;
                        m.Genre = movieIn.Genre;
                        m.Year = movieIn.Year;
                        return NoContent ();
                    }
                }
                return BadRequest ();

            } catch (Exception e) {
                return StatusCode (500, e.Message);
            }
        }

        [HttpDelete ("{name}")]
        public IActionResult DeleteMovie (string name) {
            try {
                if (name == null) return BadRequest ();
                Movie foundMovie = Movies.FirstOrDefault (x => x.Name.Equals (name));
                if (foundMovie == null) return BadRequest ();

                Movies.Remove (foundMovie);
                return NoContent ();

            } catch (Exception e) {
                return StatusCode (500, e.Message);
            }
        }
    }

}