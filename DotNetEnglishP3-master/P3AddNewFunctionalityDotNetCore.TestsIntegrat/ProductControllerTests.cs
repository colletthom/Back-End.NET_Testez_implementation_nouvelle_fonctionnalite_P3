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
            // Configurer le chemin d'acc�s de base pour la recherche de appsettings.json
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

            // R�cup�rer la cha�ne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("P3Referential");

            // Utiliser la cha�ne de connexion pour cr�er une instance du contexte de base de donn�es
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            //using et ce qu'il y a dans l'accolade permet de lib�r� les connexions � la BDD
            using (var context = new P3Referential(options, _configuration))
            {
                // Cr�er une instance du ProductService en utilisant le contexte de la base de donn�es
                var productService = new ProductService(cart, new ProductRepository(context), null, null);

                // Cr�er une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest1",
                    Description = "test d'int�gration 1",
                    Stock = "1",
                    Details = "test",
                    Price = "1"
                }) as RedirectToActionResult;

                // Assert
                Assert.NotNull(createActionResult);
                Assert.Equal("Admin", createActionResult.ActionName); // V�rifier la redirection vers l'action Admin

                // R�cup�rer la liste des produits apr�s l'ajout
                var productsAfterAdd = productService.GetAllProductsViewModel();

                // V�rifier si le produit ajout� est pr�sent dans la liste
                var addedProduct = productsAfterAdd.FirstOrDefault(p => p.Name == "ProductTest1");

                Assert.NotNull(addedProduct); // V�rifier que le produit ajout� est trouv�
                Assert.Equal("ProductTest1", addedProduct.Name);
                Assert.Equal("test d'int�gration 1", addedProduct.Description);
                Assert.Equal("1", addedProduct.Stock);
                Assert.Equal("test", addedProduct.Details);
                Assert.Equal("1", addedProduct.Price);

                //Nettoyage du produit cr�� pour le test 
                productController.DeleteProduct(addedProduct.Id);
            }
        }

        [Fact]
        [Description("\r\nI add a product on the admin side, I delete it and check that it has been deleted")]

        public void DeleteProductInStock()
        {
            //Arrange
            var cart = new Cart();

            // R�cup�rer la cha�ne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("P3Referential");

            // Utiliser la cha�ne de connexion pour cr�er une instance du contexte de base de donn�es
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            //using et ce qu'il y a dans l'accolade permet de lib�r� les connexions � la BDD
            using (var context = new P3Referential(options, _configuration))
            {
                // Cr�er une instance du ProductService en utilisant le contexte de la base de donn�es
                var productService = new ProductService(cart, new ProductRepository(context), null, null);

                // Cr�er une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest2",
                    Description = "test d'int�gration 2",
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

            // R�cup�rer la cha�ne de connexion depuis appsettings.json
            var connectionString = _configuration.GetConnectionString("P3Referential");

            // Utiliser la cha�ne de connexion pour cr�er une instance du contexte de base de donn�es
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            //using et ce qu'il y a dans l'accolade permet de lib�r� les connexions � la BDD
            using (var context = new P3Referential(options, _configuration))
            {
                // Cr�er une instance du ProductService en utilisant le contexte de la base de donn�es
                var productService = new ProductService(cart, new ProductRepository(context), null, null);

                // Cr�er une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest3",
                    Description = "test d'int�gration 3",
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

                //Nettoyage de la base de donn�e du produit cr��
                cart.RemoveLine(produitTest3);
                productController.DeleteProduct(produitTest3.Id);

            }
        }
    }
}