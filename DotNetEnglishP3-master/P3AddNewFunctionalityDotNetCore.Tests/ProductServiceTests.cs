using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
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
            Mock.Get(_localizerFrench).Setup(x => x["MissingPrice"]).Returns(new LocalizedString("MissingPrice", "Veuillez saisir un prix"));
            Mock.Get(_localizerFrench).Setup(x => x["MissingStock"]).Returns(new LocalizedString("MissingStock", "Veuillez saisir un stock"));
            Mock.Get(_localizerFrench).Setup(x => x["PriceNotANumber"]).Returns(new LocalizedString("PriceNotANumbere", "La valeur saisie pour le prix doit être un nombre"));
            Mock.Get(_localizerFrench).Setup(x => x["PriceNotGreaterThanZero"]).Returns(new LocalizedString("PriceNotGreaterThanZero", "La prix doit être supérieur à zéro"));
            Mock.Get(_localizerFrench).Setup(x => x["StockNotAnInteger"]).Returns(new LocalizedString("StockNotAnInteger", "La valeur saisie pour le stock doit être un entier"));
            Mock.Get(_localizerFrench).Setup(x => x["StockNotGreaterThanZero"]).Returns(new LocalizedString("StockNotGreaterThanZero", "La stock doit être supérieure à zéro"));

            var _localizerEnglish = new Mock<IStringLocalizer<ProductService>>().Object;
            Mock.Get(_localizerEnglish).Setup(x => x["MissingName"]).Returns(new LocalizedString("MissingName", "Please enter a name"));
            Mock.Get(_localizerEnglish).Setup(x => x["MissingPrice"]).Returns(new LocalizedString("MissingPrice", "Please enter a price"));
            Mock.Get(_localizerEnglish).Setup(x => x["MissingStock"]).Returns(new LocalizedString("MissingStock", "Please enter a stock value"));
            Mock.Get(_localizerEnglish).Setup(x => x["PriceNotANumber"]).Returns(new LocalizedString("PriceNotANumber", "The value entered for the price must be a number"));
            Mock.Get(_localizerEnglish).Setup(x => x["PriceNotGreaterThanZero"]).Returns(new LocalizedString("PriceNotGreaterThanZero", "The price must be greater than zero"));
            Mock.Get(_localizerEnglish).Setup(x => x["StockNotAnInteger"]).Returns(new LocalizedString("StockNotAnInteger", "The value entered for the stock must be a integer"));
            Mock.Get(_localizerEnglish).Setup(x => x["StockNotGreaterThanZero"]).Returns(new LocalizedString("StockNotGreaterThanZero", "The stock must greater than zero"));

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
        [Description("I do not put a name in the form, “Please enter a name”" +
            "must be returned in the correct language and if the name is filled in there must not be a “Please enter a name” message »")]

        public void MissingName()
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
                Description = "test nom rempli",
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
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithNameFrench.Count; i++)
            {
                if (messageProductWithNameFrench[i].Contains("Veuillez saisir un nom"))
                    Assert.Fail("The test failed");
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
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithNameEnglish.Count; i++)
            {
                if (messageProductWithNameEnglish[i].Contains("Please enter a name"))
                    Assert.Fail("The test failed");
            }
        }
        [Fact]
        [Description("I do not put a price in the form, “Please enter a price”" +
            "must be returned in the correct language and if the name is filled in there must not be a “Please enter a price” message »")]
        public void MissingPrice()
        {
            // Arrange
            ProductViewModel productWithoutPrice = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test prix vide",
                Stock = "5"
            };

            ProductViewModel productWithPrice = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test prix rempli",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutPriceFrench = _productServiceFrench.CheckProductModelErrors(productWithoutPrice);
            List<string> messageProductWithPriceFrench = _productServiceFrench.CheckProductModelErrors(productWithPrice);
            List<string> messageProductWithoutPriceEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutPrice);
            List<string> messageProductWithPriceEnglish = _productServiceEnglish.CheckProductModelErrors(productWithPrice);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutPriceFrench.Count; i++)
            {
                if (messageProductWithoutPriceFrench[i].Contains("Veuillez saisir un prix"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithPriceFrench.Count; i++)
            {
                if (messageProductWithPriceFrench[i].Contains("Veuillez saisir un prix"))
                    Assert.Fail("The test failed");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutPriceEnglish.Count; i++)
            {
                if (messageProductWithoutPriceEnglish[i].Contains("Please enter a price"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithPriceEnglish.Count; i++)
            {
                if (messageProductWithPriceEnglish[i].Contains("Please enter a price"))
                    Assert.Fail("The test failed");
            }
        }

        [Fact]
        [Description("I do not put a Stock in the form, “Please enter a stock value”" +
    "must be returned in the correct language and if the name is filled in there must not be a “Please enter a stock value” message »")]
        public void MissingStock()
        {
            // Arrange
            ProductViewModel productWithoutStock = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test stock vide",
                Price = "10"
            };

            ProductViewModel productWithStock = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test stock rempli",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutStockFrench = _productServiceFrench.CheckProductModelErrors(productWithoutStock);
            List<string> messageProductWithStockFrench = _productServiceFrench.CheckProductModelErrors(productWithStock);
            List<string> messageProductWithoutStockEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutStock);
            List<string> messageProductWithStockEnglish = _productServiceEnglish.CheckProductModelErrors(productWithStock);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutStockFrench.Count; i++)
            {
                if (messageProductWithoutStockFrench[i].Contains("Veuillez saisir un stock"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithStockFrench.Count; i++)
            {
                if (messageProductWithStockFrench[i].Contains("Veuillez saisir un stock"))
                    Assert.Fail("The test failed");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutStockEnglish.Count; i++)
            {
                if (messageProductWithoutStockEnglish[i].Contains("Please enter a stock value"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithStockEnglish.Count; i++)
            {
                if (messageProductWithStockEnglish[i].Contains("Please enter a stock value"))
                    Assert.Fail("The test failed");
            }
        }

        [Fact]
        [Description("I do not put a Price which are Not A Number in the form")]
        public void PriceNotANumber()
        {
            // Arrange
            ProductViewModel productWithoutPriceNotANumber = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test PriceNotANumber",
                Price = "azerty",
                Stock = "5"
            };

            ProductViewModel productWithPriceNotANumber = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test PriceANumber rempli",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutPriceNotANumberFrench = _productServiceFrench.CheckProductModelErrors(productWithoutPriceNotANumber);
            List<string> messageProductWithPriceNotANumberFrench = _productServiceFrench.CheckProductModelErrors(productWithPriceNotANumber);
            List<string> messageProductWithoutPriceNotANumberEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutPriceNotANumber);
            List<string> messageProductWithPriceNotANumberEnglish = _productServiceEnglish.CheckProductModelErrors(productWithPriceNotANumber);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutPriceNotANumberFrench.Count; i++)
            {
                if (messageProductWithoutPriceNotANumberFrench[i].Contains("La valeur saisie pour le prix doit être un nombre"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithPriceNotANumberFrench.Count; i++)
            {
                if (messageProductWithPriceNotANumberFrench[i].Contains("La valeur saisie pour le prix doit être un nombre"))
                    Assert.Fail("The test failed");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutPriceNotANumberEnglish.Count; i++)
            {
                if (messageProductWithoutPriceNotANumberEnglish[i].Contains("The value entered for the price must be a number"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithPriceNotANumberEnglish.Count; i++)
            {
                if (messageProductWithPriceNotANumberEnglish[i].Contains("The value entered for the price must be a number"))
                    Assert.Fail("The test failed");
            }
        }

        [Fact]
        [Description("I do not put a Price which are greater than zero in the form")]
        public void PriceNotGreaterThanZero()
        {
            // Arrange
            ProductViewModel productWithoutPriceNotGreaterThanZero = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test PriceNotGreaterThanZero",
                Price = "-5",
                Stock = "5"
            };

            ProductViewModel productWithPriceNotGreaterThanZero = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test Price Greater Than Zero rempli",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutPriceNotGreaterThanZeroFrench = _productServiceFrench.CheckProductModelErrors(productWithoutPriceNotGreaterThanZero);
            List<string> messageProductWithPriceNotGreaterThanZeroFrench = _productServiceFrench.CheckProductModelErrors(productWithPriceNotGreaterThanZero);
            List<string> messageProductWithoutPriceNotGreaterThanZeroEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutPriceNotGreaterThanZero);
            List<string> messageProductWithPriceNotGreaterThanZeroEnglish = _productServiceEnglish.CheckProductModelErrors(productWithPriceNotGreaterThanZero);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutPriceNotGreaterThanZeroFrench.Count; i++)
            {
                if (messageProductWithoutPriceNotGreaterThanZeroFrench[i].Contains("La prix doit être supérieur à zéro"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithPriceNotGreaterThanZeroFrench.Count; i++)
            {
                if (messageProductWithPriceNotGreaterThanZeroFrench[i].Contains("La prix doit être supérieur à zéro"))
                    Assert.Fail("The test failed");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutPriceNotGreaterThanZeroEnglish.Count; i++)
            {
                if (messageProductWithoutPriceNotGreaterThanZeroEnglish[i].Contains("The price must be greater than zero"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithPriceNotGreaterThanZeroEnglish.Count; i++)
            {
                if (messageProductWithPriceNotGreaterThanZeroEnglish[i].Contains("The price must be greater than zero"))
                    Assert.Fail("The test failed");
            }
        }

        [Fact]
        [Description("I do not put a stock which are not an integer in the form")]
        public void StockNotAnInteger()
        {
            // Arrange
            ProductViewModel productWithoutStockNotAnInteger = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test StockNotAnInteger",
                Price = "5",
                Stock = "5,2"
            };

            ProductViewModel productWithStockNotAnInteger = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test Price Greater Than Zero rempli",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutStockNotAnIntegerFrench = _productServiceFrench.CheckProductModelErrors(productWithoutStockNotAnInteger);
            List<string> messageProductWithStockNotAnIntegerFrench = _productServiceFrench.CheckProductModelErrors(productWithStockNotAnInteger);
            List<string> messageProductWithoutStockNotAnIntegerEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutStockNotAnInteger);
            List<string> messageProductWithStockNotAnIntegerEnglish = _productServiceEnglish.CheckProductModelErrors(productWithStockNotAnInteger);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutStockNotAnIntegerFrench.Count; i++)
            {
                if (messageProductWithoutStockNotAnIntegerFrench[i].Contains("La valeur saisie pour le stock doit être un entier"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithStockNotAnIntegerFrench.Count; i++)
            {
                if (messageProductWithStockNotAnIntegerFrench[i].Contains("La valeur saisie pour le stock doit être un entier"))
                    Assert.Fail("The test failed");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutStockNotAnIntegerEnglish.Count; i++)
            {
                if (messageProductWithoutStockNotAnIntegerEnglish[i].Contains("The value entered for the stock must be a integer"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithStockNotAnIntegerEnglish.Count; i++)
            {
                if (messageProductWithStockNotAnIntegerEnglish[i].Contains("The value entered for the stock must be a integer"))
                    Assert.Fail("The test failed");
            }
        }

        [Fact]
        [Description("I do not put a Stock which are greater than zero in the form")]
        public void StockNotGreaterThanZero()
        {
            ProductViewModel productWithoutStockNotGreaterThanZero = new ProductViewModel()
            {
                // Arrange
                Name = "Thomas",
                Description = "test StockNotGreaterThanZero",
                Price = "5",
                Stock = "-5"
            };

            ProductViewModel productWithStockNotGreaterThanZero = new ProductViewModel()
            {
                Name = "Thomas",
                Description = "test stock Greater Than Zero rempli",
                Stock = "5",
                Price = "10"
            };

            // Act
            List<string> messageProductWithoutStockNotGreaterThanZeroFrench = _productServiceFrench.CheckProductModelErrors(productWithoutStockNotGreaterThanZero);
            List<string> messageProductWithStockNotGreaterThanZeroFrench = _productServiceFrench.CheckProductModelErrors(productWithStockNotGreaterThanZero);
            List<string> messageProductWithoutStockNotGreaterThanZeroEnglish = _productServiceEnglish.CheckProductModelErrors(productWithoutStockNotGreaterThanZero);
            List<string> messageProductWithStockNotGreaterThanZeroEnglish = _productServiceEnglish.CheckProductModelErrors(productWithStockNotGreaterThanZero);

            // Assert 1
            bool isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutStockNotGreaterThanZeroFrench.Count; i++)
            {
                if (messageProductWithoutStockNotGreaterThanZeroFrench[i].Contains("La stock doit être supérieure à zéro"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 2
            for (int i = 0; i < messageProductWithStockNotGreaterThanZeroFrench.Count; i++)
            {
                if (messageProductWithStockNotGreaterThanZeroFrench[i].Contains("La stock doit être supérieure à zéro"))
                    Assert.Fail("The test failed");
            }

            // Assert 3
            isContainInWithout = false;
            for (int i = 0; i < messageProductWithoutStockNotGreaterThanZeroEnglish.Count; i++)
            {
                if (messageProductWithoutStockNotGreaterThanZeroEnglish[i].Contains("The stock must greater than zero"))
                    isContainInWithout = true;
            }
            if (isContainInWithout == false)
            {
                Assert.Fail("The test failed");
            }

            // Assert 4
            for (int i = 0; i < messageProductWithStockNotGreaterThanZeroEnglish.Count; i++)
            {
                if (messageProductWithStockNotGreaterThanZeroEnglish[i].Contains("The stock must greater than zero"))
                    Assert.Fail("The test failed");
            }
        }
        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}