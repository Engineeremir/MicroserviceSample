using MicroserviceSample.Services.Catalog.Dtos;
using MicroserviceSample.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroserviceSample.Services.Catalog.Services
{
    public interface IProductService
    {
        Task<Response<List<ProductDto>>> GetAllAsync();

        Task<Response<ProductDto>> GetByIdAsync(string id);

        Task<Response<List<ProductDto>>> GetAllByUserIdAsync(string userId);

        Task<Response<ProductDto>> CreateAsync(ProductCreateDto ProductCreateDto);

        Task<Response<NoContent>> UpdateAsync(ProductUpdateDto ProductUpdateDto);

        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
