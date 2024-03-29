﻿using K2GGTT.Models;
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
using X.PagedList;
using System.IO;

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
			var member = _context.Member.Where(x => x.MemberId == MemberId && x.Password == SHA256Hash(Password) && x.Gubun == 3).FirstOrDefault();
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

		[HttpGet]
		public IActionResult Member(int? pageNumber = 1)
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}

			List<Member> lists = _context.Member.Where(x => x.Gubun != 3).ToList();
			ViewBag.CurrentCount = lists.Count();
			return View(lists.ToPagedList(pageNumber ?? 1, 20));
		}

		[HttpGet]
		public IActionResult MemberDetail(string memberId)
		{
			var member = _context.Member.Where(x => x.MemberId == memberId).FirstOrDefault();

			return View(member);
		}

		public IActionResult HotTech(int? pageNumber = 1)
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}
			List<HotTech> lists = _context.HotTech.Where(x => x.Title != "ExampleTarget").ToList();
			ViewBag.CurrentCount = lists.Count();
			return View(lists.OrderByDescending(x => x.Id).ToPagedList(pageNumber ?? 1, 20));
		}

		public IActionResult HotTechRegister(int? Id)
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}

			HotTech model = new HotTech();

			// 등록일경우 작성 예시내용을 보여주기 위함(ExampleTarget 제목 데이터는 지우지마세요.)
			model.Content = _context.HotTech.Where(x => x.Title == "ExampleTarget").Select(x => x.Content).FirstOrDefault();
			if (Id > 0 || Id != null)
			{
				var p = _context.HotTech.Where(x => x.Id == Id).FirstOrDefault();
				model = new HotTech()
				{
					Id = p.Id,
					Gubun = p.Gubun,
					Title = p.Title,
					Content = p.Content,
					ApplicantImg = p.ApplicantImg,
					ApplicantImgSrc = p.ApplicantImgSrc,
					ApplicantName = p.ApplicantName,
					ApplicantMajor = p.ApplicantMajor
				};
				ViewBag.Id = Id;
				return View(model);
			}
			return View(model);
		}

		public IActionResult HotTechDelete(int? Id)
		{
			if (Id > 0 || Id != null)
			{
				var p = _context.HotTech.Where(x => x.Id == Id).FirstOrDefault();
				var filepath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "ufile", p.ApplicantImgSrc);
				if(System.IO.File.Exists(filepath)) {
					System.IO.File.Delete(filepath);
				}
				_context.HotTech.RemoveRange(_context.HotTech.Where(x => x.Id == Id));
				_context.SaveChanges();
			}

			return Redirect("/Admin/HotTech");
		}

		public IActionResult HotTechRegisterProc(HotTech model)
		{
			if (model.Id > 0)
			{
				var p = _context.HotTech.Where(x => x.Id == model.Id).FirstOrDefault();
				p.Gubun = model.Gubun;
				p.Title = model.Title;
				p.Content = model.Content;
				if (model.ApplicantImg != null && model.ApplicantImg.Length > 0)
				{
					var f = _context.HotTech.Where(x => x.Id == model.Id).FirstOrDefault();
					var origin_filepath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "ufile", f.ApplicantImgSrc);
					if (System.IO.File.Exists(origin_filepath))
					{
						System.IO.File.Delete(origin_filepath);
					}

					var filepath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "ufile", model.ApplicantImg.FileName);
					using (var uploadFile = System.IO.File.Create(filepath))
					{
						model.ApplicantImg.CopyTo(uploadFile);
					}
					p.ApplicantImgSrc = model.ApplicantImg.FileName;
				}

				p.ApplicantName = model.ApplicantName;
				p.ApplicantMajor = model.ApplicantMajor;
				p.UpdDate = DateTime.Now;
				_context.SaveChanges();
			}
			else
			{
				string FileName = string.Empty;
				if (model.ApplicantImg != null && model.ApplicantImg.Length > 0)
				{
					var filepath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "ufile", model.ApplicantImg.FileName);
					using (var uploadFile = System.IO.File.Create(filepath))
					{
						model.ApplicantImg.CopyTo(uploadFile);
					}
					FileName = model.ApplicantImg.FileName;
				}
				var hottech = new HotTech
				{
					Gubun = model.Gubun,
					Title = model.Title,
					Content = model.Content,
					ApplicantImgSrc = FileName,
					ApplicantName = model.ApplicantName,
					ApplicantMajor = model.ApplicantMajor,
					RegDate = DateTime.Now
				};
				_context.HotTech.Add(hottech);
				_context.SaveChanges();
			}
			return Redirect("/Admin/HotTech");
		}

		[HttpGet]
		public IActionResult AdminManageList(int? pageNumber = 1)
		{
			var session = HttpContext.Session.GetString("MemberId");
			if (session == null)
			{
				return Redirect("/Admin/login");
			}

			List<Member> lists = _context.Member.Where(x => x.Gubun == 3).ToList();
			ViewBag.CurrentCount = lists.Count();
			return View(lists.ToPagedList(pageNumber ?? 1, 20));
		}

		[HttpGet]
		public IActionResult AdminManageRegist()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Invitations(Member model)
		{
			var member = new Member
			{
				MemberId = model.MemberId,
				Password = SHA256Hash(model.Password),
				Name = model.Name,
				Email = model.Email,
				Contact = model.Contact,
				Gubun = 3
			};

			_context.Member.Add(member);
			_context.SaveChanges();

			return Redirect("/Admin/AdminList");
		}
	}
}
