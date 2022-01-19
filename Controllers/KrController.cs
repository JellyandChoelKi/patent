using K2GGTT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace K2GGTT.Controllers
{
	public class KrController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Action(string keywords)
		{
			ViewBag.Keywords = keywords;
			return View();
		}

		public IActionResult Result(string keywords)
		{
			ViewBag.Keywords = keywords;
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult Member()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		public IActionResult Myinfo()
		{
			return View();
		}
	}
}
