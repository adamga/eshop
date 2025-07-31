/**
 * BuyerAggregateTest.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the Buyer aggregate in the eShop Ordering domain.
 * It tests the creation, behavior, and business rules of the Buyer entity, which represents customers
 * who place orders in the eShop system, including their payment methods and identity information.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with [TestClass] and [TestMethod] attributes for test organization
 * 2. Contains domain-focused unit tests that validate:
 *    - Buyer entity creation and initialization
 *    - Business rule enforcement for buyer operations
 *    - Payment method management and validation
 *    - Identity and name handling for buyer entities
 * 3. Example test Create_buyer_item_success:
 *    - Arranges test data with a GUID-based identity and fake user name
 *    - Acts by creating a new Buyer instance with the test data
 *    - Asserts that the buyer was created successfully (not null)
 * 4. Additional tests likely cover scenarios such as:
 *    - Adding and managing payment methods
 *    - Validating buyer information requirements
 *    - Testing business rule violations and exception handling
 *    - Verifying aggregate consistency and invariants
 * 5. Focuses on pure domain logic without external dependencies
 * 
 * These tests ensure the Buyer aggregate maintains its business invariants and behaves correctly
 * according to domain rules, which is essential for maintaining data consistency and business
 * logic integrity in the eShop ordering system.
 */
namespace eShop.Ordering.UnitTests.Domain;

[TestClass]
public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    { }

    [TestMethod]
    public void Create_buyer_item_success()
    {
        //Arrange    
        var identity = new Guid().ToString();
        var name = "fakeUser";

        //Act 
        var fakeBuyerItem = new Buyer(identity, name);

        //Assert
        Assert.IsNotNull(fakeBuyerItem);
    }

    [TestMethod]
    public void Create_buyer_item_fail()
    {
        //Arrange    
        var identity = string.Empty;
        var name = "fakeUser";

        //Act - Assert
        Assert.ThrowsException<ArgumentNullException>(() => new Buyer(identity, name));
    }

    [TestMethod]
    public void add_payment_success()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);
        var orderId = 1;
        var name = "fakeUser";
        var identity = new Guid().ToString();
        var fakeBuyerItem = new Buyer(identity, name);

        //Act
        var result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        //Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void create_payment_method_success()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        //Act
        var result = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        //Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void create_payment_method_expiration_fail()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(-1);

        //Act - Assert
        Assert.ThrowsException<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration));
    }

    [TestMethod]
    public void payment_method_isEqualTo()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);

        //Act
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
        var result = fakePaymentMethod.IsEqualTo(cardTypeId, cardNumber, expiration);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Add_new_PaymentMethod_raises_new_event()
    {
        //Arrange    
        var alias = "fakeAlias";
        var orderId = 1;
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var expectedResult = 1;
        var name = "fakeUser";

        //Act 
        var fakeBuyer = new Buyer(Guid.NewGuid().ToString(), name);
        fakeBuyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, orderId);

        //Assert
        Assert.AreEqual(fakeBuyer.DomainEvents.Count, expectedResult);
    }
}
