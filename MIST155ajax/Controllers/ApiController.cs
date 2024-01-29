﻿using Microsoft.AspNetCore.Mvc;
using MIST155ajax.Models;

namespace MIST155ajax.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        public ApiController(MyDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }
    }
}
