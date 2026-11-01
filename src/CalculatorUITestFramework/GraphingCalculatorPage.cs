// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using OpenQA.Selenium.Appium.Windows;

namespace CalculatorUITestFramework
{
    /// <summary>
    /// This class contains the UI automation objects and helper methods available when the Calculator is in Graphing Mode.
    /// </summary>
    public class GraphingCalculatorPage
    {
        private WindowsDriver<WindowsElement> session => CalculatorDriver.Instance.CalculatorSession;
        public NavigationMenu NavigationMenu = new NavigationMenu();
        public AoTCalculatorPage AoTCalculatorPage = new AoTCalculatorPage();

        /// <summary>
        /// Toggle switch to flip between graph view and equation view. Only visible in compact (AoT) mode.
        /// </summary>
        public WindowsElement SwitchModeToggleButton => session.TryFindElementByAccessibilityId("SwitchModeToggleButton");

        /// <summary>
        /// The equation expression input box.
        /// </summary>
        public WindowsElement MathRichEditBox => session.TryFindElementByAccessibilityId("MathRichEditBox");

        public void NavigateToGraphingCalculator()
        {
            NavigationMenu.ChangeCalculatorMode(CalculatorMode.GraphingCalculator);
        }

        public void ClearAll()
        {
            AoTCalculatorPage.NavigateToFullViewMode();
        }
    }
}
