using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Helpers;
using Microsoft.AspNetCore.Http;
using System;

namespace Jahshaka.Core.Utilities
{
    public static class AssetHelper
    {
        public static MemoryStream SaveAsJpeg(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            MemoryStream output = new MemoryStream();
            MemoryStream temp = new MemoryStream();

            var image = Image.Load(stream);
            //image.Resize(size, size).Save(temp);

            image.SaveAsJpeg(output);
            return output;
        }

        /*public static MemoryStream Resize(int size, IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            MemoryStream output = new MemoryStream();
            MemoryStream temp = new MemoryStream();

            var image = Image.Load(stream);
            image.Size(size, size).Save(temp);

            image.SaveAsJpeg(output);
            return output;
        }*/

        public static MemoryStream ToStream(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            MemoryStream output = new MemoryStream();

            stream.CopyTo(output); 
            return output;
        }
    }
}
