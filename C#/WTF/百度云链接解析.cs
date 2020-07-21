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
        static void Main()
        {
            string link;
            string code;
            Console.WriteLine("请完整复制粘贴百度云分享链接然后回车\n");
            Console.WriteLine("请确保链接和提取码之间有空格无回车：");
            string text = Convert.ToString(Console.ReadLine());
            Regex link_reg = new Regex(@"(?<=https://pan.baidu.com/s/)\S+");
            Match link_match = link_reg.Match(text);
            if(link_match.Success)
            {
                link = link_match.Value;
            }
            else
            {
                Console.WriteLine("未匹配到链接\n");
                goto final;
            }
            Regex code_reg = new Regex(@"(?<=码: )\w+");
            Match code_match = code_reg.Match(text);
            if(code_match.Success)
            {
                code = code_match.Value;
            }
            else
            {
                code_reg = new Regex(@"(?<=码:)\w+");
                code_match = code_reg.Match(text);
                if (code_match.Success)
                {
                    code = code_match.Value;
                }
                else
                {
                    code_reg = new Regex(@"(?<=码： )\w+");
                    code_match = code_reg.Match(text);
                    if (code_match.Success)
                    {
                        code = code_match.Value;
                    }
                    else
                    {
                        code_reg = new Regex(@"(?<=码：)\w+");
                        code_match = code_reg.Match(text);
                        if (code_match.Success)
                        {
                            code = code_match.Value;
                        }
                        else
                        {
                            code_reg = new Regex(@"(?<=码)\w+");
                            code_match = code_reg.Match(text);
                            if (code_match.Success)
                            {
                                code = code_match.Value;
                            }
                            else
                            {
                                code_reg = new Regex(@"(?<=码 )\w+");
                                code_match = code_reg.Match(text);
                                if (code_match.Success)
                                {
                                    code = code_match.Value;
                                }
                                else
                                {
                                    code_reg = new Regex(@"(?<= )\w{4}");
                                    code_match = code_reg.Match(text);
                                    if (code_match.Success)
                                    {
                                        code = code_match.Value;
                                    }
                                    else
                                    {
                                        Console.WriteLine("未匹配到提取码\n");
                                        goto final;
                                    }
                                }
                            }
                        }
                    }
                }
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
            Console.WriteLine("开始爬取...\n");
            string href;
            string name;
            string size;
            HtmlNode node;
            HtmlNode name_node;
            HtmlNode size_node;
            HtmlDocument doc;
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
                    Console.WriteLine(name + " 大小：" + size);
                    if (size == "0M")
                    {
                        Console.WriteLine($"第{i}个文件为0M，自动跳过");
                        continue;
                    }

                }
                catch (Exception)
                {
                    if(i == 1)
                    {
                        Console.WriteLine("未找到下载链接");
                    }
                    break;
                }                    
                MatchCollection matches = Regex.Matches(href, reg, RegexOptions.IgnoreCase);
                foreach (Match item in matches)
                {
                    var href2 = item.Groups["href"].Value;
                    Console.WriteLine($"第{i}个下载链接：");
                    Console.WriteLine(href2);
                    try
                    {
                        // 使用指定浏览器
                        //Process.Start(@"D:\Chrome\Application\chrome.exe", TargetUrl);

                        // 使用默认浏览器，自动打开下载链接
                        Process.Start(href2);
                    }
                    catch (Exception other)
                    {
                        Console.WriteLine("Error: " + other.Message);
                    }
                }
            }
        }
    }
}