using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    [CollectionDefinition("ProductServiceCollection")]
    public class ProductServiceCollection : ICollectionFixture<ProductServiceFixture> { }

    public class ProductServiceFixture
    {
        public IProductService ProductService { get; }

        public ProductServiceFixture()
        {
            var cart = new Mock<ICart>().Object;
            var productRepository = new Mock<IProductRepository>().Object;
            var orderRepository = new Mock<IOrderRepository>().Object;
            var _localizer = new Mock<IStringLocalizer<ProductService>>().Object;
            
            ProductService = new ProductService(cart, productRepository, orderRepository, _localizer);
        }
    }

    [Collection("ProductServiceCollection")]

    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>

        private readonly IProductService _productService;
        
        public ProductServiceTests(ProductServiceFixture productServiceFixture)
        {
            _productService = productServiceFixture.ProductService;
        }

        /*public ProductServiceTests(IProductService productService)
        {
            this._productService = productService;
        }*/


        [Fact]
        [Description("je ne mets pas de nom dans le formulaire,« Veuillez saisir un nom » doit être retourné ")]

        public void TestNomVide()
        {
            // Arrange

            ProductViewModel productWithoutNom = new ProductViewModel()
            {
                Description = "test nom vide",
                Stock = "5",
                Price = "10"
            };

            ProductViewModel productWithNom = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test nom vide",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutName = _productService.CheckProductModelErrors(productWithoutNom);
            List<string> messageProductWithName = _productService.CheckProductModelErrors(productWithNom);

            // Assert
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutName.Count; i++)
            {
                if (messageProductWithoutName[i].Contains("MissingName"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("le test ne fonctionne pas");
            }


            for (int i = 0; i < messageProductWithName.Count; i++)
            {
                if (messageProductWithName[i].Contains("MissingName"))
                    Assert.Fail("le test ne fonctionne pas");
            }         
        }
        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}