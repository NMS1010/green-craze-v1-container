using green_craze_be_v1.API.Controllers;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace green_craze_be_v1.UnitTest.ControllerTesting
{
    public class BrandControllerTests
    {
        private readonly Mock<IBrandService> _brandService;

        public BrandControllerTests()
        {
            _brandService = new Mock<IBrandService>();
        }

        [Fact]
        public void GetBrandById_BrandDto_ReturnsOkResult()
        {
            // Arrange
            var brands = GetTestBrands();
            var brand = brands[0];
            _brandService.Setup(service => service.GetBrand(brand.Id).Result).Returns(brand);

            var controller = new BrandsController(_brandService.Object);

            // Act
            var result = controller.GetBrand(brand.Id)?.Result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        public List<BrandDto> GetTestBrands()
        {
            var brands = new List<BrandDto>
            {
                new() {
                    Id = 1,
                    Name = "Brand1",
                    Description = "Brand1 Description",
                },
                new() {
                    Id = 2,
                    Name = "Brand2",
                    Description = "Brand2 Description",
                }
            };

            return brands;
        }
    }
}
