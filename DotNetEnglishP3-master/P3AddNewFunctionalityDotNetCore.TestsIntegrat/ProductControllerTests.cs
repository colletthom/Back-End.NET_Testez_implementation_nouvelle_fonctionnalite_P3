using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using P3AddNewFunctionalityDotNetCore.Data;
using Microsoft.Extensions.Configuration;
using NuGet.Configuration;


namespace P3AddNewFunctionalityDotNetCore.TestsIntegrat
{
    [Collection("ProductServiceCollection")]
   
    public class ProductControllerTests
    {
 
        private readonly IConfiguration _configuration;

        public ProductControllerTests()
        {
            // Configurer le chemin d'accès de base pour la recherche de appsettings.json
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            

            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();           
        }

        [Fact]
        [Description("I add a product on the admin side, I check that it is present on the user side with each valid data")]
        public void AddProductInstock()
        {
            //Arrange
            var cart = new Cart();

            // Récupérer la chaîne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("P3Referential");

            // Utiliser la chaîne de connexion pour créer une instance du contexte de base de données
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            //using et ce qu'il y a dans l'accolade permet de libéré les connexions à la BDD
            using (var context = new P3Referential(options, _configuration))
            {
                // Créer une instance du ProductService en utilisant le contexte de la base de données
                var productService = new ProductService(cart, new ProductRepository(context), null, null);

                // Créer une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest1",
                    Description = "test d'intégration 1",
                    Stock = "1",
                    Details = "test",
                    Price = "1"
                }) as RedirectToActionResult;

                // Assert
                Assert.NotNull(createActionResult);
                Assert.Equal("Admin", createActionResult.ActionName); // Vérifier la redirection vers l'action Admin

                // Récupérer la liste des produits après l'ajout
                var productsAfterAdd = productService.GetAllProductsViewModel();

                // Vérifier si le produit ajouté est présent dans la liste
                var addedProduct = productsAfterAdd.FirstOrDefault(p => p.Name == "ProductTest1");

                Assert.NotNull(addedProduct); // Vérifier que le produit ajouté est trouvé
                Assert.Equal("ProductTest1", addedProduct.Name);
                Assert.Equal("test d'intégration 1", addedProduct.Description);
                Assert.Equal("1", addedProduct.Stock);
                Assert.Equal("test", addedProduct.Details);
                Assert.Equal("1", addedProduct.Price);

                //Nettoyage du produit créé pour le test 
                productController.DeleteProduct(addedProduct.Id);
            }
        }

        [Fact]
        [Description("\r\nI add a product on the admin side, I delete it and check that it has been deleted")]

        public void DeleteProductInStock()
        {
            //Arrange
            var cart = new Cart();

            // Récupérer la chaîne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("P3Referential");

            // Utiliser la chaîne de connexion pour créer une instance du contexte de base de données
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            //using et ce qu'il y a dans l'accolade permet de libéré les connexions à la BDD
            using (var context = new P3Referential(options, _configuration))
            {
                // Créer une instance du ProductService en utilisant le contexte de la base de données
                var productService = new ProductService(cart, new ProductRepository(context), null, null);

                // Créer une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest2",
                    Description = "test d'intégration 2",
                    Stock = "2",
                    Details = "test2",
                    Price = "2"
                });

                // Assert
                var productsAfterAdd = productService.GetAllProducts();
                var produitTest2 = productsAfterAdd.FirstOrDefault(p => p.Name == "ProductTest2");
                Assert.NotNull(produitTest2);
                productController.DeleteProduct(produitTest2.Id);
                
                var productsAfterDelete = productService.GetAllProducts();
                var verifProduitTest2 = productsAfterDelete.FirstOrDefault(p => p.Name == "ProductTest2");
                Assert.Null(verifProduitTest2);
            }
        }

        [Fact]
        [Description("I add a product on the admin side, I delete it and check that it has been deleted")]

        public void AddProductInStockAndInOrder()
        {
            //Arrange
            var cart = new Cart();

            // Récupérer la chaîne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("P3Referential");

            // Utiliser la chaîne de connexion pour créer une instance du contexte de base de données
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            //using et ce qu'il y a dans l'accolade permet de libéré les connexions à la BDD
            using (var context = new P3Referential(options, _configuration))
            {
                // Créer une instance du ProductService en utilisant le contexte de la base de données
                var productService = new ProductService(cart, new ProductRepository(context), null, null);

                // Créer une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest3",
                    Description = "test d'intégration 3",
                    Stock = "3",
                    Details = "test3",
                    Price = "3"
                });

                var productsAfterAdd = productService.GetAllProducts();
                var produitTest3 = productsAfterAdd.FirstOrDefault(p => p.Name == "ProductTest3");

                cart.AddItem(produitTest3, 1);

                // Assert
                bool PresenceProduitTest3 = false;
                foreach (var l in cart.Lines)
                {
                    if (l.Product == produitTest3)
                        PresenceProduitTest3 = true;
                }
                Assert.True(PresenceProduitTest3);

                //Nettoyage de la base de donnée du produit créé
                cart.RemoveLine(produitTest3);
                productController.DeleteProduct(produitTest3.Id);

            }
        }
    }
}