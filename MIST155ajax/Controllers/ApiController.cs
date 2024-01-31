using Microsoft.AspNetCore.Mvc;
using MIST155ajax.Models;
using MIST155ajax.Models.DTO;
using System.Text;

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
            //int a = 1;
            //int b = 0;
            //int c = a / b;
            return Content("你好阿", "text/plain", Encoding.UTF8);
        }

        public IActionResult Register(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                user.Name = "guest";
            }
            return Content($"Hello{user.Name},{ user.Age}歲了, ,電子郵件是 {user.Email}","text/plain",Encoding.UTF8);
        }

        // 檢查名字是否重複
        public IActionResult CheckUserName(string name)
        {
            bool IsExists = MemberCheckUserName(name);
            return  IsExists ?  Content("帳號已存在!", "text/plain", Encoding.UTF8): Content("帳號可使用!", "text/plain", Encoding.UTF8);
        }

        private bool MemberCheckUserName(string name)
        {
            var Member = _context.Members.FirstOrDefault(m => m.Name == name);
            return Member != null;
        }

        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }
        public IActionResult Avatar(int id = 2)
        {
            Member? member = _context.Members.Find(id);
            if(member != null)
            {
                byte[] img = member.FileData;
                if(img != null)
                {
                    return File(img, "image/jpeg");
                }
            }
            return NotFound();
        }

        public IActionResult District(string city = "台北市")
        {
            var districts = _context.Addresses.Where(a => a.City == city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }

        public IActionResult Road(string siteId = "")
        {
            var roads = _context.Addresses.Where(a => a.SiteId == siteId).Select(a => a.Road).Distinct();
            return Json(roads);
        }

    }
}
