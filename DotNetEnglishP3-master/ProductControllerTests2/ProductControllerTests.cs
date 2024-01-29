using System.ComponentModel;

namespace ProductControllerTests2
{
    public class ProductControllerTests2
    {
        private readonly IProductService _productService;

        public ProductControllerTests2(IProductService productService)
        {
            _productService = productService;
        }

        [Fact]
        [Description("J’ajoute un produit côté admin, je vérifie qu’il est présent côté utilisateur avec chacune des données valides")]

        public void AjoutProduitAuStock()
        {
            //Arrange
            ProductViewModel productTest1 = new ProductViewModel()
            {
                Name = "ProductTest1",
                Description = "test d'intégration 1",
                Stock = "1",
                Price = "1"
            };

            //Act
            IActionResult result = new ProductController(_productService).Create(productTest1);
            IEnumerable<ProductViewModel> products = _productService.GetAllProductsViewModel();

            //Arrange

        }
    }
}
