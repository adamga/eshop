/**
 * BasketServiceTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the BasketService, which handles shopping basket
 * operations in the eShop.Basket.API. It tests various scenarios for retrieving and managing
 * customer baskets through the gRPC service interface.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with [TestClass] and [TestMethod] attributes
 * 2. Creates mock instances of IBasketRepository using NSubstitute for isolated unit testing
 * 3. Tests three main scenarios:
 *    - GetBasketReturnsEmptyForNoUser: Validates that requests without user context return empty baskets
 *    - GetBasketReturnsItemsForValidUserId: Verifies that valid authenticated users receive their basket items
 *    - GetBasketReturnsEmptyForInvalidUserId: Ensures that users with invalid/missing IDs get empty baskets
 * 4. Uses TestServerCallContext to simulate gRPC call contexts with various authentication states
 * 5. Mocks HttpContext and ClaimsPrincipal to test different user authentication scenarios
 * 6. Validates responses using Assert methods to ensure correct CustomerBasketResponse behavior
 * 
 * These tests ensure the BasketService properly handles authentication, authorization, and basket
 * retrieval logic while maintaining security and data isolation between users.
 */
using System.Security.Claims;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.Grpc;
using eShop.Basket.API.Model;
using eShop.Basket.UnitTests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using BasketItem = eShop.Basket.API.Model.BasketItem;

namespace eShop.Basket.UnitTests;

[TestClass]
public class BasketServiceTests
{
    [TestMethod]
    public async Task GetBasketReturnsEmptyForNoUser()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        serverCallContext.SetUserState("__HttpContext", new DefaultHttpContext());

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsInstanceOfType<CustomerBasketResponse>(response);
        Assert.AreEqual(response.Items.Count(), 0);
    }

    [TestMethod]
    public async Task GetBasketReturnsItemsForValidUserId()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        List<BasketItem> items = [new BasketItem { Id = "some-id" }];
        mockRepository.GetBasketAsync("1").Returns(Task.FromResult(new CustomerBasket { BuyerId = "1", Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "1")]));
        serverCallContext.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsInstanceOfType<CustomerBasketResponse>(response);
        Assert.AreEqual(response.Items.Count(), 1);
    }

    [TestMethod]
    public async Task GetBasketReturnsEmptyForInvalidUserId()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        List<BasketItem> items = [new BasketItem { Id = "some-id" }];
        mockRepository.GetBasketAsync("1").Returns(Task.FromResult(new CustomerBasket { BuyerId = "1", Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        serverCallContext.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsInstanceOfType<CustomerBasketResponse>(response);
        Assert.AreEqual(response.Items.Count(), 0);
    }
}
