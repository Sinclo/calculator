// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CalculatorUITestFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace CalculatorUITests
{
    [TestClass]
    public class DateCalculationModeFunctionalTests
    {
        private static readonly DateCalculatorPage page = new DateCalculatorPage();

        /// <summary>
        /// Initializes the WinAppDriver web driver session.
        /// </summary>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Create session to launch a Calculator window
            CalculatorDriver.Instance.SetupCalculatorSession(context);

            // Ensure that calculator is in date calculation mode
            page.NavigateToDateCalculator();
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
        public void AoT_ButtonInDateCalculation()
        {
            CalculatorApp.EnsureCalculatorHasFocus();
            page.NavigationMenu.ChangeCalculatorMode(CalculatorMode.DateCalculator);
            Assert.AreEqual("Date calculation", CalculatorApp.GetCalculatorHeaderText());
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
            CalculatorApp.Header.Click();
            CalculatorApp.Window.SendKeys(Keys.Alt + Keys.ArrowUp + Keys.Alt);
            Assert.IsTrue(page.AoTCalculatorPage.IsInAlwaysOnTopMode());

            CalculatorApp.Window.SendKeys(Keys.Alt + Keys.ArrowDown + Keys.Alt);
            Assert.IsFalse(page.AoTCalculatorPage.IsInAlwaysOnTopMode());
            Assert.AreEqual("Date calculation", CalculatorApp.GetCalculatorHeaderText());
        }

        [TestMethod]
        [Priority(1)]
        public void AoT_Tooltip()
        {
            Assert.AreEqual("Keep on top (Alt+Up)", page.AoTCalculatorPage.GetAoTToolTipText());
            page.AoTCalculatorPage.NavigateToAoTMode();
            Assert.AreEqual("Back to full view (Alt+Down)", page.AoTCalculatorPage.GetAoTToolTipText());
        }

        #endregion
    }
}
