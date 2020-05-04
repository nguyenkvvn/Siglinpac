﻿using System;
using System.Collections.Generic;
using Siglinpac_core;
using static Siglinpac_core.Siglinpac;

namespace Siglinpac_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //List<string> images = Siglinpac_core.Siglinpac.get_sticker_image_links(args[0]);

            SLP_Meta sticker_pack = Siglinpac_core.Siglinpac.get_sticker_pack_meta(args[0]);

            Siglinpac.download_stickers(sticker_pack, args[1]);
        }
    }
}