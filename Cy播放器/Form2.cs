using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Cy播放器
{
    public partial class Form2 : Form
    {
        bool choose;
        string content;
        public Form2(bool a)
        {
            InitializeComponent();
            this.choose = a;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (choose)
            {
                Form1.message = content;
                this.Close();
            }
            else
            {
                Form1.update = content;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (choose)
            {
                webBrowser1.Refresh();
                this.webBrowser1.Navigate(new Uri("http://xuelingqiao.web3v.com/app/push.html"));
                webBrowser1.Refresh();
            }
            else
            {
                webBrowser1.Refresh();
                this.webBrowser1.Navigate(new Uri("http://xuelingqiao.web3v.com/app/update.html"));
                webBrowser1.Refresh();
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.ScriptErrorsSuppressed = true;
            content = Regex.Match(this.webBrowser1.DocumentText, @"(?<=<body>)(.*?)(?=</body>)").Groups[1].ToString();
        }
    }
}
