using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Specialized;

namespace MusicFinder
{
    public partial class MainForm : Form
    {
        private int id = 2891952;
        private int scope = 8 | 1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string connect = "";
            connect += "http://api.vkontakte.ru/oauth/authorize?client_id=" + id.ToString();
            connect += "&scope=" + scope.ToString() + "&display=popup&response_type=token";
            webBrowser1.Navigate(connect);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) 
        {
            label1.Visible = false;
            if (e.Url.ToString().IndexOf("user_denied") != -1)
            {
                Application.Exit();
            }
            if (e.Url.ToString().IndexOf("access_token") != -1)
            {
                Console.WriteLine(e.Url.AbsoluteUri);
                string token = "";
                int userId = 0;
                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(e.Url.ToString()))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                        token = m.Groups["value"].Value;
                    }
                    else if (m.Groups["name"].Value == "user_id")
                    {
                        userId = Convert.ToInt32(m.Groups["value"].Value);
                    }
                }
                Program.vk.token = token;
                Program.vk.userID = userId;
                Form.ActiveForm.Hide();
                MusicForm form = new MusicForm();
                form.ShowDialog();
            }
        }


    }
}
