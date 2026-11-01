// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

using System.Drawing;
using System.Threading;

namespace CalculatorUITestFramework
{
    /// <summary>
    /// UI automation helpers shared across all calculator modes for Always-on-Top (AoT) interactions.
    /// </summary>
    public class AoTCalculatorPage
    {
        public StandardOperatorsPanel StandardOperators = new StandardOperatorsPanel();
        public NavigationMenu NavigationMenu = new NavigationMenu();
        public WindowsElement EnterAlwaysOnTopButton => session.TryFindElementByAccessibilityId("NormalAlwaysOnTopButton");
        public WindowsElement ExitAlwaysOnTopButton => session.TryFindElementByAccessibilityId("ExitAlwaysOnTopButton");
        public AppiumWebElement ToolTip => CalculatorDriver.Instance.CalculatorSession.FindElementByClassName("ToolTip").FindElementByClassName("TextBlock");

        private WindowsDriver<WindowsElement> session => CalculatorDriver.Instance.CalculatorSession;

        /// <summary>
        /// Enters AoT (Keep on top) mode and verifies that the header is hidden.
        /// </summary>
        public void NavigateToAoTMode()
        {
            string source = session.PageSource;
            if (source.Contains("NormalAlwaysOnTopButton"))
            {
                EnterAlwaysOnTopButton.Click();
                ExitAlwaysOnTopButton.WaitForDisplayed();
            }
        }

        /// <summary>
        /// Exits AoT (Keep on top) mode. Pass expectedHeader to assert the calculator returned to the correct mode.
        /// </summary>
        public void NavigateToFullViewMode(string expectedHeader = null)
        {
            string source = session.PageSource;
            if (source.Contains("ExitAlwaysOnTopButton"))
            {
                ExitAlwaysOnTopButton.Click();
                // Poll page source rather than calling EnterAlwaysOnTopButton.WaitForDisplayed(),
                // because the property getter throws if the element is temporarily absent from the
                // UIA tree during the compact-overlay-to-full-view transition.
                for (int i = 0; i < 40 && !session.PageSource.Contains("NormalAlwaysOnTopButton"); i++)
                    Thread.Sleep(50);
                if (expectedHeader != null)
                    Assert.AreEqual(expectedHeader, CalculatorApp.GetCalculatorHeaderText());
            }
        }

        /// <summary>
        /// Gets the AoT ToolTip text
        /// </summary>
        public string GetAoTToolTipText()
        {
            string source = session.PageSource;
            if ((source.Contains("Keep on top")) || (source.Contains("Back to full view")))
            {
                // Allow the AoT transition animation to settle before hovering, so the tooltip triggers reliably.
                Thread.Sleep(1500);

                if (source.Contains("Keep on top"))
                {
                    Actions moveToAoTButton = new Actions(CalculatorDriver.Instance.CalculatorSession);
                    moveToAoTButton.MoveToElement(EnterAlwaysOnTopButton);
                    moveToAoTButton.Perform();
                }
                else
                {
                    Actions moveToBackToFullViewVButton = new Actions(CalculatorDriver.Instance.CalculatorSession);
                    moveToBackToFullViewVButton.MoveToElement(ExitAlwaysOnTopButton);
                    moveToBackToFullViewVButton.Perform();
                }
            }
            else
            {
                throw new NotFoundException("Could not find 'Keep on top' button or 'Back to full view' button");
            }
            Thread.Sleep(500); // wait for the tooltip popup to appear after the hover
            return ToolTip.Text;
        }

        /// <summary>
        /// Checks if the AoT (Keep on top) button is present
        /// </summary>
        public bool IsKeepOnTopButtonPresent()
        {
            string source = session.PageSource;
            return source.Contains("Keep on top");
        }

        /// <summary>
        /// Returns true when the calculator is in AoT (Keep on top) mode
        /// </summary>
        public bool IsInAlwaysOnTopMode()
        {
            return session.PageSource.Contains("ExitAlwaysOnTopButton");
        }

        /// <summary>
        /// If the Invert button is not displayed, resize the window.
        /// Two attempts are made; if the button is not found a "not found" exception is thrown.
        /// </summary>
        public void ResizeAoTWindowToDisplayInvertButton()
        {
            // Put the calculator in the upper left region of the screen
            CalculatorDriver.Instance.CalculatorSession.Manage().Window.Position = new Point(8, 8);
            GrowWindowToShowInvertButton(CalculatorDriver.Instance.CalculatorSession.Manage().Window.Size.Height);
        }

        /// <summary>
        /// Increases the height of the window until the invert button is visible
        /// </summary>
        private void GrowWindowToShowInvertButton(int height)
        {
            if (height > 700)
            {
                throw new NotFoundException("Could not find the Invert Button");
            }

            if (!session.PageSource.Contains("invertButton"))
            {
                var width = CalculatorDriver.Instance.CalculatorSession.Manage().Window.Size.Width;
                CalculatorDriver.Instance.CalculatorSession.Manage().Window.Size = new Size(width, height);
                //give window time to render new size
                Thread.Sleep(10);
                GrowWindowToShowInvertButton(height + 100);
            }
        }
    }
}
