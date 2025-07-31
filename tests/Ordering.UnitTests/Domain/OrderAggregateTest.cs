/**
 * OrderAggregateTest.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the Order aggregate in the eShop Ordering domain.
 * It comprehensively tests order creation, order item management, business rule enforcement, and
 * domain event handling for the core ordering functionality in the eShop system.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with [TestClass] and [TestMethod] attributes for test organization
 * 2. Tests multiple aspects of the Order aggregate and OrderItem entities:
 *    a) Order Item Creation and Validation:
 *       - Create_order_item_success: Tests successful OrderItem creation with valid parameters
 *       - Invalid_number_of_units: Validates that negative unit quantities throw OrderingDomainException
 *       - Invalid_total_of_order_item_lower_than_discount_applied: Ensures discount cannot exceed item value
 *       - Invalid_discount_setting: Tests that negative discounts are rejected
 *       - Invalid_units_setting: Validates that adding negative units throws exceptions
 *    b) Order Business Logic:
 *       - when_add_two_times_on_the_same_item_then_the_total_of_order_should_be_the_sum_of_the_two_items:
 *         Tests order total calculation when same item is added multiple times
 *    c) Domain Events Testing:
 *       - Add_new_Order_raises_new_event: Validates that order creation triggers domain events
 *       - Add_event_Order_explicitly_raises_new_event: Tests explicit domain event addition
 *       - Remove_event_Order_explicitly: Validates domain event removal functionality
 * 3. Uses builder patterns (OrderBuilder, AddressBuilder) for test data setup
 * 4. Validates business rules using Assert.ThrowsException for domain exceptions
 * 5. Tests both successful scenarios and failure cases to ensure robust error handling
 * 
 * These tests ensure the Order aggregate maintains business invariants, handles domain events
 * correctly, and enforces business rules essential for reliable order processing in the eShop system.
 */
namespace eShop.Ordering.UnitTests.Domain;

using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.UnitTests.Domain;

[TestClass]
public class OrderAggregateTest
{
    public OrderAggregateTest()
    { }

    [TestMethod]
    public void Create_order_item_success()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        //Act 
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.IsNotNull(fakeOrderItem);
    }

    [TestMethod]
    public void Invalid_number_of_units()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = -1;

        //Act - Assert
        Assert.ThrowsException<OrderingDomainException>(() => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units));
    }

    [TestMethod]
    public void Invalid_total_of_order_item_lower_than_discount_applied()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 1;
        
        //Act - Assert
        Assert.ThrowsException<OrderingDomainException>(() => new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units));       
    }

    [TestMethod]
    public void Invalid_discount_setting()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        //Act 
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.ThrowsException<OrderingDomainException>(() => fakeOrderItem.SetNewDiscount(-1));
    }

    [TestMethod]
    public void Invalid_units_setting()
    {
        //Arrange    
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        //Act 
        var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);

        //Assert
        Assert.ThrowsException<OrderingDomainException>(() => fakeOrderItem.AddUnits(-1));
    }

    [TestMethod]
    public void when_add_two_times_on_the_same_item_then_the_total_of_order_should_be_the_sum_of_the_two_items()
    {
        var address = new AddressBuilder().Build();
        var order = new OrderBuilder(address)
            .AddOne(1, "cup", 10.0m, 0, string.Empty)
            .AddOne(1, "cup", 10.0m, 0, string.Empty)
            .Build();

        Assert.AreEqual(20.0m, order.GetTotal());
    }

    [TestMethod]
    public void Add_new_Order_raises_new_event()
    {
        //Arrange
        var street = "fakeStreet";
        var city = "FakeCity";
        var state = "fakeState";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var expectedResult = 1;

        //Act 
        var fakeOrder = new Order("1", "fakeName", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);

        //Assert
        Assert.AreEqual(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [TestMethod]
    public void Add_event_Order_explicitly_raises_new_event()
    {
        //Arrange   
        var street = "fakeStreet";
        var city = "FakeCity";
        var state = "fakeState";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var expectedResult = 2;

        //Act 
        var fakeOrder = new Order("1", "fakeName", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        fakeOrder.AddDomainEvent(new OrderStartedDomainEvent(fakeOrder, "fakeName", "1", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration));
        //Assert
        Assert.AreEqual(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [TestMethod]
    public void Remove_event_Order_explicitly()
    {
        //Arrange    
        var street = "fakeStreet";
        var city = "FakeCity";
        var state = "fakeState";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var fakeOrder = new Order("1", "fakeName", new Address(street, city, state, country, zipcode), cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        var @fakeEvent = new OrderStartedDomainEvent(fakeOrder, "1", "fakeName", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
        var expectedResult = 1;

        //Act         
        fakeOrder.AddDomainEvent(@fakeEvent);
        fakeOrder.RemoveDomainEvent(@fakeEvent);
        //Assert
        Assert.AreEqual(fakeOrder.DomainEvents.Count, expectedResult);
    }
}
