using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using HtmlAgilityPack;
using System.CodeDom;
//Install-Package HtmlAgilityPack -Version 1.11.24

namespace BaiduyunLink
{
    class Program
    {
        static void Main(string[] args)
        {
            string link = "";
            string code = "";
            Console.WriteLine("请完整复制粘贴百度云分享链接然后回车\n");
            Console.WriteLine("请确保链接和提取码之间有空格无回车：");
            string text = Convert.ToString(Console.ReadLine());
            string link_reg = @"(?<=https://pan.baidu.com/s/)\S+";
            MatchCollection linkMatch = Regex.Matches(text, link_reg);
            foreach (Match m in linkMatch)
            {
                link = m.Value;
            }
            string code_reg = @"(?<=码: )\w+";
            MatchCollection codeMatch = Regex.Matches(text, code_reg);
            foreach (Match a in codeMatch)
            {
                code = a.Value;
            }
            code_reg = @"(?<=码:)\w+";
            codeMatch = Regex.Matches(text, code_reg);
            foreach (Match b in codeMatch)
            {
                code = b.Value;
            }
            code_reg = @"(?<=码： )\w+";
            codeMatch = Regex.Matches(text, code_reg);
            foreach (Match c in codeMatch)
            {
                code = c.Value;
            }
            code_reg = @"(?<=码：)\w+";
            codeMatch = Regex.Matches(text, code_reg);
            foreach (Match d in codeMatch)
            {
                code = d.Value;
            }
            code_reg = @"(?<=码)\w+";
            codeMatch = Regex.Matches(text, code_reg);
            foreach (Match e in codeMatch)
            {
                code = e.Value;
            }
            if(link == "")
            {
                Console.WriteLine("未匹配到链接\n");
                goto final;
            }
            if(code == "")
            {
                Console.WriteLine("未匹配到提取码\n");
                goto final;
            }
            string url = $"http://pan.naifei.cc/?share={link}%20&pwd={code}";
            Console.WriteLine("\n官网：");
            Console.WriteLine(url + "\n");

            Spider(url);
            final:
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
        // 抓取下载链接并自动用默认浏览器打开
        public static void Spider(string url)
        {
            string href = null;
            HtmlNode node = null;
            HtmlDocument doc = null;
            try
            {
                HtmlWeb web = new HtmlWeb();
                doc = web.Load(url);
                node = doc.DocumentNode.SelectSingleNode("/html/body/table/tbody/tr/td[3]/a/@href");
                href = node.OuterHtml;
            }
            catch (Exception)
            {
                Console.WriteLine($"Error：未找到下载链接\n");
                return;
            }
            string reg = @"<a[^>]*href=([""'])?(?<href>[^'""]+)\1[^>]*>";
            MatchCollection matches = Regex.Matches(href, reg, RegexOptions.IgnoreCase);
            int n = 1;
            foreach (Match item in matches)
            {
                var href2 = item.Groups["href"].Value;
                Console.WriteLine($"第{n}个下载链接：");
                Console.WriteLine(href2);
                try
                {
                    // 使用指定浏览器
                    //Process.Start(@"D:\Chrome\Application\chrome.exe", TargetUrl);

                    // 使用默认浏览器，自动打开下载链接
                    Console.WriteLine("2秒后自动打开浏览器");
                    Thread.Sleep(2000);
                    Process.Start(href2);
                }
                catch (Exception other)
                {
                    Console.WriteLine("Error" + other.Message);
                }
            }
            for (int i = 2; i < 99; i++)
            {
                try
                {
                    n++;
                    node = doc.DocumentNode.SelectSingleNode($"/html/body/table/tbody/tr[{i}]/td[3]/a/@href");
                    if (node != null)
                    {
                        href = node.OuterHtml;
                    }
                    else break;
                    matches = Regex.Matches(href, reg, RegexOptions.IgnoreCase);
                    foreach (Match item in matches)
                    {
                        var href2 = item.Groups["href"].Value;
                        Console.WriteLine($"第{n}个下载链接：");
                        Console.WriteLine(href2);
                        try
                        {
                            //Process.Start(@"D:\Chrome\Application\chrome.exe", TargetUrl);
                            Console.WriteLine("2秒后自动打开浏览器");
                            Thread.Sleep(2000);
                            Process.Start(href2);
                        }
                        catch (Exception other)
                        {
                            Console.WriteLine(other.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    break;
                }
            }
        }
    }
}