﻿using Microsoft.AspNetCore.Mvc;

namespace MIST155ajax.Controllers
{
    public class HomeworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
