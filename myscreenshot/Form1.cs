using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using DotNetBrowser;
using DotNetBrowser.WinForms;
using DotNetBrowser.Engine;
using DotNetBrowser.Browser;

namespace myscreenshot
{
    public partial class Form1 : Form
    {
        myinclude my = new myinclude();
        string PWD = "";
        string OUTPUT_PATH = "";
        //Making driver to navigate
        private readonly IEngine engine;
        private readonly IBrowser browser;

        static FirefoxDriver driver = null;

        public Form1()
        {
            InitializeComponent();
            engine = EngineFactory.Create(new EngineOptions.Builder
            {
                LicenseKey = "1BNKDJZJSD1SMGC009ATO3UR0TR4B22ATW9MWYJ6D1OEKCLOFN55VUE43FV6K47MWFA4LY"
            }
            .Build());
            browser = engine.CreateBrowser();
            bW.InitializeFrom(browser);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string URL = textBox1.Text;
            if (URL.Length < 8 || URL.Substring(0, 4) != "http")
            {
                MessageBox.Show("網址錯了...");
                return;
            }
            browser.Navigation.LoadUrl(URL);
            
        }

        private void wB_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //textBox1.Text = wB.Url.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "抓圖中...";
            button1.Enabled = false;

            new Thread(() =>
            {
                try
                {
                    var driverService = FirefoxDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true; //hide console window
                    var driverOptions = new FirefoxOptions();

                    driverOptions.AddArguments("--headless");
                    driverOptions.AddArguments("user-agent=Mozilla/5.0 (Windows NT 10.0; WOW64; rv:56.0) Gecko/20100101 Firefox/66.0");
                    driver = new OpenQA.Selenium.Firefox.FirefoxDriver(driverService, driverOptions);


                    string URL = textBox1.Text;

                    driver.Navigate().GoToUrl(URL);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                    Image ig = GetEntireScreenshot();

                    //Save the screenshot
                    string t = my.date("YmdHis") + ".png";
                    string filePath = OUTPUT_PATH + "\\" + t;
                    ig.Save(filePath, ImageFormat.Png);
                    string argument = "/select, \"" + filePath + "\"";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                    driver.Close();

                    UpdateUIText(button1, "抓圖");
                    UpdateUIEnable(button1, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                    UpdateUIText(button1, "抓圖");
                    UpdateUIEnable(button1, true);
                }
            }).Start();
        }
        private delegate void UpdateUITextCallBack(Control ctl, string value);
        private void UpdateUIText(Control ctl, string value)
        {
            if (this.InvokeRequired)
            {
                UpdateUITextCallBack uu = new UpdateUITextCallBack(UpdateUIText);
                this.Invoke(uu, ctl, value);
            }
            else
            {
                ctl.Text = value;
            }
        }
        private delegate void UpdateUIEnableCallBack(Control ctl, bool value);
        private void UpdateUIEnable(Control ctl, bool value)
        {
            if (this.InvokeRequired)
            {
                UpdateUIEnableCallBack uu = new UpdateUIEnableCallBack(UpdateUIEnable);
                this.Invoke(uu, ctl, value);
            }
            else
            {
                ctl.Enabled = value;
            }
        }
        public Image GetEntireScreenshot()
        {
            // Get the total size of the page
            var totalWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.offsetWidth"); //documentElement.scrollWidth");
            var totalHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return  document.body.parentNode.scrollHeight");
            // Get the size of the viewport
            var viewportWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.clientWidth"); //documentElement.scrollWidth");
            var viewportHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight"); //documentElement.scrollWidth");

            // We only care about taking multiple images together if it doesn't already fit
            if (totalWidth <= viewportWidth && totalHeight <= viewportHeight)
            {
                var screenshot = driver.GetScreenshot();
                return ScreenshotToImage(screenshot);
            }
            // Split the screen in multiple Rectangles
            var rectangles = new List<Rectangle>();
            // Loop until the totalHeight is reached
            for (var y = 0; y < totalHeight; y += viewportHeight)
            {
                var newHeight = viewportHeight;
                // Fix if the height of the element is too big
                if (y + viewportHeight > totalHeight)
                {
                    newHeight = totalHeight - y;
                }
                // Loop until the totalWidth is reached
                for (var x = 0; x < totalWidth; x += viewportWidth)
                {
                    var newWidth = viewportWidth;
                    // Fix if the Width of the Element is too big
                    if (x + viewportWidth > totalWidth)
                    {
                        newWidth = totalWidth - x;
                    }
                    // Create and add the Rectangle
                    var currRect = new Rectangle(x, y, newWidth, newHeight);
                    rectangles.Add(currRect);
                }
            }
            // Build the Image
            var stitchedImage = new Bitmap(totalWidth, totalHeight);
            // Get all Screenshots and stitch them together
            var previous = Rectangle.Empty;
            foreach (var rectangle in rectangles)
            {
                // Calculate the scrolling (if needed)
                if (previous != Rectangle.Empty)
                {
                    var xDiff = rectangle.Right - previous.Right;
                    var yDiff = rectangle.Bottom - previous.Bottom;
                    // Scroll
                    ((IJavaScriptExecutor)driver).ExecuteScript(String.Format("window.scrollBy({0}, {1})", xDiff, yDiff));
                }
                // Take Screenshot
                var screenshot = driver.GetScreenshot();
                // Build an Image out of the Screenshot
                var screenshotImage = ScreenshotToImage(screenshot);
                // Calculate the source Rectangle
                var sourceRectangle = new Rectangle(viewportWidth - rectangle.Width, viewportHeight - rectangle.Height, rectangle.Width, rectangle.Height);
                // Copy the Image
                using (var graphics = Graphics.FromImage(stitchedImage))
                {
                    graphics.DrawImage(screenshotImage, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                }
                // Set the Previous Rectangle
                previous = rectangle;
            }
            return stitchedImage;
        }

        private Image ScreenshotToImage(Screenshot screenshot)
        {
            Image screenshotImage;
            using (var memStream = new MemoryStream(screenshot.AsByteArray))
            {
                screenshotImage = Image.FromStream(memStream);
            }
            return screenshotImage;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PWD = my.pwd();
            OUTPUT_PATH = PWD + "\\output";
            Console.WriteLine(OUTPUT_PATH);
            if (!my.is_dir(OUTPUT_PATH))
            {
                my.mkdir(OUTPUT_PATH);
            }
            button2_Click(sender, e);

        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            //driver.Dispose();
        }
    }
}
