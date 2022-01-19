using K2GGTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace K2GGTT.Controllers
{
	public class AdminController : Controller
	{
		private readonly ILogger<HomeController> _logger;


		public AdminController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			var session = HttpContext.Session.GetString("userId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult LoginProc()
		{
			HttpContext.Session.SetString("userId", "test");
			return RedirectToAction("Index", "Admin");
		}

		[HttpGet]
		public IActionResult LogoutProc()
		{
			HttpContext.Session.Clear();
			return Redirect("/Admin/login");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
