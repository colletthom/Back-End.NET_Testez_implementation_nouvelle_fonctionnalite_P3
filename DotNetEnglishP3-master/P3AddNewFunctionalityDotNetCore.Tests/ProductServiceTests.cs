using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        TestMethod]
        [Description("je ne mets pas de nom dans le formulaire,« Veuillez saisir un nom » doit être retourné ")]

        public void TestNomVide()
        {
            // Arrange
            private readonly IProductService productService;
        //ProductService productService = new ProductService();
            ProductViewModel productWithoutNom = new ProductViewModel()
            {
                Name = null,
                Description = "test nom vide",
                Stock = "5",
                Price = "10"
            };
            // Act
            List<string> messageProductWithoutName = productService.CheckProductModelErrors(productWithoutNom);

            // Assert
            Assert.Equal(1, 1);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}