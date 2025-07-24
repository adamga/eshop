/**
 * MainViewModelTests.cs
 * 
 * Test Purpose:
 * This test class validates the functionality of the MainViewModel in the eShop client application.
 * It tests the main application shell view model's basic command initialization and initial state
 * properties that control the overall application navigation and busy state management.
 * 
 * How the Test Works:
 * 1. Uses MSTest framework with simplified mock setup in the constructor:
 *    - Creates a MockNavigationService instance for navigation dependency injection
 *    - No complex service dependencies as MainViewModel focuses on high-level navigation
 * 2. Contains two focused test methods:
 *    - SettingsCommandIsNotNullWhenViewModelInstantiatedTest:
 *      - Creates a MainViewModel instance with navigation service dependency
 *      - Validates that the SettingsCommand is properly initialized and not null
 *      - Ensures the settings navigation command is available for UI binding
 *    - IsBusyPropertyIsFalseWhenViewModelInstantiatedTest:
 *      - Creates a MainViewModel instance and checks the initial state
 *      - Validates that the IsBusy property is false by default
 *      - Ensures the application starts in a non-busy state
 * 3. Tests focus on basic initialization and default state validation
 * 
 * These tests ensure the MainViewModel properly initializes with correct default states and
 * available commands, providing a stable foundation for the main application navigation shell
 * in the eShop client application.
 */
using ClientApp.UnitTests.Mocks;

namespace ClientApp.UnitTests.ViewModels;

[TestClass]
public class MainViewModelTests
{
    private readonly INavigationService _navigationService;

    public MainViewModelTests()
    {
        _navigationService = new MockNavigationService();
    }

    [TestMethod]
    public void SettingsCommandIsNotNullWhenViewModelInstantiatedTest()
    {
        var mainViewModel = new MainViewModel(_navigationService);
        Assert.IsNotNull(mainViewModel.SettingsCommand);
    }

    [TestMethod]
    public void IsBusyPropertyIsFalseWhenViewModelInstantiatedTest()
    {
        var mainViewModel = new MainViewModel(_navigationService);
        Assert.IsFalse(mainViewModel.IsBusy);
    }
}
