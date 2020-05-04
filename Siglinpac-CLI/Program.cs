using System;
using System.Collections.Generic;
using System.Diagnostics;
using Siglinpac_core;
using static Siglinpac_core.Siglinpac;

namespace Siglinpac_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Siglinpac - Simple Sticker Fetcher");

            // borrowed from: https://stackoverflow.com/questions/909555/how-can-i-get-the-assembly-file-version
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Console.WriteLine("Version " + version);

            //  If no arguments were passed in
            if ((args == null) || (args.Length == 0))
            {
                Console.WriteLine("Oops! No arguments were given!");
                show_help();

                return;
            }

            Console.WriteLine();
            Console.WriteLine("Getting sticker metadata...");
            SLP_Meta sticker_pack = Siglinpac_core.Siglinpac.get_sticker_pack_meta(args[0]);

            Console.WriteLine("Downloading stickers to...");
            //  If there wasn't a path to save given
            if (args.Length == 1)
            {
                Console.WriteLine("\t" + Siglinpac.get_path_to_stickers(sticker_pack, Environment.CurrentDirectory));
                Siglinpac.download_stickers(sticker_pack, Environment.CurrentDirectory); 
            }
            //  Otherwise, save in the path preferred
            else
            {
                Console.WriteLine("\t" + Siglinpac.get_path_to_stickers(sticker_pack, args[1]));
                Siglinpac.download_stickers(sticker_pack, args[1]);
            }

            Console.WriteLine();
            Console.WriteLine("Done!\n");

        }

        static void show_help()
        {
            Console.WriteLine(
                "=== HOW TO USE === \n" +
                "siglinpac-cli.exe [link-to-sticker-pack-html] (local-path-to-save) \n" +
                "\n" +
                "\n" +
                "You can't just run this app by itself. It needs the following arguments:\n" +
                "\t* [link-to-sticker-pack-html] :: This is the HTML link to the sticker product page you want to fetch. It is a required parameter\n" +
                "\t* (local-path-to-save) :: This is the local path where you want the stickers to save in. It defaults to where this executable is if you don't give it a path.\n" +
                "\n" +
                "Example usage: .\\siglinpac-cli \"https://somestickersite/stickers/potato-pack\" \"c:\\temp\"");
        }
    }
}
