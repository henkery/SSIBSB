using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace RSS_Reader
{
    public partial class frmMain : Form
    {
        XmlTextReader rssReader;
        XmlDocument rssDoc;
        XmlNode nodeRss;
        XmlNode nodeChannel;
        XmlNode nodeItem;
        ListViewItem rowNews;
        public frmMain()
        {
            InitializeComponent();
        }
        public List<string> list;

        private void btnRead_Click(object sender, EventArgs e)
        {
            loadlist("", "");
            button2.Enabled = true;
            button1.Enabled = true;
        }
        public int pagina = 1;
        public void loadlist(String after, String kanarie)
        {
            lstNews.Items.Clear();
            this.Cursor = Cursors.WaitCursor;
            // Create a new XmlTextReader from the specified URL (RSS feed)
            
            if (after == "")
            {
                pagina = 1;
                kanarie = txtUrl.Text + "/.rss";
                rssReader = new XmlTextReader("http://www.reddit.com/r/" + kanarie);
            }
            else if (after != "")
            {
                pagina = pagina + 1;
                kanarie = txtUrl.Text + "/.rss?after=t3_" + after;
                rssReader = new XmlTextReader("http://www.reddit.com/r/" + kanarie);
            }

            label2.Text = "Pagina: " + pagina.ToString();
            //MessageBox.Show(rssReader.BaseURI);
            rssDoc = new XmlDocument();
            //MessageBox.Show(rssDoc.InnerText);
            // Load the XML content into a XmlDocument
            rssDoc.Load(rssReader);

            // Loop for the <rss> tag
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
                // If it is the rss tag
                if (rssDoc.ChildNodes[i].Name == "rss")
                {
                    // <rss> tag found
                    nodeRss = rssDoc.ChildNodes[i];
                }
            }

            // Loop for the <channel> tag
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
                // If it is the channel tag
                if (nodeRss.ChildNodes[i].Name == "channel")
                {
                    // <channel> tag found
                    nodeChannel = nodeRss.ChildNodes[i];
                }
            }

            // Set the labels with information from inside the nodes
            //lblTitle.Text = "Title: " + nodeChannel["title"].InnerText;
            //lblLanguage.Text = "Language: " + nodeChannel["language"].InnerText;
            //lblLink.Text = "Link: " + nodeChannel["link"].InnerText;
            //lblDescription.Text = "Description: " + nodeChannel["description"].InnerText;

            // Loop for the <title>, <link>, <description> and all the other tags
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
                // If it is the item tag, then it has children tags which we will add as items to the ListView
                if (nodeChannel.ChildNodes[i].Name == "item")
                {
                    nodeItem = nodeChannel.ChildNodes[i];

                    // Create a new row in the ListView containing information from inside the nodes
                    rowNews = new ListViewItem();
                    rowNews.Text = nodeItem["title"].InnerText;
                    rowNews.SubItems.Add(nodeItem["link"].InnerText);
                    lstNews.Items.Add(rowNews);
                }
            }

            this.Cursor = Cursors.Default;
            //lstNews.
            lstNews.SelectedIndices.Clear();
            lstNews.SelectedIndices.Add(0);

        }

        private void lstNews_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            // When an items is selected
            if (lstNews.SelectedItems.Count == 1)
            {
                
                // Loop through all the nodes under <channel>
                for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
                {
                    // Until you find the <item> node
                    if (nodeChannel.ChildNodes[i].Name == "item")
                    {
                        // Store the item as a node
                        nodeItem = nodeChannel.ChildNodes[i];
                        // If the <title> tag matches the current selected item
                        if (nodeItem["title"].InnerText == lstNews.SelectedItems[0].Text)
                        {
                            
                            // It's the item we were looking for, get the description
                            wb1.Stop();
                            String html = "<html><head><title>kaas</title></head><body style=\"border=none;margin=0px;padding-top=0px;padding-bottom=0px;bgcolor=grey;\"><center><h2>hoi</h2>" + "</center></body></html>";
                            wb1.DocumentText = html;
                            String henk = nodeItem["description"].InnerText;
                            string[] values = henk.Split(new string[] { "<", "</", ">", "table", "tr", "td", "href", "\"", "=", "src", "alt", "title" }, StringSplitOptions.RemoveEmptyEntries);
                            var temp = new List<string>();
                            foreach (var s in values)
                            {
                                if (!string.IsNullOrEmpty(s) && !string.Equals(s, " "))
                                    if (s.Contains("http"))
                                    {
                                        if (!s.Contains("http://imgur.com/") || s.Contains("http://imgur.com/a/"))
                                        {
                                            temp.Add(s);
                                        }
                                        else
                                        {
                                            String k = s.Replace("http://", "");
                                            k = "http://i." + k + ".jpg";
                                            temp.Add(k);
                                        }
                                    }
                            }
                            values = temp.ToArray();
                            //list[3] = "banaan";
                            list = new List<string>(values);
                            //list = List<string>(values);
                            //string henk2 = string.Join( "\n", list );
                            //System.Uri uri = new System.Uri(list[3]);
                            //wb1.Url = uri;
                            //wb1.Document.Window.Size = wb1.Size;
                            //wb1.Size = wb1.Document.Window.Size;
                            //wb1.Document.Window.Size = wb1.Size;
                            //wb1.Navigate();
                            //txtContent.Text = list[3];
                            if (list.Count >= 4)
                            {
                                if (list[3] == "banaan")
                                {
                                    list[3] = "henk";
                                }
                                if (list[3].Contains(".jpg") || list[3].Contains(".gif") || list[3].Contains(".png"))
                                {
                                    int precalc = wb1.Width - 1;
                                    int precalc2 = wb1.Height - 1;
                                    html = "<html><head><title>kaas</title><style>img {  height:expression(this.scrollHeight>" + precalc2 + "?\"" + wb1.Height + "px\":\"auto\"); }</style></head><body style=\"border=none;margin=0px;padding-top=0px;padding-bottom=0px;bgcolor=#736F6F;\"><center><div class=\"henk\"><img src=\"" + list[3] + "\" ></div>" + "</center></body></html>";
                                    wb1.DocumentText = html;
                                }
                                else if (list[3].Contains("imgur.com/a/"))
                                {
                                    System.Uri uri = new System.Uri(list[3] + "/embed");
                                    wb1.Url = uri;
                                }
                                else
                                {
                                    System.Uri uri = new System.Uri(list[3]);
                                    wb1.Url = uri;
                                }
                            }
                            else
                            {
                                //String html = "<html><head><title>kaas</title></head><body style=\"border=none;margin=0px;padding-top=0px;padding-bottom=0px;\"><center><h2 style=\"font-family=Helvetica\">Geen plaatje gevonden :(</h2>" + "</center></body></html>";
                                //wb1.DocumentText = html;
                                int klik = list.Count - 1;
                                System.Uri uri = new System.Uri(list[klik]);
                                wb1.Url = uri;
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void lstNews_DoubleClick(object sender, EventArgs e)
        {
            // When double clicked open the web page
            System.Diagnostics.Process.Start(list[3]);
   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            next_page();
        }
        public void next_page()
        {
            nodeItem = nodeChannel.ChildNodes[nodeChannel.ChildNodes.Count - 1];
            String henk = nodeItem["link"].InnerText;
            henk = henk.Replace("http://www.reddit.com/r/" + txtUrl.Text + "/comments/", "");
            string[] words = henk.Split('/');
            //MessageBox.Show(words[0]);
            loadlist(words[0], "");
        }
        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadlist("", "");
                button2.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
        Timer timer = new Timer();
        private void button2_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Hide();
            splitContainer1.Panel1MinSize = 0;
            splitContainer1.Panel1Collapsed = true;
            //next_item();
            timer.Tick += new EventHandler(next_item); // Everytime timer ticks, timer_Tick will be called
            //MessageBox.Show(Convert.ToInt32(numericUpDown1.Value).ToString());
            timer.Interval = (1000) * Convert.ToInt32(numericUpDown1.Value);              // Timer will tick evert second
            timer.Enabled = true;                       // Enable the timer
            timer.Start();
            numericUpDown1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;
            button1.Enabled = false;
            btnRead.Enabled = false;
            txtUrl.Enabled = false;
        }
        public void next_item(object sender, EventArgs e)
        {
            timer.Stop();
            if (lstNews.SelectedIndices.Count > 0)
            {
                int oldSelection = lstNews.SelectedIndices[0];
                lstNews.SelectedIndices.Clear();

                if (oldSelection + 1 >= lstNews.Items.Count)
                {
                    next_page();
                    lstNews.SelectedIndices.Add(0);
                }
                else
                    lstNews.SelectedIndices.Add(oldSelection + 1);
            }
            timer.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Show();
            splitContainer1.Panel1MinSize = 25;
            splitContainer1.Panel1Collapsed = false;
            timer.Stop();
            button2.Enabled = true;
            numericUpDown1.Enabled = true;
            button3.Enabled = false;
            button1.Enabled = true;
            btnRead.Enabled = true;
            txtUrl.Enabled = true;
        }
    }
}