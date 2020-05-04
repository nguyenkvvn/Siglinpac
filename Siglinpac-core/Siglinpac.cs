using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Siglinpac_core
{
    public class Siglinpac
    {
        /// <summary>
        /// Returns a meta object of the sticker pack given from the link
        /// </summary>
        /// <param name="html_path"></param>
        /// <returns></returns>
        public static SLP_Meta get_sticker_pack_meta(string html_path)
        {
            //  Create the sticker pack descriptor
            SLP_Meta sticker_pack = new SLP_Meta();

            //  Start the HAP and load the path to the HTML
            HtmlAgilityPack.HtmlDocument product_page = new HtmlAgilityPack.HtmlDocument();
            product_page.Load(html_path);

            //  Populate relevant titles
            sticker_pack.Title = product_page.DocumentNode.SelectSingleNode("//p[@class='mdCMN38Item01Ttl']").InnerText;
            sticker_pack.Author = product_page.DocumentNode.SelectSingleNode("//a[@class='mdCMN38Item01Author']").InnerText;
            sticker_pack.Description = product_page.DocumentNode.SelectSingleNode("//p[@class='mdCMN38Item01Txt']").InnerText;

            //  Then get the links to the images
            sticker_pack.image_links = get_sticker_image_links(html_path);

            //  Return the meta for the sticker pack
            return sticker_pack;
        }

        private static List<string> get_sticker_image_links(string html_path)
        {
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

        public static void download_stickers(SLP_Meta sticker_pack, string path_to_save_files_in)
        {
            string path_to_sticker_folder = path_to_save_files_in + "\\" + sticker_pack.Author + "_" + sticker_pack.Title;

            //  Create the directory if it does not exist to store the stickers
            if (!Directory.Exists(path_to_sticker_folder))
            {
                /// Makes a directory with the name of author_title
                Directory.CreateDirectory(path_to_sticker_folder);
            }

            //  Write out the meta information
            string jsonString = JsonConvert.SerializeObject(sticker_pack, Formatting.Indented);
            File.WriteAllText(path_to_sticker_folder + "\\meta.json", jsonString);

            //  Download the images with the WebClient
            int increment = 0;
            WebClient wc = new WebClient();
            foreach (string str_link in sticker_pack.image_links)
            {
                wc.DownloadFile(str_link, path_to_sticker_folder + "\\image_" + increment + ".png");
                increment++;
            }
        }

        /// <summary>
        /// Descriptor class for the sticker pack
        /// </summary>
        public class SLP_Meta
        {
            [JsonProperty]
            /// <summary>
            /// The title of the sticker pack
            /// </summary>
            public string Title;

            [JsonProperty]
            /// <summary>
            /// The author of the sticker pack
            /// </summary>
            public string Author;

            [JsonProperty]
            /// <summary>
            /// The description of the sticker pack
            /// </summary>
            public string Description;

            [JsonProperty]
            /// <summary>
            /// The links to the sticker pack images
            /// </summary>
            public List<String> image_links;

        }
    }
}
