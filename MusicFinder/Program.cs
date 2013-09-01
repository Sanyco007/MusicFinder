using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Forms;

namespace MusicFinder
{
    static class Program
    {
        public static VK vk = new VK();
        public static MediaPlayer player = new MediaPlayer();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
