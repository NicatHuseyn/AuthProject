using AuthProject.Core.DTOs;
using AuthProject.Core.Entities;
using AuthProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthProject.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IGenericService<Product, ProductDto> _genericService;

        public ProductsController(IGenericService<Product, ProductDto> genericService)
        {
            _genericService = genericService;
        }

        [HttpGet]
        public IActionResult GetProducts() => ActionResultInstance(_genericService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProduct(int id) => ActionResultInstance(await _genericService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)=> ActionResultInstance(await _genericService.AddAsync(productDto));

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto) => ActionResultInstance(await _genericService.UpdateAsync(productDto,productDto.Id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)=> ActionResultInstance(await _genericService.RemoveAsync(id));

    }
}
