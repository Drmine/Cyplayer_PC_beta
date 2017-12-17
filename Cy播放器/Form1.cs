using System;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Cy播放器
{
    public partial class Form1 : Form
    {
        string color="";//存储颜色的变量
        public static string message="";//推送消息内容的变量
        public static string update="";//检查更新的变量
        //初始化窗体
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }
        //打开软件，窗体自动载入首页（窗口载入事件）
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(Application.StartupPath + "\\Cy播放器的数据.xml");
                color = ds.Tables[0].Rows[0][0].ToString();
                message = ds.Tables[0].Rows[0][1].ToString();
                update = ds.Tables[0].Rows[0][2].ToString();
            }
            catch
            {
                MessageBox.Show("没有找到数据文件，将重新生成");
            }
            pushandupdate();
            loadcolor();
            comboBox1.SelectedIndex=0;
            webBrowser1.Navigate(new Uri("https://v.qq.com/x/search/?q&stag=7&smartbox_ab="));
        }
        //载入颜色设置
        public void loadcolor()
        {
            Color cr = Color.RoyalBlue;
            switch (color)
            {
                case "1": cr = Color.RoyalBlue; this.radioButton1.Checked = true; break;
                case "2": cr = Color.Red; this.radioButton2.Checked = true; break;
                case "3": cr = Color.Violet; this.radioButton3.Checked = true; break;
                case "4": cr = Color.Orange; this.radioButton4.Checked = true; break;
                default: break;
            }
            changeColor(cr);
        }
        //批量改变颜色
        public void changeColor(Color cr)
        {
            this.comboBox1.ForeColor = cr;
            this.BackColor = cr;
            for (int j = 1; j < 7; j++)
            {
                string str = "button" + j.ToString();
                Button btn = this.Controls[str] as Button;
                btn.BackColor = cr;
            }
            for(int k = 1; k < 5; k++)
            {
                string str = "radioButton" + k.ToString();
                RadioButton rbtn = this.Controls[str] as RadioButton;
                rbtn.ForeColor = cr;
            }
            linkLabel1.LinkColor = cr;
        }
        //首页按钮
        private void label1_Click(object sender, EventArgs e)
        {
            Uri url = new Uri("https://v.qq.com/x/search/?q&stag=7&smartbox_ab=");
            webBrowser1.Navigate(url);
        }
        //上一页按钮
        private void label2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }
        //下一页按钮
        private void label3_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }
        //刷新按钮
        private void label4_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }
        //播放按钮
        private void label5_Click(object sender, EventArgs e)
        {
            Uri url = new Uri("http://xuelingqiao.web3v.com/VIP/cyvipPC.html?url="+webBrowser1.Url.ToString());
            webBrowser1.Navigate(url);
        }
        //值解析按钮
        private void label6_Click(object sender, EventArgs e)
        {
            Uri url = new Uri("http://xuelingqiao.web3v.com/VIP/cnumberPC.html");
            webBrowser1.Navigate(url);
        }
        //强制浏览器将所有页面在本浏览器打开
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            foreach (HtmlElement archor in this.webBrowser1.Document.Links)
            {
                archor.SetAttribute("target", "_self");
            }
            foreach (HtmlElement form in this.webBrowser1.Document.Forms)
            {
                form.SetAttribute("target", "_self");
            }
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }
        //使用说明
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Uri help = new Uri("http://xuelingqiao.web3v.com/app/CyplayerHelp.html");
            webBrowser1.Navigate(help);
        }
        //保存数据
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataTable cy = new DataTable();
            cy.Columns.Add("Color");
            cy.Columns.Add("push");
            cy.Columns.Add("update");
            DataRow newRow = cy.NewRow();
            cy.Rows.Add(newRow);
            cy.Rows[0][0] = color;
            cy.Rows[0][1] = message;
            cy.Rows[0][2] = update;

            DataSet ds = new DataSet();
            ds.Tables.Add(cy);
            ds.WriteXml(Application.StartupPath + "\\Cy播放器的数据.xml");
        }
        //改成胖次蓝颜色
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            changeColor(Color.RoyalBlue);
            color = "1";
        }
        //改成姨妈红颜色
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            changeColor(Color.Red);
            color = "2";
        }
        //改成少女粉颜色
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            changeColor(Color.Violet);
            color = "3";
        }
        //改成大便黄颜色
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            changeColor(Color.Orange);
            color = "4";
        }
        //推送通知和更新
        public void pushandupdate()
        {
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] meaasageData = MyWebClient.DownloadData("http://xuelingqiao.web3v.com/app/push.html"); //从指定网站下载数据
            string m = Regex.Match(Encoding.UTF8.GetString(meaasageData), @"(?<=<body>)(.*?)(?=</body>)").Groups[1].ToString();
            System.Threading.Thread.Sleep(200);
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] updateData = MyWebClient.DownloadData("http://xuelingqiao.web3v.com/app/update.html"); //从指定网站下载数据
            string u = Regex.Match(Encoding.UTF8.GetString(updateData), @"(?<=<body>)(.*?)(?=</body>)").Groups[1].ToString();
            System.Threading.Thread.Sleep(200);
            if (message != m)
            {
                Form2 pushfrm = new Form2(true);
                pushfrm.ShowDialog();
            }
            if (update != u)
            {
                Form2 updatefrm = new Form2(false);
                updatefrm.ShowDialog();
            }
        }
        //视频源选择
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string url="";
            switch (comboBox1.SelectedIndex)
            {
                case 0: url = "https://v.qq.com/"; break;
                case 1: url = "http://www.iqiyi.com/"; break;
                case 2: url = "http://www.youku.com/"; break;
                case 3: url = "http://www.tudou.com/"; break;
                case 4: url = "http://www.pptv.com/"; break;
                case 5: url = "http://www.bilibili.com/"; break;
                case 6: url = "http://www.acfun.cn/"; break;
                default:break;
            }
            this.webBrowser1.Navigate(new Uri(url));
        }
        //取消新窗口打开
        private void webBrowser1_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
