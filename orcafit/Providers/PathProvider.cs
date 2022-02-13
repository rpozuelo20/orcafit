using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace betari_app.Providers
{
    public enum Folders { Images = 0, Videos = 1 }
    public class PathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        public PathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }


        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Videos)
            {
                carpeta = "uploadFiles/videos";
            }
            else if (folder == Folders.Images)
            {
                carpeta = "uploadFiles/images";
            }
            string path = Path.Combine
                (this.hostEnvironment.WebRootPath, carpeta, fileName);
            return path;
        }
    }
}
