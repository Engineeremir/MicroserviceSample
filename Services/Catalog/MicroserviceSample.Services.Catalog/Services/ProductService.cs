using AutoMapper;
using MicroserviceSample.Services.Catalog.Models;
using MicroserviceSample.Services.Catalog.Settings;
using MicroserviceSample.Shared.Dtos;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MicroserviceSample.Services.Catalog.Dtos;
using System.Linq;

namespace MicroserviceSample.Services.Catalog.Services
{
    public class ProductService:IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;

            _mapper = mapper;
        }

        public async Task<Response<List<ProductDto>>> GetAllAsync()
        {
            var Products = await _productCollection.Find(Product => true).ToListAsync();

            if (Products.Any())
            {
                foreach (var Product in Products)
                {
                    Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();
                }
            }
            else
            {
                Products = new List<Product>();
            }

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(Products), 200);
        }

        public async Task<Response<ProductDto>> GetByIdAsync(string id)
        {
            var Product = await _productCollection.Find<Product>(x => x.Id == id).FirstOrDefaultAsync();

            if (Product == null)
            {
                return Response<ProductDto>.Fail("Product not found", 404);
            }
            Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(Product), 200);
        }

        public async Task<Response<List<ProductDto>>> GetAllByUserIdAsync(string userId)
        {
            var Products = await _productCollection.Find<Product>(x => x.UserId == userId).ToListAsync();

            if (Products.Any())
            {
                foreach (var Product in Products)
                {
                    Product.Category = await _categoryCollection.Find<Category>(x => x.Id == Product.CategoryId).FirstAsync();
                }
            }
            else
            {
                Products = new List<Product>();
            }

            return Response<List<ProductDto>>.Success(_mapper.Map<List<ProductDto>>(Products), 200);
        }

        public async Task<Response<ProductDto>> CreateAsync(ProductCreateDto ProductCreateDto)
        {
            var newProduct = _mapper.Map<Product>(ProductCreateDto);

            newProduct.CreatedTime = DateTime.Now;
            await _productCollection.InsertOneAsync(newProduct);

            return Response<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(ProductUpdateDto ProductUpdateDto)
        {
            var updateProduct = _mapper.Map<Product>(ProductUpdateDto);

            var result = await _productCollection.FindOneAndReplaceAsync(x => x.Id == ProductUpdateDto.Id, updateProduct);

            if (result == null)
            {
                return Response<NoContent>.Fail("Product not found", 404);
            }

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _productCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Product not found", 404);
            }
        }
    }
}
