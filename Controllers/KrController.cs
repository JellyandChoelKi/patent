using K2GGTT.Models;
using K2GGTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using K2GGTT.Data;
using Newtonsoft.Json;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
namespace K2GGTT.Controllers
{
	public class KrController : Controller
	{
		private readonly ILogger<KrController> _logger;
		private readonly DBContext _context;

		public KrController(ILogger<KrController> logger, DBContext DBContext)
		{
			_logger = logger;
			_context = DBContext;
		}

		public IActionResult Index()
		{
			var SessionMemberId = HttpContext.Session.GetString("MemberId");
			if (SessionMemberId != null)
			{
				ViewBag.SessionMemberId = SessionMemberId;
			}
			return View();
		}

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return Content(@"<script type='text/javascript'>alert('정상적으로 로그아웃 되었습니다.');location.href='/Kr/Index';</script>", "text/html", System.Text.Encoding.UTF8);
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

		public IActionResult Password()
		{
			return View();
		}

		[HttpPost]
		public IActionResult NewPasswordCreate(string email)
		{
			Random r = new Random();
			const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string code = new string(Enumerable.Repeat(characters, 6).Select(s => s[r.Next(s.Length)]).ToArray());

			var member = _context.Member.Where(x => x.Email == email).FirstOrDefault();
			if (member == null)
			{
				return Content(@"<script type='text/javascript'>alert('가입된 이메일 정보가 없습니다. 다시 확인 바랍니다.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			member.Password = code;
			_context.SaveChanges();

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("no-reply@yellowknife.io", "K2G", System.Text.Encoding.UTF8);
			mail.To.Add(email);
			mail.Subject = "[K2G] 임시 비밀번호가 발급되었습니다.";
			mail.Body = "임시 비밀번호를 전송해 드립니다. 확인 후 회원정보 수정을 통해 비밀번호를 변경하시기 바랍니다. 비밀번호 : " + code;
			mail.IsBodyHtml = true;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			SmtpClient SmtpServer = new SmtpClient("email-smtp.ap-northeast-2.amazonaws.com");
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential("AKIAUEOAKJWYTC4CMF7U", "BJhoD0cuzQMgK0osvQ6r0vvr9P2SweePd3Cm3V5iz9c2");
			SmtpServer.EnableSsl = true;
			SmtpServer.Send(mail);
			mail.Dispose();

			return Content(@"<script type='text/javascript'>alert('입력하신 이메일 주소로 새로운 비밀번호가 전송되었습니다.');location.href='/Kr/Member';</script>", "text/html", System.Text.Encoding.UTF8);
		}

		[HttpPost]
		public IActionResult LoginProc(string memberid, string password)
		{
			var member = _context.Member.Where(x => x.MemberId == memberid && x.Password == password).FirstOrDefault();
			if (member == null)
			{
				return Content(@"<script type='text/javascript'>alert('로그인 정보가 없습니다.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			HttpContext.Session.SetString("Id", member.Id.ToString());
			HttpContext.Session.SetString("MemberId", member.MemberId);
			HttpContext.Session.SetString("MemberName", member.Name);
			return Redirect("/Kr/Index");
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
			MyInfoViewModel model = new MyInfoViewModel();
			int Id = Convert.ToInt32(HttpContext.Session.GetString("Id"));
			if (Id > 0)
			{
				var member = _context.Member.Where(x => x.Id == Id).FirstOrDefault();
				model.MemberId = member.MemberId;
				model.Name = member.Name;
				model.Contact = member.Contact;
			}
			return View(model);
		}

		[HttpPost]
		public IActionResult MemberIdCheck(string memberid)
		{
			string response = "N";
			if (!string.IsNullOrEmpty(memberid))
			{
				var Id = _context.Member.Where(x => x.MemberId == memberid).FirstOrDefault();
				if (Id != null) response = "Y";
			}
			return Ok(JsonConvert.SerializeObject(response));
		}

		[HttpPost]
		public IActionResult EmailCertification(string email)
		{
			Random r = new Random();
			const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string code = new string(Enumerable.Repeat(characters, 6).Select(s => s[r.Next(s.Length)]).ToArray());

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("no-reply@yellowknife.io", "K2G", System.Text.Encoding.UTF8);
			mail.To.Add(email);
			mail.Subject = "K2G에서 이메일 인증코드를 보내드립니다.";
			mail.Body = "사용자 인증을 위해 6자리 코드 보내드립니다. 회원가입 화면에 6자리 코드 입력 바랍니다. 인증코드 : " + code;
			mail.IsBodyHtml = true;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			SmtpClient SmtpServer = new SmtpClient("email-smtp.ap-northeast-2.amazonaws.com");
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential("AKIAUEOAKJWYTC4CMF7U", "BJhoD0cuzQMgK0osvQ6r0vvr9P2SweePd3Cm3V5iz9c2");
			SmtpServer.EnableSsl = true;
			SmtpServer.Send(mail);
			mail.Dispose();

			return Ok(JsonConvert.SerializeObject(code));
		}

		[HttpPost]
		public IActionResult MyInfoProc(MyInfoViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			Member member = new Member();
			member.MemberId = model.MemberId;
			member.Password = model.Password;
			member.Gubun = model.Gubun;
			member.Name = model.Name;
			member.CompanyRegistrationNumber = model.CompanyRegistrationNumber;
			member.Representative = model.Representative;
			member.TaxInvoiceOfficer = model.TaxInvoiceOfficer;
			member.Contact = model.Contact;
			member.Zipcode = model.Zipcode;
			member.Addr1 = model.Addr1;
			member.Addr2 = model.Addr2;
			member.Email = model.Email;
			_context.Member.Add(member);
			_context.SaveChanges();

			return Redirect("/Kr/MyInfo");
		}
	}
}
