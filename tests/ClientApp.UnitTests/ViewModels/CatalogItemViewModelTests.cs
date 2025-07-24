/**
 * CatalogItemViewModelTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the CatalogItemViewModel in the eShop client application.
 * It focuses on testing the view model's command initialization, execution, and messaging patterns used
 * for adding catalog items to the shopping basket.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with comprehensive mock service setup in the constructor:
 *    - Creates mock instances for all required services (Navigation, Catalog, Order, Identity, Basket)
 *    - Initializes AppEnvironmentService with all mock dependencies for isolated testing
 *    - Configures the environment service for testing mode
 * 2. Contains two main test methods:
 *    - AddCatalogItemCommandIsNotNullTest: 
 *      - Creates a CatalogItemViewModel instance with configured dependencies
 *      - Validates that the AddCatalogItemCommand is properly initialized and not null
 *      - Ensures the command property is available for UI binding
 *    - AddCatalogItemCommandSendsAddProductMessageTest:
 *      - Sets up a message listener using WeakReferenceMessenger for ProductCountChangedMessage
 *      - Creates a test catalog item with sample data (ID: 123, Name: "test", Price: 1.23)
 *      - Executes the AddCatalogItemCommand and verifies it sends the expected message
 *      - Validates the MVVM messaging pattern works correctly for basket updates
 * 3. Tests both synchronous property validation and asynchronous command execution with messaging
 * 
 * These tests ensure the CatalogItemViewModel properly handles catalog item addition to the basket,
 * including command availability and cross-component communication through messaging patterns.
 */
using ClientApp.UnitTests.Mocks;
using CommunityToolkit.Mvvm.Messaging;
using eShop.ClientApp.Messages;
using eShop.ClientApp.Models.Catalog;
using eShop.ClientApp.Services.Identity;

namespace ClientApp.UnitTests.ViewModels;

[TestClass]
public class CatalogItemViewModelTests
{
    private readonly INavigationService _navigationService;
    private readonly IAppEnvironmentService _appEnvironmentService;

    public CatalogItemViewModelTests()
    {
        _navigationService = new MockNavigationService();
        var mockCatalogService = new CatalogMockService();
        var mockOrderService = new OrderMockService();
        var mockIdentityService = new IdentityMockService();
        var mockBasketService = new BasketMockService();

        _appEnvironmentService =
            new AppEnvironmentService(
                mockBasketService, mockBasketService,
                mockCatalogService, mockCatalogService,
                mockOrderService, mockOrderService,
                mockIdentityService, mockIdentityService);

        _appEnvironmentService.UpdateDependencies(true);
    }

    [TestMethod]
    public void AddCatalogItemCommandIsNotNullTest()
    {
        var CatalogItemViewModel = new CatalogItemViewModel(_appEnvironmentService, _navigationService);
        Assert.IsNotNull(CatalogItemViewModel.AddCatalogItemCommand);
    }
    
    [TestMethod]
    public async Task AddCatalogItemCommandSendsAddProductMessageTest()
    {
        bool messageReceived = false;

        var catalogItemViewModel = new CatalogItemViewModel(_appEnvironmentService, _navigationService);

        catalogItemViewModel.CatalogItem = new CatalogItem {Id = 123, Name = "test", Price = 1.23m,};
        
        WeakReferenceMessenger.Default
            .Register<CatalogItemViewModelTests, ProductCountChangedMessage>(
                this,
                (_, message) =>
                {
                    messageReceived = true;
                });
        
        await catalogItemViewModel.AddCatalogItemCommand.ExecuteUntilComplete();

        Assert.IsTrue(messageReceived);
    }

}
