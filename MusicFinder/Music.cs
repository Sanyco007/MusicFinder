using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.ComponentModel;
using System.Threading;

namespace MusicFinder
{
    public class Music
    {
        public Panel panel = new Panel();
        private PictureBox playButton = new PictureBox();
        private PictureBox saveButton = new PictureBox();
        private PictureBox line = new PictureBox();
        private PictureBox playerBar = new PictureBox();
        private ProgressBar downloadProgress = new ProgressBar();
        private Label artistLabel = new Label();
        private Label titleLabel = new Label();
        private Label percentLabel = new Label();
        private Label currentTime = new Label();
        private Label totalTime = new Label();
        private SaveFileDialog saveDialog = new SaveFileDialog();
        private Audio audio = null;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private bool open = false;
        private bool pause = true;

        public Music(int index, int width, Audio audio)
        {
            this.audio = audio;

            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);

            panel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            panel.Size = new Size(width, 70);
            panel.Location = new Point(0, index * panel.Height);
            panel.BackColor = Color.White;

            saveDialog.DefaultExt = "mp3";
            saveDialog.Filter = "Файл музыки (*.mp3)|*.mp3";

            playButton.Height = 25;
            playButton.Width = 25;
            playButton.Image = Properties.Resources.play;
            playButton.Location = new Point(5, 5);
            playButton.Cursor = Cursors.Hand;
            playButton.MouseUp += new MouseEventHandler(playButton_MouseUp);

            saveButton.Height = 25;
            saveButton.Width = 25;
            saveButton.Image = Properties.Resources.save;
            saveButton.Cursor = Cursors.Hand;
            saveButton.Location = new Point(width - 30, 5);
            saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            saveButton.MouseUp += new MouseEventHandler(saveButton_MouseUp);

            playerBar.Image = new Bitmap(width - 80, 5);
            playerBar.Size = new Size(width - 80, 5);
            playerBar.Location = new Point(40, 50);
            playerBar.SizeMode = PictureBoxSizeMode.StretchImage;
            playerBar.Cursor = Cursors.Hand;
            playerBar.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            playerBar.MouseDown += new MouseEventHandler(playerBar_MouseDown);
            playerBar.Visible = false;

            line.Size = new Size(width, 1);
            line.SizeMode = PictureBoxSizeMode.StretchImage;
            line.Image = Properties.Resources.fill;
            line.Location = new Point(0, 69);
            line.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            Font font = new Font(new FontFamily("Tahoma"), 8, FontStyle.Bold);

            artistLabel.Text = "Исполнитель: " + audio.Artist;
            artistLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            artistLabel.Size = new Size(width - 70, 20);
            artistLabel.Location = new Point(35, 5);
            artistLabel.Font = font;
            artistLabel.ForeColor = Color.FromArgb(0x5f7d9d);

            titleLabel.Text = "Песня: " + audio.Title;
            titleLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            titleLabel.Size = new Size(width - 70, 20);
            titleLabel.Location = new Point(35, 25);
            titleLabel.Font = font;
            titleLabel.ForeColor = Color.FromArgb(0x5f7d9d);

            Font time = new Font(new FontFamily("Tahoma"), 7, FontStyle.Regular);

            currentTime.TextAlign = ContentAlignment.MiddleCenter;
            currentTime.Font = time;
            currentTime.Size = new Size(40, 18);
            currentTime.Location = new Point(0, 42);
            currentTime.ForeColor = Color.FromArgb(0x5f7d9d);
            currentTime.Visible = false;

            totalTime.TextAlign = ContentAlignment.MiddleCenter;
            totalTime.Font = time;
            totalTime.Size = new Size(40, 18);
            totalTime.Anchor = AnchorStyles.Right;
            totalTime.Location = new Point(width - 40, 42);
            totalTime.ForeColor = Color.FromArgb(0x5f7d9d);
            totalTime.Visible = false;

