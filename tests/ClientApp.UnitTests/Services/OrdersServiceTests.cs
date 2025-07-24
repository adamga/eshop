/**
 * OrdersServiceTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the orders service in the eShop client application.
 * It tests order retrieval operations both for individual orders and collections of orders using
 * mock services to ensure reliable order management functionality.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with [TestClass] and [TestMethod] attributes for test structure
 * 2. Sets up dependencies in the constructor:
 *    - Initializes _settingsService with MockSettingsService for configuration support
 * 3. Contains two main test methods for order operations:
 *    - GetFakeOrderTest: Tests individual order retrieval
 *      - Creates an OrderMockService instance
 *      - Calls GetOrderAsync(1) to fetch a specific order by ID
 *      - Validates that the returned order is not null, ensuring successful retrieval
 *    - GetFakeOrdersTest: Tests collection of orders retrieval
 *      - Uses OrderMockService to fetch multiple orders via GetOrdersAsync()
 *      - Validates that the result contains data (count is not zero)
 *      - Ensures order listing functionality works correctly
 * 4. Uses mock services to isolate the testing from external dependencies and provide consistent test data
 * 
 * These tests ensure the orders service can successfully retrieve both individual orders and order
 * collections, which are critical for order history, tracking, and management features in the client app.
 */
using ClientApp.UnitTests.Mocks;

namespace ClientApp.UnitTests.Services;

[TestClass]
public class OrdersServiceTests
{
    private readonly ISettingsService _settingsService;

    public OrdersServiceTests()
    {
        _settingsService = new MockSettingsService();
    }
    
    [TestMethod]
    public async Task GetFakeOrderTest()
    {
        var ordersMockService = new OrderMockService();
        var order = await ordersMockService.GetOrderAsync(1);

        Assert.IsNotNull(order);
    }

    [TestMethod]
    public async Task GetFakeOrdersTest()
    {
        var ordersMockService = new OrderMockService();
        var result = await ordersMockService.GetOrdersAsync();

        Assert.AreNotEqual(result.Count(), 0);
    }
}
