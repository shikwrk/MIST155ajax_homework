using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MIST155ajax.Models;
using System.Text;

namespace MIST155ajax.Controllers
{
    public class HomeworkController : Controller
    {
        private readonly MyDBContext _context;

        private readonly IWebHostEnvironment _environment;

        public HomeworkController(MyDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public IActionResult homework1()
        {
            return View();
        }

        public IActionResult homework2()
        {
            return View();
        }

        public IActionResult homework3()
        {
            return View();
        }

        public IActionResult homework4()
        {
            return View();
        }

        public IActionResult CheckUserName(string name)
        {
            bool IsExists = MemberCheckUserName(name);
            return IsExists ? Content("帳號已存在!", "text/plain", Encoding.UTF8) : Content("帳號可使用!", "text/plain", Encoding.UTF8);
        }

        private bool MemberCheckUserName(string name)
        {
            var Member = _context.Members.FirstOrDefault(m => m.Name == name);
            return Member != null;
        }
    }
}
