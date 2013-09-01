using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace MusicFinder
{
    public partial class MusicForm : Form
    {
        public MusicForm()
        {
            InitializeComponent();
        }

        private void MusicForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MusicForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xml = Program.vk.searchSong(searchText.Text);
            XmlNodeList list = xml.GetElementsByTagName("audio");
            panel.Controls.Clear();
            int width = panel.Width - 60;
            panel.AutoScroll = false;
            for (int i = 0; i < list.Count; i++)
            {
                var d = (XmlElement)list[i];
                var artist = d.GetElementsByTagName("artist")[0].InnerText;
                var title = d.GetElementsByTagName("title")[0].InnerText;
                var url = d.GetElementsByTagName("url")[0].InnerText;
                var duration = d.GetElementsByTagName("duration")[0].InnerText;
                int lenght = Int32.Parse(duration);
                var time = (lenght / 60).ToString() + ":" + (lenght % 60).ToString();
                Audio audio = new Audio(artist, title, time, url);
                Panel music = new Music(i, width, audio).panel;
                music.Left = (panel.Width - width) / 2;
                panel.Controls.Add(music);
            }
            panel.AutoScroll = true;
        }

    }
}
