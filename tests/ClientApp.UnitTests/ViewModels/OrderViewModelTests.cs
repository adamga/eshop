/**
 * OrderViewModelTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the OrderDetailViewModel in the eShop client application.
 * It tests order detail display functionality, including order data loading, property initialization,
 * and property change notifications for the order detail view.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with comprehensive mock service setup in the constructor:
 *    - Creates mock instances for Navigation, Settings, and all app services (Basket, Catalog, Order, Identity)
 *    - Initializes AppEnvironmentService with all mock dependencies for isolated testing
 *    - Configures the environment service for testing mode to ensure consistent behavior
 * 2. Contains three focused test methods for order detail functionality:
 *    - OrderPropertyIsNullWhenViewModelInstantiatedTest:
 *      - Creates an OrderDetailViewModel instance with all required dependencies
 *      - Validates that the Order property is null on initial instantiation
 *      - Ensures clean initial state before data loading
 *    - OrderPropertyIsNotNullAfterViewModelInitializationTest:
 *      - Retrieves a test order from the mock order service
 *      - Sets the OrderNumber on the view model to specify which order to load
 *      - Calls InitializeAsync() to trigger the asynchronous order loading process
 *      - Validates that the Order property is populated after initialization
 *    - SettingOrderPropertyShouldRaisePropertyChanged:
 *      - Sets up PropertyChanged event handler to monitor Order property changes
 *      - Performs the same initialization process as the previous test
 *      - Validates that PropertyChanged events are fired correctly for data binding
 * 3. Tests both synchronous initial state validation and asynchronous data loading patterns
 * 
 * These tests ensure the OrderDetailViewModel properly handles order data loading and property
 * change notifications, which are essential for displaying order details in the eShop client application.
 */
using ClientApp.UnitTests.Mocks;
using eShop.ClientApp.Services.Identity;

namespace ClientApp.UnitTests.ViewModels;

[TestClass]
public class OrderViewModelTests
{
    private readonly INavigationService _navigationService;
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;

    public OrderViewModelTests()
    {
        _navigationService = new MockNavigationService();
        _settingsService = new MockSettingsService();

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
    public void OrderPropertyIsNullWhenViewModelInstantiatedTest()
    {
        var orderViewModel = new OrderDetailViewModel(_appEnvironmentService, _navigationService, _settingsService);
        Assert.IsNull(orderViewModel.Order);
    }

    [TestMethod]
    public async Task OrderPropertyIsNotNullAfterViewModelInitializationTest()
    {
        var orderViewModel = new OrderDetailViewModel(_appEnvironmentService, _navigationService, _settingsService);

        var order = await _appEnvironmentService.OrderService.GetOrderAsync(1);

        orderViewModel.OrderNumber = order.OrderNumber;
        await orderViewModel.InitializeAsync();

        Assert.IsNotNull(orderViewModel.Order);
    }

    [TestMethod]
    public async Task SettingOrderPropertyShouldRaisePropertyChanged()
    {
        bool invoked = false;

        var orderViewModel = new OrderDetailViewModel(_appEnvironmentService, _navigationService, _settingsService);

        orderViewModel.PropertyChanged += (_, e) =>
        {
            if (e?.PropertyName?.Equals(nameof(OrderDetailViewModel.Order)) ?? false)
            {
                invoked = true;
            }
        };
        var order = await _appEnvironmentService.OrderService.GetOrderAsync(1);
        orderViewModel.OrderNumber = order.OrderNumber;
        await orderViewModel.InitializeAsync();

        Assert.IsTrue(invoked);
    }
}
