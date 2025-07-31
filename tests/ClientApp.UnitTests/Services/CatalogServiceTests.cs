/**
 * CatalogServiceTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the catalog service in the eShop client application.
 * It tests the core catalog data retrieval operations including products, brands, and product types
 * using mock services to ensure reliable and predictable test execution.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with [TestClass] and [TestMethod] attributes for test organization
 * 2. Contains three main test methods that validate different aspects of catalog data:
 *    - GetFakeCatalogTest: Tests retrieval of the main product catalog
 *      - Creates a CatalogMockService instance
 *      - Calls GetCatalogAsync() to fetch catalog items
 *      - Validates that the result contains data (count is not zero)
 *    - GetFakeCatalogBrandTest: Tests retrieval of product brands
 *      - Uses CatalogMockService to fetch brand data via GetCatalogBrandAsync()
 *      - Ensures brand data is available for filtering and categorization
 *    - GetFakeCatalogTypeTest: Tests retrieval of product types/categories
 *      - Uses CatalogMockService to fetch type data via GetCatalogTypeAsync()
 *      - Validates that product type categorization data is accessible
 * 3. All tests use mock services to ensure consistent, reliable testing without external dependencies
 * 
 * These tests ensure the catalog service can successfully retrieve all types of catalog-related data
 * that are essential for the eShop client application's product browsing and filtering functionality.
 */
namespace ClientApp.UnitTests.Services;

[TestClass]
public class CatalogServiceTests
{
    [TestMethod]
    public async Task GetFakeCatalogTest()
    {
        var catalogMockService = new CatalogMockService();
        var catalog = await catalogMockService.GetCatalogAsync();

        Assert.AreNotEqual(catalog.Count(), 0);
    }

    [TestMethod]
    public async Task GetFakeCatalogBrandTest()
    {
        var catalogMockService = new CatalogMockService();
        var catalogBrand = await catalogMockService.GetCatalogBrandAsync();

        Assert.AreNotEqual(catalogBrand.Count(), 0);
    }

    [TestMethod]
    public async Task GetFakeCatalogTypeTest()
    {
        var catalogMockService = new CatalogMockService();
        var catalogType = await catalogMockService.GetCatalogTypeAsync();

        Assert.AreNotEqual(catalogType.Count(), 0);
    }
}
