using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;

namespace Windows
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string url = GetLink();
            if (url != "")
            {
                Spider(url);
            }
            else
            {
                textBox2.Text += "请重新输入分享链接" + Environment.NewLine; 
            }
            textBox2.Text += "任务结束" + Environment.NewLine;
        }
        public string GetLink()
        {
            string link;
            string code = "";
            Regex link_reg = new Regex(@"(?<=pan.baidu.com/s/)\S+");
            Match link_match = link_reg.Match(textBox1.Text);
            if (link_match.Success)
            {
                link = link_match.Value;
            }
            else
            {
                MessageBox.Show("未匹配到链接");
                return "";
            }
            Regex code_reg = new Regex(@"(?<=码: )\w+");
            Match code_match = code_reg.Match(textBox1.Text);
            if (code_match.Success)
            {
                code = code_match.Value;
            }
            else
            {
                code_reg = new Regex(@"(?<=码:)\w+");
                code_match = code_reg.Match(textBox1.Text);
                if (code_match.Success)
                {
                    code = code_match.Value;
                }
                else
                {
                    code_reg = new Regex(@"(?<=码： )\w+");
                    code_match = code_reg.Match(textBox1.Text);
                    if (code_match.Success)
                    {
                        code = code_match.Value;
                    }
                    else
                    {
                        code_reg = new Regex(@"(?<=码：)\w+");
                        code_match = code_reg.Match(textBox1.Text);
                        if (code_match.Success)
                        {
                            code = code_match.Value;
                        }
                        else
                        {
                            code_reg = new Regex(@"(?<=码)\w+");
                            code_match = code_reg.Match(textBox1.Text);
                            if (code_match.Success)
                            {
                                code = code_match.Value;
                            }
                            else
                            {
                                code_reg = new Regex(@"(?<=码 )\w+");
                                code_match = code_reg.Match(textBox1.Text);
                                if (code_match.Success)
                                {
                                    code = code_match.Value;
                                }
                                else
                                {
                                    code_reg = new Regex(@"(?<= )\w{4}");
                                    code_match = code_reg.Match(textBox1.Text);
                                    if (code_match.Success)
                                    {
                                        code = code_match.Value;
                                    }
                                    else
                                    {
                                        MessageBox.Show("未匹配到提取码");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string url = $"http://pan.naifei.cc/?share={link}%20&pwd={code}";
            textBox2.Text = "如有任何问题请访问官网：" + Environment.NewLine + url + Environment.NewLine + Environment.NewLine;
            return url;
        }
        /// <summary>
        /// 爬取链接并输出到UI上
        /// </summary>
        /// <param name="url">通过pan.naifei.cc解析的百度云下载链接</param>
        public void Spider(string url)
        {
            textBox2.Text += "开始爬取..." + Environment.NewLine + Environment.NewLine;
            string href;
            string name;
            string size;
            HtmlNode node;
            HtmlNode name_node;
            HtmlNode size_node;
            HtmlAgilityPack.HtmlDocument doc;
            string reg = @"<a[^>]*href=([""'])?(?<href>[^'""]+)\1[^>]*>";
            HtmlWeb web = new HtmlWeb();
            doc = web.Load(url);
            for (int i = 1; i < 99; i++)
            {
                try
                {
                    //href
                    node = doc.DocumentNode.SelectSingleNode($"/html/body/table/tbody/tr[{i}]/td[3]/a/@href");
                    href = node.OuterHtml;
                    //文件大小
                    size_node = doc.DocumentNode.SelectSingleNode($"/html/body/table/tbody/tr[{i}]/td[2]");
                    size = size_node.InnerText;
                    //文件名
                    name_node = doc.DocumentNode.SelectSingleNode($"/html/body/table/tbody/tr[{i}]/td[1]");
                    name = name_node.InnerText;
                    //文件为0直接跳过
                    if (size == "0M")
                    {
                        SetLable($"文件：{name} 大小：{size} 自动跳过", i);
                        continue;
                    }
                    else
                    {
                        SetLable($"文件：{name} 大小：{size}", i);
                    }
                }
                catch (Exception)
                {
                    if (i == 1)
                    {
                        MessageBox.Show("未找到下载链接");
                    }
                    break;
                }
                MatchCollection matches = Regex.Matches(href, reg, RegexOptions.IgnoreCase);
                foreach (Match item in matches)
                {
                    var href2 = item.Groups["href"].Value;
                    if(i>8)
                    {
                        textBox2.Text += "更多文件下载请手动复制链接到浏览器" + Environment.NewLine;
                        textBox2.Text += $"第{i}个下载链接：" + Environment.NewLine + Environment.NewLine;
                        textBox2.Text += href2 + Environment.NewLine;
                    }

                    SetlinkLable(href2, i);
                }
            }
        }
        /// <summary>
        /// 把文件信息对应到UI上的label
        /// </summary>
        /// <param name="text">文件信息</param>
        /// <param name="i">第i个文件</param>
        public void SetLable(string text, int i)
        {
            if (i == 1) label2.Text = text;
            if (i == 2) label3.Text = text;
            if (i == 3) label4.Text = text;
            if (i == 4) label5.Text = text;
            if (i == 5) label6.Text = text;
            if (i == 6) label7.Text = text;
            if (i == 7) label8.Text = text;
            if (i == 8) label9.Text = text;
        }
        /// <summary>
        /// 把链接对应到UI上的LinkLabel
        /// </summary>
        /// <param name="link">下载链接</param>
        /// <param name="i">第i个链接</param>
        public void SetlinkLable(string link, int i)
        {
            if (i == 1) linkLabel1.Text = link;
            if (i == 2) linkLabel2.Text = link;
            if (i == 3) linkLabel3.Text = link;
            if (i == 4) linkLabel4.Text = link;
            if (i == 5) linkLabel5.Text = link;
            if (i == 6) linkLabel6.Text = link;
            if (i == 7) linkLabel7.Text = link;
            if (i == 8) linkLabel8.Text = link;
        }
        //点击链接跳转默认浏览器开始下载
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel2.Text);
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel3.Text);
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel4.Text);
        }

        private void LinkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel5.Text);
        }

        private void LinkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel6.Text);
        }

        private void LinkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel7.Text);
        }

        private void LinkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel8.Text);
        }
    }
}
