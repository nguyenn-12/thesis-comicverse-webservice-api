using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using thesis_comicverse_webservice_api.DTOs.RequestDTO;
using thesis_comicverse_webservice_api.Models;
using thesis_comicverse_webservice_api.Repositories;
using thesis_comicverse_webservice_api.Services;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComicController : ControllerBase
    {
        private readonly ILogger<ComicController> _logger;
        private readonly IFileServices _fileService;
        private readonly IComicRepository _comicRepository;

        public ComicController(ILogger<ComicController> logger, IFileServices fileServices, IComicRepository comicRepository)
        {
            _logger = logger;
            _fileService = fileServices;
            _comicRepository = comicRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComics()
        {
            try
            {
                _logger.LogInformation("Getting all comics");
                var comics = await _comicRepository.GetAllComics();
                return Ok(comics);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComicById(int id)
        {
            try
            {
                var comic = await _comicRepository.GetComicByIdAsync(id);
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
                    comicTitle = request.comicTitle,
                    localhostURL = request.localhostURL,
                    remoteURL = request.remoteURL,
                    releaseDate = request.releaseDate,
                    language = request.language,
                    categoryID = request.categoryID,
                    Description = request.Description
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
        public IActionResult UpdateComic(int id, [FromBody] ComicDTO request)
        {
            try
            {
                if (request == null || id != request.ComicId)
                {
                    return BadRequest();
                }

                var data = new Comic()
                {
                    comicTitle = request.comicTitle,
                    localhostURL = request.localhostURL,
                    remoteURL = request.remoteURL,
                    releaseDate = request.releaseDate,
                    language = request.language,
                    categoryID = request.categoryID,
                    Description = request.Description
                };

                var existingComic = _comicRepository.GetComicByIdAsync(id);
                if (existingComic == null)
                {
                    return NotFound();
                }

                _comicRepository.UpdateComic(data);
                return NoContent();
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComic(int id)
        {
            try
            {

                var existingComic = await _comicRepository.GetComicByIdAsync(id);
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


        [HttpGet("render-pdf")]
        public async Task<IActionResult> RenderPDF(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    //var document = new Document
                    //{
                    //    FileName = file.FileName,
                    //};
                    var ContentType = file.ContentType;
                    var Data = memoryStream.ToArray();


                    return File(Data, ContentType);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
