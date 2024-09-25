using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.DTOs.RequestDTO;
using thesis_comicverse_webservice_api.Models;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComicController : ControllerBase
    {
        private readonly ILogger<ComicController> _logger;
        private readonly IComicRepository _comicRepository;

        public ComicController(ILogger<ComicController> logger, IComicRepository comicRepository)
        {
            _logger = logger;
            _comicRepository = comicRepository;
        }

        [HttpGet]
        public IActionResult GetAllComics()
        {
            try
            {
                _logger.LogInformation("Getting all comics");
                var comics = _comicRepository.GetAllComics();
                return Ok(comics);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetComicById(int id)
        {
            try
            {
                var comic = _comicRepository.GetComicById(id);
                if (comic == null)
                {
                    return NotFound();
                }
                return Ok(comic);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult AddComic([FromBody] ComicDTO request)
        {
            try
            {
                if (request == null) return BadRequest();

                var data = new Comic()
                {
                    title = request.title,
                    price = request.price
                };

                _comicRepository.AddComic(data);



                return CreatedAtAction(nameof(GetComicById), new { id = data.ComicId }, data);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComic(int id, [FromBody] Comic comic)
        {
            try
            {
                if (comic == null || id != comic.ComicId)
                {
                    return BadRequest();
                }

                var existingComic = _comicRepository.GetComicById(id);
                if (existingComic == null)
                {
                    return NotFound();
                }

                _comicRepository.UpdateComic(comic);
                return NoContent();
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComic(int id)
        {
            try
            {

                var existingComic = _comicRepository.GetComicById(id);
                if (existingComic == null)
                {
                    return NotFound();
                }

                _comicRepository.DeleteComic(id);
                return NoContent();
            }
            catch
            {
                throw;
            }
        }
    }
}
