using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }


        //MVC Video45.MVC projelerinde Success,Fail dönmemize gerek yok.CustomResponseDto dönmemize de gerek yok.
        //public async Task<CustomResponseDto<List<ProductWithCategoryDto>>>'yu Task<List<ProductWithCategoryDto>> olarak güncelledik 

        //public async Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
        public async Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
        {
            //Video33._productRepository.GetProductsWithCategory() List<Product> dönüüyor ama bize List<ProductWithCategoryDto> dönüş tipi lazım.Mapperla biz bunu rahatlıkla yapabiliriz ama bir tık ileriye taşıyalım olayı IProductService interfacesinde direkt Task<CustomResponseDto> dönelim zaten API controllerdeki actionlar da hep customResponseDto çevirme işlemi yapıyorduk. Bu işlemi controllerApı'da değil serviste yapmış olalım.
            var products = await _productRepository.GetProductsWithCategory();

            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            //API'nın istemiş olduğu CustomResponseDto<List<ProductWithCategoryDto>> datayı dönmüş olduk.
            //return CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto);
            return productsDto;
        }
    }
}
