using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;

namespace MusicFinder
{
    public class VK
    {
        public string token { get; set; }
        public int userID { get; set; }


        public XmlDocument searchSong(string title)
        {

            string url = "https://api.vk.com/method/audio.search.xml?q=";
            url += title;
            url += "&access_token=" + token;

            XmlDocument xml = new XmlDocument();
            xml.Load(url);
           
            return xml;
        }

    }
}
