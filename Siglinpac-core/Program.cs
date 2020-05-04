using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Siglinpac_core
{
    class Program
    {
        static void Main(string[] args)
        {
            

            //  User passes in HTML to Store sticker pack

            //  Iniitalize HAP to load the HTML content
            /// Create the doc
            HtmlAgilityPack.HtmlDocument product_page = new HtmlAgilityPack.HtmlDocument();
            /// DEBUG:: direct path
            product_page.Load(@"C:\Users\Vinh Nguyen\Desktop\Siglinpac\Tanu-Kitu! – LINE stickers _ LINE STORE.html");

            //  Extract all span tags that contain images (hopefully)
            List<string> spanTags = new List<string>();

            //"background-image:url(https://stickershop.line-scdn.net/stickershop/v1/sticker/297435927/android/sticker.png;compress=true);"
            foreach (HtmlNode link in product_page.DocumentNode.SelectNodes("//span[@style]"))
            {
                //  get the value of the Style tag
                HtmlAttribute att = link.Attributes["style"];
                
                //  Put the value into a string so we can manipulate it
                string img_link = att.Value;
                img_link = img_link.Replace("background-image:url(", null);
                img_link = img_link.Replace(";compress=true);", null);

                spanTags.Add(img_link);
            }

            spanTags = spanTags.Distinct().ToList();

            Console.WriteLine("Hello World!");


        }
    }
}
