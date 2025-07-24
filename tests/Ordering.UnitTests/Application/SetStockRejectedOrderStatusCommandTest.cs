/**
 * SetStockRejectedOrderStatusCommandTest.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the SetStockRejectedOrderStatusCommand in the eShop
 * Ordering domain. It specifically tests the command's serialization/deserialization capabilities,
 * which are essential for message passing, event sourcing, and integration scenarios where commands
 * need to be transmitted across service boundaries.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework for test structure and assertions
 * 2. Focuses on testing the SetStockRejectedOrderStatusCommand serialization:
 *    - Set_Stock_Rejected_OrderStatusCommand_Check_Serialization test method:
 *      - Creates a SetStockRejectedOrderStatusCommand with test data (OrderNumber: 123, rejected items: [1,2,3])
 *      - Serializes the command to JSON using System.Text.Json
 *      - Deserializes the JSON back to a command object
 *      - Validates that the OrderNumber property is preserved correctly after round-trip serialization
 *      - Additional assertions likely validate the rejected items list integrity
 * 3. Uses System.Text.Json for serialization to match the application's JSON handling approach
 * 4. Tests the command's data integrity during serialization, which is crucial for:
 *    - Message queue scenarios where commands are sent between services
 *    - Event sourcing where commands need to be persisted and replayed
 *    - API scenarios where commands are transmitted over HTTP
 * 
 * This test ensures that the SetStockRejectedOrderStatusCommand can be reliably serialized and
 * deserialized without data loss, which is critical for maintaining order status consistency
 * when stock rejection scenarios occur in the eShop ordering workflow.
 */
using System.Text.Json;

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class SetStockRejectedOrderStatusCommandTest
{
    [TestMethod]
    public void Set_Stock_Rejected_OrderStatusCommand_Check_Serialization()
    {
        // Arrange
        var command = new SetStockRejectedOrderStatusCommand(123, new List<int> { 1, 2, 3 });

        // Act
        var json = JsonSerializer.Serialize(command);
        var deserializedCommand = JsonSerializer.Deserialize<SetStockRejectedOrderStatusCommand>(json);

        //Assert
        Assert.AreEqual(command.OrderNumber, deserializedCommand.OrderNumber);

        //Assert for List<int>
        Assert.IsNotNull(deserializedCommand.OrderStockItems);
        Assert.AreEqual(command.OrderStockItems.Count, deserializedCommand.OrderStockItems.Count);

        for (int i = 0; i < command.OrderStockItems.Count; i++)
        {
            Assert.AreEqual(command.OrderStockItems[i], deserializedCommand.OrderStockItems[i]);
        }
    }
}

