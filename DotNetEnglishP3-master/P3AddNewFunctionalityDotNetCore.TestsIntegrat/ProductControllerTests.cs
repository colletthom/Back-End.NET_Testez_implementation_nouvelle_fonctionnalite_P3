using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore;
using Moq;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Security.Policy;
using System;



namespace P3AddNewFunctionalityDotNetCore.TestsIntegrat
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

    
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        /*[Fact]

public async Task retourServeurHelloWorld()
{
    var webAppFactory = new WebApplicationFactory<Program>(); //d�j� initialis�
    var httpClient = webAppFactory.CreateDefaultClient();  //cf "var client = _factory.CreateClient();"

    var response = await httpClient.GetAsync("");
    var stringResult = await response.Content.ReadAsStringAsync();

    Assert.Equal("Hello World", stringResult);
}*/
        private readonly IProductService _productService;
        private readonly WebApplicationFactory<Program> _factory;  //pour les tests d'int�gration
        public ProductControllerTests(ProductServiceFixture productServiceFixture, WebApplicationFactory<Program> factory )
        {
            _productService = productServiceFixture.ProductService;
            _factory = factory;
        }


        [Fact]
        [Description("J�ajoute un produit c�t� admin, je v�rifie qu�il est pr�sent c�t� utilisateur avec chacune des donn�es valides")]

        public async Task AjoutProduitAuStock()  //passer le void en async Task
        //public void AjoutProduitAuStock()
        {
            //Arrange
            //CreateClient() cr�e une instance de HttpClient qui suit automatiquement les redirections et g�re cookie.
            //var client = _factory.CreateClient(); 

            ProductViewModel productTest1 = new ProductViewModel()
            {
                Name = "ProductTest1",
                Description = "test d'int�gration 1",
                Stock = "1",
                Details = "",
                Price = "1"
            };

            //Act
            //var response = await client.GetAsync("/"); //appeler la page d'accueil

            IActionResult result = new ProductController(_productService).Create(productTest1);
            IEnumerable<ProductViewModel> products = _productService.GetAllProductsViewModel();

            //Assert
            //var stringResult = await response.Content.ReadAsStringAsync();
           
            Assert.False(true);
        }
    }
}