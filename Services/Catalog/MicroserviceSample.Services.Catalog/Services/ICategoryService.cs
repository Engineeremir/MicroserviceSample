using MicroserviceSample.Services.Catalog.Dtos;
using MicroserviceSample.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroserviceSample.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> CreateAsync(CategoryDto category);

        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
