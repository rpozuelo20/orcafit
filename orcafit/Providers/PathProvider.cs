using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace betari_app.Providers
{
    public enum Folders { Images=0, Videos=1, Users=2 }
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
                carpeta = "videos";
            }
            else if (folder == Folders.Images)
            {
                carpeta = Path.Combine("images");
            }
            else if (folder == Folders.Users)
            {
                carpeta = Path.Combine("images", "users");
            }
            string path = Path.Combine
                (this.hostEnvironment.WebRootPath, carpeta, fileName);
            return path;
        }
    }
}
