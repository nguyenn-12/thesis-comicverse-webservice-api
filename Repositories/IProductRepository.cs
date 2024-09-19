using Microsoft.EntityFrameworkCore;
using System;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void UpdateProduct(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                //Condition
                if (_context.Products == null) throw new ArgumentNullException(nameof(_context.Products));

                _logger.LogInformation("aaaaaaaaaaaaaaa {DT}", DateTime.UtcNow.ToLongTimeString());
                // Return
                return _context.Products.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve products: {ex.Message}");
            }
        }

        public Product GetProductById(int id)
        {
            try
            {
                //Condition
                if (_context.Products == null) throw new ArgumentNullException(nameof(_context.Products));

                // Do something
                var product = _context.Products!.FirstOrDefault(p => p.Id == id);

                // Verify
                if (product == null) throw new ArgumentNullException(nameof(product));

                // Return
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve product: {ex.Message}");
            }
        }

        public void AddProduct(Product product)
        {
            try
            {
                //Condition
                if (product == null) throw new ArgumentNullException(nameof(product));


                // Do something
                _context.Products!.Add(product);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add product: {ex.Message}");
            }
        }

        public void UpdateProduct(Product product)
        {
            try
            {
                if (product == null) throw new ArgumentNullException(nameof(product));

                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update product: {ex.Message}");
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                if (_context.Products == null) throw new ArgumentNullException(nameof(_context.Products));

                var product = _context.Products.Find(id);

                if (product == null) throw new ArgumentNullException(nameof(product));

                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete product: {ex.Message}");
            }
        }
    }
}
