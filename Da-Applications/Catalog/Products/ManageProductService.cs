using Da.Data.Ef;
using Da.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ViewModels.Common;
using Da_Applications.Common;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using ViewModels.Catalog.Products;
using ViewModels.Catalog.ProductImages;

namespace Da_Applications.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly CafeDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(CafeDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTransLation>()
                {
                    new ProductTransLation()
                    {
                        Name=request.Nane,
                        Description=request.Description,
                        Details=request.Details,
                        SeoDesCription=request.SeoDescription,
                        SeoAlias=request.SeoAlias,
                        SeoTitle=request.SeoTitle,
                        LanguageId=request.LanguageId
                    }
                }
            };
            //save
            if(request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    { 
                        Caption="Thumbnail Image ",
                        DateCreated=DateTime.Now,
                        FileSize=request.ThumbnailImage.Length,
                        ImagePath=await this.SaveFile(request.ThumbnailImage),
                        IsDefault=true,
                        SortOrder=1
                    }
                };
            }
            _context.Products.Add(product);
          await  _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new CafeException($"cannotfind a product{productId}");
            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach(var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);

            }
            _context.Products.Remove(product);
          return  await _context.SaveChangesAsync();
        }

       

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.ProductTranslations.FirstOrDefaultAsync(x =>x.ProductId == request.Id
                && x.LanguageId == request.LanguageId);
            if (product == null || productTranslations==null) throw new CafeException($"Cannot find a product with id:{request.Id}");
            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDesCription = request.SeoDesCription;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;
            //save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage =await _context.ProductImages.FirstOrDefaultAsync
                    (i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
                

            }
            return await _context.SaveChangesAsync();

        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new CafeException($"Cannot find a product with id:{productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new CafeException($"Cannotfind a produdct with id:{productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        List<ProductViewModel> IManageProductService.GetAll()
        {
            throw new NotImplementedException();
        }

     /*   public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //fill
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            //paging
            int totalRow = await query.CountAsync();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
             .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDesCription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    
                }).ToListAsync();

            //selecr proj
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = await data
            };
            return pagedResult;     
        }*/

        public async Task<ProductViewModel> GetById(int productId,string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId==languageId);
            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : null,
                LanguageId=productTranslation.LanguageId,
                Details=productTranslation!=null?productTranslation.Details:null,
                Name=productTranslation!=null ? productTranslation.Name:null,
                OriginalPrice=product.OriginalPrice,
                Price=product.Price,
                SeoAlias=productTranslation!=null? productTranslation.SeoAlias:null,
                SeoDescription = productTranslation!=null? productTranslation.SeoDesCription : null,
                SeoTitle=productTranslation!=null?productTranslation.SeoTitle:null,
                Stock=product.Stock,
                ViewCount=product.ViewCount
            };
            return productViewModel;
        }

        public Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
           return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateImage(int imageId,  ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new CafeException($"Cannot find an image with id{imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new CafeException($"Cannot find an image with id{imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                 .Select(i => new ProductImageViewModel()
                 {
                     Caption = i.Caption,
                     DateCreated = i.DateCreated,
                     FileSize = i.FileSize,
                     Id = i.Id,
                     ImagePath = i.ImagePath,
                     IsDefault = i.IsDefault,
                     ProductId = i.ProductId,
                     SortOrder = i.SortOrder
                 }).ToListAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new CafeException($"Cannot find an image with id{imageId}");
            var viewModel=new ProductImageViewModel()
                  {
                      Caption = image.Caption,
                      DateCreated = image.DateCreated,
                      FileSize = image.FileSize,
                      Id = image.Id,
                      ImagePath = image.ImagePath,
                      IsDefault = image.IsDefault,
                      ProductId = image.ProductId,
                      SortOrder = image.SortOrder
                  };
            return viewModel;

        }
    }
}
