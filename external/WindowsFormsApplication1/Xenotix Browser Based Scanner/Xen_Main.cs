using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using XenotixBBScanner;
using OpenQA.Selenium.Support.UI;
using System.Threading;


namespace WindowsFormsApplication1
{
   
    public partial class Main : Form
    {
     
        public Main()
        {
            InitializeComponent();

        }
        IWebDriver ie;
        IWebDriver crm;
        IWebDriver ff;
        Boolean first_time = true;
        Boolean iexplorer = false;
        Boolean gchrome = false;
        Boolean ffox = false;
        private void button1_Click(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.TopLevel = true;
       

            iex.Enabled = false;
            gc.Enabled = false;
            fir.Enabled = false;
            
          
                    
     
                    if (first_time)
                    {

                        if (iex.Checked)
                        {
                            try
                            {

                               LogTextEvent(res,"Starting Internet Explorer", Color.Green);

                                var options = new InternetExplorerOptions();
                                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                                ie = new InternetExplorerDriver(options);
                                ie.Manage().Window.Position = new Point(0, 25);
                                ie.Manage().Window.Size = new Size(150, 700);
                               LogTextEvent(res,"Internet Explorer Started!", Color.Green);
                                iexplorer = true;
                                first_time = false;
                            }
                            catch (Exception ee)
                            {
                               LogTextEvent(res,"Failed to Start Internet Explorer" + Environment.NewLine + "Error:" + ee.Message.ToString(), Color.Red);
                                iexplorer = false;
                            }



                        }
                        if (gc.Checked)
                        {
                            try
                            {
                               LogTextEvent(res,"Starting Google Chrome", Color.Green);

                                crm = new ChromeDriver();
                                crm.Manage().Window.Position = new Point(300, 25);
                                crm.Manage().Window.Size = new Size(150, 700);
                               LogTextEvent(res,"Google Chrome Started!", Color.Green);
                                gchrome = true;
                                first_time = false;
                            }
                            catch (Exception ee)
                            {
                               LogTextEvent(res,"Failed to Start Google Chrome" + Environment.NewLine + "Error:" + ee.Message.ToString(), Color.Red);
                                gchrome = false;
                            }

                        }
                        if (fir.Checked)
                        {
                            try
                            {
                               LogTextEvent(res,"Starting Firefox", Color.Green);

                                ff = new FirefoxDriver();
                                ff.Manage().Window.Maximize();
                               LogTextEvent(res,"Firefox Started!", Color.Green);
                                ffox = true;
                                first_time = false;
                            }
                            catch (Exception ee)
                            {
                               LogTextEvent(res,"Failed to Start Firefox" + Environment.NewLine + "Error:" + ee.Message.ToString(), Color.Red);
                                ffox = false;


                            }

                        }
                       
                    }
                 
                 //Start Scanning
                     Thread thread =new Thread(() => Scan(iexplorer, gchrome, ffox, url.Text));
                   thread.Start();
                   button1.Enabled = false;

          
        }
    
        public string checkAlert(IWebDriver driver)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

                IAlert alert = driver.SwitchTo().Alert();

                String txt = alert.Text;
                alert.Accept();

