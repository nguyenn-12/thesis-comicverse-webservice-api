using Microsoft.EntityFrameworkCore;
using System;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IComicRepository
    {
        void AddComic(Comic comic);
        void DeleteComic(int id);
        List<Comic> GetAllComics();
        Comic GetComicById(int id);
        void UpdateComic(Comic comic);
    }

    public class ComicRepository : IComicRepository
    {
        private readonly AppDbContext _context;

        private readonly ILogger<ComicRepository> _logger;
        public ComicRepository(AppDbContext context, ILogger<ComicRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Comic> GetAllComics()
        {
            try
            {
                //Condition
                if (_context.Comic == null) throw new ArgumentNullException(nameof(_context.Comic));

                _logger.LogInformation("aaaaaaaaaaaaaaa {DT}", DateTime.UtcNow.ToLongTimeString());
                // Return
                return _context.Comic.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve comics: {ex.Message}");
            }
        }

        public Comic GetComicById(int id)
        {
            try
            {
                //Condition
                if (_context.Comic == null) throw new ArgumentNullException(nameof(_context.Comic));

                // Do something
                var comic = _context.Comic!.FirstOrDefault(p => p.ComicId == id);

                // Verify
                if (comic == null) throw new ArgumentNullException(nameof(comic));

                // Return
                return comic;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve comic: {ex.Message}");
            }
        }

        public void AddComic(Comic comic)
        {
            try
            {
                //Condition
                if (comic == null) throw new ArgumentNullException(nameof(comic));


                _logger.LogInformation($"comic {comic.ComicId}__{comic.title}__{comic.price}");

                // Do something
                _context.Comic!.Add(comic);
                //_context.
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add comic: {ex.Message}");
            }
        }

        public void UpdateComic(Comic comic)
        {
            try
            {
                if (comic == null) throw new ArgumentNullException(nameof(comic));

                _context.Entry(comic).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update comic: {ex.Message}");
            }
        }

        public void DeleteComic(int id)
        {
            try
            {
                if (_context.Comic == null) throw new ArgumentNullException(nameof(_context.Comic));

                var comic = _context.Comic.Find(id);

                if (comic == null) throw new ArgumentNullException(nameof(comic));

                _context.Comic.Remove(comic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete comic: {ex.Message}");
            }
        }
    }
}
