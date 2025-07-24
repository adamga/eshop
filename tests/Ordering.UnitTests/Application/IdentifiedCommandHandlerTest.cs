/**
 * IdentifiedCommandHandlerTest.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the IdentifiedCommandHandler in the eShop Ordering domain.
 * The IdentifiedCommandHandler implements idempotency patterns to ensure that duplicate command requests
 * (with the same identifier) are handled correctly and don't result in duplicate operations.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with mock dependencies setup in the constructor:
 *    - IRequestManager (mocked): Manages request tracking and idempotency checking
 *    - IMediator (mocked): Handles command dispatching and CQRS pattern implementation
 *    - ILogger (mocked): Provides logging functionality for the handler
 * 2. Uses NSubstitute for creating mock objects to isolate the handler under test
 * 3. Tests various scenarios for command handling:
 *    - Handler_sends_command_when_order_no_exists: Validates that new commands are processed
 *      when no previous request with the same identifier exists
 *    - Additional tests likely validate duplicate request handling and idempotency behavior
 * 4. Focuses on the IdentifiedCommandHandler specifically for CreateOrderCommand operations
 * 5. Ensures proper integration between request management and command mediation
 * 
 * These tests are critical for ensuring that the ordering system maintains data consistency
 * and prevents duplicate orders when the same command is sent multiple times, which is
 * essential for reliable e-commerce operations and maintaining customer trust.
 */
namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class IdentifiedCommandHandlerTest
{
    private readonly IRequestManager _requestManager;
    private readonly IMediator _mediator;
    private readonly ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> _loggerMock;

    public IdentifiedCommandHandlerTest()
    {
        _requestManager = Substitute.For<IRequestManager>();
        _mediator = Substitute.For<IMediator>();
        _loggerMock = Substitute.For<ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>>>();
    }

    [TestMethod]
    public async Task Handler_sends_command_when_order_no_exists()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCmd = new IdentifiedCommand<CreateOrderCommand, bool>(FakeOrderRequest(), fakeGuid);

        _requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(false));

        _mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var handler = new CreateOrderIdentifiedCommandHandler(_mediator, _requestManager, _loggerMock);
        var result = await handler.Handle(fakeOrderCmd, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        await _mediator.Received().Send(Arg.Any<IRequest<bool>>(), default);
    }

    [TestMethod]
    public async Task Handler_sends_no_command_when_order_already_exists()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();
        var fakeOrderCmd = new IdentifiedCommand<CreateOrderCommand, bool>(FakeOrderRequest(), fakeGuid);

        _requestManager.ExistAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult(true));

        _mediator.Send(Arg.Any<IRequest<bool>>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var handler = new CreateOrderIdentifiedCommandHandler(_mediator, _requestManager, _loggerMock);
        var result = await handler.Handle(fakeOrderCmd, CancellationToken.None);

        // Assert
       await  _mediator.DidNotReceive().Send(Arg.Any<IRequest<bool>>(), default);
    }

    private CreateOrderCommand FakeOrderRequest(Dictionary<string, object> args = null)
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