            percentLabel.TextAlign = ContentAlignment.MiddleCenter;
            percentLabel.Font = font;
            percentLabel.Size = new Size(35, 20);
            percentLabel.Location = new Point(0, 47);
            percentLabel.ForeColor = Color.FromArgb(0x5f7d9d);
            percentLabel.Visible = false;

            downloadProgress.Size = new Size(width - 40, 18);
            downloadProgress.Location = new Point(35, 47);
            downloadProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            downloadProgress.Visible = false;

            panel.Controls.Add(playButton);
            panel.Controls.Add(saveButton);
            panel.Controls.Add(artistLabel);
            panel.Controls.Add(titleLabel);
            panel.Controls.Add(percentLabel);
            panel.Controls.Add(downloadProgress);
            panel.Controls.Add(playerBar);
            panel.Controls.Add(currentTime);
            panel.Controls.Add(totalTime);
            panel.Controls.Add(line);
        }

        void playerBar_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X;
            TimeSpan total = TimeSpan.Parse("00:" + audio.Time);
            double lenght = total.TotalSeconds;
            int width = playerBar.Width;
            TimeSpan position = new TimeSpan(0, 0, (int)(((double)x * lenght) / (double)width));
            Program.player.Position = position;
            ShowPlayerBar();
        }

        private void ShowPlayerBar()
        {
            playerBar.Visible = true;
            currentTime.Visible = true;
            totalTime.Visible = true;
            using (Graphics gr = Graphics.FromImage(playerBar.Image))
            {
                gr.Clear(Color.Silver);
                TimeSpan total = TimeSpan.Parse("00:" + audio.Time);
                double lenght = total.TotalSeconds;
                string minutes = total.Minutes.ToString();
                string seconds = total.Seconds.ToString();
                if (seconds.Length < 2) seconds = "0" + seconds;
                totalTime.Text = minutes + ":" + seconds;

                double position = Program.player.Position.TotalSeconds;
                minutes = Program.player.Position.Minutes.ToString();
                seconds = Program.player.Position.Seconds.ToString();
                if (seconds.Length < 2) seconds = "0" + seconds;
                currentTime.Text = minutes + ":" + seconds;

                double barPosition = (playerBar.Width * position) / lenght;
                Console.WriteLine(lenght);
                Console.WriteLine(position);
                Console.WriteLine(barPosition);
                Console.WriteLine();
                gr.FillRectangle(Brushes.CadetBlue, new RectangleF(0, 0, (float)barPosition, playerBar.Height));
            }
            playerBar.Refresh();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ShowPlayerBar();
        }

        void playButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (pause)
            {
                pause = false;
                if (!open) Program.player.Open(new Uri(audio.URL));
                open = true;
                timer.Enabled = true;
                playButton.Image = Properties.Resources.pause;
                Program.player.Play();
            }
            else
            {
                playButton.Image = Properties.Resources.play;
                Program.player.Pause();
                pause = true;
            }
        }

        void saveButton_MouseUp(object sender, MouseEventArgs e)
        {
            saveDialog.FileName = audio.Artist + " - " + audio.Title + ".mp3";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveDialog.FileName;

                downloadProgress.Minimum = 0;
                downloadProgress.Maximum = 100;
                downloadProgress.Value = 0;
                saveButton.Enabled = false;
                saveButton.Image = Properties.Resources.save_disabled;
                downloadProgress.Visible = true;
                percentLabel.Visible = true;

                WebClient webClient = new WebClient();

                webClient.DownloadProgressChanged += new 
                    DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);

                webClient.DownloadFileCompleted += new 
                    AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

                webClient.DownloadFileAsync(new Uri(audio.URL), fileName);
            }
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgress.Value = e.ProgressPercentage;
            percentLabel.Text = e.ProgressPercentage.ToString() + "%";
        }

        void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadProgress.Visible = false;
            percentLabel.Visible = false;
            saveButton.Enabled = true;
            saveButton.Image = Properties.Resources.save;
        }
    }
}
