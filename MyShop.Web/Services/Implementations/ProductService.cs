using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Services.Interfaces;
using MyShop.Web.Services.Interfaces;
using MyShop.Web.Settings;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Services.Implementations
{
 
    public class ProductService:IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        //private readonly CategoryService categoryService;

        public ProductService(IUnitOfWork unitOfWork,IFileService fileService/*,CategoryService categoryService*/)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
            //this.categoryService = categoryService;
        }

        public async Task<int> CreateProductAsync(CreateProductVM productVM)
        {
            var fileExtension = Path.GetExtension(productVM.ProductImage.FileName).ToLowerInvariant();
            var FileSize = productVM.ProductImage.Length;

            if (!FileSettings.AllowedExtensions.Contains(fileExtension)) return 0;

            if (FileSettings.MaxFileSizeInBytes < FileSize) return 0;

            var image = await fileService.UploadFileAsync(productVM.ProductImage, FileSettings.ImagePath);
            if(string.IsNullOrEmpty(image)) return 0;


            var product = new Product()
            {
                Name = productVM.Name,
                Description = productVM.Description,
                ImgSrc = image,
                CategoryID = productVM.CatID,
                Price = productVM.Price
            };
            

             unitOfWork.Products.Add(product);
            var effectedRows = unitOfWork.Complete();
            return effectedRows;
        }

        public  List<ProductTableVM>GetProductForTable()
        {
            var producttable = unitOfWork.Products.GetAll(["Category"]).Select(s => new ProductTableVM
            {
                Id=s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                Category = s.Category.Name
            }).ToList();



            return producttable;

        }

        public async Task<bool>EditProduct(EditProductVM editProductVM)
        {
            var product = await unitOfWork.Products.GetByIdAsync(editProductVM.Id);
            if(product==null) return false;

            // transfer data from VM to model
            product.Name = editProductVM.Name;
            product.Description = editProductVM.Description;
            product.Price = editProductVM.Price;
            product.CategoryID = editProductVM.CatID;

            var oldPath = product.ImgSrc;
            var imgSrc= await fileService.UploadFileAsync(editProductVM.ProductImage, Settings.FileSettings.ImagePath);
            if (!string.IsNullOrEmpty(imgSrc)) product.ImgSrc = imgSrc;
            
           unitOfWork.Products.Update(product);
            var effectedRows = unitOfWork.Complete();
            if (effectedRows < 1)
            {
                await fileService.DeleteFileAsync(imgSrc);
                product.ImgSrc = oldPath;
                return false;
            }
            else { 
                await fileService.DeleteFileAsync(oldPath);
                return true;
            }
        
        }
        public async Task<Product> GetProductById(int id)
        {
            return unitOfWork.Products.GetById(id);
        }

        public async Task<EditProductVM> GetDataForEdit(Product product)
        {
            
            return new EditProductVM()
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
               ImgSrc = product.ImgSrc,
               Price = product.Price,
               //Categories= categoryService.GetSelectListItems(),
               CatID=product.CategoryID

            };
           
        }
        public async Task<bool> DeleteProduct(Product product)
        {
            var oldImage = product.ImgSrc;
            // delete it normal 
            unitOfWork.Products.Delete(product);
            var effectedRows= unitOfWork.Complete();

            // if all thing is right => delete image from server
            if (effectedRows < 1) {
                return false;
            }

            await fileService.DeleteFileAsync(oldImage);
            return true;
        }
        public  IEnumerable<ProductForIndexVM>GetAllProductForIndex()
        {
            return  unitOfWork.Products.GetAll().Select(p => new ProductForIndexVM
            {
                Id = p.Id,
                Name = p.Name,
                ImgSrc = p.ImgSrc,
                Price = p.Price
            }).ToList();
        }
        public ProductForDetails GetProductDetails(int id)
        {
            // don't forget to include category
            var product=unitOfWork.Products.Find(p => p.Id == id, new string[] { "Category" });
            return new ProductForDetails
            {
                Id = product.Id,
                Description = product.Description,
                ImgSrc = product.ImgSrc,
                Price = product.Price,
                Name = product.Name,
                CategoryName = product.Category.Name
            };

        }

    }
}
