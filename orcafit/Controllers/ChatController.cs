using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class ChatController : Controller
    {
        public static Dictionary<int, string> Rooms = new Dictionary<int, string>()
        {
            {1,"General"}
        };
        [AuthorizeUsuarios]
        public IActionResult Index()
        {
            return View();
        }
        [AuthorizeUsuarios]
        public IActionResult Room(int room)
        {
            return View("Room", room);
        }
    }
}