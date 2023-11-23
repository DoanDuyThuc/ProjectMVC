using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectMVC.Areas.Controllers
{
    [Area("database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}