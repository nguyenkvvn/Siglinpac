using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Siglinpac_core
{
    public class Siglinpac
    {
        public static SLP_Meta get_sticker_pack_meta(string html_path)
        {
            SLP_Meta sticker_pack = new SLP_Meta();

            HtmlAgilityPack.HtmlDocument product_page = new HtmlAgilityPack.HtmlDocument();
            product_page.Load(html_path);

            sticker_pack.Title = product_page.DocumentNode.SelectSingleNode("//p[@class='mdCMN38Item01Ttl']").InnerText;
            sticker_pack.Author = product_page.DocumentNode.SelectSingleNode("//a[@class='mdCMN38Item01Author']").InnerText;
            sticker_pack.Description = product_page.DocumentNode.SelectSingleNode("//p[@class='mdCMN38Item01Txt']").InnerText;

            sticker_pack.image_links = get_sticker_image_links(html_path);

            return sticker_pack;
        }

        private static List<string> get_sticker_image_links(string html_path)
        {
            //  User passes in HTML to Store sticker pack

            //  Iniitalize HAP to load the HTML content
            /// Create the doc and load it in memory
            HtmlAgilityPack.HtmlDocument product_page = new HtmlAgilityPack.HtmlDocument();
            product_page.Load(html_path);

            //  Extract all span tags
            ///     Specifically, the ones with a style attribute, which we will use to filter out the unecessary fluff
            List<string> spanTags = new List<string>();

            /// example: "background-image:url(https://stickershop.line-scdn.net/stickershop/v1/sticker/297435927/android/sticker.png;compress=true);"
            foreach (HtmlNode link in product_page.DocumentNode.SelectNodes("//span[@style]"))
            {
                //  Get the value of the Style tag
                HtmlAttribute att = link.Attributes["style"];
                
                //  Put the value into a string so we can manipulate it
                string img_link = att.Value;
                img_link = img_link.Replace("background-image:url(", null);
                img_link = img_link.Replace(";compress=true);", null);

                spanTags.Add(img_link);
            }

            // Return the list of image links
            return spanTags.Distinct().ToList();

        }

        /*public static bool download_images_from_links(List<string> image_links, string path_to_save_files_in)
        {
            // Initialize WebClient
            WebClient wc = new WebClient();

            Directory directory = new Directory(path_to_save_in);

            foreach (string str_link in image_links)
            {
                

                wc.DownloadFile(str_link, path_to_save_in + @"")
            }
            */
        }

        
    

    public class SLP_Meta
    {
        public string Title;
        public string Author;
        public string Description;

        public List<String> image_links;

    }
}
