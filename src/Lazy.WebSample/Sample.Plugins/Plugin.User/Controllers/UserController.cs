using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Plugin.Task
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
