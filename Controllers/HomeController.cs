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
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return RedirectToAction("index", "kr");
		}
	}
}
