using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MIST155ajax.Models;
using MIST155ajax.Models.DTO;
using System;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MIST155ajax.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;

        private readonly IWebHostEnvironment _environment;


        public ApiController(MyDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public IActionResult Index()
        {
            //int a = 1;
            //int b = 0;
            //int c = a / b;
            return Content("你好阿", "text/plain", Encoding.UTF8);
        }
        [HttpPost]
        //public IActionResult Register(string name, int age = 28)
        // public IActionResult Register(UserDTO _user)
        [HttpPost]
        public IActionResult Register(Member _user, IFormFile Avatar)
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "guest";
            }
            //todo
            //1. 只允許上傳圖檔
            //2. 圖檔最大2M
            //3. 檔案名稱重複處理

            //string uploadPath = @"C:\Shared\AjaxWorkspace\MSIT155Site\wwwroot\uploads\a.jpg";
            string fileName = "empty.jpg";
            if (Avatar != null)
            {
                fileName = Avatar.FileName;
            }
            string uploadPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                Avatar?.CopyTo(fileStream);
            }

            // return Content($"Hello {_user.Name}, {_user.Age}歲了, 電子郵件是 {_user.Email}","text/plain", Encoding.UTF8);
            //return Content($"{_user.Avatar?.FileName} - {_user.Avatar?.ContentType} - {_user.Avatar?.Length}");

            //新增到資料庫
            _user.FileName = fileName;
            //轉成二進位
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                Avatar?.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            _user.FileData = imgByte;

            _context.Members.Add(_user);
            _context.SaveChanges();


            return Content(uploadPath);
        }

        // 檢查名字是否重複
        public IActionResult CheckAccount(string name)
        {
            if(name != null) { 

            bool IsExists = MemberCheckUserName(name);
            return  IsExists ?  Content("帳號已存在!", "text/plain", Encoding.UTF8): Content("帳號可使用!", "text/plain", Encoding.UTF8);
            }
            return Content("", "text/plain", Encoding.UTF8);
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

        [HttpPost]
        public IActionResult Spots([FromBody] SearchDTO _search)
        {
            //根據分類編號搜尋
            var spots = _search.CategoryId == 0 ? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s => s.CategoryId == _search.CategoryId);

            //根據關鍵字搜尋
            if (!string.IsNullOrEmpty(_search.Keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_search.Keyword) || s.SpotDescription.Contains(_search.Keyword));
            }

            //排序
            switch (_search.SortBy)
            {
                case "spotTitle":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending(s => s.CategoryId);
                    break;
                default: //spotId
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
                    break;
            }

            //總共有幾筆
            int totalCount = spots.Count();
            //一頁幾筆資料
            int pageSize = _search.PageSize ?? 9;
            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            //目前第幾頁
            int page = _search.Page ?? 1;

            //分頁
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);

            //page = 1, Skip((1-1)*9), take(9)  跳過 0 筆資料，取 9 筆資料
            //page = 2, Skip((2-1)*9), take(9)  跳過前 9 筆資料，取 9 筆資料
            //page = 3, Skip((3-1)*9), take(9)  跳過前 18 筆資料，取 9 筆資料


            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalPages = totalPages;
            spotsPaging.TotalCount = totalCount;
            spotsPaging.SpotsResult = spots.ToList();
            return Json(spotsPaging);


            //return Json(spots);
        }

        public IActionResult SpotTitle(string title)
        {
            var titles = _context.Spots.Where(s => s.SpotTitle.Contains(title)).Select(s => s.SpotTitle).Take(8);
            return Json(titles);
        }



    }
}
