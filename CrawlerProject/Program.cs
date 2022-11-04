using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CrawlerProject.Model;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace CrawlerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 這是爬網頁新聞的案例

                HtmlWeb webClient = new HtmlWeb(); //建立htmlweb
                //處理C# 連線 HTTPS 網站發生驗證失敗導致基礎連接已關閉
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                string strUrl1 = "https://www.chinatimes.com/newspapers/2601?chdtv";
                
                HtmlDocument doc = webClient.Load(strUrl1); //載入網址資料
                HtmlNodeCollection list = doc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[2]/div/section/ul/li"); //抓取Xpath資料

                string strUrl2 = "https://www.appledaily.com.tw/realtime/new/";
                HtmlDocument appledoc = webClient.Load(strUrl2); //載入網址資料
                // id有被設定隨機給值，所以要用contains來取
                HtmlNodeCollection applelist = appledoc.DocumentNode.SelectNodes("//div[contains(@id,\"stories-container\")]/div/a"); //抓取Xpath資料

                string strUrl3 = "https://today.line.me/tw/v2/tab/entertainment";
                HtmlDocument linedoc = webClient.Load(strUrl3);  //載入網址資料
                HtmlNodeCollection linelist = linedoc.DocumentNode.SelectNodes("//div[@class=\"listModule\"]/a"); //抓取Xpath資料

                Console.WriteLine("中國時報");
                for (int i = 0; i < 3; i++)
                {
                    string time = list[i].SelectSingleNode("div/div/div[2]/div/time/span[1]").InnerText;
                    string date = list[i].SelectSingleNode("div/div/div[2]/div/time/span[2]").InnerText;
                    Console.WriteLine("標題:" + list[i].SelectSingleNode("div/div/div[2]/h3").InnerText);

                    // 印出完整URL
                    var baseurl = new Uri(strUrl1);
                    var urlInfo = new Uri(baseurl, list[i].SelectSingleNode("div/div/div[2]/h3/a").GetAttributeValue("href", "unknown"));

                    Console.WriteLine(time + " " + date);
                    Console.WriteLine("網址： " + urlInfo);
                }
                Console.WriteLine(Environment.NewLine + "蘋果日報");
                for (int i = 0; i < 3; i++)
                {
                    string time = applelist[i].SelectSingleNode("div[2]/div[1]/div").InnerText;
                    string h1 = applelist[i].SelectSingleNode("div[2]/div[2]/span").InnerText;

                    // 印出完整URL
                    var baseur2 = new Uri(strUrl2);
                    var urlInfo = new Uri(baseur2, applelist[i].GetAttributeValue("href", "unknown"));

                    Console.WriteLine(h1);
                    Console.WriteLine(time);
                    Console.WriteLine("網址： " + urlInfo);
                }
                #region LineToday失效
                //Console.WriteLine(Environment.NewLine + "LineToday");
                //for (int i = 0; i < linelist.Count() ; i++)
                //{
                //    string h1 = linelist[i].SelectSingleNode("div[2]/span").InnerText;
                //    string h2 = linelist[i].SelectSingleNode("div[2]/div/span").InnerText;

                //    // 消除換行文字及頭尾空白
                //    h1 = h1.Replace(@"\n","");
                //    h1 = h1.Trim();

                //    // 印出完整URL
                //    var baseur3 = new Uri(strUrl3);
                //    var urlInfo = new Uri(baseur3, linelist[i].GetAttributeValue("href", "unknown"));
                //    Console.WriteLine(h2);
                //    Console.WriteLine(h1);
                //    Console.WriteLine("網址： " + urlInfo);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR=" + ex.ToString());
            }
            Console.ReadLine();
        }
    }
}
