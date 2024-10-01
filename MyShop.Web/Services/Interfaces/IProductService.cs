using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyShop.DAL.Repositories;
using MyShop.Entities.Models;
using MyShop.Services.Implementations;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Services.Interfaces
{
    public interface IProductService
    {
        public Task<int> CreateProductAsync(CreateProductVM productVM);
        public List<ProductTableVM> GetProductForTable();
        public Task<bool> EditProduct(EditProductVM editProductVM);
        public  Task<Product> GetProductById(int id);
        public  Task<EditProductVM> GetDataForEdit(Product product);
        public  Task<bool> DeleteProduct(Product product);
        public IEnumerable<ProductForIndexVM> GetAllProductForIndex();
        public ProductForDetails GetProductDetails(int id);




    }
}
