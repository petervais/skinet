using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> productRepository;
        private readonly IGenericRepository<ProductBrand> productBrandRepository;
        private readonly IGenericRepository<ProductType> productTypeRepository;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> productRepository,
            IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType> productTypeRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.productBrandRepository = productBrandRepository;
            this.productTypeRepository = productTypeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            
            var products = await productRepository.ListAsync(spec);

            return Ok(mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ProductDTO> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await productRepository.GetEntityWithSpec(spec);

            return mapper.Map<Product, ProductDTO>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productBrandRepository.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await productTypeRepository.ListAllAsync());
        }
    }
}