using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    [CollectionDefinition("ProductServiceCollection")]
    public class ProductServiceCollection : ICollectionFixture<ProductServiceFixture> { }
    public class ProductServiceFixture
    {
        public IProductService ProductServiceFrench { get; }
        public IProductService ProductServiceEnglish { get; }
        public ProductServiceFixture()
        {
            var cart = new Mock<ICart>().Object;
            var productRepository = new Mock<IProductRepository>().Object;
            var orderRepository = new Mock<IOrderRepository>().Object;
            
            var _localizerFrench = new Mock<IStringLocalizer<ProductService>>().Object;
            Mock.Get(_localizerFrench).Setup(x => x["MissingName"]).Returns(new LocalizedString("MissingName", "Veuillez saisir un nom"));

            var _localizerEnglish = new Mock<IStringLocalizer<ProductService>>().Object;
            Mock.Get(_localizerEnglish).Setup(x => x["MissingName"]).Returns(new LocalizedString("MissingName", "Please enter a name"));

            ProductServiceFrench = new ProductService(cart, productRepository, orderRepository, _localizerFrench);
            ProductServiceEnglish = new ProductService(cart, productRepository, orderRepository, _localizerEnglish);
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

        private readonly IProductService _productServiceFrench;
        private readonly IProductService _productServiceEnglish;

        public ProductServiceTests(ProductServiceFixture productServiceFixture)
        {
            _productServiceFrench = productServiceFixture.ProductServiceFrench;
            _productServiceEnglish = productServiceFixture.ProductServiceEnglish;
        }
     

        [Fact]
        [Description("je ne mets pas de nom dans le formulaire,« Veuillez saisir un nom » " +
            "doit être retourné dans la bonne langue et si on nom est rempli il ne doit pas y avoir de message « Veuillez saisir un nom »")]

        public void TestNomVide()
        {
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
            List<string> messageProductWithoutNameFrench = _productServiceFrench.CheckProductModelErrors(productWithoutNom);
            List<string> messageProductWithNameFrench = _productServiceFrench.CheckProductModelErrors(productWithNom);
            List<string> messageProductWithoutNameEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutNom);
            List<string> messageProductWithNameEnglish = _productServiceEnglish.CheckProductModelErrors(productWithNom);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutNameFrench.Count; i++)
            {
                if (messageProductWithoutNameFrench[i].Contains("Veuillez saisir un nom"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("le test ne fonctionne pas");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithNameFrench.Count; i++)
            {
                if (messageProductWithNameFrench[i].Contains("Veuillez saisir un nom"))
                    Assert.Fail("le test ne fonctionne pas");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutNameEnglish.Count; i++)
            {
                if (messageProductWithoutNameEnglish[i].Contains("Please enter a name"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("le test ne fonctionne pas");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithNameEnglish.Count; i++)
            {
                if (messageProductWithNameEnglish[i].Contains("Please enter a name"))
                    Assert.Fail("le test ne fonctionne pas");
            }
        }
        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}