/**
 * CatalogViewModelTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the CatalogViewModel in the eShop client application.
 * It comprehensively tests the catalog browsing interface including product listing, filtering by brands
 * and types, command initialization, property initialization states, and data binding behaviors.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with extensive mock service setup in the constructor:
 *    - Creates mock instances for all required services (Navigation, Basket, Catalog, Order, Identity)
 *    - Initializes AppEnvironmentService with all mock dependencies for isolated testing
 *    - Configures the environment service for testing mode
 * 2. Tests are organized into several categories:
 *    a) Command Initialization Tests:
 *       - FilterCommand and ClearFilterCommand are properly initialized and not null
 *    b) Initial State Tests:
 *       - Products, Brands, Types collections are empty on instantiation
 *       - SelectedBrand, SelectedType are null initially
 *       - IsFiltering property is false by default
 *    c) Post-Initialization Tests:
 *       - After InitializeAsync(), Products, Brands, Types are populated (not null)
 *    d) Property Change Notification Tests:
 *       - BadgeCount property raises PropertyChanged events correctly
 *    e) Command Behavior Tests:
 *       - ClearFilterCommand properly resets filter selections and maintains product collection
 * 3. Uses async/await patterns to test asynchronous initialization and command execution
 * 4. Validates MVVM property change notification patterns using PropertyChanged events
 * 
 * These tests ensure the CatalogViewModel properly manages catalog data, filtering states, and
 * user interactions for the product browsing experience in the eShop client application.
 */
using ClientApp.UnitTests.Mocks;
using eShop.ClientApp.Services.Identity;

namespace ClientApp.UnitTests.ViewModels;

[TestClass]
public class CatalogViewModelTests
{
    private readonly INavigationService _navigationService;
    private readonly IAppEnvironmentService _appEnvironmentService;

    public CatalogViewModelTests()
    {
        _navigationService = new MockNavigationService();

        var mockBasketService = new BasketMockService();
        var mockCatalogService = new CatalogMockService();
        var mockOrderService = new OrderMockService();
        var mockIdentityService = new IdentityMockService();

        _appEnvironmentService =
            new AppEnvironmentService(
                mockBasketService, mockBasketService,
                mockCatalogService, mockCatalogService,
                mockOrderService, mockOrderService,
                mockIdentityService, mockIdentityService);

        _appEnvironmentService.UpdateDependencies(true);
    }
    
    [TestMethod]
    public void FilterCommandIsNotNullTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.IsNotNull(catalogViewModel.FilterCommand);
    }

    [TestMethod]
    public void ClearFilterCommandIsNotNullTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.IsNotNull(catalogViewModel.ClearFilterCommand);
    }

    [TestMethod]
    public void ProductsPropertyIsEmptyWhenViewModelInstantiatedTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.AreEqual(catalogViewModel.Products.Count,0);
    }

    [TestMethod]
    public void BrandsPropertyIsEmptyWhenViewModelInstantiatedTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.AreEqual(catalogViewModel.Brands.Count, 0);
    }

    [TestMethod]
    public void BrandPropertyIsNullWhenViewModelInstantiatedTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.IsNull(catalogViewModel.SelectedBrand);
    }

    [TestMethod]
    public void TypesPropertyIsEmptyWhenViewModelInstantiatedTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.AreEqual(catalogViewModel.Types.Count, 0);
    }

    [TestMethod]
    public void TypePropertyIsNullWhenViewModelInstantiatedTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.IsNull(catalogViewModel.SelectedType);
    }

    [TestMethod]
    public void IsFilterPropertyIsFalseWhenViewModelInstantiatedTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);
        Assert.IsFalse(catalogViewModel.IsFiltering);
    }

    [TestMethod]
    public async Task ProductsPropertyIsNotNullAfterViewModelInitializationTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);

        await catalogViewModel.InitializeAsync();

        Assert.IsNotNull(catalogViewModel.Products);
    }

    [TestMethod]
    public async Task BrandsPropertyIsNotNullAfterViewModelInitializationTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);

        await catalogViewModel.InitializeAsync();

        Assert.IsNotNull(catalogViewModel.Brands);
    }

    [TestMethod]
    public async Task TypesPropertyIsNotNullAfterViewModelInitializationTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);

        await catalogViewModel.InitializeAsync();

        Assert.IsNotNull(catalogViewModel.Types);
    }

    [TestMethod]
    public async Task SettingBadgeCountPropertyShouldRaisePropertyChanged()
    {
        bool invoked = false;

        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);

        catalogViewModel.PropertyChanged += (_, e) =>
        {
            if (e?.PropertyName?.Equals(nameof(CatalogViewModel.BadgeCount)) ?? false)
            {
                invoked = true;
            }
        };
        await catalogViewModel.InitializeAsync();

        Assert.IsTrue(invoked);
    }

    [TestMethod]
    public async Task ClearFilterCommandResetsPropertiesTest()
    {
        var catalogViewModel = new CatalogViewModel(_appEnvironmentService, _navigationService);

        await catalogViewModel.InitializeAsync();
        await catalogViewModel.ClearFilterCommand.ExecuteUntilComplete(null);

        Assert.IsNull(catalogViewModel.SelectedBrand);
        Assert.IsNull(catalogViewModel.SelectedType);
        Assert.IsNotNull(catalogViewModel.Products);
    }
}
