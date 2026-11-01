// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CalculatorUITestFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace CalculatorUITests
{
    [TestClass]
    public class GraphingModeFunctionalTests
    {
        private static readonly GraphingCalculatorPage page = new GraphingCalculatorPage();

        /// <summary>
        /// Initializes the WinAppDriver web driver session.
        /// </summary>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Create session to launch a Calculator window
            CalculatorDriver.Instance.SetupCalculatorSession(context);

            // Ensure that calculator is in graphing mode
            page.NavigateToGraphingCalculator();
        }

        /// <summary>
        /// Closes the app and WinAppDriver web driver session.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Tear down Calculator session.
            CalculatorDriver.Instance.TearDownCalculatorSession();
        }

        /// <summary>
        /// Ensures the calculator is in a cleared state
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            page.ClearAll();
            CalculatorApp.EnsureCalculatorHasFocus();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            page.ClearAll();
        }

        #region AlwaysOnTop tests

        [TestMethod]
        [Priority(2)]
        public void AoT_ButtonInGraphing()
        {
            CalculatorApp.EnsureCalculatorHasFocus();
            page.NavigationMenu.ChangeCalculatorMode(CalculatorMode.GraphingCalculator);
            Assert.AreEqual("Graphing", CalculatorApp.GetCalculatorHeaderText());
            Assert.IsTrue(page.AoTCalculatorPage.IsKeepOnTopButtonPresent());
        }

        [TestMethod]
        [Priority(0)]
        public void AoT_EnterExitKeepOnTop()
        {
            Assert.IsTrue(page.AoTCalculatorPage.IsKeepOnTopButtonPresent());
            page.AoTCalculatorPage.NavigateToAoTMode();
            Assert.IsTrue(page.AoTCalculatorPage.IsInAlwaysOnTopMode());
        }

        [TestMethod]
        [Priority(1)]
        public void AoT_HotkeyEnterExit()
        {
            CalculatorApp.EnsureCalculatorHasFocus();
            CalculatorApp.Window.SendKeys(Keys.Alt + Keys.ArrowUp + Keys.Alt);
            Assert.IsTrue(page.AoTCalculatorPage.IsInAlwaysOnTopMode());

            CalculatorApp.Window.SendKeys(Keys.Alt + Keys.ArrowDown + Keys.Alt);
            Assert.IsFalse(page.AoTCalculatorPage.IsInAlwaysOnTopMode());
            Assert.AreEqual("Graphing", CalculatorApp.GetCalculatorHeaderText());
        }

        [TestMethod]
        [Priority(1)]
        public void AoT_Tooltip()
        {
            Assert.AreEqual("Keep on top (Alt+Up)", page.AoTCalculatorPage.GetAoTToolTipText());
            page.AoTCalculatorPage.NavigateToAoTMode();
            Assert.AreEqual("Back to full view (Alt+Down)", page.AoTCalculatorPage.GetAoTToolTipText());
        }

        [TestMethod]
        [Priority(2)]
        public void AoT_EnterEquationInAoTMode()
        {
            // Enter AoT mode — SwitchModeToggleButton is only visible in the small/compact overlay state
            page.AoTCalculatorPage.NavigateToAoTMode();
            Assert.IsTrue(page.AoTCalculatorPage.IsInAlwaysOnTopMode());

            // Switch to equation view
            page.SwitchModeToggleButton.Click();

            // Enter an expression and submit it
            var mathInput = page.MathRichEditBox;
            mathInput.Click();
            mathInput.SendKeys("2x+5y=100");
            mathInput.SendKeys(Keys.Enter);

            // Assert the expression was retained after submission
            Assert.IsFalse(string.IsNullOrEmpty(mathInput.Text));

            // Switch back to graph view
            page.SwitchModeToggleButton.Click();
        }

        #endregion
    }
}
