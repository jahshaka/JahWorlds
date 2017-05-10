using System.IO;
using ImageSharp;
using ImageSharp.Processing;
using Microsoft.AspNetCore.Http;
using System;

namespace Jahshaka.Core.Utilities
{
    public static class ImageHelper
    {
        public static MemoryStream Thumbnail(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            MemoryStream output = new MemoryStream();
            MemoryStream temp = new MemoryStream();

            Image image = Image.Load(stream);
            image.Resize(100, 100).Save(temp);

            //Image final = new Image(temp);

            image.SaveAsJpeg(output);
            return output;
        }
    }
}
