using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YoutubeLinker
{
    public partial class Form1 : Form
    {
        Thread thread;
        public Form1()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            thread = new Thread(linkesStatus);
            thread.Start();
           
        }      
        private static Random random = new Random();

        public static string GenerateLink(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

       public void linkesStatus()
        {
            var count = 50;
            var API_KEY = "AIzaSyByyh9l7zc7_EBVgbR7LyrEYYTPp_SvpK0";
            var q = GenerateLink(3);
            var url = "https://www.googleapis.com/youtube/v3/search?key=" + API_KEY + "&maxResults=" + count + "&part=snippet&type=video&q=" + q;

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                dynamic jsonObject = JsonConvert.DeserializeObject(json);
                foreach (var line in jsonObject["items"])
                {
                    Action action = () => linkLabel1.Text = "https://www.youtube.com/watch?v=" + line["id"]["videoId"]; ;
                    // Свойство InvokeRequired указывает, нeжно ли обращаться к контролу с помощью Invoke
                    if (InvokeRequired)
                        Invoke(action);
                    else
                        action(); 
                    /*store your id*/
                }

            }
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Label label = sender as Label;

            Process.Start(linkLabel1.Text);
            if (label != null)
            {
                Clipboard.SetText(label.Text, TextDataFormat.UnicodeText);
            }
        }
    }
}
