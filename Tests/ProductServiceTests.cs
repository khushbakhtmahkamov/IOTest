using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ProductService.DTOs.Product;
using ProductService.Models;
using ProductService.Repositories.Category;
using ProductService.Repositories.Product;
using ProductService.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<ILogger<ProductService.Services.ProductService>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private ProductService.Services.ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _loggerMock = new Mock<ILogger<ProductService.Services.ProductService>>();
            _mapperMock = new Mock<IMapper>();

            _productService = new ProductService.Services.ProductService(
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnPaginatedProducts()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var products = new List<Product> { new Product() };
            var totalProducts = 1;

            _productRepositoryMock.Setup(r => r.GetCount()).ReturnsAsync(totalProducts);
            _productRepositoryMock.Setup(r => r.GetProductsAsync(pageNumber, pageSize)).ReturnsAsync(products);

            var productDtos = new List<ProductDto> { new ProductDto() };
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _productService.GetAllAsync(pageNumber, pageSize);

            // Assert
            Assert.AreEqual(totalProducts, result.TotalCount);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.AreEqual(pageNumber, result.PageNumber);
        }

        [Test]
        public async Task GetByIdAsync_ProductFound_ShouldReturnProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId };
            var productDto = new ProductDto { Id = productId };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productId, result.Id);
        }

        [Test]
        public void GetByIdAsync_ProductNotFound_ShouldThrowBusinessException()
        {
            // Arrange
            var productId = 1;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await _productService.GetByIdAsync(productId));
            Assert.AreEqual(BusinessExceptionCode.NOT_FOUND, ex.BusinessExceptionCode);
        }

        [Test]
        public async Task AddAsync_ValidProduct_ShouldReturnProductDto()
        {
            // Arrange
            var createUpdateProductDto = new CreateUpdateProductDto { CategoryId = 1 };
            var category = new Category { Id = 1 };
            var product = new Product { Id = 1, Category = category };
            var productDto = new ProductDto { Id = 1 };

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(createUpdateProductDto.CategoryId)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<Product>(createUpdateProductDto)).Returns(product);
            _productRepositoryMock.Setup(r => r.AddAsync(product)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _productService.AddAsync(createUpdateProductDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void AddAsync_CategoryNotFound_ShouldThrowBusinessException()
        {
            // Arrange
            var createUpdateProductDto = new CreateUpdateProductDto { CategoryId = 1 };

            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(createUpdateProductDto.CategoryId)).ReturnsAsync((Category)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await _productService.AddAsync(createUpdateProductDto));
            Assert.AreEqual(BusinessExceptionCode.NOT_FOUND, ex.BusinessExceptionCode);
        }

        [Test]
        public async Task UpdateAsync_ProductFound_ShouldReturnUpdatedProductDto()
        {
            // Arrange
            var productId = 1;
            var createUpdateProductDto = new CreateUpdateProductDto { CategoryId = 1 };
            var category = new Category { Id = 1 };
            var product = new Product { Id = productId, Category = category };
            var updatedProduct = new Product { Id = productId, Category = category };
            var productDto = new ProductDto { Id = productId };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _categoryRepositoryMock.Setup(r => r.GetByIdAsync(createUpdateProductDto.CategoryId)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<Product>(createUpdateProductDto)).Returns(updatedProduct);
            _productRepositoryMock.Setup(r => r.UpdateAsync(updatedProduct)).ReturnsAsync(updatedProduct);
            _mapperMock.Setup(m => m.Map<ProductDto>(updatedProduct)).Returns(productDto);

            // Act
            var result = await _productService.UpdateAsync(productId, createUpdateProductDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(productId, result.Id);
        }

        [Test]
        public void UpdateAsync_ProductNotFound_ShouldThrowBusinessException()
        {
            // Arrange
            var productId = 1;
            var createUpdateProductDto = new CreateUpdateProductDto { CategoryId = 1 };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await _productService.UpdateAsync(productId, createUpdateProductDto));
            Assert.AreEqual(BusinessExceptionCode.NOT_FOUND, ex.BusinessExceptionCode);
        }

        [Test]
        public async Task DeleteAsync_ProductFound_ShouldDeleteProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _productRepositoryMock.Setup(r => r.DeleteAsync(product)).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _productRepositoryMock.Verify(r => r.DeleteAsync(product), Times.Once);
        }

        [Test]
        public void DeleteAsync_ProductNotFound_ShouldThrowBusinessException()
        {
            // Arrange
            var productId = 1;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BusinessException>(async () => await _productService.DeleteAsync(productId));
            Assert.AreEqual(BusinessExceptionCode.NOT_FOUND, ex.BusinessExceptionCode);
        }
    }
}