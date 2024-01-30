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
            // Configurer le chemin d'accès de base pour la recherche de appsettings.json
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        [Fact]
        [Description("J’ajoute un produit côté admin, je vérifie qu’il est présent côté utilisateur avec chacune des données valides")]
        public void AjoutProduitAuStock()
        {
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
                var productService = new ProductService(null, new ProductRepository(context), null, null);

                // Créer une instance du ProductController en utilisant le ProductService
                var productController = new ProductController(productService);

                // Act
                var createActionResult = productController.Create(new ProductViewModel
                {
                    Name = "ProductTest1",
                    Description = "test d'intégration 1",
                    Stock = "1",
                    Details = "",
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
            }
        }
    }
}