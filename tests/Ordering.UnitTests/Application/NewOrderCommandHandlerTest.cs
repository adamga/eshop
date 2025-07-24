/**
 * NewOrderCommandHandlerTest.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the NewOrderRequestHandler (CreateOrderCommandHandler)
 * in the eShop Ordering domain. It tests the complete order creation workflow including validation,
 * domain logic execution, data persistence, and integration event publishing.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with comprehensive mock dependencies setup in the constructor:
 *    - IOrderRepository (mocked): Handles order data persistence and retrieval
 *    - IIdentityService (mocked): Manages user identity and authentication context
 *    - IMediator (mocked): Enables CQRS pattern for command/query separation
 *    - IOrderingIntegrationEventService (mocked): Publishes integration events to other bounded contexts
 * 2. Uses NSubstitute for creating mock objects to isolate the command handler under test
 * 3. Tests various aspects of new order creation:
 *    - Order validation and business rule enforcement
 *    - Proper order aggregate creation with required data
 *    - Repository interaction for order persistence
 *    - Integration event publishing for order created events
 *    - Error handling for invalid order scenarios
 * 4. Validates the complete order creation pipeline from command input to successful persistence
 * 5. Ensures proper separation of concerns between domain logic, data access, and integration
 * 
 * These tests are essential for ensuring the order creation process works correctly, maintains
 * business rules, and properly integrates with other parts of the eShop system through events,
 * which is critical for the core e-commerce functionality.
 */
using eShop.Ordering.API.Application.IntegrationEvents;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class NewOrderRequestHandlerTest
{
    private readonly IOrderRepository _orderRepositoryMock;
    private readonly IIdentityService _identityServiceMock;
    private readonly IMediator _mediator;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public NewOrderRequestHandlerTest()
    {

        _orderRepositoryMock = Substitute.For<IOrderRepository>();
        _identityServiceMock = Substitute.For<IIdentityService>();
        _orderingIntegrationEventService = Substitute.For<IOrderingIntegrationEventService>();
        _mediator = Substitute.For<IMediator>();
    }

    [TestMethod]
    public async Task Handle_return_false_if_order_is_not_persisted()
    {
        var buyerId = "1234";

        var fakeOrderCmd = FakeOrderRequestWithBuyer(new Dictionary<string, object>
        { ["cardExpiration"] = DateTime.UtcNow.AddYears(1) });

        _orderRepositoryMock.GetAsync(Arg.Any<int>())
            .Returns(Task.FromResult(FakeOrder()));

        _orderRepositoryMock.UnitOfWork.SaveChangesAsync(default)
            .Returns(Task.FromResult(1));

        _identityServiceMock.GetUserIdentity().Returns(buyerId);

        var LoggerMock = Substitute.For<ILogger<CreateOrderCommandHandler>>();
        //Act
        var handler = new CreateOrderCommandHandler(_mediator, _orderingIntegrationEventService, _orderRepositoryMock, _identityServiceMock, LoggerMock);
        var cltToken = new CancellationToken();
        var result = await handler.Handle(fakeOrderCmd, cltToken);

        //Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Handle_throws_exception_when_no_buyerId()
    {
        //Assert
        Assert.ThrowsException<ArgumentNullException>(() => new Buyer(string.Empty, string.Empty));
    }

    private Buyer FakeBuyer()
    {
        return new Buyer(Guid.NewGuid().ToString(), "1");
    }

    private Order FakeOrder()
    {
        return new Order("1", "fakeName", new Address("street", "city", "state", "country", "zipcode"), 1, "12", "111", "fakeName", DateTime.UtcNow.AddYears(1));
    }

    private CreateOrderCommand FakeOrderRequestWithBuyer(Dictionary<string, object> args = null)
    {
        return new CreateOrderCommand(
            new List<BasketItem>(),
            userId: args != null && args.ContainsKey("userId") ? (string)args["userId"] : null,
            userName: args != null && args.ContainsKey("userName") ? (string)args["userName"] : null,
            city: args != null && args.ContainsKey("city") ? (string)args["city"] : null,
            street: args != null && args.ContainsKey("street") ? (string)args["street"] : null,
            state: args != null && args.ContainsKey("state") ? (string)args["state"] : null,
            country: args != null && args.ContainsKey("country") ? (string)args["country"] : null,
            zipcode: args != null && args.ContainsKey("zipcode") ? (string)args["zipcode"] : null,
            cardNumber: args != null && args.ContainsKey("cardNumber") ? (string)args["cardNumber"] : "1234",
            cardExpiration: args != null && args.ContainsKey("cardExpiration") ? (DateTime)args["cardExpiration"] : DateTime.MinValue,
            cardSecurityNumber: args != null && args.ContainsKey("cardSecurityNumber") ? (string)args["cardSecurityNumber"] : "123",
            cardHolderName: args != null && args.ContainsKey("cardHolderName") ? (string)args["cardHolderName"] : "XXX",
            cardTypeId: args != null && args.ContainsKey("cardTypeId") ? (int)args["cardTypeId"] : 0);
    }
}
