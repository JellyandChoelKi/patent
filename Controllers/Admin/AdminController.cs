using K2GGTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using K2GGTT.Data;
using System.Security.Cryptography;
using System.Text;

namespace K2GGTT.Controllers
{
	public class AdminController : Controller
	{
		public string SHA256Hash(string password)
		{
			SHA256 sha = new SHA256Managed();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in hash)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		private readonly ILogger<HomeController> _logger;
		private readonly DBContext _context;
		public AdminController(ILogger<HomeController> logger, DBContext DBContext)
		{
			_logger = logger;
			_context = DBContext;
		}

		public IActionResult Index()
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}
			return Redirect("/Admin/Member");
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult LoginProc(string MemberId, string Password)
		{
			var member = _context.Member.Where(x => x.MemberId == MemberId && x.Password == SHA256Hash(Password)).FirstOrDefault();
			if (member == null)
			{
				return Content(@"<script type='text/javascript'>alert('로그인 정보가 없습니다.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			HttpContext.Session.SetString("Id", member.Id.ToString());
			HttpContext.Session.SetString("MemberId", member.MemberId);
			HttpContext.Session.SetString("MemberName", member.Name);
			return Redirect("/Admin/Member");
		}

		[HttpGet]
		public IActionResult LogoutProc()
		{
			HttpContext.Session.Clear();
			return Redirect("/Admin/login");
		}

		public IActionResult Member()
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}
			return View();
		}

		public IActionResult HotTech()
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}
			return View();
		}
	}
}
