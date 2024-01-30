using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
using P3AddNewFunctionalityDotNetCore.Data;
using Microsoft.Extensions.Configuration;
using P3AddNewFunctionalityDotNetCore.Models.Entities;


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
        [Description("J�ajoute un produit c�t� admin, je v�rifie qu�il est pr�sent c�t� utilisateur avec chacune des donn�es valides")]
        public void AjoutProduitAuStock()
        {
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
                var productService = new ProductService(null, new ProductRepository(context), null, null);

                // Cr�er une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest1",
                    Description = "test d'int�gration 1",
                    Stock = "1",
                    Details = "",
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
            }
        }
    }
}