                return txt;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message.ToString();
            }
        }

        private void Scan(Boolean ieb, Boolean gcb, Boolean ffb, String URL)
        {


               LogTextEvent(res,"Scanning Started", Color.Green);

                String payload = @"""><svg onload=alert(0)>";

                try
                {
                    if (ffb == true)
                    {
                        try
                        {
                           LogTextEvent(res,"Navigating to: " + URL, Color.Green);

                            ff.Navigate().GoToUrl(URL);
                           LogTextEvent(res,"Using Payload: " + payload, Color.Green);

                        }
                        catch (Exception)
                        {
                           LogTextEvent(res,"Navigation to " + URL + " failed!", Color.Red);

                        }
                        try
                        {
                           LogTextEvent(res,"Looking for Select", Color.Green);

                            IWebElement select = ff.FindElement(By.XPath("//select"));
                            IList<IWebElement> allOptions = select.FindElements(By.TagName("option"));
                            foreach (IWebElement option in allOptions)
                            {
                               LogTextEvent(res,"Select Option : " + option.GetAttribute("name"),Color.Orange);
                                option.Click();
                            }
                        }
                        catch (Exception p)
                        {
                           LogTextEvent(res,"No Select Identified. Select Error: " + p.Message.ToString(), Color.Red);

                        }
                        try
                        {
                           LogTextEvent(res,"Looking for Textarea", Color.Green);

                            IList<IWebElement> txs = ff.FindElements(By.XPath("//textarea"));

                            foreach (IWebElement tx in txs)
                            {
                               LogTextEvent(res,"Textarea : " + tx.GetAttribute("name"),Color.Orange);

                                tx.SendKeys(payload);

                            }

                        }
                        catch (Exception p)
                        {

                           LogTextEvent(res,"No Textarea Identified. Textarea Error: " + p.Message.ToString(), Color.Red);

                        }
                        string formsub = "submit";
                        try
                        {
                           LogTextEvent(res,"Looking for Input", Color.Green);

                            IList<IWebElement> inputs = ff.FindElements(By.XPath("//input"));

                            foreach (IWebElement input in inputs)
                            {
                               LogTextEvent(res,"Input : " + input.GetAttribute("name"),Color.Orange);


                                if (input.GetAttribute("name") == "submit" || input.GetAttribute("name") == "SUMBIT")
                                {
                                    formsub = input.GetAttribute("name");
                                }
                                if (input.GetAttribute("name") == "email" || input.GetAttribute("name") == "Email" || input.GetAttribute("name") == "EMAIL")
                                {
                                    input.SendKeys("xenotix@xenotix.com");
                                }
                                else
                                {
                                    input.SendKeys(payload);
                                }
                            }


                        }
                        catch (Exception ee)
                        {
                           LogTextEvent(res,"No Input Identiied. Input Error: " + ee.Message.ToString(), Color.Red);

                        }

                       LogTextEvent(res,"Looking Form Submit", Color.Green);
                        try
                        {
                            ff.FindElement(By.Name(formsub)).Click();
                           LogTextEvent(res,"Submitting the Form", Color.Green);

                        }
                        catch (Exception)
                        {
                            try
                            {
                                ff.FindElement(By.Id(formsub)).Click();
                               LogTextEvent(res, "Submitting the Form", Color.Green);

                            }
                            catch (Exception z)
                            {
                               LogTextEvent(res,"Form Error: " + z.Message.ToString(), Color.Red);
                            }
                        }
                  
                       LogTextEvent(res, checkAlert(ff), Color.Blue);


                        //Scan Finished     
                    }
                   LogTextEvent(res, " Scanning Finished!", Color.Green);

                   ButtonEnable();


                }
                catch (Exception ee)
                {
                   LogTextEvent(res, "Error:" + ee.Message.ToString(), Color.Red);

                }

            }
        delegate void SetTextCallback();
        private void Enable()
        {
            this.button1.Enabled= true;
        }
        private void ButtonEnable()
        {
            Thread.Sleep(2000);
          if (this.button1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Enable);
                this.Invoke(d, new object[] {});
            }
            else
            {
                this.button1.Enabled = true;
            }
        } 
       private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ie.Close();
                crm.Close();
                ff.Close();
            }
            catch(Exception eee)
            {
               LogTextEvent(res,"Error:" + eee.Message.ToString(), Color.Red);
        
            }
            }

        private void url_TextChanged(object sender, EventArgs e)
        {

        }
        public void LogTextEvent(RichTextBox TextEventLog, string EventText,Color TextColor)
        {
            if (TextEventLog.InvokeRequired)
            {
                TextEventLog.BeginInvoke(new Action(delegate
                {
                    LogTextEvent(TextEventLog, EventText, TextColor );
                }));
                return;
            }

            string nDateTime = DateTime.Now.ToString("[hh:mm:ss tt]") + " - ";

            // color text.
            TextEventLog.SelectionStart = TextEventLog.Text.Length;
            TextEventLog.SelectionColor = TextColor;

            // newline if first line, append if else.
            if (TextEventLog.Lines.Length == 0)
            {
                TextEventLog.AppendText(nDateTime + EventText);
                TextEventLog.ScrollToCaret();
                TextEventLog.AppendText(System.Environment.NewLine);
            }
            else
            {
                TextEventLog.AppendText(nDateTime + EventText + System.Environment.NewLine);
                TextEventLog.ScrollToCaret();
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
   
}

