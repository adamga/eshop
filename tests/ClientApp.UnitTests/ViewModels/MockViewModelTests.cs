/**
 * MockViewModelTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the MockViewModel in the eShop client application.
 * It comprehensively tests form validation patterns, property change notifications, and data binding
 * behaviors using a mock view model that represents typical MVVM validation scenarios with Forename
 * and Surname properties.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with MockNavigationService dependency injection in the constructor
 * 2. Tests are organized into validation and property change notification categories:
 *    a) Validation Logic Tests:
 *       - CheckValidationFailsWhenPropertiesAreEmptyTest: Validates that empty properties fail validation
 *         and produce appropriate error collections for both Forename and Surname
 *       - CheckValidationFailsWhenOnlyForenameHasDataTest: Tests partial validation where only one
 *         field is filled, ensuring individual field validation works correctly
 *       - CheckValidationPassesWhenOnlySurnameHasDataTest: Tests the opposite partial validation scenario
 *       - CheckValidationPassesWhenBothPropertiesHaveDataTest: Validates that complete data passes
 *         validation with no errors in error collections
 *    b) Property Change Notification Tests:
 *       - SettingForenamePropertyShouldRaisePropertyChanged: Validates INotifyPropertyChanged implementation
 *         for the Forename property to ensure proper data binding
 *       - SettingSurnamePropertyShouldRaisePropertyChanged: Validates INotifyPropertyChanged implementation
 *         for the Surname property
 * 3. Each test validates multiple aspects: property values, validation states, error collections, and event firing
 * 4. Uses property change event handlers to verify MVVM binding patterns work correctly
 * 
 * These tests ensure the MockViewModel properly demonstrates and validates common MVVM patterns including
 * validation logic, error handling, and property change notifications that are fundamental to form-based
 * user interfaces in the eShop client application.
 */
using ClientApp.UnitTests.Mocks;

namespace ClientApp.UnitTests.ViewModels;

[TestClass]
public class MockViewModelTests
{
    private readonly INavigationService _navigationService;

    public MockViewModelTests()
    {
        _navigationService = new MockNavigationService();
    }

    [TestMethod]
    public void CheckValidationFailsWhenPropertiesAreEmptyTest()
    {
        var mockViewModel = new MockViewModel(_navigationService);

        bool isValid = mockViewModel.Validate();

        Assert.IsFalse(isValid);
        Assert.IsNull(mockViewModel.Forename.Value);
        Assert.IsNull(mockViewModel.Surname.Value);
        Assert.IsFalse(mockViewModel.Forename.IsValid);
        Assert.IsFalse(mockViewModel.Surname.IsValid);
        Assert.AreNotEqual(mockViewModel.Forename.Errors.Count(), 0);
        Assert.AreNotEqual(mockViewModel.Surname.Errors.Count(), 0);
    }

    [TestMethod]
    public void CheckValidationFailsWhenOnlyForenameHasDataTest()
    {
        var mockViewModel = new MockViewModel(_navigationService);
        mockViewModel.Forename.Value = "John";

        bool isValid = mockViewModel.Validate();

        Assert.IsFalse(isValid);
        Assert.IsNotNull(mockViewModel.Forename.Value);
        Assert.IsNull(mockViewModel.Surname.Value);
        Assert.IsTrue(mockViewModel.Forename.IsValid);
        Assert.IsFalse(mockViewModel.Surname.IsValid);
        Assert.AreEqual(mockViewModel.Forename.Errors.Count(), 0);
        Assert.AreNotEqual(mockViewModel.Surname.Errors.Count(), 0);
    }

    [TestMethod]
    public void CheckValidationPassesWhenOnlySurnameHasDataTest()
    {
        var mockViewModel = new MockViewModel(_navigationService);
        mockViewModel.Surname.Value = "Smith";

        bool isValid = mockViewModel.Validate();

        Assert.IsFalse(isValid);
        Assert.IsNull(mockViewModel.Forename.Value);
        Assert.IsNotNull(mockViewModel.Surname.Value);
        Assert.IsFalse(mockViewModel.Forename.IsValid);
        Assert.IsTrue(mockViewModel.Surname.IsValid);
        Assert.AreNotEqual(mockViewModel.Forename.Errors.Count(), 0);
        Assert.AreEqual(mockViewModel.Surname.Errors.Count(), 0);
    }

    [TestMethod]
    public void CheckValidationPassesWhenBothPropertiesHaveDataTest()
    {
        var mockViewModel = new MockViewModel(_navigationService);
        mockViewModel.Forename.Value = "John";
        mockViewModel.Surname.Value = "Smith";

        bool isValid = mockViewModel.Validate();

        Assert.IsTrue(isValid);
        Assert.IsNotNull(mockViewModel.Forename.Value);
        Assert.IsNotNull(mockViewModel.Surname.Value);
        Assert.IsTrue(mockViewModel.Forename.IsValid);
        Assert.IsTrue(mockViewModel.Surname.IsValid);
        Assert.AreEqual(mockViewModel.Forename.Errors.Count(), 0);
        Assert.AreEqual(mockViewModel.Surname.Errors.Count(), 0);
    }

    [TestMethod]
    public void SettingForenamePropertyShouldRaisePropertyChanged()
    {
        bool invoked = false;
        var mockViewModel = new MockViewModel(_navigationService);

        mockViewModel.Forename.PropertyChanged += (_, e) =>
        {
            if (e?.PropertyName?.Equals(nameof(mockViewModel.Forename.Value)) ?? false)
            {
                invoked = true;
            }
        };
        mockViewModel.Forename.Value = "John";

        Assert.IsTrue(invoked);
    }

    [TestMethod]
    public void SettingSurnamePropertyShouldRaisePropertyChanged()
    {
        bool invoked = false;
        var mockViewModel = new MockViewModel(_navigationService);

        mockViewModel.Surname.PropertyChanged += (_, e) =>
        {
            if (e?.PropertyName?.Equals(nameof(mockViewModel.Surname.Value)) ?? false)
            {
                invoked = true;
            }
        };
        mockViewModel.Surname.Value = "Smith";

        Assert.IsTrue(invoked);
    }
}
