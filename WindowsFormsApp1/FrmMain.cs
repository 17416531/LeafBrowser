using CefSharp;
using CefSharp.WinForms;
using LeafBrower.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LeafBrower
{
    [ComVisible(true)]
    public partial class FrmMain : Form
    {
        private ChromiumWebBrowser Browser { get; set; }

        public FrmMain()
        {
            InitializeComponent();

            InitBrowser();
            this.WindowState = FormWindowState.Maximized;
        }

        public void InitBrowser()
        {
            var settines = new CefSettings()
            {
                Locale = "zh-CN",
                AcceptLanguageList = "zh-CN",
                MultiThreadedMessageLoop = true
            };
            if (File.Exists("./main.proxy"))
            {
                var proxy = File.ReadAllText("./main.proxy");
                if (!string.IsNullOrEmpty(proxy))
                {
                    settines.CefCommandLineArgs.Add("proxy-server", proxy);
                    settines.CefCommandLineArgs.Add("proxy-bypass-list", "localhost;127.0.0.1"); // 可选，添加绕过列表
                    // settines.CefCommandLineArgs.Add("no-proxy-server", ""); // 可选，如果你不希望使用系统代理，可以添加这个参数
                }
            }
            if (File.Exists("./main.menus"))
            {
                var menus = File.ReadLines("./main.menus");
                txtUrl.Items.AddRange(menus.ToArray());
            }
            Cef.Initialize(settines);

            var bw = new ChromiumWebBrowser("");
            panel1.Controls.Add(bw);
            bw.Dock = DockStyle.Fill;



            Application.ApplicationExit += (s, e) => Cef.Shutdown();

            Browser = bw;
        }





        private void Form1_Load(Object sender, EventArgs e)
        {
#if DEBUG
            //var result = File.ReadAllText("result.json");
            //DecodeResult(result);
#endif


        }



        private void BtnGo_Click(Object sender, EventArgs e)
        {
            var url = txtUrl.Text;
            if (string.IsNullOrWhiteSpace(url)) return;



            Browser.Load(url);
        }









    }
}