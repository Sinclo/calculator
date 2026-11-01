// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using OpenQA.Selenium.Appium.Windows;

namespace CalculatorUITestFramework
{
    /// <summary>
    /// This class contains the UI automation objects and helper methods available when the Calculator is in Date Calculation Mode.
    /// </summary>
    public class DateCalculatorPage
    {
        private WindowsDriver<WindowsElement> session => CalculatorDriver.Instance.CalculatorSession;
        public NavigationMenu NavigationMenu = new NavigationMenu();
        public AoTCalculatorPage AoTCalculatorPage = new AoTCalculatorPage();

        public void NavigateToDateCalculator()
        {
            NavigationMenu.ChangeCalculatorMode(CalculatorMode.DateCalculator);
        }

        public void ClearAll()
        {
            AoTCalculatorPage.NavigateToFullViewMode();
        }
    }
}
