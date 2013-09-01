using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MusicFinder
{
    public class Audio
    {
        public string Artist { get; set; }
        public string Title  { get; set; }
        public string Time   { get; set; }
        public string URL    { get; set; }
        public bool loadFile { get; set; }
        public Stream memory = new MemoryStream();

        public Audio(string artist, string title, string time, string url)
        {
            Artist = artist;
            Title = title;
            Time = time;
            URL = url;
            loadFile = false;
        }
    }
}
